using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Models;


namespace TwitchBot.Backend
{
    public static class Bot
    {
        public static string Prefix = "c!",Channel= "harbonator";
        public static List<int> OnMinutes = new List<int> { 30 };

        public static TwitchClient Client;
        public static void Start()
        {
            ConnectionCredentials credentials = new ConnectionCredentials("OwlCoinCommunistBot", "oauth:");
            Client = new TwitchClient();
            Client.Initialize(credentials,Channel);
            Client.OnMessageReceived += OnMessage.HandleMessage;
            Client.Connect();
            Console.WriteLine("Bot Started");
            CommunismSystem.LoadComrades();
            System.Threading.Thread.Sleep(1000);
            CommunismSystem.SignUpMessage();
            CommunismSystem.Timer();
        }

    }
}
