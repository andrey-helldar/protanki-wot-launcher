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
        Classes.POST POST = new Classes.POST();
        Classes.Debug Debug = new Classes.Debug();

        private string downloadLink = String.Empty;


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
                    MainWindow.JsonSettingsSet("info.notification", new Classes.Variables().VersionFromSharp(newVersion.Content.ToString()).ToString());

                if (downloadLink!=String.Empty)
                    Process.Start(downloadLink);

                MainWindow.LoadingPanelShow(1);

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
                    }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Settings.xaml", "bClose_Click()", ex.Message, ex.StackTrace)); }
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

        public void MultipackUpdate()
        {
            try
            {
                string lang = (string)MainWindow.JsonSettingsGet("info.language");
                string mType = (string)MainWindow.JsonSettingsGet("multipack.type");
                string thisVersion = (string)MainWindow.JsonSettingsGet("multipack.version");
                downloadLink = (string)MainWindow.JsonSettingsGet("multipack.link");

                var json = POST.JsonResponse(Properties.Resources.JsonUpdates);
                tbContent.Text = (string)json.SelectToken(mType + ".changelog." + lang);
                newVersion.Content = new Version((string)json.SelectToken("version"));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Update.xaml", "MultipackUpdate()", ex.Message, ex.StackTrace)); }
        }

        private void PageUpdate_Loaded(object sender, RoutedEventArgs e)
        {
            try { MainWindow.LoadingPanelShow(); }
            catch (Exception) { }
        }
    }
}
