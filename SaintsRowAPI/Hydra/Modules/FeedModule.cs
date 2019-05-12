using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintsRowAPI.Hydra.DataTypes;
using SaintsRowAPI.Models;

namespace SaintsRowAPI.Hydra.Modules
{
    public class FeedModule : IModule
    {
        private HydraConnection Connection;

        public FeedModule(HydraConnection connection)
        {
            Connection = connection;
        }

        public void HandleRequest(HydraRequest request)
        {
            switch (request.Action)
            {
                case "get_channels_by_owner":
                    {
                        GetChannelsByOwner(request);
                        break;
                    }
                case "get_items_by_channel":
                    {
                        GetItemsByChannel(request);
                        break;
                    }
                default:
                    {
                        request.DumpToFile();
                        break;
                    }
            }
        }

        /*
         * Parameters:
         * 0: channel owner
         */
        private void GetChannelsByOwner(HydraRequest request)
        {

            ChannelOwner owner = (ChannelOwner)int.Parse(request.Parameters[0]);

            Game game = Game.GetFromApiKey(request.ApiKey);
            Channel channel = Channel.ChannelGetByOwner(game, owner);

            HydraArray array = new HydraArray(new List<IHydraItem>()
                {
                    new HydraInt64(channel.ID),
                });

            HydraResponse response = new HydraResponse(Connection, array);
            response.Send();
        }

        /*
         * Parameters:
         * 0: channel id
         * 1: page number
         * 2: page size
         * 3: include non-published? 1 or 0
         */
        private void GetItemsByChannel(HydraRequest request)
        {
            int channelId = int.Parse(request.Parameters[0]);

            Channel channel = Channel.ChannelGetById(channelId);
            List<ChannelMessage> messages = ChannelMessage.GetMessagesForChannel(channel);

            HydraArray array = new HydraArray();
            foreach (ChannelMessage message in messages)
            {
                HydraHashMap map = new HydraHashMap();

                foreach (var str in message.Strings)
                {
                    map.Items.Add(str.Key, new HydraUtf8String(str.Value));
                }

                array.Items.Add(new HydraHashMap(new Dictionary<string, IHydraItem>() {
                        { "input", map }
                    }));
            }

            HydraResponse response = new HydraResponse(Connection, array);
            response.Send();
        }
    }
}
