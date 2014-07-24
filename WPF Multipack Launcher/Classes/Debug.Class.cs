using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Newtonsoft.Json;

namespace WPF_Multipack_Launcher.Classes
{
    class Debug
    {
        public void Save(string module, string func, params string[] args)
        {
            try
            {
                string version = Application.Current.GetType().Assembly.GetName().Version.ToString();

                Dictionary<string, string> jData = new Dictionary<string, string>();
                jData.Add("uid", new Variables.Variables().GetUserID());
                jData.Add("version", version);
                jData.Add("date", DateTime.Now.ToString("yyyy-MM-dd h-m-s"));
                jData.Add("module", module);
                jData.Add("function", func);

                for (int i = 0; i < args.Length; i++)
                    jData.Add("param" + i.ToString(), args[i]);

                if (!Directory.Exists("temp")) { Directory.CreateDirectory("temp"); }
                string filename = String.Format("{0}_{1}.debug", version, DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff"));
                File.WriteAllText(@"temp\" + filename, JsonConvert.SerializeObject(jData), Encoding.UTF8);
            }
            finally { }
        }

        public void Message(string caption, string text)
        {
            //MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
