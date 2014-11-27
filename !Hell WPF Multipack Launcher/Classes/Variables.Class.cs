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
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Variables
    {
        Debug Debug = new Debug();
        Language Language = new Language();


        //public string SettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.json";
        private string SettingsPath = "settings.json";

        string lang = Properties.Resources.Default_Lang;

        // Product
        /*public string ProductName = String.Empty;
        public string Lang = Properties.Resources.Default_Lang;

        // Multipack
        public Version MultipackVersion = new Version("0.0.0.0");
        public string MultipackType = Properties.Resources.Default_Multipack_Type,
                      MultipackDate = "1970-1-1";

        // Tanks
        public Version TanksVersion = new Version("0.0.0.0");

        // Paths
        public string PathTanks = String.Empty;

        public string UpdateLink = Properties.Resources.LinkVideoAll,
                      UpdateMessage = String.Empty,
                      UpdateNotify = String.Empty;

        public bool UpdateMultipack = false,
                    UpdateTanks = false;

        public Version UpdateMultipackVersion = new Version("0.0.0.0"),
                       UpdateTanksVersion = new Version("0.0.0.0");

        // Launcher settings
        //public XDocument Doc = null;

        public bool AutoKill = false,
                    AutoForceKill = false,
                    AutoAero = false,
                    AutoVideo = false,
                    AutoWeak = false,
                    AutoCPU = true;

        // OS Version
        public bool WinXP = true;

        // Other
        public bool ShowVideoNotify = true,
                    CommonTest = false;*/


        /********************
         * Functions
         * ******************/
        public void Start()
        {
            try
            {
                SaveFromResources();

                MainWindow.JsonSettingsSet("info.ProductName", Application.Current.GetType().Assembly.GetName().Name);
                MainWindow.JsonSettingsSet("settings.winxp", Environment.OSVersion.Version.Major == 5, "bool");

                string lang_pack = Properties.Resources.Default_Lang;
                string lang_game = Properties.Resources.Default_Lang;

                /*
                 *      Дополнительные переменные
                 */
                MainWindow.JsonSettingsSet("info.user_id", GetUserID());


                /*
                 *      Путь к игре
                 */
                try
                {
                    string path_tanks;
                    if (File.Exists(@"..\version.xml"))
                        path_tanks = CorrectPath(Directory.GetCurrentDirectory(), -1);
                    else
                    {
                        var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{1EAC1D02-C6AC-4FA6-9A44-96258C37C812RU}_is1");
                        path_tanks = key != null ? (string)key.GetValue("InstallLocation") : null;
                    }
                    if (path_tanks != null)
                        MainWindow.JsonSettingsSet("game.path", path_tanks);
                    else
                        MainWindow.MessageShow(Language.Set("MainProject", "Game_Not_Found", lang));


                    if (path_tanks != null)
                    {
                        /*
                         *  Версия клиента игры
                         */
                        XDocument doc = XDocument.Load(@"..\version.xml");

                        if (doc.Root.Element("version").Value.IndexOf("Test") > 0)
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
                    }
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Variables.Class", "Start()", "Row: PathTanks", ex.Message, ex.StackTrace)); }


                /*
                 *  Грузим конфиг мультипака
                 */
                try
                {
                    if (File.Exists(Properties.Resources.Multipack_Config))
                    {
                        using (StreamReader reader = File.OpenText(Properties.Resources.Multipack_Config))
                        {
                            JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

                            MainWindow.JsonSettingsSet("multipack.type", (string)o.SelectToken("type"));
                            MainWindow.JsonSettingsSet("multipack.version", (string)o.SelectToken("path") + "." + (string)o.SelectToken("version"));
                            MainWindow.JsonSettingsSet("multipack.date", (string)o.SelectToken("date"));
                            lang_pack = (string)o.SelectToken("language");
                        }
                    }
                    else
                        MainWindow.MessageShow(Language.Set("MainProject", "Pack_Not_Found", lang));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Variables.Class", "Start()", "Row: Multipack config", ex.Message, ex.StackTrace)); }


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
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Variables.Class", "Start()", "Row: Apply language", ex.Message, ex.StackTrace)); }



                /*
                 *      Загружаем инфу об обновлении мультипака
                 */

            }
            catch (Exception e) { Task.Factory.StartNew(() => Debug.Save("Variables.Class", "Start()", e.Message, e.StackTrace)); }
        }

        /*private bool ReadCheckStateBool(XDocument doc, string attr)
        {
            try
            {
                if (doc.Root.Element("settings") != null)
                    if (doc.Root.Element("settings").Attribute(attr) != null)
                    {
                        switch (doc.Root.Element("settings").Attribute(attr).Value)
                        {
                            case "Checked": return true;
                            case "Indeterminate": return true;
                            default: return false;
                        }
                    }
                return false;
            }
            catch (Exception ex) { Debug.Save("Variables.Class", "ReadCheckStateBool()", "Attribute: " + attr, doc.ToString(), ex.Message, ex.StackTrace); return false; }
        }*/

        /*private string GetTanksRegistry()
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{1EAC1D02-C6AC-4FA6-9A44-96258C37C812RU}_is1");
                return key != null ? (string)key.GetValue("InstallLocation") : null;
            }
            catch (Exception ex) { Debug.Save("Variables.Class", "GetTanksRegistry()", ex.Message, ex.StackTrace); return null; }
        }*/

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
                Debug.Save("Variables.Class", "CorrectPath()", "sourcePath = " + sourcePath, "remove = " + remove.ToString(), "newPath = " + newPath, ex.Message, ex.StackTrace);
                return sourcePath;
            }
        }

        public string GetUserID()
        {
            try
            {
                string name = Environment.MachineName +
                    Environment.UserName +
                    Environment.UserDomainName +
                    Environment.OSVersion.ToString();

                using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
                {
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(name));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++) { sBuilder.Append(data[i].ToString("x2")); }

                    return sBuilder.ToString();
                }
            }
            catch (Exception ex) { Debug.Save("Variables.Class", "GetUserID()", ex.Message, ex.StackTrace); return null; }
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
            catch (Exception ex) { Debug.Save("Variables.Class", "Md5()", "Input: "+input, ex.Message, ex.StackTrace); return null; }
        }

        /*public Version Version(string version)
        {
            try { return new Version(String.Format("{0}.{1}.{2}.{3}", TanksVersion.Major, TanksVersion.Minor, TanksVersion.Build, version)); }
            catch (Exception ex) { Debug.Save("Variables.Class", "Version()", version, ex.Message, ex.StackTrace); return new Version("0.0.0.0"); }
        }*/

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
            catch (Exception ex) { Debug.Save("Variables.Class", "VersionSharp()", "Version: " + ver.ToString(), ex.Message, ex.StackTrace); return "0.0.0 #0"; }
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
        /// Формирование префикса из версии
        /// </summary>
        /// <param name="ver">Версия для преобразования</param>
        /// <returns>формат 0.0.0.</returns>
        /*public string VersionPrefix(Version ver)
        {
            try { return String.Format("{0}.{1}.{2}.", ver.Major, ver.Minor, ver.Build); }
            catch (Exception ex) { Debug.Save("Variables.Class", "VersionPrefix()", "Version: " + ver.ToString(), ex.Message, ex.StackTrace); return "0.0.0."; }
        }*/

        /*public void ItXP()
        {
            try { WinXP = Environment.OSVersion.Version.Major == 5; }
            catch (Exception ex) { Debug.Save("Variables.Class", "ItXP()", ex.Message, ex.StackTrace); }
        }*/

        /// <summary>
        /// Проверка внесен ли элемент новости/видео в так называемый "черный список"
        /// </summary>
        /// <param name="item">Входящий идентификатор записи для проверки</param>
        /// <returns>
        ///     TRUE - запись находится в черном списке;
        ///     FALSE - запись "чистая"
        /// </returns>
        /*public bool ElementBan(string item, string block = "video")
        {
            try
            {
                if (MainWindow.XmlDocument.Root.Element("do_not_display") != null)
                    if (MainWindow.XmlDocument.Root.Element("do_not_display").Element(block) != null)
                        if (MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Elements("item").Count() > 0)
                            foreach (string str in MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Elements("item"))
                                if (str == item) return true;
            }
            catch (Exception ex) { Debug.Save("Variables.Class", "ElementBan()", item, block, ex.Message, ex.StackTrace); }

            return false;
        }*/


        /// <summary>
        /// Достаем булевое значение элемента
        /// </summary>
        /// <param name="block">Имя ключевого элемента</param>
        /// <param name="attr">Имя аттрибута</param>
        /// <returns>Булевое значение существования элемента</returns>
        /*public bool GetElement(string block, string attr)
        {
            try
            {
                if (MainWindow.XmlDocument.Root.Element(block) != null)
                    if (MainWindow.XmlDocument.Root.Element(block).Attribute(attr) != null)
                        return MainWindow.XmlDocument.Root.Element(block).Attribute(attr).Value == "True";
            }
            catch (Exception ex) { Debug.Save("Variables.Class", "GetElement()", block, attr, ex.Message, ex.StackTrace); }

            return true;
        }*/

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
                    
                    Task.Factory.StartNew(() => SavingFile(SettingsPath, Properties.Resources.Settings)),
                    
                    Task.Factory.StartNew(() => SavingFile("Ionic.Zip.dll", Properties.Resources.Ionic_Zip)),
                    Task.Factory.StartNew(() => SavingFile("Newtonsoft.Json.dll", Properties.Resources.Newtonsoft_Json)),
                    Task.Factory.StartNew(() => SavingFile("Ookii.Dialogs.Wpf.dll", Properties.Resources.Ookii_Dialogs_Wpf))
                });

                return true;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => new Debug().Save("Update.Class", "SaveFromResources()", ex.Message, ex.StackTrace));
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
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ElementToBan()", "Block: " + block, "Item: " + item, ex.Message, ex.StackTrace)); }
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
                if (ja.IndexOf(item) == -1) return false;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ElementToBan()", "Block: " + block, "Item: " + item, ex.Message, ex.StackTrace)); }
            return true;
        }
    }
}
