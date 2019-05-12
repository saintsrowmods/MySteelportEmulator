using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Models
{
    public class Channel
    {
        public int ID { get; set; }
        public Game Game { get; set; }
        public ChannelOwner Owner { get; set; }
        public string Name { get; set; }

        public static Channel ChannelGetById(int channelId)
        {
            return new Channel()
            {
                ID = channelId,
                Owner = (ChannelOwner)channelId,
            };
        }

        public static Channel ChannelGetByOwner(Game game, ChannelOwner owner)
        {
            return new Channel()
            {
                ID = (int)owner,
                Game = game,
                Owner = owner,
                Name = owner.ToString()
            };
        }
    }
}
