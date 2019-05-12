using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public class HydraInt64 : IHydraItem
    {
        public long Value { get; set; }

        public HydraInt64()
        {
        }

        public HydraInt64(long value)
        {
            Value = value;
        }

        public HydraDataType DataType
        {
            get { return HydraDataType.Int64; }
        }

        public void Serialize(System.IO.Stream s)
        {
            s.WriteInt64(Value.Swap());
        }

        public void Deserialize(System.IO.Stream s)
        {
            Value = s.ReadInt64().Swap();
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
            output.AppendFormatLine("{0}{1}: {2}", indentString, this.GetType().Name, Value);
            return output.ToString();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
