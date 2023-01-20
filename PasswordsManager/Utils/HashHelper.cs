using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordsManager.Utils
{
    class HashHelper
    {
        public static string SHA256(string input)
        {
            //SHA256.Create() returns an instance of SHA256 class, it picks the best implementation for current platform automatically,
            //like SHA256Managed, SHA256Cng, or SHA256CryptoServiceProvider.
            using (SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
