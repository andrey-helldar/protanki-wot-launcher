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
                // Если файл настроек поврежден, то удаляем его
                if (File.Exists("settings.xml") && new FileInfo("settings.xml").Length == 0) { File.Delete("settings.xml"); }

                // Если в папке лежит уже скачанное обновление, то применяем его,
                if (File.Exists("launcher.update"))
                {
                    // а перед этим, на всякий случай, сверяем версии файлов
                    if (new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion) > new Version(Application.ProductVersion))
                    {
                        Process.Start("updater.exe", "launcher.update \"!Hell PRO Tanki Launcher.exe\"");
                        Process.GetCurrentProcess().Kill();
                    }
                }

                framework framework = new framework();
                if (framework.Check())
                {
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
