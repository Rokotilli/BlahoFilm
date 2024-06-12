using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogicLayer.Services
{
    public class EncryptionHelper : IEncryptionHelper
    {
        private readonly byte[] ProtectKey;
        private readonly byte[] InitVector;

        public EncryptionHelper(IConfiguration configuration, IOptions<AppSettings> options)
        {
            ProtectKey = Encoding.UTF8.GetBytes(options.Value.Security.CookieProtectKey);
            InitVector = Encoding.UTF8.GetBytes(options.Value.Security.InitVectorKey);
        }

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = ProtectKey;
                aesAlg.IV = InitVector;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        byte[] encrypted = msEncrypt.ToArray();
                        string base64Encrypted = Convert.ToBase64String(encrypted);
                        return MakeUrlSafe(base64Encrypted);
                    }
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            string base64CipherText = MakeUrlUnsafe(cipherText);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = ProtectKey;
                aesAlg.IV = InitVector;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(base64CipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        private string MakeUrlSafe(string input)
        {
            return input.Replace('+', '-').Replace('/', '_').Replace("=", "");
        }

        private string MakeUrlUnsafe(string input)
        {
            string output = input.Replace('-', '+').Replace('_', '/');
            switch (output.Length % 4)
            {
                case 2: output += "=="; break;
                case 3: output += "="; break;
            }
            return output;
        }
    }
}
