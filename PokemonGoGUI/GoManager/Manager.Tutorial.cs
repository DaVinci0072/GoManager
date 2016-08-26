using POGOProtos.Data.Player;
using POGOProtos.Enums;
using POGOProtos.Networking.Responses;
using PokemonGo.RocketAPI;
using PokemonGoGUI.GoManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoGUI.GoManager
{
    public partial class Manager
    {
        public async Task<MethodResult> MarkStartUpTutorialsComplete(bool forceAvatarUpdate)
        {
            LogCaller(new LoggerEventArgs("Marking startup tutorials completed", LoggerTypes.Debug));

            MethodResult reauthResult = await CheckReauthentication();

            if(!reauthResult.Success)
            {
                return reauthResult;
            }

            bool success = true;

            if(PlayerData == null)
            {
                MethodResult result = await GetProfile();

                if(!result.Success)
                {
                    return result;
                }

                await Task.Delay(CalculateDelay(UserSettings.GeneralDelay, UserSettings.GeneralDelayRandom));
            }


            List<TutorialState> startupTutorials = new List<TutorialState>
            {
                TutorialState.LegalScreen,
                TutorialState.NameSelection,
                TutorialState.FirstTimeExperienceComplete
            };

            List<TutorialState> completedTutorials = PlayerData.TutorialState.ToList();

            foreach(TutorialState completed in completedTutorials)
            {
                startupTutorials.Remove(completed);
            }

            if(!completedTutorials.Contains(TutorialState.AvatarSelection) || forceAvatarUpdate)
            {
                await SetPlayerAvatar();
            }
            else
            {
                LogCaller(new LoggerEventArgs("Avatar already set", LoggerTypes.Info));
            }

            if(startupTutorials.Count != 0)
            {
                MethodResult result = await MarkTutorialsComplete(startupTutorials);

                if(!result.Success)
                {
                    success = false;
                }

                await Task.Delay(CalculateDelay(UserSettings.GeneralDelay, UserSettings.GeneralDelayRandom));
            }
            else
            {
                LogCaller(new LoggerEventArgs("Startup tutorials already complete", LoggerTypes.Info));
            }

            //Complete encounter
            List<PokemonId> pokemon = new List<PokemonId>
            {
                PokemonId.Charmander,
                PokemonId.Bulbasaur,
                PokemonId.Squirtle,
                PokemonId.Pikachu
            };

            PokemonId chosenPokemon = PokemonId.Pikachu;

            lock(_rand)
            {
                chosenPokemon = pokemon[_rand.Next(0, pokemon.Count)];
            }

            if(!completedTutorials.Contains(TutorialState.PokemonCapture))
            {
                MethodResult result = await CompleteEncounterTutorial(chosenPokemon);

                if(!result.Success)
                {
                    success = false;
                }

                await Task.Delay(CalculateDelay(UserSettings.GeneralDelay, UserSettings.GeneralDelayRandom));
            }
            else
            {
                LogCaller(new LoggerEventArgs("Encounter tutorial already complete", LoggerTypes.Info));
            }

            if (success)
            {
                LogCaller(new LoggerEventArgs("Finished tutorial", LoggerTypes.Debug));
            }
            else
            {
                LogCaller(new LoggerEventArgs("Error occured when completing tutorial", LoggerTypes.Warning));
            }

            return new MethodResult()
            {
                Success = true
            };
        }

        public async Task<MethodResult> MarkTutorialsComplete(List<TutorialState> tutorials)
        {
            try
            {
                MarkTutorialCompleteResponse response = await _client.Misc.MarkTutorialComplete(tutorials);

                if(response.Success)
                {
                    PlayerData = response.PlayerData;

                    return new MethodResult
                    {
                        Success = true
                    };
                }

                LogCaller(new LoggerEventArgs("Tutorial completion request wasn't successful", LoggerTypes.Warning));

                return new MethodResult();
            }
            catch(Exception ex)
            {
                LogCaller(new LoggerEventArgs("Failed to mark tutorials as complete", LoggerTypes.Exception, ex));

                return new MethodResult();
            }
        }

        public async Task<MethodResult> CompleteEncounterTutorial(PokemonId pokemon)
        {
            try
            {
                EncounterTutorialCompleteResponse response = await _client.Encounter.EncounterTutorialComplete(pokemon);

                if(response.Result == EncounterTutorialCompleteResponse.Types.Result.Success)
                {
                    LogCaller(new LoggerEventArgs(String.Format("Caught a {0}", pokemon), LoggerTypes.Success));

                    return new MethodResult
                    {
                        Success = true
                    };
                }

                LogCaller(new LoggerEventArgs(String.Format("Invalid response from complete encounter tutorial. Response: {0}", response.Result), LoggerTypes.Warning));

                return new MethodResult();
            }
            catch(Exception ex)
            {
                LogCaller(new LoggerEventArgs("Failed to complete encounter tutorial", LoggerTypes.Exception, ex));

                return new MethodResult();
            }
        }

        public async Task<MethodResult> SetPlayerAvatar()
        {
            PlayerAvatar avatar = new PlayerAvatar();

            try
            {
                SetAvatarResponse response = await _client.Player.SetAvatar(avatar);

                LogCaller(new LoggerEventArgs("Avatar set to defaults", LoggerTypes.Success));

                if (response.Status == SetAvatarResponse.Types.Status.Success)
                {
                    return new MethodResult
                    {
                        Success = true
                    };
                }

                return new MethodResult();
            }
            catch (Exception ex)
            {
                LogCaller(new LoggerEventArgs("Failed to set avatar", LoggerTypes.Exception, ex));

                return new MethodResult();
            }
        }
    }
}
