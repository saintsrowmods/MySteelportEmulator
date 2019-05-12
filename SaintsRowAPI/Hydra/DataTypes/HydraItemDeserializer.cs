using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public static class HydraItemDeserializer
    {
        public static List<IHydraItem> DeserializeAll(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return DeserializeAll(ms);
            }
        }

        public static List<IHydraItem> DeserializeAll(Stream s)
        {
            List<IHydraItem> items = new List<IHydraItem>();

            while (s.Position != s.Length)
            {
                items.Add(Deserialize(s));
            }

            return items;
        }

        public static IHydraItem Deserialize(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Deserialize(ms);
            }
        }

        public static IHydraItem Deserialize(Stream s)
        {
            HydraDataType type = (HydraDataType)s.ReadUInt8();

            switch (type)
            {
                case HydraDataType.None:
                    {
                        HydraNone hNone = new HydraNone();
                        hNone.Deserialize(s);
                        return hNone;
                    }
                case HydraDataType.Int32:
                    {
                        HydraInt32 hInt32 = new HydraInt32();
                        hInt32.Deserialize(s);
                        return hInt32;
                    }
                case HydraDataType.Binary:
                    {
                        HydraBinary hBinary = new HydraBinary();
                        hBinary.Deserialize(s);
                        return hBinary;
                    }
                case HydraDataType.Bool:
                    {
                        HydraBool hBool = new HydraBool();
                        hBool.Deserialize(s);
                        return hBool;
                    }
                case HydraDataType.Float64:
                    {
                        HydraFloat64 hFloat64 = new HydraFloat64();
                        hFloat64.Deserialize(s);
                        return hFloat64;
                    }
                case HydraDataType.DateTime:
                    {
                        HydraDateTime hDateTime = new HydraDateTime();
                        hDateTime.Deserialize(s);
                        return hDateTime;
                    }
                case HydraDataType.Array:
                    {
                        HydraArray hArray = new HydraArray();
                        hArray.Deserialize(s);
                        return hArray;
                    }
                case HydraDataType.Int64:
                    {
                        HydraInt64 hInt64 = new HydraInt64();
                        hInt64.Deserialize(s);
                        return hInt64;
                    }
                case HydraDataType.Hashmap:
                    {
                        HydraHashMap hHashMap = new HydraHashMap();
                        hHashMap.Deserialize(s);
                        return hHashMap;
                    }
                case HydraDataType.Utf8String:
                    {
                        HydraUtf8String hUtf8String = new HydraUtf8String();
                        hUtf8String.Deserialize(s);
                        return hUtf8String;
                    }

                default:
                    throw new NotImplementedException(String.Format("Deserialization for {0} is not implemented!", type));
            }
        }
    }
}
