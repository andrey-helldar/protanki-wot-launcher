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
        Classes.YoutubeVideo YoutubeClass = new Classes.YoutubeVideo();


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

            Youtube();
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
                //Optimize.Start().Wait();
                StartGame();
            }
            catch (Exception ex) { Debug.Save("MainWindow", "bPlay_Click()", ex.Message).Wait(); }
        }

        private void bLauncher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Optimize.Start().Wait();
                Process.Start(Variables.PathTanks + "WoTLauncher.exe");
            }
            catch (Exception ex) { Debug.Save("MainWindow", "bLauncher_Click()", ex.Message).Wait(); }
        }

        private async void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            /// Tanks updates
            if (Variables.UpdateTanks)
            {
                if (File.Exists(Variables.PathTanks + "WoTLauncher.exe"))
                    try { Process.Start(Variables.PathTanks + "WoTLauncher.exe"); }
                    catch (Exception ex) { Debug.Save("MainWindow", "bUpdate_Click()", "UpdateTanks = " + Variables.UpdateTanks.ToString(), ex.Message).Wait(); }
                else
                    await Debug.Message(Variables.ProductName, Language.DynamicLanguage("noTanks", Variables.Lang));
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
            try { Optimize.Start(Variables.AutoKill, Variables.AutoForceKill, Variables.AutoAero, Variables.AutoVideo, Variables.AutoWeak, true).Wait(); }
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
            catch (Exception ex) { Debug.Save("MainForm", "CheckUpdates()", ex.Message).Wait(); }
        }

        private async void StartGame(string file = "WorldOfTanks.exe")
        {
            try
            {
                if (File.Exists(Variables.PathTanks + file))
                {
                    State().Wait();
                    Process.Start(new ProcessStartInfo("cmd", @"/c " + Variables.PathTanks + file));
                }
            }
            finally { }
        }

        /// <summary>
        ///     0   Не закрывать лаунчер
        ///     1   Сворачивать в трей при запуске игры
        ///     2   Минимизировать лаунчер на панель задач
        ///     3   Закрывать при запуске игры
        /// </summary>
        /// <param name="select"></param>
        private async Task State()
        {
            if (!File.Exists("settings.xml")) new Classes.Update().SaveFromResources().Wait();

            XDocument docState = XDocument.Load("settings.xml");
            string select = "0";

            if (docState.Root.Element("launcher") != null)
                if (docState.Root.Element("launcher").Attribute("minimize") != null)
                    select = docState.Root.Element("launcher").Attribute("minimize").Value;

            switch (select)
            {
                case "1": Hide(); break;
                case "2": WindowState = System.Windows.WindowState.Minimized; break;
                case "3": Close(); break;
                default: break;
            }
        }

        public async Task Youtube()
        {
            try
            {
                int topOffset = 0,
                    fontSize = 16;

                XDocument doc = XDocument.Load(String.Format(Properties.Resources.RssYoutube, Properties.Resources.Youtube));
                XNamespace ns = "http://www.w3.org/2005/Atom";

                gGrid.Children.Remove(lLoadingVideo);

                foreach (XElement el in doc.Root.Elements(ns + "entry"))
                {
                    string link = String.Empty;
                    foreach (XElement subEl in el.Elements(ns + "link")) { if (subEl.Attribute("rel").Value == "alternate") { link = subEl.Attribute("href").Value; break; } }

                    string content = (el.Element(ns + "title").Value.IndexOf(" / PRO") >= 0 ? el.Element(ns + "title").Value.Remove(el.Element(ns + "title").Value.IndexOf(" / PRO")) : el.Element(ns + "title").Value);
                    // Write array
                    YoutubeClass.Add(
                        el.Element(ns + "id").Value.Remove(0, 42),
                            content,
                            el.Element(ns + "content").Value.Remove(256) + (el.Element(ns + "content").Value.Length > 256 ? "..." : ""),
                            link,
                            el.Element(ns + "published").Value.Remove(10)
                        );

                    // Creating window controls
                    if (topOffset < gGrid.RowDefinitions[3].Height.Value)
                    {
                        // Date
                        Label label = new Label();
                        label.Foreground = new SolidColorBrush(Colors.LightGray);
                        label.FontSize = fontSize;
                        label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        label.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        label.Margin = new Thickness(5, topOffset, 0, 0);
                        label.Content = FormatDateNews(el.Element(ns + "published").Value.Remove(10));
                        Grid.SetRow(label, 3);
                        Grid.SetColumn(label, 1);
                        gGrid.Children.Add(label);

                        // News title
                        Label labelT = new Label();
                        Hyperlink hyperlink = new Hyperlink(new Run(content));
                        hyperlink.NavigateUri = new Uri(link);
                        hyperlink.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_Open);

                        labelT.Content = hyperlink;
                        labelT.FontSize = fontSize;
                        labelT.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        labelT.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        labelT.Margin = new Thickness(60, topOffset, 0, 0);
                        Grid.SetRow(labelT, 3);
                        Grid.SetColumn(labelT, 1);
                        gGrid.Children.Add(labelT);

                        topOffset += fontSize + 6;
                    }
                }
            }
            catch (Exception) { }
            finally { }
        }

        private string FormatDateNews(string dt)
        {
            try
            {
                if (dt != null)
                {
                    DateTime nd = DateTime.Parse(dt);
                    return nd.ToString("dd/MM").Replace(".", "/");
                }
                return "null";
            }
            catch (Exception) { return "error"; }
        }
    }
}