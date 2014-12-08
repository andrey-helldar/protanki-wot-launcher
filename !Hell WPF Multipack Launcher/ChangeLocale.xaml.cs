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
    /// Interaction logic for ChangeLocale.xaml
    /// </summary>
    public partial class ChangeLocale : Page
    {
        Classes.Language Lang = new Classes.Language();
        Classes.Debugging Debugging = new Classes.Debugging();

        public ChangeLocale()
        {
            InitializeComponent();
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                MainWindow.LoadPage.Content = Lang.Set("PageLoading", "lLoading", (string)MainWindow.JsonSettingsGet("info.language"));
                MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible;

                MainWindow.Flag.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/flag_{1}.png", (string)MainWindow.JsonSettingsGet("info.ProductName"), (string)MainWindow.JsonSettingsGet("info.language"))));

                Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

                Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
            }));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try { MainWindow.LoadPage.Visibility = Visibility.Hidden; }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("ChangeLocale.xaml", "Page_Loaded", ex.Message, ex.StackTrace)); }
            }));
        }

        private void lbLocales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);

                MainWindow.JsonSettingsSet("info.language", lbi.Name);
                MainWindow.JsonSettingsSet("info.locale", 2, "int");

                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    bClose.Content = Lang.Set("PageSettings", "bClose", lbi.Name);
                    MainWindow.PlayBtn.Text = Lang.Set("MainProject", "bPlay", lbi.Name);
                    MainWindow.Flag.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/flag_{1}.png", (string)MainWindow.JsonSettingsGet("info.ProductName"), lbi.Name)));

                    try { MainWindow.LoadPage.Visibility = Visibility.Hidden; }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("ChangeLocale.xaml", "lbLocales_SelectionChanged", ex.Message, ex.StackTrace)); }
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("ChangeLocale.xaml", "lbLocales_SelectionChanged", ex.Message, ex.StackTrace)); }
        }
    }
}
