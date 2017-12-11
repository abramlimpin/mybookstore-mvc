using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace MyBookstoreMVC.App_Code
{
    public class Helper
    {
        /// <summary>
        /// Returns existing connection string value found inside web.config.
        /// </summary>
        /// <returns>Connection string value of MyCon</returns>
        public static string GetConnection()
        {
            return ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString;
        }

        /// <summary>
        /// Returns a hash value using a secured hash algorithm (SHA-2)
        /// </summary>
        public static string Hash(string phrase)
        {
            SHA512Managed HashTool = new SHA512Managed();
            Byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(phrase));
            Byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }
    }
}