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
using Ini;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Variables
    {
        //public string SettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings2.xml";
        public string SettingsPath = "settings.xml";

        // Product
        public string ProductName = String.Empty;
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
        public XDocument Doc = null;

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
                    CommonTest = false;

        public string notifyLink = Properties.Resources.LinkVideoAll;

        Debug Debug = new Debug();


        /********************
         * Functions
         * ******************/

        public void Start()
        {
            try
            {
                ProductName = Application.Current.GetType().Assembly.GetName().Name;

                ItXP();

                Task.Factory.StartNew(() => new Update().SaveFromResources()).Wait();
                Task.Factory.StartNew(() => LoadSettings()).Wait();
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Variables.Class", "Start()", ex.Message)); }
        }

        private void LoadSettings()
        {
            string tmpLang = Properties.Resources.Default_Lang;

            if (File.Exists(SettingsPath)) Doc = XDocument.Load(SettingsPath);

            // Загружаем версию клиента игры
            try
            {
                if (Doc.Root.Element("game") != null)
                    if (Doc.Root.Element("game").Element("path") != null)
                        if (Doc.Root.Element("game").Element("path").Value != "")
                            PathTanks = Doc.Root.Element("game").Element("path").Value;
                        else
                        {
                            PathTanks = File.Exists(@"..\version.xml") ? CorrectPath(Directory.GetCurrentDirectory(), -1) : GetTanksRegistry();
                            Doc.Root.Element("game").Element("path").SetValue(PathTanks);
                            Doc.Save(SettingsPath);
                        }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("Variables.Class", "LoadSettings()", "Row: PathTanks", ex.Message));
                PathTanks = String.Empty;
            }

            try
            {
                if (File.Exists(@"..\version.xml"))
                {
                    XDocument doc = XDocument.Load(@"..\version.xml");

                    if (doc.Root.Element("version").Value.IndexOf("Test") > 0)
                    {
                        CommonTest = true;
                        TanksVersion = new Version(doc.Root.Element("version").Value.Trim().Remove(0, 2).Replace(" Common Test #", "."));
                    }
                    else
                        TanksVersion = new Version(doc.Root.Element("version").Value.Trim().Remove(0, 2).Replace(" #", "."));


                    tmpLang = doc.Root.Element("meta").Element("localization").Value.Trim();
                    tmpLang = tmpLang.Remove(0, tmpLang.IndexOf(" ") + 1).ToLower();
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Variables.Class", "LoadSettings()", "Row: TanksVersion", ex.Message)); }


            // Загружаем config.ini
            try
            {
                if (File.Exists(Properties.Resources.SettingsMultipack))
                {
                    // Загружаем данные
                    string pathINI = Directory.GetCurrentDirectory() + @"\" + Properties.Resources.SettingsMultipack;

                    MultipackDate = new IniFile(pathINI).IniReadValue(Properties.Resources.INI, "date");
                    MultipackType = new IniFile(pathINI).IniReadValue(Properties.Resources.INI, "type").ToLower();
                    MultipackVersion = new Version(VersionPrefix(TanksVersion) + new IniFile(pathINI).IniReadValue(Properties.Resources.INI, "version"));

                    Lang = Properties.Resources.Default_Settings_Priority == "multipack" ? new IniFile(pathINI).IniReadValue(Properties.Resources.INI, "language") : tmpLang;
                }
            }
            catch (Exception ex) { Debug.Save("Variables.Class", "LoadSettings()", "Row: reading config.ini", ex.Message); }


            try { UpdateNotify = Doc.Root.Element("info") != null ? (Doc.Root.Element("info").Attribute("notification") != null ? Doc.Root.Element("info").Attribute("notification").Value : null) : null; }
            catch (Exception ex) { Debug.Save("Variables.Class", "LoadSettings()", "Row: UpdateNotify", ex.Message); }


            try { ShowVideoNotify = Doc.Root.Element("info") != null ? (Doc.Root.Element("info").Attribute("video") != null ? (Doc.Root.Element("info").Attribute("video").Value == "True") : true) : true; }
            catch (Exception ex) { Debug.Save("Variables.Class", "LoadSettings()", "Row: ShowVideoNotify", ex.Message); }

            try
            {
                if (Doc.Root.Element("settings") != null)
                {
                    AutoKill = Doc.Root.Element("settings").Attribute("kill") != null ? Doc.Root.Element("settings").Attribute("kill").Value == "True" : false;
                    AutoForceKill = Doc.Root.Element("settings").Attribute("force") != null ? Doc.Root.Element("settings").Attribute("force").Value == "True" : false;

                    AutoAero = Doc.Root.Element("settings").Attribute("aero") != null ? Doc.Root.Element("settings").Attribute("aero").Value == "True" : false;
                    AutoVideo = ReadCheckStateBool(Doc, "video");
                    AutoWeak = Doc.Root.Element("settings").Attribute("weak") != null ? Doc.Root.Element("settings").Attribute("weak").Value == "True" : false;
                    AutoCPU = Doc.Root.Element("settings").Attribute("balance") != null ? Doc.Root.Element("settings").Attribute("balance").Value == "True" : false;
                }
            }
            catch (Exception ex) { Debug.Save("Variables.Class", "LoadSettings()", "Row: reading XML Element `settings`", ex.Message); }

            try { if (Doc.Root.Element("common.test") != null) CommonTest = true; }
            catch (Exception ex) { Debug.Save("Variables.Class", "LoadSettings()", "Row: reading XML Element `common.test`", ex.Message); }
        }

        private bool ReadCheckStateBool(XDocument doc, string attr)
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
            catch (Exception ex) { Debug.Save("Variables.Class", "ReadCheckStateBool()", ex.Message, "Attribute: " + attr, doc.ToString()); return false; }
        }

        private string GetTanksRegistry()
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{1EAC1D02-C6AC-4FA6-9A44-96258C37C812RU}_is1");
                return key != null ? (string)key.GetValue("InstallLocation") : null;
            }
            catch (Exception ex) { Debug.Save("Variables.Class", "GetTanksRegistry()", ex.Message); return null; }
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
                Debug.Save("Variables.Class", "CorrectPath()", "sourcePath = " + sourcePath, "remove = " + remove.ToString(), "newPath = " + newPath, ex.Message);
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
            catch (Exception ex) { Debug.Save("Variables.Class", "GetUserID()", ex.Message); return null; }
        }

        public Version Version(string version)
        {
            try { return new Version(String.Format("{0}.{1}.{2}.{3}", TanksVersion.Major, TanksVersion.Minor, TanksVersion.Build, version)); }
            catch (Exception ex) { Debug.Save("Variables.Class", "Version()", version, ex.Message); return new Version("0.0.0.0"); }
        }

        /// <summary>
        /// Формирование версии в решетку
        /// </summary>
        /// <param name="ver"></param>
        /// <returns>формат 0.0.0 #0</returns>
        public string VersionToSharp(Version ver)
        {
            try { return String.Format("{0}.{1}.{2} #{3}", ver.Major, ver.Minor, ver.Build, ver.Revision); }
            catch (Exception ex) { Debug.Save("Variables.Class", "VersionToSharp()", "Version: " + ver.ToString(), ex.Message); return "0.0.0 #0"; }
        }

        /// <summary>
        /// Формирование префикса из версии
        /// </summary>
        /// <param name="ver"></param>
        /// <returns></returns>
        public string VersionPrefix(Version ver)
        {
            try { return String.Format("{0}.{1}.{2}.", ver.Major, ver.Minor, ver.Build); }
            catch (Exception ex) { Debug.Save("Variables.Class", "VersionPrefix()", "Version: " + ver.ToString(), ex.Message); return "0.0.0."; }
        }

        public void ItXP()
        {
            try { WinXP = Environment.OSVersion.Version.Major == 5; }
            catch (Exception ex) { Debug.Save("Variables.Class", "ItXP()", ex.Message); }
        }

        /// <summary>
        /// Проверка внесен ли элемент новости/видео в так называемый "черный список"
        /// </summary>
        /// <param name="item">Входящий идентификатор записи для проверки</param>
        /// <returns>
        ///     TRUE - запись находится в черном списке;
        ///     FALSE - запись "чистая"
        /// </returns>
        public bool ElementBan(string item, string block = "video")
        {
            if (MainWindow.XmlDocument.Root.Element("do_not_display") != null)
                if (MainWindow.XmlDocument.Root.Element("do_not_display").Element(block) != null)
                    if (MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Elements("item").Count() > 0)
                        foreach (string str in MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Elements("item"))
                            if (str == item) return true;

            return false;
        }


        /// <summary>
        /// Достаем булевое значение элемента
        /// </summary>
        /// <param name="block">Имя ключевого элемента</param>
        /// <param name="attr">Имя аттрибута</param>
        /// <returns>Булевое значение существования элемента</returns>
        public bool GetElement(string block, string attr)
        {
            if (MainWindow.XmlDocument.Root.Element(block) != null)
                if (MainWindow.XmlDocument.Root.Element(block).Attribute(attr) != null)
                    return MainWindow.XmlDocument.Root.Element(block).Attribute(attr).Value == "True";

            return true;
        }
    }
}
