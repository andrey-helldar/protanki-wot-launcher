using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            if (Properties.Resources.API_DEV_CRYPT == "0")
            {
                return text;
            }
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
            if (Properties.Resources.API_DEV_CRYPT == "0")
            {
                return text;
            }
            else
            {
                try { return Encoding.UTF8.GetString(System.Convert.FromBase64String(text)); }
                catch (Exception) { return "FAIL"; }
            }
        }
    }
}
