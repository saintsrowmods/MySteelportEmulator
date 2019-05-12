using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintsRowAPI.Hydra.DataTypes;

namespace SaintsRowAPI.Hydra
{
    public class HydraResponse
    {
        private HydraConnection Connection;
        public byte[] Payload { get; set; }

        public Dictionary<string, string> Headers = new Dictionary<string, string>();
        public string ContentType { get; set; }

        public int StatusCode { get; set; }
        public string Status { get; set; }

        public HydraResponse(HydraConnection connection) : this(connection, null)
        {
        }

        public HydraResponse(HydraConnection connection, IHydraItem item)
        {
            Connection = connection;

            ContentType = "application/x-hydra-binary";
            StatusCode = 200;
            Status = "OK";

            if (item != null)
                Payload = GetBytesForItem(item);
        }

        private byte[] GetBytesForItem(IHydraItem item)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.WriteUInt8((byte)item.DataType);
                item.Serialize(ms);

                return ms.ToArray();
            }
        }

        public void Send()
        {
            Headers.Add("Content-Type", ContentType);
            Headers.Add("Content-Length", Payload.Length.ToString());

            Connection.WriteStringAndCrLf("HTTP/1.1 {0} {1}", StatusCode, Status);
            foreach (var header in Headers)
            {
                Connection.WriteStringAndCrLf("{0}: {1}", header.Key, header.Value);
            }
            Connection.WriteStringAndCrLf("");

            Connection.WriteBytes(Payload);
        }
    }
}
