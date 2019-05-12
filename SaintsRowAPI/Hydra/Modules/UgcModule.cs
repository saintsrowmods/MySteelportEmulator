using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintsRowAPI.Hydra;
using SaintsRowAPI.Hydra.DataTypes;
using SaintsRowAPI.Models;

namespace SaintsRowAPI.Hydra.Modules
{
    public class UgcModule : IModule
    {
        private HydraConnection Connection;

        public UgcModule(HydraConnection connection)
        {
            Connection = connection;
        }

        public void HandleRequest(HydraRequest request)
        {
            switch (request.Action)
            {
                case "create":
                    {
                        Create(request);
                        break;
                    }
                case "list_selected_for_owner":
                    {
                        ListSelectedForOwner(request);
                        break;
                    }
                default:
                    {
                        request.DumpToFile();
                        break;
                    }
            }
        }

        private void Create(HydraRequest request)
        {
            List<IHydraItem> items = HydraItemDeserializer.DeserializeAll(request.PostData);

            HydraInt64 profileId = items[0] as HydraInt64;
            HydraHashMap map = items[1] as HydraHashMap;
            HydraBinary imageBinary = items[2] as HydraBinary;

            Game game = Game.GetFromApiKey(request.ApiKey);

            string category = ((HydraUtf8String)map.Items["category"]).Value;

            if (category == "characters")
            {
                Character character = new Character(game, profileId, map, imageBinary);
                character.Save();

                Console.WriteLine("Saved new character. ID {0}", character.ID);

                HydraResponse response = new HydraResponse(Connection, new HydraInt64(character.ID));
                response.Send();
            }
            else if (category == "screenshots")
            {
                Screenshot screenshot = new Screenshot(game, profileId, map, imageBinary);
                screenshot.Save();

                Console.WriteLine("Saved new screenshot. ID {0}", screenshot.ID);

                HydraResponse response = new HydraResponse(Connection, new HydraInt64(screenshot.ID));
                response.Send();
            }
            else
            {
                throw new Exception("Unknown category? " + category);
            }
        }

        private void ListSelectedForOwner(HydraRequest request)
        {
            int userId = int.Parse(request.Parameters[0]);
            int pageNo = int.Parse(request.Parameters[1]);
            int maxItems = int.Parse(request.Parameters[2]);
            string itemType = request.Parameters[3];

            var characters = Character.GetCharacters(pageNo, maxItems);

            Console.WriteLine("Sending characters to game (max {0}):", maxItems);

            HydraArray array = new HydraArray();
            foreach (Character character in characters)
            {
                Console.WriteLine("{0} - {1}", character.ID, character.Name);
                HydraHashMap map = new HydraHashMap(new Dictionary<string,IHydraItem>()
                    {
                        { "meta_data", new HydraHashMap(new Dictionary<string,IHydraItem>()
                            {
                                { "Name", new HydraUtf8String(character.Name) },
                                { "Avatar", new HydraBinary(character.AvatarData) },
                                { "Avatar Version", new HydraInt32(character.AvatarVersion) }
                            }
                        )}
                    });

                array.Items.Add(map);
            }

            Console.WriteLine();

            HydraResponse response = new HydraResponse(Connection, array);
            response.Send();
        }
    }
}
