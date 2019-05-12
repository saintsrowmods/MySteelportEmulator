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
    public class Screenshot
    {
        private static string GetScreenshotFolder()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(path);

            string folder = Path.Combine(directory, "screenshots");
            Directory.CreateDirectory(folder);
            return folder;
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

        public byte[] ImageData { get; set; }

        public Screenshot(HydraHashMap map)
        {
            Data = map;
        }

        public Screenshot(Game game, HydraInt64 profileId, HydraHashMap metadata, HydraBinary imageData)
        {
            byte[] random = new byte[8];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
            }

            long newGuid = BitConverter.ToInt64(random, 0);
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
                { "meta_data", metadata },
                { "guid", new HydraInt64(newGuid) },
                { "owner_guid", new HydraInt64(1) },
                { "size", new HydraInt32(imageData.Value.Length) }
            });

            ImageData = imageData.Value;
        }

        public void Save()
        {
            string filename = String.Format("{0}.sr_screenshot", ID);
            string path = Path.Combine(GetScreenshotFolder(), filename);

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
