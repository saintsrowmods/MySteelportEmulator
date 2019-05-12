using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public class HydraDateTime : IHydraItem
    {
        private static uint UnixTimestampFromDateTime(DateTime date)
        {
            DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = date - epoch;

            return (uint)Math.Round(span.TotalSeconds);
        }

        private static DateTime DateTimeFromUnixTimestamp(uint timestamp)
        {
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            // Add the number of seconds in UNIX timestamp to be converted.
            dateTime = dateTime.AddSeconds((double)timestamp);

            return dateTime;
        }

        public DateTime Value { get; set; }

        public HydraDateTime()
        {
        }

        public HydraDateTime(DateTime value)
        {
            Value = value;
        }

        public HydraDataType DataType
        {
            get { return HydraDataType.DateTime; }
        }

        public void Serialize(System.IO.Stream s)
        {
            uint timestamp = UnixTimestampFromDateTime(Value);
            s.WriteUInt32(timestamp.Swap());
        }

        public void Deserialize(System.IO.Stream s)
        {
            Value = DateTimeFromUnixTimestamp(s.ReadUInt32().Swap());
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
