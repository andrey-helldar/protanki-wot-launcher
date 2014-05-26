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
        // notifyIcon
        System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();

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
            WargamingNews();

            // NotifyIcon

            Stream iconStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + Application.Current.MainWindow.GetType().Assembly.GetName().Name + ";component/Resources/WOT.ico")).Stream;
            if (iconStream != null)
                notifyIcon.Icon = new System.Drawing.Icon(iconStream);
            notifyIcon.Visible = true;
            notifyIcon.Text = lCaption.Content.ToString();
            notifyIcon.BalloonTipClicked += new EventHandler(NotifyClick);

            ShowNotify(Language.DynamicLanguage("welcome", Variables.Lang));
            VideoNotify();

            ShowUpdateWindow();
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
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
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

                try // Check updates tanks version
                {
                    Dictionary<string, string> sendStatus = POST.FromJson(POST.Send(Properties.Resources.DeveloperWotVersion, "data=" + POST.Json(json)));
                    Variables.UpdateTanksVersion = Convert.ToInt32(sendStatus["count"]) > new Variables.Variables().Accept ? new Version(sendStatus["id"]) : Variables.TanksVersion;
                }
                catch (Exception) { Variables.UpdateTanksVersion = new Version("0.0.0.0"); }


                var remoteJson = POST.JsonResponse(Properties.Resources.JsonUpdates);
                Variables.UpdateMultipackVersion = Variables.Version(remoteJson[Variables.MultipackType]["version"].ToString());

                Variables.UpdateTanks = Variables.TanksVersion < Variables.UpdateTanksVersion; // Сравниваем версии танков
                Variables.UpdateMultipack = Variables.MultipackVersion < Variables.UpdateMultipackVersion; // Сравниваем версии мультипака

                if (Variables.UpdateMultipack)
                {
                    //Variables.UpdateMessage = POST.DataRegex(remoteJson[Variables.MultipackType]["changelog"][Variables.Lang].ToString());
                    Variables.UpdateMessage = remoteJson[Variables.MultipackType]["changelog"][Variables.Lang].ToString();
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

                    if (Variables.UpdateMultipack) lStatusUpdates.Content = String.Format("{0} ({1})", Language.DynamicLanguage("llActuallyNewMods", Variables.Lang), Variables.UpdateMultipackVersion.ToString());
                    if (Variables.UpdateTanks && !Variables.UpdateMultipack) lStatusUpdates.Content = String.Format("{0} ({1})", Language.DynamicLanguage("llActuallyNewGame", Variables.Lang), Variables.UpdateTanksVersion.ToString());
                    if (Variables.UpdateMultipack && Variables.UpdateMultipack) lStatusUpdates.Content += String.Format(Environment.NewLine + "{0} ({1})", Language.DynamicLanguage("llActuallyNewGame", Variables.Lang), Variables.UpdateTanksVersion.ToString());

                    lStatusUpdates.Foreground = System.Windows.Media.Brushes.Yellow;
                }
                else
                {
                    bUpdate.IsEnabled = false; // Выключаем кнопку обновлений
                    lStatusUpdates.Content = Language.DynamicLanguage("llActuallyActually", Variables.Lang);
                    lStatusUpdates.Foreground = System.Windows.Media.Brushes.GreenYellow;
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
                        //Hyperlink hyperlink = new Hyperlink(new Run(content));
                        Hyperlink hyperlink = new Hyperlink(new Run(TrimText(content, gGrid.ColumnDefinitions[1].ActualWidth - 100, fontSize)));
                        hyperlink.NavigateUri = new Uri(link);
                        hyperlink.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_Open);

                        labelT.Content = hyperlink;
                        labelT.FontSize = fontSize;
                        labelT.ToolTip = content;
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
            finally { }
        }

        private async Task WargamingNews()
        {
            try
            {
                int topOffset = 0,
                    fontSize = 16;

                gGrid.Children.Remove(lLoadingNews);

                XDocument doc = XDocument.Load(Variables.Lang == "ru" ? Properties.Resources.RssWotRU : Properties.Resources.RssWotEn);

                foreach (XElement el in doc.Root.Element("channel").Elements("item"))
                {
                    if (topOffset < gGrid.RowDefinitions[3].Height.Value)
                    {
                        // Date
                        Label label = new Label();
                        label.Foreground = new SolidColorBrush(Colors.LightGray);
                        label.FontSize = fontSize;
                        label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        label.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        label.Margin = new Thickness(5, topOffset, 0, 0);
                        label.Content = FormatDateNews(el.Element("pubDate").Value);
                        Grid.SetRow(label, 3);
                        Grid.SetColumn(label, 2);
                        gGrid.Children.Add(label);

                        // News title
                        Label labelT = new Label();
                        //Hyperlink hyperlink = new Hyperlink(new Run(el.Element("title").Value));
                        Hyperlink hyperlink = new Hyperlink(new Run(TrimText(el.Element("title").Value, gGrid.ColumnDefinitions[2].ActualWidth - 100, fontSize)));
                        hyperlink.NavigateUri = new Uri(el.Element("link").Value);
                        hyperlink.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_Open);

                        labelT.Content = hyperlink;
                        labelT.ToolTip = el.Element("title").Value;
                        labelT.FontSize = fontSize;
                        labelT.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        labelT.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        labelT.Margin = new Thickness(60, topOffset, 0, 0);
                        Grid.SetRow(labelT, 3);
                        Grid.SetColumn(labelT, 2);
                        gGrid.Children.Add(labelT);

                        topOffset += fontSize + 6;
                    }
                    else break;
                }
            }
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

        private string TrimText(string text, double maxWidth = 0, int fontSize = 14)
        {
            try
            {
                if (text.Length > 0 && (int)maxWidth > 0 && TextWidth(text, fontSize) > maxWidth)
                {
                    string result = String.Empty;

                    while (TextWidth(result, fontSize) < (int)maxWidth)
                        result = text.Remove(result.Length+1);

                    return result + "...";
                }
                else
                    return text;
            }
            catch (Exception) { return text; }
        }

        private double TextWidth(string text, int fontSize=14)
        {
            try
            {
                return new FormattedText(text,
                    System.Globalization.CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(lCaption.FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                    fontSize,
                    System.Windows.Media.Brushes.Black
                ).Width;
            }
            catch (Exception) { return 0; }
        }

        private async void ShowNotify(string text, string caption=null)
        {
            try
            {
                caption = caption != null ? caption : lCaption.Content.ToString();
                notifyIcon.ShowBalloonTip(5000, caption, text, System.Windows.Forms.ToolTipIcon.Info);
            }
            finally { }
        }

        private async Task VideoNotify()
        {
            try
            {
                if (!File.Exists("settings.xml")) new Classes.Update().SaveFromResources().Wait();

                XDocument doc = XDocument.Load("settings.xml");

                if (doc.Root.Element("youtube") != null)
                    foreach (var el in doc.Root.Element("youtube").Elements("video")) { YoutubeClass.Delete(el.Value); }
                else doc.Root.Add(new XElement("youtube", null));

                DeleteVideo(); // Перед выводом уведомлений проверяем даты. Все лишние удаляем

                foreach (var el in YoutubeClass.List)
                {
                    await Task.Delay(5000);

                    for (int i = 0; i < 2; i++) // Если цикл прерван случайно, то выжидаем еще 7 секунд перед повторным запуском
                    {
                        while (System.Diagnostics.Process.GetProcessesByName("WorldOfTanks").Length > 0 ||
                            System.Diagnostics.Process.GetProcessesByName("WoTLauncher").Length > 0)
                            await Task.Delay(5000);

                        await Task.Delay(7000);
                    }

                    Variables.notifyLink = el.Link;
                    ShowNotify(Language.DynamicLanguage("viewVideo", Variables.Lang), el.Title);

                    doc.Root.Element("youtube").Add(new XElement("video", el.ID));
                    doc.Save("settings.xml");
                }
            }
            finally { }
        }

        /// <summary>
        /// Если мы удалили 1 пункт из списка, то дальнейший перебор невозможен.
        /// Но используя рекурсию мы повторяем перебор до тех пор, пока все ненужные
        /// элементы не будут удалены из списка. Profit!
        /// </summary>
        /// <returns>Функция как таковая ничего не возвращает</returns>
        private async void DeleteVideo()
        {
            try
            {
                foreach (var el in YoutubeClass.List)
                    if (!Variables.ParseDate(Variables.MultipackDate, el.Date))
                        YoutubeClass.Delete(el.ID);
            }
            catch (Exception) { DeleteVideo(); }
        }

        private void bSettings_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private async void NotifyClick(object sender, EventArgs e)
        {
            await OpenLink(Variables.notifyLink);
        }

        private async void ShowUpdateWindow()
        {
            Notify MainNotify = new Notify();
            MainNotify.lCaption.Content = lStatusUpdates.Content;
            MainNotify.lCaption.FontSize = 16;
            MainNotify.tbDescription.Text = new Classes.POST().DataRegex(Variables.UpdateMessage);

            this.Effect = new System.Windows.Media.Effects.BlurEffect();
            MainNotify.ShowDialog();

            this.Effect = null;
        }
    }
}