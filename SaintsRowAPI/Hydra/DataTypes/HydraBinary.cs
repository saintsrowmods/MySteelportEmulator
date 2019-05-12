using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public class HydraBinary : IHydraItem
    {
        public byte[] Value { get; set; }

        public HydraBinary()
        {
        }

        public HydraBinary(byte[] value)
        {
            Value = value;
        }
        

        public HydraDataType DataType
        {
            get { return HydraDataType.Binary; }
        }

        public void Serialize(System.IO.Stream s)
        {
            s.WriteInt32(Value.Length.Swap());
            s.Write(Value, 0, Value.Length);
        }

        public void Deserialize(System.IO.Stream s)
        {
            int length = s.ReadInt32().Swap();
            Value = new byte[length];
            s.Read(Value, 0, length);
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
            output.AppendFormatLine("{0}{1}: {2}", indentString, this.GetType().Name, Convert.ToBase64String(Value));
            return output.ToString();
        }
    }
}
