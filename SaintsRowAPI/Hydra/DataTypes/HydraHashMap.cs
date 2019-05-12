using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public class HydraHashMap : IHydraItem
    {
        public Dictionary<string, IHydraItem> Items = new Dictionary<string, IHydraItem>();

        public HydraHashMap()
        {
        }

        public HydraHashMap(Dictionary<string, IHydraItem> items)
        {
            Items = items;
        }

        public HydraDataType DataType
        {
            get { return HydraDataType.Hashmap; }
        }

        public void Serialize(System.IO.Stream s)
        {
            s.WriteInt32(Items.Count.Swap());
            foreach (var item in Items)
            {
                HydraUtf8String keyString = new HydraUtf8String(item.Key);
                s.WriteUInt8((byte)HydraDataType.Utf8String);
                keyString.Serialize(s);
                s.WriteUInt8((byte)item.Value.DataType);
                item.Value.Serialize(s);
            }
        }

        public void Deserialize(System.IO.Stream s)
        {
            int itemCount = s.ReadInt32().Swap();
            for (int i = 0; i < itemCount; i++)
            {
                IHydraItem key = HydraItemDeserializer.Deserialize(s);
                if (!(key is HydraUtf8String))
                    throw new NotImplementedException();
                HydraUtf8String stringKey = key as HydraUtf8String;

                IHydraItem value = HydraItemDeserializer.Deserialize(s);

                Items.Add(stringKey.Value, value);
            }
        }

        public string GetString()
        {
            return GetString(0);
        }

        public string GetString(int indentLevel)
        {
            string indentString = "";
            for (int i = 0; i < indentLevel; i++)
                indentString += "  ";

            StringBuilder output = new StringBuilder();
            output.AppendFormatLine("{0}{1}:", indentString, this.GetType().Name);

            foreach (var pair in Items)
            {
                output.AppendFormat("{0}  '{1}': {2}", indentString, pair.Key, pair.Value.GetString(indentLevel + 2));
            }

            return output.ToString();
        }
    }
}
