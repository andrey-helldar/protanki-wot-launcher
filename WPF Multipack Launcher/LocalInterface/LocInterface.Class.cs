﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace WPF_Multipack_Launcher.LocalInterface
{
    class LocInterface
    {
        private string ProductName = String.Empty;

        public bool loop = true;

        public async Task Start()
        {
            ProductName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;
        }

        public void Message(string text, string caption = null)
        {
            try
            {
                //caption = caption != null ? caption : Application.Current.MainWindow.GetType().Assembly.GetName().Version.ToString();
                caption = caption != null ? caption : this.ProductName;

                MessageBox.Show(caption, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally { }
        }

        public async Task<string> VersionToSharp(Version ver)
        {
            string[] exp = ver.ToString().Split('.');
            return exp[0] + "." + exp[1] + "." + exp[2] + " #" + exp[3];
        }
    }
}
