﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Multipack_Launcher.Variables
{
    class Variables
    {
        public string ProductName = String.Empty;
        public Version ProductVersion = new Version("0.0.0.0");

        public int BackgroundIndex = 1;
        public bool BackgroundLoop = true;
        public int BackgroundDelay = 3000;

        public async Task Start()
        {
            ProductName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;
            ProductVersion = Application.Current.MainWindow.GetType().Assembly.GetName().Version;
        }
    }
}
