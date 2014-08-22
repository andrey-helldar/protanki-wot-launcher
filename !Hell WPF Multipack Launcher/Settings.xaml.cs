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

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        private bool openedPage = true;

        public Settings()
        {
            InitializeComponent();

            try { SettingsFrame.NavigationService.Navigate(new Uri("SettingsOptimize.xaml", UriKind.Relative)); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void bChangeSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            if (openedPage)
            {
                try
                {
                    SettingsFrame.NavigationService.Navigate(new Uri("SettingsProcesses.xaml", UriKind.Relative));
                    bChangeSettingsPage.Content = "Оптимизация";
                    openedPage = false;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
            {
                try
                {
                    SettingsFrame.NavigationService.Navigate(new Uri("SettingsOptimize.xaml", UriKind.Relative));
                    bChangeSettingsPage.Content = "Процессы";
                    openedPage = true;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("General", "Settings.xaml");
        }
    }
}
