using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintsRowAPI.Hydra;
using SaintsRowAPI.Hydra.DataTypes;

namespace SaintsRowAPI.Models
{
    public class Character
    {
        private static string GetCharacterFolder()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(path);

            string folder = Path.Combine(directory, "characters");

            Directory.CreateDirectory(folder);

            return folder;
        }

        private static Character LoadCharacterFile(string characterFile)
        {
            using (Stream s = File.OpenRead(characterFile))
            {
                HydraHashMap map = new HydraHashMap();
                map.Deserialize(s);
                Character character = new Character(map);
                return character;
            }
        }

        public static List<Character> GetCharacters()
        {
            var characters = new List<Character>();

            if (!Directory.Exists(GetCharacterFolder()))
            {
                Directory.CreateDirectory(GetCharacterFolder());
            }

            string[] oldCharacterFiles = Directory.GetFiles(GetCharacterFolder(), "*.bin");

            foreach (string characterFile in oldCharacterFiles)
            {
                characters.Add(LoadCharacterFile(characterFile));
            }

            string[] newCharacterFiles = Directory.GetFiles(GetCharacterFolder(), "*.sr_character");

            foreach (string characterFile in newCharacterFiles)
            {
                characters.Add(LoadCharacterFile(characterFile));
            }

            return characters;
        }

        public static IEnumerable<Character> GetCharacters(int pageNo, int maxItems)
        {
            var available = GetCharacters();
            var toShow = available.Skip((pageNo - 1) * maxItems).Take(maxItems);

            return toShow;
        }

        private HydraHashMap Data;
        private HydraHashMap Metadata
        {
            get
            {
                return (HydraHashMap)Data.Items["meta_data"];
            }
        }

        public long ID
        {
            get
            {
                return ((HydraInt64)Data.Items["guid"]).Value;
            }
        }

        public string Name
        {
            get
            {
                return ((HydraUtf8String)Metadata.Items["Name"]).Value;
            }
            set
            {
                ((HydraUtf8String)Metadata.Items["Name"]).Value = value;
            }
        }

        public byte[] AvatarData
        {
            get
            {
                return ((HydraBinary)Metadata.Items["Avatar"]).Value;
            }
        }

        public int AvatarVersion
        {
            get
            {
                return int.Parse(((HydraUtf8String)Metadata.Items["Avatar Version"]).Value);
            }
        }

        public byte[] ImageData { get; set; }

        public Character(HydraHashMap map)
        {
            Data = map;
        }

        private HydraInt64 GenerateGuid()
        {
            byte[] random = new byte[8];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
            }
            long newGuid = BitConverter.ToInt64(random, 0);
            return new HydraInt64(newGuid);
        }

        public Character(Game game, HydraInt64 profileId, HydraHashMap metadata, HydraBinary imageData)
        {
            metadata.Items["Avatar Version"] = new HydraUtf8String(((HydraInt32)metadata.Items["Avatar Version"]).Value.ToString());
            
            Data = new HydraHashMap(new Dictionary<string, IHydraItem>()
            {
                { "emu", new HydraBool(true) },
                { "rating", new HydraInt32(0) },
                { "tags", new HydraArray() },
                { "url", new HydraUtf8String("") },
                { "created_at", new HydraDateTime(DateTime.Now) },
                {
                    "rating_history",
                    new HydraHashMap(new Dictionary<string, IHydraItem>()
                    {
                        {
                            "monthly",
                            new HydraHashMap(new Dictionary<string, IHydraItem>()
                            {
                                { "count", new HydraInt32(0) },
                                { "sum", new HydraInt32(0) },
                                { "average", new HydraInt32(0) },
                            })
                        },
                        {
                            "daily",
                            new HydraHashMap(new Dictionary<string, IHydraItem>()
                            {
                                { "count", new HydraInt32(0) },
                                { "sum", new HydraInt32(0) },
                                { "average", new HydraInt32(0) },
                            })
                        },
                        {
                            "weekly",
                            new HydraHashMap(new Dictionary<string, IHydraItem>()
                            {
                                { "count", new HydraInt32(0) },
                                { "sum", new HydraInt32(0) },
                                { "average", new HydraInt32(0) },
                            })
                        }
                    })
                },
                { "queue_count", new HydraInt32(0) },
                { "meta_data", metadata },
                { "guid", GenerateGuid() },
                { "owner_guid", new HydraInt64(1) },
                { "size", new HydraInt32(imageData.Value.Length) }
            });

            ImageData = imageData.Value;
        }

        private string GenerateFileName()
        {
            string filename = String.Format("{0}.sr_character", ID);
            string path = Path.Combine(GetCharacterFolder(), filename);

            return path;
        }

        public void Save()
        {
            string path = GenerateFileName();

            while (File.Exists(path))
            {
                path = GenerateFileName();
            }
            
            using (Stream s = File.Create(path))
            {
                Data.Serialize(s);
            }

            if (ImageData != null)
            {
                string imagePath = Path.ChangeExtension(path, ".jpg");
                File.WriteAllBytes(imagePath, ImageData);
            }
        }
    }
}
