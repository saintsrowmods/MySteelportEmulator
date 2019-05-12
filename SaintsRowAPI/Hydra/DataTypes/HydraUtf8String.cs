using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public class HydraUtf8String : IHydraItem
    {
        public static Encoding UTF8 = new UTF8Encoding(false);

        public string Value { get; set; }

        public HydraUtf8String()
        {
            Value = "";
        }

        public HydraUtf8String(string value)
        {
            Value = value;
        }

        public HydraDataType DataType
        {
            get { return HydraDataType.Utf8String; }
        }

        public void Serialize(System.IO.Stream s)
        {
            byte[] utf8Bytes = UTF8.GetBytes(Value);
            s.WriteInt32(utf8Bytes.Length.Swap());
            s.Write(utf8Bytes, 0, utf8Bytes.Length);
        }

        public void Deserialize(System.IO.Stream s)
        {
            int length = s.ReadInt32().Swap();
            byte[] utf8Bytes = new byte[length];
            s.Read(utf8Bytes, 0, length);
            Value = UTF8.GetString(utf8Bytes);
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
            output.AppendFormatLine("{0}{1}: '{2}'", indentString, this.GetType().Name, Value);
            return output.ToString();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
