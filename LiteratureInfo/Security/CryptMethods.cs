using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LiteratureApp.Core
{
    public class CryptMethods
    {
        public string Encrypt(string pass)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(pass);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(DataBank.hash));
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDES.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    string passenc = Convert.ToBase64String(results, 0, results.Length);
                    return passenc;
                }
            }
        }
        public string Decript(string pass)
        {
            byte[] data = Convert.FromBase64String(pass);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(DataBank.hash));
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDES.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    string passdec = UTF8Encoding.UTF8.GetString(results);
                    return passdec;
                }
            }
        }
    }
}
