using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra
{
    public class HydraRequest
    {
        private HydraConnection Connection;

        public Dictionary<string, string> Headers { get; private set; }

        public string HttpMethod { get; private set; }

        public string Uri { get; private set; }

        public byte[] PostData { get; private set; }

        public string ApiKey { get; private set; }
        public string Module { get; private set; }
        public string Action { get; private set; }
        public string[] Parameters { get; private set; }

        public HydraRequest(HydraConnection connection)
        {
            Connection = connection;

            ParseRequest();
        }

        private void ParseUri(string uri)
        {
            string[] pieces = uri.Split(new char[] { '/' }, StringSplitOptions.None);
            ApiKey = pieces[1];
            Module = pieces[2];
            Action = pieces[3];

            if (pieces.Length > 4)
            {
                Parameters = new string[pieces.Length - 4];
                Array.Copy(pieces, 4, Parameters, 0, pieces.Length - 4);
            }
            else
            {
                Parameters = null;
            }
        }

        private void ParseRequest()
        {
            string headerLine = Connection.ReadStringToCrLf();

            string[] linePieces = headerLine.Split(' ');

            HttpMethod = linePieces[0].ToUpperInvariant();
            Uri = linePieces[1];
            string version = linePieces[2];

            ParseUri(Uri);

            Headers = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            headerLine = Connection.ReadStringToCrLf();
            while (headerLine != "")
            {
                string[] headerPieces = headerLine.Split(':');
                Headers.Add(headerPieces[0].Trim(), headerPieces[1].Trim());
                headerLine = Connection.ReadStringToCrLf();
            }

            PostData = null;

            if (HttpMethod == "POST")
            {
                string contentLengthHeader = Headers["Content-Length"];
                long contentLength = long.Parse(contentLengthHeader);
                PostData = Connection.ReadBytes(contentLength);
            }
        }

        public void DumpToFile()
        {
            if (PostData != null)
            {
                List<Hydra.DataTypes.IHydraItem> items = Hydra.DataTypes.HydraItemDeserializer.DeserializeAll(PostData);

                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in items)
                {
                    sb.AppendFormat("{0}: {1}", i, item.GetString());
                    i++;
                }
                string data = sb.ToString();
                System.IO.File.WriteAllText(String.Format("{0}_{1}_{2}-{3}-{4}_{5}-{6}-{7}.txt", Uri.Replace("/", "_"), Connection.IPAddress, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), data);
            }
        }
    }
}
