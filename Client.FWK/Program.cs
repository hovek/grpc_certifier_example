using Certify;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Client.FWK
{
    class Program
    {
        static void Main(string[] args)
        {
            // The server will return 403 (Forbidden). The method requires a certificate
            //CallCertificateInfo(includeClientCertificate: false).Wait();

            // The server will return a successful gRPC response
            CallCertificateInfo(includeClientCertificate: true).Wait();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task CallCertificateInfo(bool includeClientCertificate)
        {
            try
            {
                Console.WriteLine($"Setting up HttpClient. Client has certificate: {includeClientCertificate}");
                var client = new Certifier.CertifierClient(GetChannel(includeClientCertificate));

                Console.WriteLine("Sending gRPC call...");
                var certificateInfo = await client.GetCertificateInfoAsync(new Empty());

                Console.WriteLine($"Server received client certificate: {certificateInfo.HasCertificate}");
                if (certificateInfo.HasCertificate)
                {
                    Console.WriteLine($"Client certificate name: {certificateInfo.Name}");
                }
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"gRPC error from calling service: {ex.Status.Detail}");
            }
            catch
            {
                Console.WriteLine($"Unexpected error calling service.");
                throw;
            }
        }

        private static Channel GetChannel(bool includeClientCertificate)
        {
            ChannelCredentials credentials;

            if (includeClientCertificate)
            {
                var cert = new X509Certificate2(@"Certs\client.pfx", "1111");
                credentials = new SslCredentials(ExportToPEM(cert));
            }
            else
                credentials = ChannelCredentials.Insecure;

            return new Channel("127.0.0.1", 5001, credentials);
        }

        public static string ExportToPEM(X509Certificate cert)
        {
            var certBytes = cert.Export(X509ContentType.Cert);
            return ExportToPEM(certBytes);
        }

        public static string ExportToPEM(byte[] certBytes)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(certBytes, Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }

    }
}
