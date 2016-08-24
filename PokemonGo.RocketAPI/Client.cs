﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.Helpers;
using PokemonGo.RocketAPI.HttpClient;
using PokemonGo.RocketAPI.Login;
using POGOProtos.Inventory;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Envelopes;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

namespace PokemonGo.RocketAPI
{
    public class Client
    {
        public Rpc.Login Login;
        public Rpc.Player Player;
        public Rpc.Download Download;
        public Rpc.Inventory Inventory;
        public Rpc.Map Map;
        public Rpc.Fort Fort;
        public Rpc.Encounter Encounter;
        public Rpc.Misc Misc;

        public ISettings Settings { get; private set; }
        public string AuthToken { get; set; }

        public double CurrentLatitude { get; internal set; }
        public double CurrentLongitude { get; internal set; }
        public double CurrentAltitude { get; internal set; }

        public AuthType AuthType
        {
            get
            {
                return Settings.AuthType;
            }
        }

        internal PokemonHttpClient PokemonHttpClient;
        internal string ApiUrl { get; set; }
        
        internal AuthTicket AuthTicket { get; set; }

        public bool AuthExpired
        {
            get
            {
                if (AuthTicket == null)
                {
                    return true;
                }

                DateTime expired = DateTimeExtensions.GetDateTimeFromMilliseconds(AuthTicket.ExpireTimestampMs);

                //1 minute buffer
                return DateTime.UtcNow > expired.AddMinutes(-1);
            }
        }

        public bool LoggedIn
        {
            get
            {
                if (Login == null)
                {
                    return false;
                }

                return Login.LoggedIn;
            }
        }

        public Client()
        {

        }

        public void SetSettings(ISettings settings)
        {
            Settings = settings;

            Login = new Rpc.Login(this);
            Player = new Rpc.Player(this);
            Download = new Rpc.Download(this);
            Inventory = new Rpc.Inventory(this);
            Map = new Rpc.Map(this);
            Fort = new Rpc.Fort(this);
            Encounter = new Rpc.Encounter(this);
            Misc = new Rpc.Misc(this);

            Player.SetCoordinates(Settings.DefaultLatitude, Settings.DefaultLongitude, Settings.DefaultAltitude);

            ProxyEx proxy = new ProxyEx
            {
                Address = Settings.ProxyIP,
                Port = Settings.ProxyPort,
                Username = Settings.ProxyUsername,
                Password = Settings.ProxyPassword
            };

            HttpClientHandler handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = false,
                Proxy = proxy.AsWebProxy(),
                UseProxy = true,
            };

            PokemonHttpClient = new PokemonHttpClient(handler);

        }

        public async Task<MethodResult> DoLogin(ISettings settings)
        {
            SetSettings(settings);

            await Login.DoLogin();

            return new MethodResult
            {
                Message = "Successfully logged into server.",
                Success = true
            };
        }

        public async Task<MethodResult> ReAuthenticate()
        {
            await Login.ReAuthenticate();

            return new MethodResult
            {
                Message = "Successfully reauthenticated.",
                Success = true
            };
        }

        public void Logout()
        {
            AuthTicket = null;
            ApiUrl = null;
            AuthToken = null;
        }
    }
}