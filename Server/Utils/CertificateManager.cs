using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils
{
    public static class CertificateManager
    {
        public static X509Certificate2 GetCertificate()
        {
            byte[] certBytes;
            using (var certFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("Server.HanksiteCertificate.pfx"))
            using (var stream = new MemoryStream())
            {
                certFile.CopyTo(stream);
                certBytes = stream.GetBuffer();
            }

            return new X509Certificate2(certBytes, "bCt4h7SXSmFAzggR");
        }
    }
}
