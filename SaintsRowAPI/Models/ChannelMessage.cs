using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Models
{
    public class ChannelMessage
    {
        public int ID { get; set; }
        public Channel Channel { get; set; }
        public bool Active { get; set; }
        public DateTime Starts { get; set; }
        public DateTime Ends { get; set; }
        public Dictionary<string, string> Strings { get; set; }

        public ChannelMessage()
        {
            Strings = new Dictionary<string, string>();
        }

        public static List<ChannelMessage> GetMessagesForChannel(Channel channel)
        {
            if (channel.Owner == ChannelOwner.GameAll)
            {
                string[] strings = new string[]
                {
                    "Connected to My Steelport emulator!",
                    "Log in with any username and password.",
                };

                string[] languages = new string[]
                {
                    "US", "ES", "IT", "JP", "DE", "FR", "NL", "SE", "DK", "CZ", "PL", "SK", "RU", "CH"
                };
                List<ChannelMessage> messages = new List<ChannelMessage>();

                int i = 0;
                foreach (string str in strings)
                {
                    i++;

                    ChannelMessage message = new ChannelMessage();
                    message.ID = i;

                    foreach (string language in languages)
                    {
                        message.Strings.Add(language, str);
                    }

                    messages.Add(message);
                }

                return messages;
            }
            else
            {
                return new List<ChannelMessage>();
            }
        }
    }
}
