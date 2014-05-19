using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*********************
         * Variables
         * *******************/
        LocalInterface.LocInterface LocalInterface = new LocalInterface.LocInterface();
        Variables.Variables Variables = new Variables.Variables();


        /*********************
         * Functions
         * *******************/

        public MainWindow()
        {
            InitializeComponent();
            MouseDown += delegate { DragMove(); };

            LocalInterface.Start();
            Variables.Start();
        }

        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Factory.StartNew(()=>SetBack());
            SetBackground();

            DataLoading();

            lCaption.Content = Variables.ProductName;
            lMultipackVersion.Content = MultipackVersion().Result;
            lLauncherVersion.Content = LauncherVersion().Result;
        }

        private async Task SetBackground()
        {
            string uri = @"pack://application:,,,/Multipack Launcher;component/Resources/back_{0}.jpg";

            while (Variables.BackgroundLoop)
            {
                try
                {
                    if (Variables.BackgroundIndex < 1 || Variables.BackgroundIndex > Variables.BackgroundMax) Variables.BackgroundIndex = 1;

                    try { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, (Variables.BackgroundIndex++).ToString())))); }
                    catch (Exception) { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, (Variables.BackgroundIndex - 1).ToString())))); }
                }
                catch (Exception) { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, "1")))); }

                await Task.Delay(Variables.BackgroundDelay);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Multipack Launcher;component/Resources/back_2.jpg")));
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
                OpenLink(Properties.Resources.DeveloperLinkSite);
        }

        private async Task DataLoading()
        {
            Variables.MultipackVersion = new Version("0.9.0.10");
        }

        private async Task<string> MultipackVersion()
        {
            return LocalInterface.VersionToSharp(Variables.MultipackVersion).Result;
        }

        private async Task<string> LauncherVersion()
        {
            return LocalInterface.VersionToSharp(Variables.ProductVersion).Result;
        }

        private async Task OpenLink(string url)
        {
            try
            {
                Process.Start(Properties.Resources.DeveloperLinkSite);
            }
            finally { }
        }
    }
}