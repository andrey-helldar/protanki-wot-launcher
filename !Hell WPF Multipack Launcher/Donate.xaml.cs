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

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Donate.xaml
    /// </summary>
    public partial class Donate : Page
    {
        Classes.Language Lang = new Classes.Language();
        string lang = Properties.Resources.Default_Lang;

        public Donate()
        {
            InitializeComponent();

            Task.Factory.StartNew(() =>
            {
                lang = (string)MainWindow.JsonSettingsGet("info.language");

                bClose.Content = Lang.Set("Button", "Close", lang);
            });
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
        }
    }
}
