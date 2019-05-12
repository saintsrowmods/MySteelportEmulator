using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public class HydraBool : IHydraItem
    {
        public bool Value { get; set; }
        
        public HydraBool()
        {
        }

        public HydraBool(bool value)
        {
            Value = value;
        }

        public HydraDataType DataType
        {
            get { return HydraDataType.Bool; }
        }

        public void Serialize(System.IO.Stream s)
        {
            s.WriteUInt8(Value ? (byte)1 : (byte)0);
        }

        public void Deserialize(System.IO.Stream s)
        {
            if (s.ReadUInt8() == 0)
                Value = false;
            else
                Value = true;
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
