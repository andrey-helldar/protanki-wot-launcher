using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace WPF_Multipack_Launcher.LocalInterface
{
    class LocInterface
    {
        private string ProductName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;
        public bool loop = true;

        public void Message(string text, string caption = null)
        {
            try
            {
                caption = caption != null ? caption : this.ProductName;
                MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally { }
        }

        public async Task<string> VersionToSharp(Version ver)
        {
            try
            {
                string[] exp = ver.ToString().Split('.');
                return String.Format("{0}.{1}.{2} #{3}", exp[0], exp[1], exp[2], exp[3]);
            }
            catch (Exception) { return "0.0.0 #0"; }
        }

        public async Task<string> VersionPrefix(Version ver)
        {
            try
            {
                string[] exp = ver.ToString().Split('.');
                return String.Format("{0}.{1}.{2}.", exp[0], exp[1], exp[2]);
            }
            catch (Exception) { return "0.0.0."; }
        }
    }
}
