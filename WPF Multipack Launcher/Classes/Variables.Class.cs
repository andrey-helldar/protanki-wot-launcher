﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Ini;

namespace WPF_Multipack_Launcher.Variables
{
    class Variables
    {
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
        public string TanksVersionPrefix = "0.0.0";

        // Paths
        public string PathTanks = String.Empty;

        // Updates
        public string UpdateLink = Properties.Resources.LinkVideoAll,
                      UpdateMessage = String.Empty,
                      UpdateNotify = String.Empty;

        public bool UpdateMultipack = false,
                    UpdateTanks = false;

        public Version UpdateMultipackVersion = new Version("0.0.0.0"),
                       UpdateTanksVersion = new Version("0.0.0.0");

        // Launcher settings
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


        /********************
         * Functions
         * ******************/

        public async Task<bool> Start()
        {
            try
            {
                ProductName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;
                ProductVersion = Application.Current.MainWindow.GetType().Assembly.GetName().Version;

                LoadSettings();
            }
            catch (Exception) { return false; }
            return true;
        }

        private void LoadSettings()
        {
            // Загружаем config.ini
            if (File.Exists(Properties.Resources.SettingsPathMultipack))
            {
                // Загружаем данные
                string pathINI = Directory.GetCurrentDirectory() + @"\" + Properties.Resources.SettingsPathMultipack;

                MultipackDate = new IniFile(pathINI).IniReadValue("protanki", "date");
                MultipackType = new IniFile(pathINI).IniReadValue("protanki", "type").ToLower();
                MultipackVersion = new Version(TanksVersionPrefix + "." + new IniFile(pathINI).IniReadValue("protanki", "version"));
                Lang = new IniFile(pathINI).IniReadValue("protanki", "language");
            }
            else
            {
                // Мультипак не обнаружен
                new Classes.Debug().Message(ProductName, new LocalInterface.Language().DynamicLanguage("noMods", Lang)).Wait();
            }

            // Загружаем настройки лаунчера
            if (!File.Exists("settings.xml")) new Classes.Update().SaveFromResources().Wait();

            if (File.Exists("settings.xml"))
            {
                XDocument doc = XDocument.Load("settings.xml");

                UpdateNotify = doc.Root.Element("notification") != null ? doc.Root.Element("notification").Value : String.Empty;
                ShowVideoNotify = doc.Root.Element("info") != null ? (doc.Root.Element("info").Attribute("video") != null ? (doc.Root.Element("info").Attribute("video").Value == "True") : true) : true;

                if (doc.Root.Element("settings") != null)
                {
                    AutoKill = doc.Root.Element("settings").Attribute("kill") != null ? doc.Root.Element("settings").Attribute("kill").Value == "True" : false;
                    AutoForceKill = doc.Root.Element("settings").Attribute("force") != null ? doc.Root.Element("settings").Attribute("force").Value == "True" : false;

                    AutoAero = doc.Root.Element("settings").Attribute("aero") != null ? doc.Root.Element("settings").Attribute("aero").Value == "True" : false;
                    AutoVideo = ReadCheckStateBool(doc, "video");
                    AutoWeak = doc.Root.Element("settings").Attribute("weak") != null ? doc.Root.Element("settings").Attribute("weak").Value == "True" : false;
                    AutoCPU = doc.Root.Element("settings").Attribute("balance") != null ? doc.Root.Element("settings").Attribute("balance").Value == "True" : false;
                }

                if (doc.Root.Element("common.test") != null) CommonTest = true;
            }
            else
            {
                // Файл настроек не обнаружен
                new Classes.Debug().Message(ProductName, new LocalInterface.Language().DynamicLanguage("noSettings", Lang)).Wait();
            }
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
    }
}
