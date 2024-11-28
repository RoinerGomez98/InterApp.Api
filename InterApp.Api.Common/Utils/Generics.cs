using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace InterApp.Common.Utils
{
    public class Generics<T>
    {
        public T? Request { get; set; }
        public bool IsNew { get; set; }

    }
    public class GenericResponse<T>
    {
        public string? Message { get; set; }
        public HttpStatusCode Status { get; set; }
        public T? Result { get; set; } = default;
    }

    public class Security
    {
        private readonly IConfiguration _config;
        public Security(IConfiguration config)
        {
            _config = config;
        }
        public string EncryptP(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_config["Security:Key"]!.ToString().Trim());
                aes.IV = iv;
                using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }
                array = memoryStream.ToArray();
            }
            return Convert.ToBase64String(array);
        }
    }
}
