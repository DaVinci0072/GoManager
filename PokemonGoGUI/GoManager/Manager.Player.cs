﻿using POGOProtos.Data;
using POGOProtos.Enums;
using POGOProtos.Inventory;
using POGOProtos.Networking.Responses;
using POGOProtos.Settings.Master;
using PokemonGo.RocketAPI;
using PokemonGoGUI.GoManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonGoGUI.Extensions;

namespace PokemonGoGUI.GoManager
{
    public partial class Manager
    {
        public async Task<MethodResult> UpdateDetails()
        {
            LogCaller(new LoggerEventArgs("Updating details", LoggerTypes.Debug));

            if(!_client.LoggedIn || _client.AuthExpired)
            {
                MethodResult loginResult = await Login();

                if(!loginResult.Success)
                {
                    return loginResult;
                }
            }

            await GetProfile(); //Don't care if it fails

            await Task.Delay(CalculateDelay(UserSettings.GeneralDelay, UserSettings.GeneralDelayRandom));

            bool potentialAccountban = false;

            MethodResult<Dictionary<PokemonId, PokemonSettings>> result = await GetItemTemplates();

            if(result.Success && result.Data != null && result.Data.Count == 0)
            {
                potentialAccountban = true;
            }

            await Task.Delay(CalculateDelay(UserSettings.GeneralDelay, UserSettings.GeneralDelayRandom));

            MethodResult inventoryResult = await UpdateInventory();

            if(inventoryResult.Success)
            {
                if(AccountState == Enums.AccountState.PermAccountBan)
                {
                    AccountState = Enums.AccountState.Good;
                }
            }

            if (!inventoryResult.Success)
            {
                if (inventoryResult.Message == "Failed to get inventory." && potentialAccountban)
                {
                    AccountState = Enums.AccountState.PermAccountBan;

                    LogCaller(new LoggerEventArgs("Potential account ban", LoggerTypes.Warning));
                }
            }
            else
            {
                if(AccountState == Enums.AccountState.PermAccountBan)
                {
                    AccountState = Enums.AccountState.Good;
                }

                LogCaller(new LoggerEventArgs("Finished updating details", LoggerTypes.Debug));
            }

            await ClaimLevelUpRewards(Level);

            return new MethodResult
            {
                Success = true
            };
        }

        public async Task<MethodResult> GetProfile()
        {
            try
            {
                if (!_client.LoggedIn)
                {
                    MethodResult result = await Login();

                    if (!result.Success)
                    {
                        return result;
                    }
                }

                GetPlayerResponse response = await _client.Player.GetPlayer();

                if(response.Success)
                {
                    PlayerData = response.PlayerData;
                }

                return new MethodResult
                {
                    Success = response.Success
                };

            }
            catch(Exception ex)
            {
                LogCaller(new LoggerEventArgs("Failed to get player stats", LoggerTypes.Exception, ex));

                return new MethodResult
                {
                    Message = "Failed to get player stats"
                };
            }
        }

        public async Task<MethodResult> ExportStats()
        {
            MethodResult result = await UpdateDetails();

            //Prevent API throttling
            await Task.Delay(500);

            if (!result.Success)
            {
                return result;
            }

            //Possible some objects were empty.
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("=== Trainer Stats ===");

            if (Stats != null && PlayerData != null)
            {
                builder.AppendLine(String.Format("Group: {0}", UserSettings.GroupName));
                builder.AppendLine(String.Format("Username: {0}", UserSettings.PtcUsername));
                builder.AppendLine(String.Format("Password: {0}", UserSettings.PtcPassword));
                builder.AppendLine(String.Format("Level: {0}", Stats.Level));
                builder.AppendLine(String.Format("Current Trainer Name: {0}", PlayerData.Username));
                builder.AppendLine(String.Format("Team: {0}", PlayerData.Team));
                builder.AppendLine(String.Format("Stardust: {0:N0}", TotalStardust));
                builder.AppendLine(String.Format("Unique Pokedex Entries: {0}", Stats.UniquePokedexEntries));
            }
            else
            {
                builder.AppendLine("Failed to grab stats");
            }

            builder.AppendLine();

            builder.AppendLine("=== Pokemon ===");

            if (Pokemon != null)
            {
                foreach (PokemonData pokemon in Pokemon.OrderByDescending(x => x.Cp))
                {
                    string candy = "Unknown";

                    MethodResult<PokemonSettings> pSettings = GetPokemonSetting(pokemon.PokemonId);

                    if (pSettings.Success)
                    {
                        Candy pCandy = PokemonCandy.FirstOrDefault(x => x.FamilyId == pSettings.Data.FamilyId);

                        if (pCandy != null)
                        {
                            candy = pCandy.Candy_.ToString("N0");
                        }
                    }


                    MethodResult<double> perfectResult = CalculateIVPerfection(pokemon);
                    string iv = "Unknown";

                    if (perfectResult.Success)
                    {
                        iv = Math.Round(perfectResult.Data, 2).ToString() + "%";
                    }

                    builder.AppendLine(String.Format("Pokemon: {0,-10} CP: {1, -5} IV: {2,-7} Primary: {3, -14} Secondary: {4, -14} Candy: {5}", pokemon.PokemonId, pokemon.Cp, iv, pokemon.Move1.ToString().Replace("Fast", ""), pokemon.Move2, candy));
                }
            }

            //Remove the hardcoded directory later
            try
            {
                string directoryName = "AccountStats";

                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                string fileName = String.Format("{1}_{0}.txt", UserSettings.PtcUsername.Split('@').First(), Stats.Level);

                string filePath = Path.Combine(directoryName, fileName);

                File.WriteAllText(filePath, builder.ToString());

                LogCaller(new LoggerEventArgs(String.Format("Finished exporting stats to file {0}", filePath), LoggerTypes.Info));

                return new MethodResult
                {
                    Message = "Success",
                    Success = true
                };
            }
            catch(Exception ex)
            {
                LogCaller(new LoggerEventArgs("Failed to export stats due to exception", LoggerTypes.Warning, ex));

                return new MethodResult();
            }
        }

        public async Task<MethodResult> ClaimLevelUpRewards(int level)
        {
            if(!UserSettings.ClaimLevelUpRewards)
            {
                return new MethodResult();
            }

            try
            {
                if (!_client.LoggedIn)
                {
                    MethodResult result = await Login();

                    if (!result.Success)
                    {
                        return result;
                    }
                }

                LevelUpRewardsResponse response = await _client.Player.GetLevelUpRewards(level);

                if(response.Result == LevelUpRewardsResponse.Types.Result.Success)
                {
                    string rewards = StringUtil.GetSummedFriendlyNameOfItemAwardList(response.ItemsAwarded);

                    LogCaller(new LoggerEventArgs(String.Format("Grabbed rewards for level {0}. Rewards: {1}", level, rewards), LoggerTypes.Info));
                }
                else if(response.Result == LevelUpRewardsResponse.Types.Result.Unset)
                {
                    LogCaller(new LoggerEventArgs("Failed to request rewards", LoggerTypes.Warning));

                    return new MethodResult();
                }

                return new MethodResult
                {
                    Success = true
                };

            }
            catch (Exception ex)
            {
                LogCaller(new LoggerEventArgs("Failed to get level up rewards", LoggerTypes.Exception, ex));

                return new MethodResult
                {
                    Message = "Failed to get level up rewards"
                };
            }
        }

        public async Task<MethodResult<bool>> GetGameSettings(string minVersion)
        {
            try
            {
                DownloadSettingsResponse response = await _client.Download.GetSettings();

                if(String.IsNullOrEmpty(response.Settings.MinimumClientVersion))
                {
                    return new MethodResult<bool>();
                }

                return new MethodResult<bool>
                {
                    Data = response.Settings.MinimumClientVersion == minVersion,
                    Message = String.Format("Minimum client is now {0}", response.Settings.MinimumClientVersion),
                    Success = true
                };
            }
            catch(Exception ex)
            {
                LogCaller(new LoggerEventArgs("Failed to request game settings", LoggerTypes.Exception, ex));

                return new MethodResult<bool>();
            }
        }
    }
}
