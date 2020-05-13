using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TestTool.Transport.Security
{
    class UsernameTokenValidator
    {
        public static bool Validate(UsernameToken token)
        {
            Account account = UsersList.Current.GetUser(token.Username);
            if (account == null)
            {
                return false;
            }
            else
            {
                string password = account.Password;
                string nonce = token.Nonce;
                string timestamp = token.Created.ToString("yyyy-MM-ddTHH:mm:ssZ");

                string passwordHash = GetPasswordHash(nonce, timestamp, password);
                return passwordHash == token.Password.Value;
            }
        }


        protected static string GetPasswordHash(string nonce, string timestamp, string password)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            List<byte> data = new List<byte>();
            data.AddRange(System.Convert.FromBase64String(nonce));
            data.AddRange(System.Text.Encoding.UTF8.GetBytes(timestamp + password));
            byte[] hash = algorithm.ComputeHash(data.ToArray());
            return System.Convert.ToBase64String(hash);
        }
    }
}
