using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public interface IHydraItem
    {
        HydraDataType DataType { get; }

        void Serialize(Stream s);
        void Deserialize(Stream s);

        string GetString();
        string GetString(int indentLevel);
    }
}
