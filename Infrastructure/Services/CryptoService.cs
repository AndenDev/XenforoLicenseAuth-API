using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using Sodium;

namespace Infrastructure.Services
{
    public class CryptoService : ICryptoService
    {

        private const int NONCE_BYTES = 24;

        private readonly byte[] _key; 

        public CryptoService(IConfiguration config)
        {
            _key = Convert.FromBase64String(config["Encryption:Key"]);
        }
        public string Encrypt(string plainText)
        {
            var nonce = Sodium.SecretBox.GenerateNonce(); 

            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipher = Sodium.SecretBox.Create(plainBytes, nonce, _key);

            var combined = new byte[NONCE_BYTES + cipher.Length];
            Buffer.BlockCopy(nonce, 0, combined, 0, NONCE_BYTES);
            Buffer.BlockCopy(cipher, 0, combined, NONCE_BYTES, cipher.Length);

            return Convert.ToBase64String(combined);
        }

        public string Decrypt(string cipherText)
        {
            var combined = Convert.FromBase64String(cipherText);

            if (combined.Length < NONCE_BYTES)
                throw new Exception($"Invalid ciphertext length: too short for nonce.");

            var nonce = new byte[NONCE_BYTES];
            var cipher = new byte[combined.Length - NONCE_BYTES];

            Buffer.BlockCopy(combined, 0, nonce, 0, NONCE_BYTES);
            Buffer.BlockCopy(combined, NONCE_BYTES, cipher, 0, cipher.Length);

            var plainBytes = Sodium.SecretBox.Open(cipher, nonce, _key);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
