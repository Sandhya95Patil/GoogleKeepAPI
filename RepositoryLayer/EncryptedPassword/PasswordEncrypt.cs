using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.EncryptedPassword
{
    public class PasswordEncrypt
    {
        public static string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
    }
}
