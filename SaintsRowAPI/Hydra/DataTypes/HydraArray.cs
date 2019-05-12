using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public class HydraArray : IHydraItem
    {
        public List<IHydraItem> Items = new List<IHydraItem>();

        public HydraArray()
        {
        }

        public HydraArray(List<IHydraItem> items)
        {
            Items = items;
        }

        public HydraDataType DataType
        {
            get { return HydraDataType.Array; }
        }

        public void Serialize(System.IO.Stream s)
        {
            s.WriteInt32(Items.Count.Swap());
            foreach (IHydraItem item in Items)
            {
                s.WriteUInt8((byte)item.DataType);
                item.Serialize(s);
            }
        }

        public void Deserialize(System.IO.Stream s)
        {
            int itemCount = s.ReadInt32().Swap();
            for (int i = 0; i < itemCount; i++)
            {
                IHydraItem item = HydraItemDeserializer.Deserialize(s);
                Items.Add(item);
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
            foreach (IHydraItem item in Items)
            {
                output.AppendFormat(item.GetString(indentLevel + 1));
            }

            return output.ToString();
        }
    }
}
