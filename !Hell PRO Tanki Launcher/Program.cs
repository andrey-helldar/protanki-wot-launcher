﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;
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

            // Запускаем прелоадер
            try
            {
                using (var fLoader = new fLoader())
                {
                    fLoader.Show();

                    new UpdateLauncher().Check().Wait(); // Инициализируем обновление библиотек
                    Task.Factory.StartNew(() => new Debug().Delete()).Wait(); // Удаляем старые дебаги

                    // Так как нам больше не нужен файл настроек в папке с прогой - удаляем его
                    if (File.Exists("settings.xml")) File.Delete("settings.xml");

                    fLoader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                using (var fIndex = new fIndex())
                    SingleApplication.Run(fIndex);
            }
            catch (Exception ex)
            {                
                Process.Start("restart.exe", "\"" + Process.GetCurrentProcess().ProcessName + ".exe\"");
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
