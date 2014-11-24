using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Crypt
    {
        Classes.Debug Debug = new Debug();

        /// <summary>
        /// Шифрование информации
        /// </summary>
        /// <param name="text">Исходный текс</param>
        /// <returns>Шифрованная строка</returns>
        public string Encode(string text)
        {
            if (Properties.Resources.Default_Debug_Crypt == "0")
                return text;
            else
            {
                try { return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(text)); }
                catch (Exception) { return "FAIL"; }
            }
        }

        /// <summary>
        /// Дешифрование информации
        /// </summary>
        /// <param name="text">Шифрованная строка</param>
        /// <returns>Расшифрованный результат</returns>
        public string Decode(string text)
        {
            if (Properties.Resources.Default_Debug_Crypt == "0")
                return text;
            else
            {
                try { return Encoding.UTF8.GetString(System.Convert.FromBase64String(text)); }
                catch (Exception) { return "FAIL"; }
            }
        }

        static public string Encrypt(string plainText, string key)
        {
            string cipherText;
            var rijndael = new RijndaelManaged()
            {
                Mode = CipherMode.ECB,
                BlockSize = 256,
                Key = Encoding.UTF8.GetBytes(key),
                IV = Encoding.UTF8.GetBytes(Properties.Resources.API),
            };

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(rijndael.Key, rijndael.IV), CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                        streamWriter.Flush();
                    }
                    cipherText = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return cipherText;
        }

        static public string Decrypt(string Text, string KeyString)
        {
            byte[] cypher = Convert.FromBase64String(Text);
            var sRet = String.Empty;

            using (var rj = new RijndaelManaged())
            {
                try
                {
                    rj.Mode = CipherMode.ECB;
                    rj.KeySize = 256;
                    rj.BlockSize = 256;
                    rj.Key = Encoding.UTF8.GetBytes(KeyString);
                    rj.IV = Encoding.UTF8.GetBytes(Properties.Resources.API);
                    var ms = new MemoryStream(cypher);

                    using (var cs = new CryptoStream(ms, rj.CreateDecryptor(rj.Key, rj.IV), CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            sRet = sr.ReadLine();
                        }
                    }
                }
                finally { rj.Clear(); }
            }

            return sRet;
        }
    }
}
