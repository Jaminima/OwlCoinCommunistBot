using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using System.Net;
using System.Threading;

namespace TwitchBot.Backend
{
    public static class CommunismSystem
    {
        static List<string> Comrades = new List<string> { };

        static void SaveComrades()
        {
            System.IO.File.WriteAllLines("./Comrades.info",Comrades.ToArray<String>());
        }

        public static void LoadComrades()
        {
            if (System.IO.File.Exists("./Comrades.info"))
            { Comrades = System.IO.File.ReadAllLines("./Comrades.info").ToList<String>(); }
        }

        public static void AddComrade(OnMessageReceivedArgs e)
        {
            if (Comrades.Contains(e.ChatMessage.Username))
            { Bot.Client.SendMessage(e.ChatMessage.Channel,"@"+e.ChatMessage.Username+" You are already part of the regime comrade!"); }
            else
            {
                Comrades.Add(e.ChatMessage.Username);
                SaveComrades();
                Bot.Client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + " Welcome comrade to the regime!");
            }
        }

        public static void RemoveComrade(OnMessageReceivedArgs e)
        {
            if (Comrades.Contains(e.ChatMessage.Username))
            {
                Comrades.Remove(e.ChatMessage.Username);
                SaveComrades();
                Bot.Client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + " comrade you have left the regime!");
            }
            else { Bot.Client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + " You aren't part of the regime!"); }
        }

        public static void CountComrades(OnMessageReceivedArgs e)
        {
            Bot.Client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + " There are " + Comrades.Count + " comrades participating in the redistribution!");
        }

        public static void TimeTillRedistribution(OnMessageReceivedArgs e)
        {
            Bot.Client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + " There is " + TimeLeft() + " minute(s) untill the next redistribution!");
        }

        static int TimeLeft()
        {
            int TimeLeft = 1000;
            foreach (int Minute in Bot.OnMinutes)
            {
                int Remaining = Minute - DateTime.Now.Minute;
                if (Remaining < 0) { Remaining += 60; }
                if (Remaining >= 0 && Remaining < TimeLeft)
                {
                    TimeLeft = Remaining;
                }
            }
            return TimeLeft;
        }

        public static void Timer()
        {
            while (true)
            {
                if (TimeLeft() == 0)
                {
                    Redistributer();
                }
                else if ((DateTime.Now.Minute+15)%30==0)
                {
                    SignUpMessage();
                }
                System.Threading.Thread.Sleep(20000);
            }
        }

        public static int GetOwlCoin()
        {
            int MyOwlCoin = int.Parse(Newtonsoft.Json.Linq.JObject.Parse(
                Encoding.UTF8.GetString(
                    new WebClient().DownloadData("https://api.streamelements.com/kappa/v2/points/5ae2745c81718b608487d75a/owlcoincommunistbot"))
                )["points"].ToString());
            return MyOwlCoin;
        }

        public static void SignUpMessage()
        {
            Bot.Client.SendMessage(Bot.Channel, "Remember to signup for Owlcoin redistribution by typing \""+Bot.Prefix+"communism\" and make sure to donate Owlcoin or it cant be redistributed.");
        }

        public static void ForceRedistribute(OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Broadcaster ||
                e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Moderator ||
                e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Admin ||
                e.ChatMessage.Username == "jccjaminima")
            { new Thread(() => Redistributer()).Start(); }
            else
            { Bot.Client.SendMessage(e.ChatMessage.Channel,"@"+e.ChatMessage.Username+" you don't have the required perms!"); }
        }
        static Random Rnd = new Random();
        static void Redistributer()
        {
            Quote();
            System.Threading.Thread.Sleep(10000);

            if (Comrades.Count == 0) { Bot.Client.SendMessage(Bot.Channel,"There are no comrades to redistibute too!"); SignUpMessage(); return; }
            int OwlCoin = GetOwlCoin();
            if (OwlCoin == 0) { Bot.Client.SendMessage(Bot.Channel, "There's no owlcoin to redistibute!"); SignUpMessage(); return; }
            
            int OCPerComrade = (int)Math.Floor((decimal)OwlCoin / Comrades.Count);
            if (OCPerComrade>2000) { OCPerComrade = 2000; }
            foreach (string Comrade in Comrades)
            {
                Bot.Client.SendMessage(Bot.Channel, "!giveowlcoin " + Comrade + " " + OCPerComrade);
                System.Threading.Thread.Sleep(5000);
            }
            SignUpMessage();
            System.Threading.Thread.Sleep(60000);
        }

        public static void RedistributionDetails(OnMessageReceivedArgs e)
        {
            Bot.Client.SendMessage(e.ChatMessage.Channel, "@"+e.ChatMessage.Username+" The next redistribution will be in "+TimeLeft()+" minute(s) and has "+GetOwlCoin()+" Owlcoin to distribute to "+Comrades.Count+" comrades!");
        }

        public static void Quote()
        {
            string[] Quotes = System.IO.File.ReadAllLines("./Quotes.info");
            Bot.Client.SendMessage(Bot.Channel, Quotes[Rnd.Next(0, Quotes.Length - 1)]);
        }

    }
}
