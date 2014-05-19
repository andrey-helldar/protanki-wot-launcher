using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Multipack_Launcher.Classes
{
    class Debug
    {
        public async Task Message(string caption, params string[] args){
            if (Properties.Resources.Debug == "True")
            {
                string template = Environment.NewLine + "/*********************************" + Environment.NewLine +
                                  " * {0}" + Environment.NewLine +
                                  "**********************************/" + Environment.NewLine +
                                  "{1}" + Environment.NewLine;

                string export = String.Format(template, args);

                MessageBox.Show(export, caption);
            }
        }

        public async Task Save(string caption, params string[] args)
        {
        }
    }
}
