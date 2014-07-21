using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.IO;

namespace WPF_Multipack_Launcher.Classes
{
    class Debug
    {
        public void Save(string caption, params string[] args)
        {
            try
            {
                if (Properties.Resources.Debug == "True")
                {
                    string export = String.Empty,
                           template = "***************************************************" + Environment.NewLine +
                                      "*    Function: " + caption + Environment.NewLine +
                                      "***************************************************" + Environment.NewLine +
                                      "{0}",
                           split = Environment.NewLine + "------------------" + Environment.NewLine;

                    foreach (string str in args)
                        export += str + split;

                    export = String.Format(template, export);

                    if (!Directory.Exists("temp")) { Directory.CreateDirectory("temp"); }
                    File.WriteAllText(@"temp\" + String.Format("{0}_{1}.debug", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(), DateTime.Now.ToString("yyyy-MM-dd_h-m-s.ffffff")), export, Encoding.UTF8);
                }
            }
            catch (Exception) { }
        }

        public void Message(string caption, string text)
        {
            //MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
