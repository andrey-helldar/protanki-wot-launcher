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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        Classes.Debug Debug = new Classes.Debug();

        private bool openedPage = true;


        public Settings()
        {
            InitializeComponent();

            try { SettingsFrame.NavigationService.Navigate(new Uri("SettingsGeneral.xaml", UriKind.Relative)); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Settings.xaml", "Settings()", ex.Message, ex.StackTrace)); }
        }

        private void bChangeSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            if (openedPage)
            {
                try
                {
                    try { MainWindow.LoadingPanelShow(1); }
                    catch (Exception) { }

                    SettingsFrame.NavigationService.Navigate(new Uri("SettingsProcesses.xaml", UriKind.Relative));
                    bChangeSettingsPage.Content = "Общие";
                    tbSettingsSubTitle.Text = "Процессы";
                    openedPage = false;
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Settings.xaml", "bChangeSettingsPage_Click()", ex.Message, ex.StackTrace)); }
            }
            else
            {
                try
                {
                    SettingsFrame.NavigationService.Navigate(new Uri("SettingsGeneral.xaml", UriKind.Relative));
                    bChangeSettingsPage.Content = "Процессы";
                    tbSettingsSubTitle.Text = "Общие";
                    openedPage = true;
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Settings.xaml", "bChangeSettingsPage_Click()", ex.Message, ex.StackTrace)); }
            }
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
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

        private void PageSettings_Loaded(object sender, RoutedEventArgs e)
        {
            try { MainWindow.LoadingPanelShow(); }
            catch (Exception) { }
        }
    }
}
