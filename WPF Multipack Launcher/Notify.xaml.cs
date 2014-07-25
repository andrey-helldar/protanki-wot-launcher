using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Notify.xaml
    /// </summary>
    public partial class Notify : Window
    {
        public string DownloadLink = String.Empty;


        public Notify()
        {
            InitializeComponent();
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void bDownload_Click(object sender, RoutedEventArgs e)
        {
            try { Process.Start(DownloadLink); }
            catch (Exception ex) { new Classes.Debug().Save("MainNotify", "bDownload_Click()", "URL: " + DownloadLink, ex.Message); }

            Close();
        }
    }
}
