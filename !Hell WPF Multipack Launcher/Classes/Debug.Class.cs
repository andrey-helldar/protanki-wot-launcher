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

                string filename = String.Format("{0}_{1}_{2}.debug", version, ErrorCode(module, func), DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff"));

                string encoded = new Crypt().Encode(json.ToString());
                if (encoded != "FAIL") File.WriteAllText(@"temp\" + filename, new Crypt().Encode(json.ToString()), Encoding.UTF8);
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

        /// <summary>
        /// Вывод кода ошибки
        /// </summary>
        /// <param name="formID">Идентификатор формы</param>
        /// <param name="func">Имя функции для определения кода ошибки</param>
        /// <returns>Код ошибки в формате "0x1", где:
        /// 0 - код формы
        /// 1 - код функции
        /// </returns>
        public string ErrorCode(string formID, string func)
        {
            int form = 0;
            string num = "0";

            switch(formID){
                case "MainWindow":
                    form = 1;

                    switch (func)
                    {
                        case "MainWindow()": num = FormatNum(1); break;
                        case "Window_Loaded(2)": num = FormatNum(2); break;
                        case "Window_Loaded(3)": num = FormatNum(3); break;
                        case "Window_Loaded(4)": num = FormatNum(4); break;
                        case "Debug.ClearLogs()": num = FormatNum(5); break;
                        case "Window_Closing(0)": num = FormatNum(6); break;
                        case "Window_Closing(1)": num = FormatNum(7); break;
                        case "bClose_Click()": num = FormatNum(8); break;
                        case "bMinimize_Click()": num = FormatNum(9); break;
                        case "NotifyClick()": num = FormatNum(10); break;
                        case "OpenLink()": num = FormatNum(11); break;
                        case "bPlay_Click()": num = FormatNum(12); break;
                        case "bAirus_Click()": num = FormatNum(13); break;
                        case "bLauncher_Click()": num = FormatNum(14); break;
                        case "Button_Click()": num = FormatNum(15); break;
                        case "ProcessStart()": num = FormatNum(16); break;

                        default: num = FormatNum(0); break;
                    }
            break;

                default: form = 0; break;
            }

            return String.Format("{0}x{1}", form, form == 0 ? FormatNum(0) : num);
        }

        /// <summary>
        /// Форматирование строки по минимальному количеству знаков.
        /// По-умолчанию 4 знака, то есть входящее число 4 на выходе будет иметь вид "00004"
        /// </summary>
        /// <param name="num">Число</param>
        /// <returns>Формат в соответствии с количеством символов кода</returns>
        private string FormatNum(int num)
        {
            int nums = 4;
            string result = String.Empty;

            int max = nums - num.ToString().Length;

            if (max > 0)
            for (int i = 0; i < max; i++)
                result += "0";

            result += max.ToString();

            return result;
        }
    }
}
