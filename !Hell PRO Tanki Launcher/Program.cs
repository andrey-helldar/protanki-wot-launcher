using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;

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
                Application.Run(new fIndex());
            }
            catch (Exception)
            {
                if (!File.Exists("LanguagePack.dll"))
                {
                    var client = new WebClient();
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/LanguagePack.dll"), "LanguagePack.dll");

                    Thread.Sleep(2000);

                    Application.Run(new fIndex());
                }
            }
        }
    }
}
