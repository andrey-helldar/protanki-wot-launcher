using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Security.Principal;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Variables
    {
        Debugging Debugging = new Debugging();
        Language Language = new Language();

        string lang = Properties.Resources.Default_Lang;


        /********************
         * Functions
         * ******************/
        public void Start()
        {
            try
            {
                MainWindow.JsonSettingsSet("info.ProductName", Application.Current.GetType().Assembly.GetName().Name);
                MainWindow.JsonSettingsSet("settings.winxp", Environment.OSVersion.Version.Major != 6 && Environment.OSVersion.Version.Minor > 1, "bool");
                MainWindow.JsonSettingsSet("info.user_id", GetUserID());

                // Проверяем вывод уведомлений
                if (MainWindow.JsonSettingsGet("info.manual_info") == null)
                {
                    MainWindow.JsonSettingsSet("info.video", false, "bool");
                    MainWindow.JsonSettingsSet("info.news", false, "bool");
                    MainWindow.JsonSettingsSet("info.manual_info", true, "bool");
                }

                // Ставим параметр наличия уведомлений в начальную точку
                MainWindow.JsonSettingsSet("multipack.update", false, "bool");
                MainWindow.JsonSettingsSet("game.update", false, "bool");

                // Проверяем права администратора
                MainWindow.JsonSettingsSet("settings.admin", new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator), "bool");
                

                string lang_pack = Properties.Resources.Default_Lang;
                string lang_game = Properties.Resources.Default_Lang;


                /*
                 *      Путь к игре
                 */
                try
                {
                    if (File.Exists(@"..\version.xml"))
                    {
                        string path_tanks = CorrectPath(Directory.GetCurrentDirectory(), -1);

                        XDocument doc = XDocument.Load(path_tanks + "version.xml");

                        if (doc.Root.Element("version").Value.IndexOf("Test") > -1)
                        {
                            MainWindow.JsonSettingsSet("game.test", true, "bool");
                            MainWindow.JsonSettingsSet("game.version", doc.Root.Element("version").Value.Trim().Remove(0, 2).Replace(" Common Test #", "."));
                        }
                        else
                        {
                            MainWindow.JsonSettingsSet("game.test", false, "bool");
                            MainWindow.JsonSettingsSet("game.version", doc.Root.Element("version").Value.Trim().Remove(0, 2).Replace(" #", "."));
                        }

                        /*
                         *  Язык локализации игры
                         */
                        lang_game = doc.Root.Element("meta").Element("localization").Value;
                        lang_game = lang_game.Remove(0, lang_game.IndexOf(" ")).ToLower().Trim();
                        MainWindow.JsonSettingsSet("game.language", lang_game);
                        
                        MainWindow.JsonSettingsSet("game.path", path_tanks);
                    }
                    else
                        MainWindow.MessageShow(Language.Set("MainProject", "Game_Not_Found", lang));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Variables.Class", "Start()", "Row: PathTanks", ex.Message, ex.StackTrace)); }


                /*
                 *  Грузим конфиг мультипака
                 */
                try
                {
                    try
                    {
                        // Обрабатываем файл конфига
                        if (File.Exists("config.json"))
                        {
                            using (StreamReader file = File.OpenText("config.json"))
                            using (JsonTextReader reader = new JsonTextReader(file))
                            {
                                JObject obj = (JObject)JToken.ReadFrom(reader);

                                MainWindow.JsonSettingsSet("multipack.type", (string)obj["type"]);
                                MainWindow.JsonSettingsSet("multipack.version", (string)obj["path"] + "." + (string)obj["version"]);
                                MainWindow.JsonSettingsSet("multipack.path", (string)obj["path"]);
                                MainWindow.JsonSettingsSet("multipack.date", (string)obj["date"]);
                                lang_pack = (string)obj["language"];
                            }
                        }
                        else if (File.Exists(Properties.Resources.Multipack_Config))
                        {
                            string path = Directory.GetCurrentDirectory() + @"\" + Properties.Resources.Multipack_Config;

                            MainWindow.JsonSettingsSet("multipack.type", new IniFile(path).IniReadValue(Properties.Resources.INI, "type").ToLower());
                            MainWindow.JsonSettingsSet("multipack.version", new IniFile(path).IniReadValue(Properties.Resources.INI, "path") + "." + new IniFile(path).IniReadValue(Properties.Resources.INI, "version"));
                            MainWindow.JsonSettingsSet("multipack.path", new IniFile(path).IniReadValue(Properties.Resources.INI, "path"));
                            MainWindow.JsonSettingsSet("multipack.date", new IniFile(path).IniReadValue(Properties.Resources.INI, "date"));
                            lang_pack = new IniFile(path).IniReadValue(Properties.Resources.INI, "language");
                        }
                        else MainWindow.MessageShow(Language.Set("MainProject", "Pack_Not_Found", lang));
                    }
                    catch (Exception) { }
                    finally { MainWindow.JsonSettingsSet("multipack.language", lang_pack); }
                }
                catch (Exception ex)
                {
                    Task.Factory.StartNew(() => Debugging.Save("Variables.Class", "Start()", "Row: Multipack config", ex.Message, ex.StackTrace));
                    MainWindow.MessageShow(Language.Set("MainProject", "Pack_Not_Found", lang));
                }


                /*
                 *  Определяем дефолтный язык лаунчера
                 */
                try
                {
                    //  Устанавливаем язык приложения
                    switch ((int)MainWindow.JsonSettingsGet("info.locale"))
                    {
                        case 0: lang = lang_pack; break;  //  Мультипак 0
                        case 1: lang = lang_game; break;       //  Клиент игры 1
                        case 2: lang = (string)MainWindow.JsonSettingsGet("info.language"); break;//  Вручную 2
                        default: lang = Properties.Resources.Default_Lang; break;
                    }

                    MainWindow.JsonSettingsSet("info.language", lang);
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Variables.Class", "Start()", "Row: Apply language", ex.Message, ex.StackTrace)); }
            }
            catch (Exception e) { Task.Factory.StartNew(() => Debugging.Save("Variables.Class", "Start()", e.Message, e.StackTrace)); }
        }

        private string CorrectPath(string sourcePath, int remove = 0)
        {
            string newPath = String.Empty;

            try
            {
                string[] temp = sourcePath.Split('\\');

                for (int i = 0; i < temp.Length + remove; i++)
                    newPath += temp[i] + @"\";

                return newPath;
            }
            catch (Exception ex)
            {
                Debugging.Save("Variables.Class", "CorrectPath()", "sourcePath = " + sourcePath, "remove = " + remove.ToString(), "newPath = " + newPath, ex.Message, ex.StackTrace);
                return sourcePath;
            }
        }

        public string GetUserID()
        {
            try
            {
                return Md5(Environment.MachineName +
                    Environment.UserName +
                    Environment.UserDomainName +
                    Environment.OSVersion.ToString());
            }
            catch (Exception ex) { Debugging.Save("Variables.Class", "GetUserID()", ex.Message, ex.StackTrace); return null; }
        }

        public string Md5(string input)
        {
            try
            {
                using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
                {
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++) { sBuilder.Append(data[i].ToString("x2")); }

                    return sBuilder.ToString();
                }
            }
            catch (Exception ex) { Debugging.Save("Variables.Class", "Md5()", "Input: " + input, ex.Message, ex.StackTrace); return null; }
        }

        /// <summary>
        /// Формирование версии в решетку
        /// </summary>
        /// <param name="ver">Версия для обработки</param>
        /// 
        /// <returns>Вывод версии в формате 0.0.0 #0</returns>
        public string VersionSharp(object ver, bool version = true)
        {
            try
            {
                if (version)
                    return String.Format("{0}.{1}.{2} #{3}", ((Version)ver).Major, ((Version)ver).Minor, ((Version)ver).Build, ((Version)ver).Revision);
                else
                {
                    Version str_ver = new Version((string)ver);
                    return String.Format("{0}.{1}.{2} #{3}", str_ver.Major, str_ver.Minor, str_ver.Build, str_ver.Revision);
                }
            }
            catch (Exception ex) { Debugging.Save("Variables.Class", "VersionSharp()", "Version: " + ver.ToString(), ex.Message, ex.StackTrace); return "0.0.0 #0"; }
        }

        /// <summary>
        /// Преобразование версии из решетки в нормальную
        /// </summary>
        /// <param name="ver">Входящий формат вида 0.0.0. #0</param>
        /// <returns>0.0.0.0</returns>
        public Version Version(string ver)
        {
            try { return new Version(ver.Replace(" #", ".")); }
            catch (Exception) { return new Version("0.0.0.0"); }
        }

        /// <summary>
        /// ИЗвлечение ресурсов, если оригинальные файлы не найдены
        /// </summary>
        /// <returns>TRUE при успешном извлечении, иначе - FALSE</returns>
        public bool SaveFromResources()
        {
            try
            {
                Task.WaitAll(new Task[]{
                    Task.Factory.StartNew(() => SavingFile("restart.exe", Properties.Resources.Restart)),
                    Task.Factory.StartNew(() => SavingFile("Processes.Library.dll", Properties.Resources.Processes_Library)),
                    
                    Task.Factory.StartNew(() => {                        
                        if(Properties.Resources.Default_Crypt_Settings == "1")
                            SavingFile(MainWindow.SettingsPath, Properties.Resources.Settings_Encoded);
                        else
                            SavingFile(MainWindow.SettingsPath, Properties.Resources.Settings);
                    }),
                    
                    Task.Factory.StartNew(() => SavingFile("Ionic.Zip.dll", Properties.Resources.Ionic_Zip)),
                    Task.Factory.StartNew(() => SavingFile("Newtonsoft.Json.dll", Properties.Resources.Newtonsoft_Json)),
                    Task.Factory.StartNew(() => SavingFile("Ookii.Dialogs.Wpf.dll", Properties.Resources.Ookii_Dialogs_Wpf))
                });

                return true;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => new Debugging().Save("Update.Class", "SaveFromResources()", ex.Message, ex.StackTrace));
                return false;
            }
        }

        private void SavingFile(string filename, byte[] resource)
        {
            try { if (!File.Exists(filename)) File.WriteAllBytes(filename, resource); }
            catch (Exception) { }
        }

        public bool ElementToBan(string block, string item)
        {
            try
            {
                item = Md5(item);
                var jo = MainWindow.JsonSettingsGet("do_not_display." + block);
                JArray ja;

                if (jo != null && jo.ToString().Length > 0)
                    ja = JArray.Parse(jo.ToString());
                else
                    ja = new JArray();

                ja.Add(item);
                MainWindow.jSettings["do_not_display"][block] = ja;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "ElementToBan()", "Block: " + block, "Item: " + item, ex.Message, ex.StackTrace)); }
            return true;
        }

        /// <summary>
        /// Находится ли элемент в бане?
        /// </summary>
        /// <param name="block"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ElementIsBan(string block, string item)
        {
            try
            {
                string ja = MainWindow.JsonSettingsGet("do_not_display." + block).ToString();
                if (ja.IndexOf(Md5(item)) == -1) return false;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "ElementToBan()", "Block: " + block, "Item: " + item, ex.Message, ex.StackTrace)); }
            return true;
        }
    }
}
