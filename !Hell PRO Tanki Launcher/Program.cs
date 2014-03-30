using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace _Hell_PRO_Tanki_Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {

                framework framework = new framework();
                if (framework.Check())
                {
                    update_launcher update = new update_launcher();
                    update.CheckLocal(true);

                    Application.Run(new fIndex());
                }
                else
                {
                    Process.GetCurrentProcess().Close();
                }
            }
            catch (Exception)
            {
                if (!File.Exists("restart.exe"))
                {
                    var client = new WebClient();
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/restart.exe"), "restart.exe");
                }

                if (!File.Exists("LanguagePack.dll"))
                {
                    var client = new WebClient();
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/LanguagePack.dll"), "LanguagePack.dll");

                    // Restart software
                    Process.Start("restart.exe", "\"" + Process.GetCurrentProcess().ProcessName + "\"");
                    Process.GetCurrentProcess().Kill();
                }
            }
        }
    }
}
