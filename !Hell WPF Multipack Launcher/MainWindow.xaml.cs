using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public static System.Windows.Forms.NotifyIcon Notifier { get { return notifier; } }
        private static System.Windows.Forms.NotifyIcon notifier;

        public static Frame MainFrame0 { get { return mainFrame; } }
        private static Frame mainFrame;

        public static XDocument XmlDocument { get { return xmlDocument; } }
        private static XDocument xmlDocument;

        /*********************
         * Variables
         * *******************/
        Classes.Variables Variables = new Classes.Variables();
        Classes.Debug Debug = new Classes.Debug();
        Classes.Optimize Optimize = new Classes.Optimize();

        public static string MultipackDate = "1970-1-1";
        public static string ProductName = String.Empty;


        /*********************
         * Functions
         * *******************/

        public static bool Navigator(string page = "General", string from = null)
        {
            try { MainFrame0.NavigationService.Navigate(new Uri(page + ".xaml", UriKind.Relative)); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return true;
        }

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                MouseDown += delegate { DragMove(); };

                // Делаем общей иконку в трее
                notifier = this.notifyIcon;
                this.Closing += delegate { notifier = null; };

                // Делаем общим фрейм
                mainFrame = this.MainFrame;
                this.Closing += delegate { mainFrame = null; };

                // Загружаем настройки из XML-файла
                if(File.Exists(Variables.SettingsPath))
                    xmlDocument = XDocument.Load(Variables.SettingsPath);
                this.Closing += delegate { xmlDocument = null; };

                Navigator("Loading"); // Изменение страницы
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "MainWindow()", ex.Message)); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                Task.Factory.StartNew(() => Variables.Start()).Wait();
                MultipackDate = Variables.MultipackDate;
                ProductName = Variables.ProductName;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded()", ex.Message)).Wait();
                Task.Factory.StartNew(() => Debug.Restart());
            }

            try
            {
                Stream iconStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + Variables.ProductName + ";component/Resources/WOT.ico")).Stream;
                if (iconStream != null) notifyIcon.Icon = new System.Drawing.Icon(iconStream);
                notifyIcon.Visible = true;
                notifyIcon.Text = Variables.ProductName;
                notifyIcon.BalloonTipClicked += new EventHandler(NotifyClick);
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded()", "iconStream", ex.Message)); }


            try { MainFrame.NavigationService.Navigate(new Uri("General.xaml", UriKind.Relative)); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "General form loading", ex.Message)); }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIcon.Dispose();

            xmlDocument.Save(Variables.SettingsPath);
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            try { Close(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bClose_Click()", ex.Message)); }
        }

        private void bMinimize_Click(object sender, RoutedEventArgs e)
        {
            try { this.WindowState = WindowState.Minimized; }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bMinimize_Click()", ex.Message)); }
        }

        private void NotifyClick(object sender, EventArgs e)
        {
            try { OpenLink(Variables.notifyLink); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "NotifyClick()", "Link: " + Variables.notifyLink, ex.Message)); }
        }

        private void OpenLink(string url)
        {
            try { Process.Start(url); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "OpenLink()", "URL = " + url, ex.Message)); }
        }

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (File.Exists(Variables.PathTanks + "WorldOfTanks.exe"))
                    {
                        Optimize.Start(Variables.WinXP);

                        Optimize.Start(
                                GetElement("settings", "winxp"),
                                GetElement("settings", "kill"),
                                GetElement("settings", "force"),
                                GetElement("settings", "aero"),
                                GetElement("settings", "video"),
                                GetElement("settings", "weak"),
                                true
                            );

                        Process.Start(new ProcessStartInfo(Variables.PathTanks + "WorldOfTanks.exe"));
                    }
                    else
                        MessageBox.Show("Клиент игры не обнаружен!");
                }
                catch (Exception ex) { Debug.Save("MainWindow", "bPlay_Click()", ex.Message); }
            });
        }

        private void bAirus_Click(object sender, RoutedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo(Properties.Resources.Developer)); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "bAirus_Click()", ex.Message, "Link: " + Properties.Resources.Developer)); }
        }

        private void bLauncherWOT_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (File.Exists(Variables.PathTanks + "WoTLauncher.exe"))
                    {
                        Optimize.Start(Variables.WinXP);
                        Process.Start(new ProcessStartInfo(Variables.PathTanks + "WoTLauncher.exe"));
                    }
                    else
                        MessageBox.Show("Клиент игры не обнаружен!");
                }
                catch (Exception ex) { Debug.Save("MainWindow", "bLauncher_Click()", ex.Message); }
            });
        }
    }
}
