using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Debug
    {
        public void Save(string module, string func, params string[] args)
        {
            try
            {
                string version = Application.Current.GetType().Assembly.GetName().Version.ToString();

                Dictionary<string, string> jData = new Dictionary<string, string>();
                jData.Add("uid", new Classes.Variables().GetUserID());
                jData.Add("version", version);
                jData.Add("date", DateTime.Now.ToString("yyyy-MM-dd h-m-s"));
                jData.Add("module", module);
                jData.Add("function", func);

                for (int i = 0; i < args.Length; i++)
                    jData.Add("param" + i.ToString(), args[i]);

                if (!Directory.Exists("temp")) { Directory.CreateDirectory("temp"); }
                string filename = String.Format("{0}_{1}.debug", version, DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff"));
                File.WriteAllText(@"temp\" + filename, Crypt(JsonConvert.SerializeObject(jData)), Encoding.UTF8);
            }
            finally { }
        }

        public void Restart()
        {
            Process.Start("restart.exe", String.Format("\"{0}.exe\"", Process.GetCurrentProcess().ProcessName));
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// Если в настройках включен параметр дебага, выводить сообщения на форму
        /// </summary>
        private void Message()
        {
            //MainFrame.NavigationService.Navigate(new Uri("Error.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Шифруем сообщение перед сохранением/отправкой
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Шифрованная строка</returns>
        public string Crypt(string input)
        {
            try
            {
                return input;
            }
            catch (Exception) { return input; }
        }

        /// <summary>
        /// Дешифруем входящую строку
        /// </summary>
        /// <param name="input">Зашифрованная строка</param>
        /// <returns>Дешифрованная строка</returns>
        public string Decrypt(string input)
        {
            try
            {
                return input;
            }
            catch (Exception) { return input; }
        }
    }
}
