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
        Classes.Debugging Debugging = new Debugging();
        Classes.Variables Variables = new Variables();

        /// <summary>
        /// Шифрование строки
        /// </summary>
        /// <param name="text">Текст для шифра</param>
        /// <param name="key">Ключ шифрования</param>
        /// <param name="debug">Определяем шифруем ли дебаг</param>
        /// <returns>Шифрованная строка</returns>
        public string Encrypt(string text, string key, bool debug = false)
        {
            try
            {
                if ((debug && Properties.Resources.Default_Debug_Crypt == "1") || Properties.Resources.API_DEV_CRYPT == "1")
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
                else
                    return text;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("Crypt.Class", "Encrypt()", "Debug: " + debug.ToString(), text, ex.Message, ex.StackTrace));
                return "FAIL";
            }
        }


        public string Decrypt(string encoded, string key, bool debug = false)
        {
            try
            {
                if ((debug && Properties.Resources.Default_Debug_Crypt == "1") ||
                    Properties.Resources.API_DEV_CRYPT == "1" ||
                    Properties.Resources.Default_Crypt_Settings == "1")
                    return Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                else
                    return encoded;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("Crypt.Class", "Decrypt()", "Debug: " + debug.ToString(), encoded, ex.Message, ex.StackTrace));
                return null;
            }
        }
    }
}
