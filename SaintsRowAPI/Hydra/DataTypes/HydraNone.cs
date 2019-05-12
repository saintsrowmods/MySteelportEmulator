using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public class HydraNone : IHydraItem
    {
        public HydraDataType DataType
        {
            get { return HydraDataType.None; }
        }

        public void Serialize(System.IO.Stream s)
        {
        }

        public void Deserialize(System.IO.Stream s)
        {
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
            output.AppendFormatLine("{0}{1}", indentString, this.GetType().Name);
            return output.ToString();
        }
    }
}
