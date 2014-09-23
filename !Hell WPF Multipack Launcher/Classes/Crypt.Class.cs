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
            try
            {
                return text;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Crypt.Class", "Encode()", ex.Message, ex.StackTrace)); }

            return Properties.Resources.Class_Fail;
        }

        /// <summary>
        /// Дешифрование информации
        /// </summary>
        /// <param name="text">Шифрованная строка</param>
        /// <returns>Расшифрованный результат</returns>
        public string Decode(string text)
        {
            try
            {
                return text;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Crypt.Class", "Decode()", ex.Message, ex.StackTrace)); }

            return Properties.Resources.Class_Fail;
        }
    }
}
