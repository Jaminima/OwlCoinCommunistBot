using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace TwitchBot.Backend
{
    public static class Details
    {
        public static void BotBalance(OnMessageReceivedArgs e)
        {
            Bot.Client.SendMessage(e.ChatMessage.Channel, "The bot currently has " + CommunismSystem.GetOwlCoin() + " OwlCoin.");
        }

        public static void Help(OnMessageReceivedArgs e)
        {
            string[] Help = new string[] {
                "Commands are available here: https://pastebin.com/njaDhTTG",
                "This bot was made by twitch.tv/jccjaminima"
            };
            foreach (string Line in Help)
            { Bot.Client.SendWhisper(e.ChatMessage.Username, Line); }
        }
    }
}
