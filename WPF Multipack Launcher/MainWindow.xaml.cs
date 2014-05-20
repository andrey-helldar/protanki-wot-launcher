using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
        LocalInterface.Language Language = new LocalInterface.Language();


        /*********************
         * Functions
         * *******************/

        public MainWindow()
        {
            InitializeComponent();
            MouseDown += delegate { DragMove(); };

            //OverlayPanel();

            LocalInterface.Start().Wait();
            Variables.Start().Wait();
        }

        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Factory.StartNew(()=>SetBack());
            SetBackground();

            DataLoading().Wait();

            lCaption.Content = Variables.ProductName + " (" + Language.DynamicLanguage("WindowCaption", Variables.Lang, Variables.MultipackType) + ")";
            lMultipackVersion.Content = MultipackVersion().Result;
            lLauncherVersion.Content = LauncherVersion().Result;

            CheckUpdates().Wait(); // Check multipack & tanks updates

            new Classes.POST().CountUsers(); // Запускаем обновление статистики
        }

        private async Task OverlayPanel(string page = null)
        {
            // Создаем панель
            StackPanel panel = new StackPanel();
            panel.Name = "pOverlayPanel";
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.VerticalAlignment = VerticalAlignment.Center;
            panel.Width = MainForm.Width;
            panel.Height = MainForm.Height;
            panel.Opacity = 0.8;
            panel.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/" + Application.Current.MainWindow.GetType().Assembly.GetName().Name + ";component/Resources/back_6.jpg")));

            Label label = new Label();
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            label.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            label.Width = MainForm.Width;
            label.Height = MainForm.Height;
            label.FontSize = 20;
            label.Content = "Loading data... Please, wait...";

            panel.Children.Add(label);
            RegisterName("pOverlayPanel", panel);
            Content = panel;
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

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", @"/c net start uxsms"));
        }

        private async Task CheckUpdates()
        {
            try
            {
                Classes.POST POST = new Classes.POST();
                string status = String.Empty;

                if (!File.Exists("settings.xml")) new Classes.Update().SaveFromResources().Wait();

                XDocument docSettings = XDocument.Load("settings.xml");
                if (Variables.CommonTest)
                    if (docSettings.Root.Element("common.test") == null) docSettings.Root.Add(new XElement("common.test", null));
                    else
                        if (docSettings.Root.Element("common.test") != null) docSettings.Root.Element("common.test").Remove();
                docSettings.Save("settings.xml");


                XDocument doc = XDocument.Load(Properties.Resources.XmlPro);
                Dictionary<string, string> json = new Dictionary<string, string>();

                json.Add("code", Properties.Resources.Code);
                json.Add("user", new Variables.Variables().GetUserID().Result);
                json.Add("youtube", Properties.Resources.Youtube);
                json.Add("test", Variables.CommonTest ? "1" : "0");
                json.Add("version", Variables.TanksVersion.ToString());
                json.Add("lang", Variables.Lang);

                try
                {
                    Dictionary<string, string> sendStatus = POST.FromJson(POST.Send(Properties.Resources.DeveloperWotVersion, "data=" + POST.Json(json)));
                    Variables.UpdateTanksVersion = Convert.ToInt32(sendStatus["count"]) > new Variables.Variables().Accept ? new Version(sendStatus["id"]) : Variables.TanksVersion;
                }
                catch (Exception) { Variables.UpdateTanksVersion = new Version("0.0.0.0"); }


                var remoteJson = POST.JsonResponse(Properties.Resources.JsonUpdates);
                //Variables.UpdateTanksVersion = new Version(tanksPrefixVersion + remoteJson[modpackType]["version"].ToString());
                Variables.UpdateTanksVersion = Variables.Version(remoteJson[Variables.MultipackType]["version"].ToString());

                Variables.UpdateTanks = Variables.TanksVersion < Variables.UpdateTanksVersion; // Сравниваем версии танков
                Variables.UpdateMultipack = Variables.MultipackVersion < Variables.UpdateMultipackVersion; // Сравниваем версии мультипака

                if (Variables.UpdateMultipack)
                {
                    Variables.UpdateMessage = POST.DataRegex(remoteJson[Variables.MultipackType]["changelog"][Variables.Lang].ToString());
                    Variables.UpdateLink = remoteJson[Variables.MultipackType]["download"].ToString();
                    status += Language.DynamicLanguage("llActuallyNewMods", Variables.Lang) + ": " + Variables.UpdateMultipackVersion.ToString() + Environment.NewLine;
                }

                if (Variables.UpdateTanks)
                {
                    status += Language.DynamicLanguage("llActuallyNewGame", Variables.Lang) + ": " + Variables.UpdateTanksVersion.ToString() + Environment.NewLine;
                    bPlay.IsEnabled = false;
                }
                else
                    bPlay.IsEnabled = true;


                if (Variables.UpdateMultipack || Variables.UpdateTanks) // Если есть одно из обновлений
                {
                    bUpdate.IsEnabled = true; // Включаем кнопку обновлений

                    if (Variables.UpdateMultipack) lStatusUpdates.Content = Language.DynamicLanguage("llActuallyNewMods", Variables.Lang);
                    if (Variables.UpdateTanks && !Variables.UpdateMultipack) lStatusUpdates.Content = Language.DynamicLanguage("llActuallyNewGame", Variables.Lang);

                    /// ****************************************
                    /// ВСТАВИТЬ ОКНО ВЫВОДА СПИСКА ОБНОВЛЕНИЙ
                    /// ****************************************
                }
                else
                {
                    bUpdate.IsEnabled = false; // Выключаем кнопку обновлений
                    lStatusUpdates.Content = Language.DynamicLanguage("llActuallyActually", Variables.Lang);
                }
            }
            catch (Exception ex) { new Classes.Debug().Save("MainForm", "CheckUpdates()", ex.Message); }
        }
    }
}