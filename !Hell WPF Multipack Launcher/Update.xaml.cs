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
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Page
    {
        Classes.Debug Debug = new Classes.Debug();

        public Update()
        {
            InitializeComponent();
            Task.Factory.StartNew(() => MultipackUpdate());
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbNotify.IsChecked == true)
                    MainWindow.JsonSettingsSet("info.notification", MainWindow.JsonSettingsGet("multipack.new_version"));

                string link = (string)MainWindow.JsonSettingsGet("multipack.link");
                if (link != String.Empty) Process.Start(link);

                MainWindow.LoadingPanelShow(1);

                Task.Factory.StartNew(() =>
                {
                    try { Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); })); }
                    catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("Update.xaml", "bUpdate_Click()", ex0.Message, ex0.StackTrace)); }
                });
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Update.xaml", "bUpdate_Click()", ex.Message, ex.StackTrace)); }
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoadingPanelShow(1);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Update.xaml", "bCancel_Click()", ex.Message, ex.StackTrace)); }
            });
        }

        private void MultipackUpdate()
        {
            try
            {
                Classes.Language Lang = new Classes.Language();
                string lang = (string)MainWindow.JsonSettingsGet("info.language");

                /*
                 *      Применяем локализацию интерфейса
                 */
                gbCaption.Content = Lang.Set("PageUpdate", "gbCaption", lang);
                lDownloadFromLink.Content = Lang.Set("PageUpdate", "lDownloadFromLink", lang);
                cbNotify.Content = Lang.Set("PageUpdate", "cbNotify", lang);
                bUpdate.Content = Lang.Set("PageUpdate", "bUpdate", lang);
                bCancel.Content = Lang.Set("PageUpdate", "bCancel", lang);

                /*
                 *      Подгружаем другие данные
                 */
                newVersion.Content = new Classes.Variables().VersionSharp((string)MainWindow.JsonSettingsGet("multipack.new_version"), false);
                tbContent.Text = (string)MainWindow.JsonSettingsGet("multipack.changelog");
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Update.xaml", "MultipackUpdate()", ex.Message, ex.StackTrace)); }


            try { MainWindow.LoadingPanelShow(); }
            catch (Exception) { }
        }

        private string ParseChangelog(string log) {
            try {
                if (log.Length > 0)
                {

                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Update.xaml", "ParseChangelog()", ex.Message, ex.StackTrace)); }
            return log;
        }
    }
}
