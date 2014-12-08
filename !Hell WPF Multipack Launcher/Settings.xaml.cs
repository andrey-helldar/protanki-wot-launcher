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
        Classes.Debugging Debugging = new Classes.Debugging();
        Classes.Language Lang = new Classes.Language();

        private bool openedPage = true;
        string lang = (string)MainWindow.JsonSettingsGet("info.language");


        public Settings()
        {
            InitializeComponent();

            bClose.Content = Lang.Set("PageSettings", "bClose", lang);
            tbSettingsTitle.Text = Lang.Set("PageSettings", "tbSettingsTitle", lang);
            tbSettingsSubTitle.Text = Lang.Set("PageSettings", "tbSettingsStShare", lang);
            bChangeSettingsPage.Content = Lang.Set("PageSettings", "tbSettingsStProcesses", lang);
            bClearAutorization.Content = Lang.Set("PageSettings", "bClearAutorization", lang);

            // Если блок токена найден - делаем доступным, иначе вырубаем
            bClearAutorization.IsEnabled = MainWindow.jSettings["token"] != null ;

            try { SettingsFrame.NavigationService.Navigate(new Uri("SettingsGeneral.xaml", UriKind.Relative)); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Settings.xaml", "Settings()", ex.Message, ex.StackTrace)); }
        }

        private void bChangeSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            if (openedPage)
            {
                try
                {
                    SettingsFrame.NavigationService.Navigate(new Uri("SettingsProcesses.xaml", UriKind.Relative));
                    bChangeSettingsPage.Content = Lang.Set("PageSettings", "tbSettingsStShare", lang);
                    tbSettingsSubTitle.Text = Lang.Set("PageSettings", "tbSettingsStProcesses", lang);
                    openedPage = false;
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Settings.xaml", "bChangeSettingsPage_Click()", ex.Message, ex.StackTrace)); }
            }
            else
            {
                try
                {
                    SettingsFrame.NavigationService.Navigate(new Uri("SettingsGeneral.xaml", UriKind.Relative));
                    bChangeSettingsPage.Content = Lang.Set("PageSettings", "tbSettingsStProcesses", lang);
                    tbSettingsSubTitle.Text = Lang.Set("PageSettings", "tbSettingsStShare", lang);
                    openedPage = true;
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Settings.xaml", "bChangeSettingsPage_Click()", ex.Message, ex.StackTrace)); }
            }
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PlayBtn.Text = Lang.Set("MainProject", "bPlay", (string)MainWindow.JsonSettingsGet("info.language"));

            Dispatcher.BeginInvoke(new ThreadStart(delegate {
                MainWindow.LoadPage.Content = Lang.Set("PageLoading", "lLoading", (string)MainWindow.JsonSettingsGet("info.language"));
                MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible;

                MainWindow.Flag.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/flag_{1}.png", (string)MainWindow.JsonSettingsGet("info.ProductName"), (string)MainWindow.JsonSettingsGet("info.language"))));
            }));
            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
        }

        private void PageSettings_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try { MainWindow.LoadPage.Visibility = Visibility.Hidden; }
                catch (Exception) { }
            }));
        }

        private void bClearCache_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>{
                try
                {
                    if (MainWindow.MessageShow(Lang.Set("PageSettings", "ClearCacheSure", lang), "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (MainWindow.JsonSettingsRemove("token"))
                        {
                            Dispatcher.BeginInvoke(new ThreadStart(delegate { bClearAutorization.IsEnabled = false; }));
                            MainWindow.MessageShow(Lang.Set("PageSettings", "ClearCacheSuccess", lang));
                        }
                        else
                            MainWindow.MessageShow(Lang.Set("PageSettings", "ClearCacheError", lang));
                    }
                }
                catch (Exception ex) { Debugging.Save("Settings.xaml", "bClearCache_Click()", ex.Message, ex.StackTrace); }
                });
        }
    }
}
