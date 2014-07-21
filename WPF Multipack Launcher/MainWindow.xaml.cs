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
        LocalInterface.Language LocalLanguage = new LocalInterface.Language();
        Variables.Variables Variables = new Variables.Variables();
        Classes.Debug Debug = new Classes.Debug();
        Classes.Optimize Optimize = new Classes.Optimize();
        Classes.YoutubeVideo YoutubeClass = new Classes.YoutubeVideo();


        /*********************
         * Functions
         * *******************/

        public MainWindow()
        {
            InitializeComponent();
            MouseDown += delegate { DragMove(); };

            Variables.Start();
        }

        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(SetBackground).Start();

            lMultipackVersion.Content = Variables.MultipackVersion.ToString();

            lCaption.Content = Variables.ProductName + " (" + LocalLanguage.DynamicLanguage("WindowCaption", Variables.Lang, Variables.MultipackType) + ")";
            lMultipackVersion.Content = LocalInterface.VersionToSharp(Variables.MultipackVersion);
            lLauncherVersion.Content = LocalInterface.VersionToSharp(Variables.ProductVersion);
            
            new Thread(Youtube).Start();
            new Thread(WargamingNews).Start();

            CheckUpdates(); // Check multipack & tanks updates

            // NotifyIcon
            Stream iconStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + Variables.ProductName + ";component/Resources/WOT.ico")).Stream;
            if (iconStream != null)
                notifyIcon.Icon = new System.Drawing.Icon(iconStream);
            notifyIcon.Visible = true;
            notifyIcon.Text = lCaption.Content.ToString();
            notifyIcon.BalloonTipClicked += new EventHandler(NotifyClick);

            ShowNotify(LocalLanguage.DynamicLanguage("welcome", Variables.Lang));
            new Thread(VideoNotify).Start();

            new Thread(ShowUpdateWindow).Start();
        }

        private void SetBackground()
        {
            string uri = @"pack://application:,,,/" + Variables.ProductName + ";component/Resources/back_{0}.jpg";

            if (Variables.BackgroundLoop)
            {
                while (Variables.BackgroundLoop)
                {
                    try
                    {
                        if (Variables.BackgroundIndex < 1 || Variables.BackgroundIndex > Variables.BackgroundMax) Variables.BackgroundIndex = 1;

                        try { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, (Variables.BackgroundIndex++).ToString())))); }
                        catch (Exception) { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, (Variables.BackgroundIndex - 1).ToString())))); }
                    }
                    catch (Exception) { Thread.Sleep(5000); this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, "1")))); }

                    Thread.Sleep(Variables.BackgroundDelay);
                }
            }
            else
            {

                try
                {
                    Variables.BackgroundIndex = new Random().Next(1, 7);
                    this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, Variables.BackgroundIndex.ToString()))));
                }
                catch (Exception) { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, (Variables.BackgroundIndex - 1).ToString())))); }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartGame();
            }
            catch (Exception ex) { Debug.Save("MainWindow", "bPlay_Click()", ex.Message); }
        }

        private void bLauncher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Optimize.Start();
                Process.Start(Variables.PathTanks + "WoTLauncher.exe");
            }
            catch (Exception ex) { Debug.Save("MainWindow", "bLauncher_Click()", ex.Message); }
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            /// Tanks updates
            if (Variables.UpdateTanks)
            {
                if (File.Exists(Variables.PathTanks + "WoTLauncher.exe"))
                    try { Process.Start(Variables.PathTanks + "WoTLauncher.exe"); }
                    catch (Exception ex) { Debug.Save("MainWindow", "bUpdate_Click()", "UpdateTanks = " + Variables.UpdateTanks.ToString(), ex.Message); }
                else
                    Debug.Message(Variables.ProductName, LocalLanguage.DynamicLanguage("noTanks", Variables.Lang));
            }

            /// Multipack updates
            if (Variables.UpdateMultipack)
            {
                try { OpenLink(Variables.UpdateLink); }
                catch (Exception ex) { Debug.Save("MainWindow", "bUpdate_Click()", "LinkUpdate = " + Variables.UpdateLink, ex.Message); }
            }
        }

        private void OpenLink(string url)
        {
            try { Process.Start(url); }
            catch (Exception ex) { Debug.Save("MainWindow", "OpenLink()", "URL = " + url, ex.Message); }
        }

        private void bOptimize_Click(object sender, RoutedEventArgs e)
        {
            try { Optimize.Start(Variables.AutoKill, Variables.AutoForceKill, Variables.AutoAero, Variables.AutoVideo, Variables.AutoWeak, true); }
            catch (Exception ex) { Debug.Save("MainWindow", "bOptimize_Click()", ex.Message); }
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Variables.AutoAero) Process.Start(new ProcessStartInfo("cmd", @"/c net start uxsms"));

            notifyIcon.Dispose();
            Variables.Doc.Save("settings.xml");
        }

        private void CheckUpdates()
        {
            try
            {
                Classes.POST POST = new Classes.POST();
                string status = String.Empty;

                if (Variables.CommonTest)
                    if (Variables.Doc.Root.Element("common.test") == null) Variables.Doc.Root.Add(new XElement("common.test", null));
                    else
                        if (Variables.Doc.Root.Element("common.test") != null) Variables.Doc.Root.Element("common.test").Remove();


                Dictionary<string, string> json = new Dictionary<string, string>();

                json.Add("code", Properties.Resources.Code);
                json.Add("user", new Variables.Variables().GetUserID());
                json.Add("youtube", Properties.Resources.Youtube);
                json.Add("test", Variables.CommonTest ? "1" : "0");
                json.Add("version", Variables.TanksVersion.ToString());
                json.Add("lang", Variables.Lang);
                json.Add("resolution", SystemParameters.PrimaryScreenWidth.ToString() + "x" + SystemParameters.PrimaryScreenHeight.ToString());

                try // Check updates tanks version
                {
                    Dictionary<string, string> postStatus = POST.FromJson(POST.Send(Properties.Resources.DeveloperSite + Properties.Resources.DeveloperWotVersion, "data=" + POST.Json(json)));
                    Variables.UpdateTanksVersion = postStatus["id"] != "0.0.0.0" ? new Version(postStatus["id"]) : Variables.TanksVersion;
                }
                catch (Exception) { Variables.UpdateTanksVersion = new Version("0.0.0.0"); }


                var remoteJson = POST.JsonResponse(Properties.Resources.JsonUpdates);
                Variables.UpdateMultipackVersion = Variables.Version(remoteJson[Variables.MultipackType]["version"].ToString());

                Variables.UpdateTanks = Variables.TanksVersion < Variables.UpdateTanksVersion; // Сравниваем версии танков
                Variables.UpdateMultipack = Variables.MultipackVersion < Variables.UpdateMultipackVersion; // Сравниваем версии мультипака

                if (Variables.UpdateMultipack)
                {
                    Variables.UpdateMessage = remoteJson[Variables.MultipackType]["changelog"][Variables.Lang].ToString();
                    Variables.UpdateLink = remoteJson[Variables.MultipackType]["download"].ToString();
                    lStatusUpdates.Content = String.Format("{0} ({1})", LocalLanguage.DynamicLanguage("llActuallyNewMods", Variables.Lang), Variables.UpdateMultipackVersion.ToString());
                }

                if (Variables.UpdateTanks)
                {
                    lStatusUpdates.Content += String.Format(Environment.NewLine + "{0} ({1})", LocalLanguage.DynamicLanguage("llActuallyNewGame", Variables.Lang), Variables.UpdateTanksVersion.ToString());
                    bPlay.IsEnabled = false;
                }
                else
                    bPlay.IsEnabled = true;


                if (Variables.UpdateMultipack || Variables.UpdateTanks) // Если есть одно из обновлений
                {
                    bUpdate.IsEnabled = true; // Включаем кнопку обновлений
                    lStatusUpdates.Foreground = System.Windows.Media.Brushes.Yellow;
                }
                else
                {
                    bUpdate.IsEnabled = false; // Выключаем кнопку обновлений
                    lStatusUpdates.Content = LocalLanguage.DynamicLanguage("llActuallyActually", Variables.Lang);
                    lStatusUpdates.Foreground = System.Windows.Media.Brushes.GreenYellow;
                }
            }
            catch (Exception ex) { Debug.Save("MainForm", "CheckUpdates()", ex.Message); }
        }

        private void StartGame(string file = "WorldOfTanks.exe")
        {
            try
            {
                if (File.Exists(Variables.PathTanks + file))
                {
                    State();
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
        private void State()
        {
            string select = "0";

            if (Variables.Doc.Root.Element("launcher") != null)
                if (Variables.Doc.Root.Element("launcher").Attribute("minimize") != null)
                    select = Variables.Doc.Root.Element("launcher").Attribute("minimize").Value;

            switch (select)
            {
                case "1": Hide(); break;
                case "2": WindowState = System.Windows.WindowState.Minimized; break;
                case "3": Close(); break;
                default: break;
            }
        }

        public void Youtube()
        {
            try
            {
                int topOffset = 0,
                    fontSize = 16;

                XDocument doc = XDocument.Load(String.Format(Properties.Resources.RssYoutube, Properties.Resources.Youtube));
                XNamespace ns = "http://www.w3.org/2005/Atom";

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
                    if (topOffset + fontSize + 6 < gGrid.RowDefinitions[3].Height.Value)
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

        private void WargamingNews()
        {
            try
            {
                int topOffset = 0,
                    fontSize = 16;

                XDocument doc = XDocument.Load(Variables.Lang == "ru" ? Properties.Resources.RssWotRU : Properties.Resources.RssWotEn);

                foreach (XElement el in doc.Root.Element("channel").Elements("item"))
                {
                    if (topOffset + fontSize + 6 < gGrid.RowDefinitions[3].Height.Value)
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
                        result = text.Remove(result.Length + 1);

                    return result + "...";
                }
                else
                    return text;
            }
            catch (Exception) { return text; }
        }

        private double TextWidth(string text, int fontSize = 14)
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

        private void ShowNotify(string text, string caption = null)
        {
            try
            {
                caption = caption != null ? caption : lCaption.Content.ToString();
                notifyIcon.ShowBalloonTip(5000, caption, text, System.Windows.Forms.ToolTipIcon.Info);
            }
            finally { }
        }

        private void VideoNotify()
        {
            try
            {
                if (Variables.Doc.Root.Element("youtube") != null)
                    foreach (var el in Variables.Doc.Root.Element("youtube").Elements("video")) { YoutubeClass.Delete(el.Value); }
                else Variables.Doc.Root.Add(new XElement("youtube", null));

                DeleteVideo(); // Перед выводом уведомлений проверяем даты. Все лишние удаляем

                foreach (var el in YoutubeClass.List)
                {
                    Thread.Sleep(5000);

                    for (int i = 0; i < 2; i++) // Если цикл прерван случайно, то выжидаем еще 7 секунд перед повторным запуском
                    {
                        while (System.Diagnostics.Process.GetProcessesByName("WorldOfTanks").Length > 0 ||
                            System.Diagnostics.Process.GetProcessesByName("WoTLauncher").Length > 0)
                            Thread.Sleep(5000);

                        Thread.Sleep(7000);
                    }

                    Variables.notifyLink = el.Link;
                    ShowNotify(LocalLanguage.DynamicLanguage("viewVideo", Variables.Lang), el.Title);

                    Variables.Doc.Root.Element("youtube").Add(new XElement("video", el.ID));
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
        private void DeleteVideo()
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
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            Settings stg = new Settings();
            stg.lang = Variables.Lang;
            stg.ShowDialog();

            this.Effect = null;
        }

        private void NotifyClick(object sender, EventArgs e)
        {
            OpenLink(Variables.notifyLink);
        }

        private void ShowUpdateWindow()
        {
            if (Variables.Doc.Root.Element("notification") != null)
                if (Variables.Doc.Root.Element("notification").Value != Variables.UpdateMultipackVersion.ToString())
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
}