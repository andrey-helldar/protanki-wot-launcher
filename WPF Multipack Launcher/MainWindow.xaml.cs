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
        Classes.Debug Debug = new Classes.Debug();
        Classes.Optimize Optimize = new Classes.Optimize();


        /*********************
         * Functions
         * *******************/

        public MainWindow()
        {
            InitializeComponent();
            MouseDown += delegate { DragMove(); };

            LocalInterface.Start().Wait();
            Variables.Start().Wait();
        }

        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Factory.StartNew(()=>SetBack());
            SetBackground();

            DataLoading().Wait();

            lCaption.Content = Variables.ProductName;
            lMultipackVersion.Content = MultipackVersion().Result;
            lLauncherVersion.Content = LauncherVersion().Result;
        }

        private async Task SetBackground()
        {
            string uri = @"pack://application:,,,/" + Application.Current.MainWindow.GetType().Assembly.GetName().Name + ";component/Resources/back_{0}.jpg";

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

        private void Hyperlink_Open(object sender, RequestNavigateEventArgs e)
        {
            try { Process.Start(e.Uri.AbsoluteUri); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { }
        }

        private async Task DataLoading()
        {
            lMultipackVersion.Content = Variables.MultipackVersion.ToString();
        }

        private async Task<string> MultipackVersion()
        {
            return LocalInterface.VersionToSharp(Variables.MultipackVersion).Result;
        }

        private async Task<string> LauncherVersion()
        {
            return LocalInterface.VersionToSharp(Variables.ProductVersion).Result;
        }

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Optimize.Start(false).Wait();
                Process.Start(Variables.PathTanks + "WorldOfTanks.exe");
            }
            catch (Exception ex) { Debug.Save("MainWindow", "bPlay_Click()", ex.Message).Wait(); }
        }

        private void bLauncher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Optimize.Start(false).Wait();
                Process.Start(Variables.PathTanks + "WoTLauncher.exe");
            }
            catch (Exception ex) { Debug.Save("MainWindow", "bLauncher_Click()", ex.Message).Wait(); }
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            /// Tanks updates
            if (Variables.UpdateTanks)
            {
                try { Process.Start(Variables.PathTanks + "WoTLauncher.exe"); }
                catch (Exception ex) { Debug.Save("MainWindow", "bUpdate_Click()", "UpdateTanks = " + Variables.UpdateTanks.ToString(), ex.Message).Wait(); }
            }

            /// Multipack updates
            if (Variables.UpdateMultipack)
            {
                try { OpenLink(Variables.UpdateLink).Wait(); }
                catch (Exception ex) { Debug.Save("MainWindow", "bUpdate_Click()", "LinkUpdate = " + Variables.UpdateLink, ex.Message).Wait(); }
            }
        }

        private async Task OpenLink(string url)
        {
            try { Process.Start(url); }
            catch (Exception ex) { Debug.Save("MainWindow", "OpenLink()", "URL = " + url, ex.Message).Wait(); }
        }

        private void bOptimize_Click(object sender, RoutedEventArgs e)
        {
            try { Optimize.Start(true).Wait(); }
            catch (Exception ex) { Debug.Save("MainWindow", "bOptimize_Click()", ex.Message).Wait(); }
        }
    }
}