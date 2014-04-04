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

            framework framework = new framework();
            if (framework.Check())
            {
                UpdateLauncher update = new UpdateLauncher();
                update.Check();

                Application.Run(new fIndex());
            }
            else
            {
                Process.GetCurrentProcess().Close();
            }
        }
    }
}
