using System;
using System.Collections.Generic;
using System.Linq;
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
            if (File.Exists(Properties.Resources.SettingsPathMultipack))
            {
                // Загружаем данные
                string pathINI = Properties.Resources.SettingsPathMultipack;

                MessageBox.Show("start");
                MultipackDate = new IniFile(pathINI).IniReadValue("protanki", "date");
                MultipackType = new IniFile(pathINI).IniReadValue("protanki", "type").ToLower();
                MessageBox.Show("1");
                MessageBox.Show(new IniFile(pathINI).IniReadValue("protanki", "version"));
                //MultipackVersion = new Version(TanksVersionPrefix + "." + new IniFile(pathINI).IniReadValue("protanki", "version"));
                MessageBox.Show("2");
                Lang = new IniFile(pathINI).IniReadValue("protanki", "language");

                MessageBox.Show(MultipackVersion.ToString());
            }
            else
            {
                MultipackVersion = new Version("1.1.1.1");
                // Мультипак не обнаружен
            }
        }
    }
}
