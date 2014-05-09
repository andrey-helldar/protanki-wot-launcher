using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using _Hell_PRO_Tanki_Launcher.UserInterface;

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

            // Запускаем прелоадер
            using (var fLoader = new fLoader())
            {
                fLoader.Show();

                UpdateLauncher update = new UpdateLauncher(); // Инициализируем обновление библиотек
                Task.Factory.StartNew(() => update.Check()).Wait();

                fLoader.Close();
            }

            using (var fIndex = new fIndex())
                SingleApplication.Run(fIndex);
        }
    }
}
