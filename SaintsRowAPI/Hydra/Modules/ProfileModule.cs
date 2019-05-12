using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintsRowAPI.Hydra.DataTypes;
using SaintsRowAPI.Models;

namespace SaintsRowAPI.Hydra.Modules
{
    public class ProfileModule : IModule
    {
        private HydraConnection Connection;

        public ProfileModule(HydraConnection connection)
        {
            Connection = connection;
        }

        public void HandleRequest(HydraRequest request)
        {
            switch (request.Action)
            {
                case "get_by_platform_account_id":
                    {
                        GetByPlatformAccountId(request);
                        break;
                    }
                case "update":
                    {
                        Update(request);
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
         * 0: Steam ID (this is a SteamID3 - it is the same as a Steamworks AccountID)
         */
        private void GetByPlatformAccountId(HydraRequest request)
        {
            ulong steamId = ulong.Parse(request.Parameters[0]);

            HydraHashMap map = new HydraHashMap(new Dictionary<string, IHydraItem>()
                {
                    { "guid", new HydraInt64(1) },
                    { "vip_code", new HydraHashMap(new Dictionary<string, IHydraItem>() {
                        { "code", new HydraUtf8String("1234567890") },
                        { "accepted", new HydraBool(true) },
                        { "accepted_at", new HydraDateTime(DateTime.Now) }
                    })}
                });

            HydraResponse response = new HydraResponse(Connection, map);
            response.Send();
        }

        private void Create(HydraRequest request)
        {
            List<IHydraItem> items = HydraItemDeserializer.DeserializeAll(request.PostData);

            HydraResponse response = new HydraResponse(Connection, new HydraInt64(1));
            response.Send();
}

        private void Update(HydraRequest request)
        {
            List<IHydraItem> items = HydraItemDeserializer.DeserializeAll(request.PostData);

            HydraInt64 profileId = items[0] as HydraInt64;
            HydraHashMap map = items[1] as HydraHashMap;

            HydraResponse response = new HydraResponse(Connection, new HydraNone());
            response.Send();
        }
    }
}
