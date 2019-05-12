using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.DataTypes
{
    public enum HydraDataType : byte
    {
        None = 1,
        Int32 = 2,
        Binary = 3,
        Bool = 4,
        UnsignedByte = 5,
        Float64 = 6,
        DateTime = 7,
        Array = 8,
        Struct = 9,
        Int64 = 10,
        Bitstruct = 11,
        Hashmap = 12,
        UnsignedInt64 = 13,
        Utf8String = 14
    }
}
