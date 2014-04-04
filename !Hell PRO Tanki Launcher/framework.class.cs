using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Windows.Forms;

namespace _Hell_PRO_Tanki_Launcher
{
    class framework
    {
        Debug Debug = new Debug();

        public bool Check()
        {
            string mess = "";
            List<string> frameworkLinks = new List<string>();

            // v2.0.50727
            /*var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727");
            if (key) != null)
            {
                if ((int)key.GetValue("Install") != 1) { 
                    mess += ".NET Framework " + (string)key.GetValue("Version") + " not installed!" + Environment.NewLine;                        
                    frameworkLinks.Add(isX64() ? "http://www.microsoft.com/ru-ru/download/details.aspx?id=6041" : "http://www.microsoft.com/ru-ru/download/details.aspx?id=1639");
                }
            }
            else
            {
                mess += ".NET Framework " + (string)key.GetValue("Version") + " not installed!" + Environment.NewLine;
            }*/


            // v3.0.30729
            /*var key30 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0");
            if (key30) != null)
            {
                if ((int)key30.GetValue("Install") != 1) { mess += ".NET Framework " + (string)key30.GetValue("Version") + " not installed!" + Environment.NewLine; }
            }
            else
            {
                mess += ".NET Framework " + (string)key30.GetValue("Version") + " not installed!" + Environment.NewLine;
            }*/


            // v3.5
            /*var key35 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5");
            if (key35 != null)
            {
                if ((int)key35.GetValue("Install") != 1) { mess += ".NET Framework " + (string)key35.GetValue("Version") + " not installed!" + Environment.NewLine; }
            }
            else
            {
                mess += ".NET Framework " + (string)key35.GetValue("Version") + " not installed!" + Environment.NewLine;
            }*/


            // v4.0
            try
            {
                var key40 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4.0\Client");
                if (key40 != null)
                {
                    if ((int)key40.GetValue("Install") != 1)
                    {
                        mess += ".NET Framework " + (string)key40.GetValue("Version") + " not installed!" + Environment.NewLine;
                        frameworkLinks.Add("http://www.microsoft.com/ru-ru/download/details.aspx?id=24872");
                    }
                }
                else
                {
                    mess += ".NET Framework " + (string)key40.GetValue("Version") + " not installed!" + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Debug.Save("public void getFramework()", "v4.0", ex.Message);
            }

            try
            {
                if (mess.Length > 0)
                {
                    foreach (string link in frameworkLinks)
                    {
                        Process.Start(link);
                    }
                    MessageBox.Show(fIndex.ActiveForm, "Для корректной работы приложения требуется установка следующих пакетов .NET FRamework:" + Environment.NewLine + mess + Environment.NewLine +
                    "---------------------------------------------------" + Environment.NewLine +
                    "ВНИМАНИЕ! В Вашем браузере открыты ссылки на страницы для скачивания нужных Вам библиотек .NET Framework с сайта microsoft.com", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Save("public void getFramework()", "Show message", ex.Message);
                return false;
            }
        }
    }
}
