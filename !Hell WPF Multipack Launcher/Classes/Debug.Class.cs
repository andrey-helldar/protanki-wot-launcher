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
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Debug
    {
        public void Save(string module, string func, params string[] args)
        {
            try
            {
                string version = Application.Current.GetType().Assembly.GetName().Version.ToString();

                JObject json = new JObject(
                    new JProperty("uid", new Classes.Variables().GetUserID()),
                                           new JProperty("version", version),
                                           new JProperty("date", DateTime.Now.ToString("yyyy-MM-dd h-m-s")),
                                           new JProperty("module", module),
                                           new JProperty("function", func)
                                       );

                for (int i = 0; i < args.Length; i++)
                    json.Add(new JProperty("param" + i.ToString(), args[i]));

                if (!Directory.Exists("temp")) { Directory.CreateDirectory("temp"); }

                string filename = String.Format("{0}_{1}.debug", version, DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff"));
                File.WriteAllText(@"temp\" + filename, Crypt(json.ToString()), Encoding.UTF8);
            }
            catch (IOException) { Thread.Sleep(3000); Save(module, func, args); }
            catch (Exception) { }
            finally { }
        }

        public void Restart()
        {
            try
            {
                Process.Start("restart.exe", String.Format("\"{0}.exe\"", Process.GetCurrentProcess().ProcessName));
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Save("Debug.Class", "Restart()", ex.Message, ex.StackTrace)); }
        }

        /// <summary>
        /// Если в настройках включен параметр дебага, выводить сообщения на форму
        /// </summary>
        private void Message(string message)
        {
            MainWindow.Navigator("Error", "Debug.Class");
        }

        /// <summary>
        /// Шифруем сообщение перед сохранением/отправкой
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Шифрованная строка</returns>
        public string Crypt(string input)
        {
            try { return input; }
            catch (Exception) { return input; }
        }

        /// <summary>
        /// Дешифруем входящую строку
        /// </summary>
        /// <param name="input">Зашифрованная строка</param>
        /// <returns>Дешифрованная строка</returns>
        public string Decrypt(string input)
        {
            try { return input; }
            catch (Exception) { return input; }
        }

        /// <summary>
        /// Очистка папки с логами
        /// </summary>
        /// <param name="temp">Если TRUE - чистим папку TEMP, иначе - DEBUG</param>
        public void ClearLogs(bool temp = false)
        {
            if (!temp)
            {
                Thread.Sleep(10000); // При запуске выжидаем 10 секунд перед началом архивации

                ClearLogs(true); // Очищаем папки от старых логов
                ArchiveLogs(); // Запускаем архивацию логов
            }

            string path = Environment.CurrentDirectory;

            try
            {
                if (Directory.Exists(path + (temp ? @"\temp" : @"\debug")))
                {
                    foreach (FileInfo file in new DirectoryInfo(path + (temp ? @"\temp" : @"\debug")).GetFiles())
                    {
                        DateTime now = DateTime.Now;
                        DateTime cf = File.GetCreationTime(file.FullName);

                        /* 
                         * Удаляем все файлы, старше 3-х суток
                         * 1 минута = 60 сек
                         * 1 час = 60 минут = 3600 сек
                         * 1 сутки = 24 часа = 86400 сек
                         * 2 суток = 172800 сек
                         * 3 суток = 259200 сек
                         */

                        if (((TimeSpan)(now - cf)).TotalSeconds > (86400 * Convert.ToInt16(Properties.Resources.LogDays))) { File.Delete(file.FullName); }
                    }
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Save("Debug.Class", "ClearLogs()", "Temp: " + temp.ToString(), ex.Message, ex.StackTrace)); }
        }

        /// <summary>
        /// Архивация логов.
        /// ВНИМАНИЕ!!!
        /// При архивации используется пароль, равный User ID.
        /// </summary>
        public void ArchiveLogs()
        {
            try
            {
                string path = Environment.CurrentDirectory;

                if (File.Exists("Ionic.Zip.dll"))
                {
                    if (Directory.Exists(path + @"\temp"))
                    {
                        if (!Directory.Exists(path + @"\debug")) { Directory.CreateDirectory(path + @"\debug"); }

                        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                        {
                            string uid = new Classes.Variables().GetUserID();
                            string version = Application.Current.GetType().Assembly.GetName().Version.ToString();

                            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                            zip.Password = uid;
                            zip.AddDirectory(path + @"\temp");
                            zip.Save(path + @"\debug\" + String.Format("{0}_{1}_{2}.zip", uid, version, DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff")));
                        }

                        Directory.Delete(path + @"\temp", true);
                    }
                }
            }
            catch (Exception ex) { Save("Debug.Class", "ArchiveLogs()", ex.Message, ex.StackTrace); }
        }
    }
}
