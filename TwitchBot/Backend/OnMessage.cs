using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace TwitchBot.Backend
{
    public static class OnMessage
    {
        public static void HandleMessage(object sender, OnMessageReceivedArgs e)
        {
            List<string> SegmentedString = e.ChatMessage.Message.Split(" ".ToCharArray()).ToList<String>();
            string Command = SegmentedString[0].ToLower();

            Console.WriteLine("<"+e.ChatMessage.Username+"> "+e.ChatMessage.Message);

            if (Command.StartsWith(Bot.Prefix))
            {
                Command = Command.Remove(0, Bot.Prefix.Length);

                if (Command == "communism")
                { CommunismSystem.AddComrade(e); }
                if (Command == "capitalism")
                { CommunismSystem.RemoveComrade(e); }
                if (Command == "timeleft")
                { CommunismSystem.TimeTillRedistribution(e); }
                if (Command == "owlcoin"||Command=="balance")
                { Details.BotBalance(e); }
                if (Command == "comrades")
                { CommunismSystem.CountComrades(e); }
                if (Command == "details")
                { CommunismSystem.RedistributionDetails(e); }
                if (Command == "redistribute")
                { CommunismSystem.ForceRedistribute(e); }
                if (Command == "quote")
                { CommunismSystem.Quote(); }
                if (Command == "help")
                { Details.Help(e); }
            }

            if (e.ChatMessage.Message.Contains("a Raffle has begun for")||e.ChatMessage.Message.Contains("a Multi-Raffle has begun for") &&e.ChatMessage.Username== "streamelements")
            {
                Bot.Client.SendMessage(e.ChatMessage.Channel, "!join");
            }

            if (e.ChatMessage.Message.Contains("Owlcoin to owlcoincommunistbot") && e.ChatMessage.Username == "streamelements")
            {
                Bot.Client.SendWhisper(SegmentedString[0], "Thankyou for donating Owlcoin to the regime!");
            }

        }
    }
}
