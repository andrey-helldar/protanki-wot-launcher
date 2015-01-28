using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Donate.xaml
    /// </summary>
    public partial class Donate : Page
    {
        public Donate()
        {
            InitializeComponent();

            Task.Factory.StartNew(() =>
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate { bClose.Content = new Classes.Language().Set("Button", "Close", (string)MainWindow.JsonSettingsGet("info.language")); }));
            });
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try { MainWindow.LoadPage.Visibility = Visibility.Hidden; }
                catch (Exception) { }
            }));
        }

        /// <summary>
        ////Открываем гиперссылку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Параметры</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => new Classes.Debugging().Save("General.xaml", "Hyperlink_RequestNavigate()", "Link: " + e.Uri.AbsoluteUri, ex.Message, ex.StackTrace)); }
        }
    }
}
