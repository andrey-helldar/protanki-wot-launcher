using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using _Hell_PRO_Tanki_Launcher.InnerInterface;

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
                // Запускаем прелоадер
                using (var fLoader = new fLoader())
                {
                    fLoader.Show();

                    /* -----------------------------
                     *  Проверяем целостность файла настроек
                     */
                    string patoToSettings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";
                    try { XDocument doc = XDocument.Load(patoToSettings); }
                    catch (Exception)
                    {
                        if (File.Exists(patoToSettings)) File.Delete(patoToSettings);
                        File.WriteAllText(patoToSettings, Properties.Resources.Settings);
                    }
                    /*
                     * -----------------------------*/

                    Task.Factory.StartNew(() => new UpdateLauncher().Check()).Wait(); // Инициализируем обновление библиотек
                    Task.Factory.StartNew(() => new Debug().Delete()).Wait(); // Удаляем старые дебаги

                    Thread.Sleep(5000);

                    fLoader.Close();


                    using (var fIndex = new fIndex())
                        SingleApplication.Run(fIndex);
                }
            }
            catch (Exception ex)
            {
                new Debug().Save("static void Main()", ex.Message);

                Process.Start("restart.exe", "\"" + Process.GetCurrentProcess().ProcessName + ".exe\"");
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
