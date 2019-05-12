using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using AaltoTLS;
using AaltoTLS.HandshakeLayer;
using AaltoTLS.PluginInterface;

namespace SaintsRowAPI
{
    public static class Certificates
    {
        public static X509Certificate Certificate { get; private set; }
        public static CertificatePublicKey PublicKey { get; private set; }
        public static CertificatePrivateKey PrivateKey { get; private set; }

        public static SecurityParameters SecurityParameters { get; private set; }

        private static string Pfx = @"MIIJEQIBAzCCCNcGCSqGSIb3DQEHAaCCCMgEggjEMIIIwDCCA3cGCSqGSIb3DQEHBqCCA2gwggNkAgEAMIIDXQYJKoZIhvcNAQcBMBwGCiqGSIb3DQEMAQYwDgQINxwuKtJah4gCAggAgIIDMCamXwzukvxUdvghVim6Mp6U37fhNY3PBzGSKqrsZo2mcdGyFH/+uKjGDkn5ta921qMN8lW2N14/TBXASEp31exOJeclbubRLDOCXYi5j7mFaoR/SRJdp5DofT2elz0CzYBZV3B07IfYPO55kgsPLIJ/1iCtoNrMrvQQuzX+EbkpaSzXvfZmGkLPM2kmHHj/KrE1SYvqdEGGY+IOQGeom69SDhyl8SKzr3vJ1m6kKNnS1vCtxUojA32CMIsvejdp3FBA6/55XKKpKaQ//AmzNjNAaPqZwJ7W4QBxWTMAwVd/EyXG5fdUui1wpoNiG0idunbxe3WI9FK5kG3Sgxa9WqoTeVqbPMFMrOzmQ5tBC/1wwx6utoat8SiyCGTd0d138bCYl/wcSIAETlxJ8UGAUnYjiii+IRf116mU2gN9wJx1S1YRAMFYwM0Uy55lfYcKjPwvez2BeQf8wygaeMzf/ZcdCAgpIFUP4tfvjj5fcbsBh6ixPlW0jpDM8kUi0ZtmHpKRaIP74WyE+vI73vZ+LlInEtcNZ+D3EH//ShjGWxvA7T1v8DDIuEcvzT4ZvCkhbyN/V2BwRXEc4ETbk6zMHKWefL3FGZIB5N3B+dRik0jSb0/i4JRo5i9Gz7cjj2gdBziwLuuA59zmShtG6JosDIyqwZIQwVwyHmUiM9dJLYJtwG+/0u8c8IKQcS1P2ok1naOEdrlgPpWhj21ptPeWkll+VaCbbxIPJjZEwTZZOn2zO1ivZu4o8VXT7dK6QgwIZlM3p01fLQV0+7aaBOn3nqY31LTkqP21nzy0PVIv5522YiugaQMzynR6cDhRPkwfLgAz2m1Ytde7QAOu31ZpcRe412yc1S6d0SyifGYD1wp3WjdWw8aJ8yIzWSlNwCqglFhNIHFiAV+hSVTxEVVtLYRvefF06vWKoW7WhChetYw/gGbxn58t7W3qgi9GgNOdphlOUvEq8D8SoybzCKQDJLeu8RbF0cAtvH2g5BERHD5DlCrtSCkFYDgv/Ob5mK9NgGD0+1Z8eRaiv+tzWmPZdOiZT2Jpn7N6RtXcP+MGrLRWQe6mOyPfxxex1ZQNB8unLTCCBUEGCSqGSIb3DQEHAaCCBTIEggUuMIIFKjCCBSYGCyqGSIb3DQEMCgECoIIE7jCCBOowHAYKKoZIhvcNAQwBAzAOBAgGjp8rtdFWzwICCAAEggTIT9bJQHM2xnmq8HPXne/UdbD2aQXvZygaW+opDHLzH3uM53jINYMqkaphnJNbMjjSZ/ADy/Du988HRucFdfOKzkJIqIeAmQXc9uYjGrCaqvvHx3TGPKu8v5tpfWyNLpXPR92CKAJXuJe4KB135rUlvf/W7rBihpsaPn5QlFH1pha77CxmXh9GcW5L75lmaTl8drYhXRATRnj2i1tXuglb9kYf2j+SDIwNE0d0u5rO+pPr0Lf0NvN+eQrTTxUerA2Tbz6txDenZjW3DjUCB8Ay25TylqNnKpdsw3M8gHN5eER8w5Y5NpxTrZrnjICoDY1LLl60fGO8/jTwFW4Rhx+t6J+vo9j8X05SoQRZeswhzQZk5moBrSFEN4fEOrf3WUMRwKNZtLv30sv3CDRQDha3EFreMGP3TsrpxLdZQLFe0O+2/N516tip77TsX1NOfGi2oHMwDmX+GFT1SxYg5chPe5ih78OlDNeSDoaWT+f5nk+Rn86gmLaMLRb/MnKeC69Rwm9C09P9RiavfCMDJGGjSdMv3IYD8GdWi8Ldna/eHEjCNd+iWQFLV3zOuRwgTwqlwXOvGTuzu3N4CmgJUXAg1v/Fy4FQLKIqbsgX/MhexG2LjRydaHAGVWYVen0rONJdtDIu6wTWnf0UYQeBgvYCBgQEehp4pItELol62FIp0wrpPdtJH4odQ6JF4bAFyQmYCrZ/NQWQX/qU8bsiP7L6Y+IDc+3RUI7yil5Xt8hAUOxdvriWaiLV0ZTqcuvumj9UsAtBlhpm9ZLVoEzyFk0YcSaHA+Urt0jgD7eXTe2Bq1JdYZw2vwC43z9VaHEagOUilnmt/mayepFXXGCm1mmuYNH4s6zwVigAMM8h916U+8MPwqk3IFfmEpES4fuuG4muGk0FjxMmJglxfjWjXybg0q3Gi0Qu9ESsLTTZmc3C6/6jdtFJZCT/Rnrc1aGmt3rX3GibNqC3BzssAh8BAF7MJCUhODwiI102gLvPpnRMw/tuOYWnn/FuG5vvQwOlo/OXIculVvXeeOjf13wp8vtQEQ0gWShIo9PSNjjqF21RoCcD1MKUvzZXzek8MXYXMV2ZF8F20jZaFgwgX8FgEtjq7xE6N8EyO+3W9li3xf4LK8Qq48jx7ntxSBlBk6xVfat2WWuxkKfWfmyDQa302ZMFvsXlopwhswJdvCug0iWzUMQ1ncKESuTp78sMCG/ndKzO07Oiu0qYgi9Ldq9py8hJyJ/lr74W+D0r+XfqPf2ZfjXkSFkE62F9jYcwMZq/nSjQ0du5GUB8BsgtYG/YqRQPFSTiq8Qw7JHy0Dbaf71EUfTxoe2esp1zU3UHlzg/icNvC/pS943Lxnn4xSntOMs40+b8HlkJ6RydGa6Y9hLjReczCiipm2nBi01p4/P2ZTT6zM1SS+VbyNJlB6gsIV0Pta4jN4wQ+OEsjDktIunD/JsuHVgbQt625SZqB1TnP13mARFwVKNFG/QG/ghFhybEE7P/u80yVnk5waQopmu9YbaofOa0Si0hN6r5EbJN/tldp+oAkHhmACl8z+VaVn20fs9o3X8drxzivm5pwy3y03BY+pQoDwfD1QEjfxN5AlmSd6pz+wtcbR03FDwQimd0UlIM9mnDoxHNMSUwIwYJKoZIhvcNAQkVMRYEFErWkX1pFE1eDY12X8ojC0VEOsgZMDEwITAJBgUrDgMCGgUABBSkOBVjSFr7cDJ4L3sXWmHt0toNZwQI6vF7QniHaY8CAggA";

        public static void Load()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
	        string directory = Path.GetDirectoryName(path);
			CipherSuitePluginManager pluginManager = new CipherSuitePluginManager(directory);

            X509Certificate2 cert = new X509Certificate2(Convert.FromBase64String(Pfx), "temp", X509KeyStorageFlags.Exportable);
            PublicKey = new CertificatePublicKey(cert);
            PrivateKey = new CertificatePrivateKey("1.2.840.113549.1.1.1", cert.PrivateKey);
            SecurityParameters = new SecurityParameters()
            {
                MaximumVersion = ProtocolVersion.TLS1_0
            };
            SecurityParameters.CipherSuiteIDs.Add(0x0039); // TLS_DHE_RSA_WITH_AES_256_CBC_SHA
            SecurityParameters.AddCertificate(new X509CertificateCollection(new X509Certificate[] { cert }), PrivateKey);

            Certificate = cert; 
        }
    }
}
