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

            // Запускаем прелоадер
            fLoader fLoader = new fLoader();
            fLoader.Show();

            UpdateLauncher update = new UpdateLauncher(); // Инициализируем обновление библиотек
            update.Check().Wait();
            
            fLoader.Close();

            Application.Run(new fIndex());
        }
    }
}
