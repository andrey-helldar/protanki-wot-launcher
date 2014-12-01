﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Task.Factory.StartNew(() => new Classes.Variables().SaveFromResources()).Wait();

            MessageBox.Show(Environment.Version.ToString());

            // hook on error before app really starts
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            base.OnStartup(e);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // put your tracing or logging code here (I put a message box as an example)
            //MessageBox.Show(e.ExceptionObject.ToString());
            Task.Factory.StartNew(() => new Classes.Debugging().Save("App.xaml", "CurrentDomain_UnhandledException", e.ExceptionObject.ToString())).Wait();
        }
    }
}
