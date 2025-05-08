using NSec.Cryptography;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class Ed25519SigningService : IEd25519SigningService
    {
        private readonly Key _privateKey;
        public Ed25519SigningService(IConfiguration config)
        {
            var privateKeyHex = config["Ed25519:PrivateKey"];
            var privateKeyBytes = HexToBytes(privateKeyHex);

            var creationParameters = new KeyCreationParameters
            {
                ExportPolicy = KeyExportPolicies.AllowPlaintextArchiving
            };

            _privateKey = Key.Import(SignatureAlgorithm.Ed25519, privateKeyBytes, KeyBlobFormat.RawPrivateKey, creationParameters);
        }

        public string Sign(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var signature = SignatureAlgorithm.Ed25519.Sign(_privateKey, messageBytes);
            var signatureHex = BitConverter.ToString(signature).Replace("-", "").ToLower();

            return signatureHex;
        }
        private static byte[] HexToBytes(string hex)
        {
            return Enumerable.Range(0, hex.Length / 2)
                             .Select(i => Convert.ToByte(hex.Substring(i * 2, 2), 16))
                             .ToArray();
        }
    }
}
