using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Multipack_Launcher.Variables
{
    class Variables
    {
        // Product
        public string ProductName = String.Empty;
        public Version ProductVersion = new Version("0.0.0.0");

        // Background
        public int BackgroundMax = 7,
                   BackgroundIndex = 1,
                   BackgroundDelay = 10000;
        public bool BackgroundLoop = true;

        // Multipack
        public string MultipackType = "Base";
        public Version MultipackVersion = new Version("0.0.0.0");

        // Paths
        public string PathTanks = String.Empty;

        // Updates
        public string LinkUpdate = Properties.Resources.LinkVideoAll;

        public bool UpdateMultipack = false,
                    UpdateTanks = false;

        // Local variables
        public bool autoOptimize = false,
                    playGame = false,
                    playLauncher = false;

        public string TempStatus = "Loading status...";


        /********************
         * Functions
         * ******************/

        public async Task Start()
        {
            ProductName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;
            ProductVersion = Application.Current.MainWindow.GetType().Assembly.GetName().Version;
        }
    }
}
