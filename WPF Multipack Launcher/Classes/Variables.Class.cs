using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json;
using Ini;

namespace WPF_Multipack_Launcher.Variables
{
    class Variables
    {
        // TEMP
        public int Accept = 300;

        // Wargaming API
        public string Api = String.Empty;

        // Product
        public string ProductName = String.Empty;
        public Version ProductVersion = new Version("0.0.0.0");
        public string Lang = "en";

        // Background
        public int BackgroundMax = 7,
                   BackgroundIndex = 1,
                   BackgroundDelay = 10000;
        public bool BackgroundLoop = true;

        // Multipack
        public Version MultipackVersion = new Version("0.0.0.0");
        public string MultipackType = "Base",
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

        // Other
        public string TempStatus = "Loading status...";
        public bool ShowVideoNotify = true,
                    CommonTest = false;

        public string notifyLink = Properties.Resources.LinkVideoAll;


        /********************
         * Functions
         * ******************/

        public async Task<bool> Start()
        {
            try
            {
                ProductName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;
                ProductVersion = Application.Current.MainWindow.GetType().Assembly.GetName().Version;

                GetApiKey().Wait();

                new Classes.Update().SaveFromResources().Wait();

                if (File.Exists("settings.xml")) Doc = XDocument.Load("settings.xml");
                LoadSettings().Wait();
            }
            catch (Exception) { return false; }
            return true;
        }

        private async Task LoadSettings()
        {
            // Загружаем версию клиента игры
            PathTanks = File.Exists(@"..\version.xml") ? CorrectPath(Directory.GetCurrentDirectory(), -1) : GetTanksRegistry();

            if (File.Exists(@"..\version.xml") && PathTanks != String.Empty)
            {
                XDocument doc = XDocument.Load(PathTanks + "version.xml");

                if (doc.Root.Element("version").Value.IndexOf("Test") > 0)
                {
                    CommonTest = true;
                    TanksVersion = new Version(doc.Root.Element("version").Value.Trim().Remove(0, 2).Replace(" Common Test #", "."));
                }
                else
                    TanksVersion = new Version(doc.Root.Element("version").Value.Trim().Remove(0, 2).Replace(" #", "."));
            }
            else
            {
                // КЛиент игры не существует
            }


            // Загружаем config.ini
            if (File.Exists(Properties.Resources.SettingsPathMultipack))
            {
                // Загружаем данные
                string pathINI = Directory.GetCurrentDirectory() + @"\" + Properties.Resources.SettingsPathMultipack;

                MultipackDate = new IniFile(pathINI).IniReadValue(Properties.Resources.INI, "date");
                MultipackType = new IniFile(pathINI).IniReadValue(Properties.Resources.INI, "type").ToLower();
                MultipackVersion = new Version(new LocalInterface.LocInterface().VersionPrefix(TanksVersion).Result + new IniFile(pathINI).IniReadValue(Properties.Resources.INI, "version"));
                Lang = new IniFile(pathINI).IniReadValue(Properties.Resources.INI, "language");
            }
            else
            {
                // Мультипак не обнаружен
                new Classes.Debug().Message(ProductName, new LocalInterface.Language().DynamicLanguage("noMods", Lang)).Wait();
            }

            
            UpdateNotify = Doc.Root.Element("notification") != null ? Doc.Root.Element("notification").Value : String.Empty;
            ShowVideoNotify = Doc.Root.Element("info") != null ? (Doc.Root.Element("info").Attribute("video") != null ? (Doc.Root.Element("info").Attribute("video").Value == "True") : true) : true;

            if (Doc.Root.Element("settings") != null)
                {
                    AutoKill = Doc.Root.Element("settings").Attribute("kill") != null ? Doc.Root.Element("settings").Attribute("kill").Value == "True" : false;
                    AutoForceKill = Doc.Root.Element("settings").Attribute("force") != null ? Doc.Root.Element("settings").Attribute("force").Value == "True" : false;

                    AutoAero = Doc.Root.Element("settings").Attribute("aero") != null ? Doc.Root.Element("settings").Attribute("aero").Value == "True" : false;
                    AutoVideo = ReadCheckStateBool(Doc, "video");
                    AutoWeak = Doc.Root.Element("settings").Attribute("weak") != null ? Doc.Root.Element("settings").Attribute("weak").Value == "True" : false;
                    AutoCPU = Doc.Root.Element("settings").Attribute("balance") != null ? Doc.Root.Element("settings").Attribute("balance").Value == "True" : false;
                }

            if (Doc.Root.Element("common.test") != null) CommonTest = true;
        }

        /*************************
         * GET Wargaming API key
         * **********************/
        private async Task GetApiKey()
        {
            try
            {
                Api = new Classes.POST().RequestInfo("api");
            }
            catch (Exception ex) { new Classes.Debug().Save("Variables Class", ex.Message); }
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
            catch (Exception) { return false; }
        }

        private string GetTanksRegistry()
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{1EAC1D02-C6AC-4FA6-9A44-96258C37C812RU}_is1");
                return key != null ? (string)key.GetValue("InstallLocation") : null;
            }
            catch (Exception ex)
            {
                new Classes.Debug().Save("fIndex", "GetTanksRegistry()", ex.Message).Wait();
                //MessageBox.Show(this, Language.DynamicLanguage("admin", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
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
                new Classes.Debug().Save("fIndex", "CorrectPath", "sourcePath = " + sourcePath, "remove = " + remove.ToString(), "newPath = " + newPath, ex.Message).Wait();
                return sourcePath;
            }
        }

        public async Task<string> GetUserID()
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
            catch (Exception) { return null; }
        }

        public Version Version(string version)
        {
            return new Version(String.Format("{0}.{1}.{2}.{3}", TanksVersion.Major, TanksVersion.Minor, TanksVersion.Build, version));
        }


        /// <summary>
        /// Если дата новости старее даты выпуска модпака,
        /// то выводим в результат "false" как запрет на вывод.
        /// </summary>
        /// <param name="packDate"></param>
        /// <param name="newsDate"></param>
        /// <returns>Во всех иных случаях выводим "true",
        /// то есть дата валидная</returns>
        public bool ParseDate(string packDate = null, string newsDate = null)
        {
            try
            {
                if (packDate != null && newsDate != null)
                    if (DateTime.Parse(newsDate) < DateTime.Parse(packDate)) { return false; }
                return true;
            }
            catch (Exception) { return true; }
        }
    }
}
