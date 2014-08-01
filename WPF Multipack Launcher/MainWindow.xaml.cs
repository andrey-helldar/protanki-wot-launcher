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
        Classes.Language LocalLanguage = new Classes.Language();
        Classes.Variables Variables = new Classes.Variables();
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
        }

        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() => Variables.Start()).Wait();

                //Task.Factory.StartNew(() => SetBackground());

                Dispatcher.BeginInvoke(new ThreadStart(delegate { lMultipackVersion.Content = Variables.MultipackVersion.ToString(); }));
                Dispatcher.BeginInvoke(new ThreadStart(delegate { lCaption.Content = Variables.ProductName + " (" + LocalLanguage.DynamicLanguage("WindowCaption", Variables.Lang, Variables.MultipackType) + ")"; }));
                Dispatcher.BeginInvoke(new ThreadStart(delegate { lMultipackVersion.Content = Variables.VersionToSharp(Variables.MultipackVersion); }));

                Task.Factory.StartNew(() => Youtube());
                Task.Factory.StartNew(() => WargamingNews());

                Task.Factory.StartNew(() => CheckUpdates());

                // NotifyIcon
                Stream iconStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + Variables.ProductName + ";component/Resources/WOT.ico")).Stream;
                if (iconStream != null)
                    notifyIcon.Icon = new System.Drawing.Icon(iconStream);
                notifyIcon.Visible = true;
                notifyIcon.Text = lCaption.Content.ToString();
                notifyIcon.BalloonTipClicked += new EventHandler(NotifyClick);

                Task.Factory.StartNew(() => ShowNotify(LocalLanguage.DynamicLanguage("welcome", Variables.Lang)));
                Task.Factory.StartNew(() => VideoNotify());
            }
            catch (Exception ex)
            {
                Debug.Save("MainWindow", "MainForm_Loaded()", ex.Message);
                Process.Start("restart.exe", String.Format("\"{0}.exe\"", Process.GetCurrentProcess().ProcessName));
                Process.GetCurrentProcess().Kill();
            }
        }

        /*private void SetBackground()
        {
            try
            {
                string uri = @"pack://application:,,,/" + Variables.ProductName + ";component/Resources/back_{0}.jpg";

                if (Variables.BackgroundLoop)
                {
                    while (Variables.BackgroundLoop)
                    {
                        try
                        {
                            if (Variables.BackgroundIndex < 1 || Variables.BackgroundIndex > Variables.BackgroundMax) Variables.BackgroundIndex = 1;

                            
                                try { Dispatcher.BeginInvoke(new ThreadStart(delegate  {this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, (Variables.BackgroundIndex++).ToString()))));})); }
                                catch (Exception) { Dispatcher.BeginInvoke(new ThreadStart(delegate { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, (Variables.BackgroundIndex - 1).ToString())))); })); }
                            
                        }
                        catch (Exception) { Thread.Sleep(5000); Dispatcher.BeginInvoke(new ThreadStart(delegate { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, "1")))); })); }

                        Thread.Sleep(Variables.BackgroundDelay);
                    }
                }
                else
                {

                    try
                    {
                        Variables.BackgroundIndex = new Random().Next(1, 7);
                        Dispatcher.BeginInvoke(new ThreadStart(delegate { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, Variables.BackgroundIndex.ToString())))); }));
                    }
                    catch (Exception) { Dispatcher.BeginInvoke(new ThreadStart(delegate { this.Background = new ImageBrush(new BitmapImage(new Uri(String.Format(uri, (Variables.BackgroundIndex - 1).ToString())))); })); }
                }
            }
            catch (Exception ex) { Debug.Save("MainWindow", "SetBackground()", ex.Message); }
        }*/

        private void Hyperlink_Open(object sender, RequestNavigateEventArgs e)
        {
            try { Process.Start(e.Uri.AbsoluteUri); }
            catch (Exception ex) { Debug.Save("MainWindow", "Hyperlink_Open()", "Link: " + e.Uri.AbsoluteUri, ex.Message); MessageBox.Show(ex.Message); }
            finally { }
        }

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try { Optimize.Start(Variables.WinXP); StartGame(); }
                catch (Exception ex) { Debug.Save("MainWindow", "bPlay_Click()", ex.Message); }
            });
        }

        private void bLauncher_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (File.Exists(Variables.PathTanks + "WoTLauncher.exe"))
                    {
                        Optimize.Start(Variables.WinXP);
                        Process.Start(Variables.PathTanks + "WoTLauncher.exe");
                    }
                    else
                        Information("Клиент игры не обнаружен");

                }
                catch (Exception ex) { Debug.Save("MainWindow", "bLauncher_Click()", ex.Message); }
            });
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            /// Tanks updates
            if (Variables.UpdateTanks)
            {
                try
                {
                    if (File.Exists(Variables.PathTanks + "WoTLauncher.exe"))
                    {
                        Process.Start(Variables.PathTanks + "WoTLauncher.exe");
                    }
                    else
                        Information(LocalLanguage.DynamicLanguage("noTanks", Variables.Lang));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bUpdate_Click()", "UpdateTanks = " + Variables.UpdateTanks.ToString(), ex.Message)); }
            }

            /// Multipack updates
            if (Variables.UpdateMultipack)
            {
                try { OpenLink(Variables.UpdateLink); }
                catch (Exception ex)
                {
                    Task.Factory.StartNew(() => Debug.Save("MainWindow", "bUpdate_Click()", "LinkUpdate = " + Variables.UpdateLink, ex.Message));
                    Information(LocalLanguage.DynamicLanguage("noTanks", Variables.Lang));
                }
            }
        }

        private void OpenLink(string url)
        {
            try { Process.Start(url); }
            catch (Exception ex) { Debug.Save("MainWindow", "OpenLink()", "URL = " + url, ex.Message); }
        }

        private void bOptimize_Click(object sender, RoutedEventArgs e)
        {
            try { Optimize.Start(Variables.WinXP, Variables.AutoKill, Variables.AutoForceKill, Variables.AutoAero, Variables.AutoVideo, Variables.AutoWeak, true); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bOptimize_Click()", ex.Message)); }
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!Variables.WinXP)
                    if (Variables.AutoAero) Process.Start(new ProcessStartInfo("cmd", @"/c net start uxsms"));

                Variables.Doc.Save("settings.xml");
                Dispatcher.BeginInvoke(new ThreadStart(delegate { notifyIcon.Dispose(); })).Wait();
            }
            catch (Exception ex) { Debug.Save("MainWindow", "MainForm_Closing()", ex.Message); }
        }

        private bool CheckUpdates()
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
                json.Add("user", new Classes.Variables().GetUserID());
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
                catch (Exception ex0) { Debug.Save("MainWindow", "CheckUpdates()", ex0.Message); Variables.UpdateTanksVersion = new Version("0.0.0.0"); }


                var remoteJson = POST.JsonResponse(Properties.Resources.JsonUpdates);
                Variables.UpdateMultipackVersion = Variables.Version(remoteJson[Variables.MultipackType]["version"].ToString());

                Variables.UpdateTanks = Variables.TanksVersion < Variables.UpdateTanksVersion; // Сравниваем версии танков
                Variables.UpdateMultipack = Variables.MultipackVersion < Variables.UpdateMultipackVersion; // Сравниваем версии мультипака

                if (Variables.UpdateMultipack)
                {
                    Variables.UpdateMessage = remoteJson[Variables.MultipackType]["changelog"][Variables.Lang].ToString();
                    Variables.UpdateLink = remoteJson[Variables.MultipackType]["download"].ToString();
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        lStatusUpdates.Content = String.Format("{0} ({1})", LocalLanguage.DynamicLanguage("llActuallyNewMods", Variables.Lang), Variables.VersionToSharp(Variables.UpdateMultipackVersion));
                    }));
                }

                if (Variables.UpdateTanks)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { lStatusUpdates.Content += String.Format(Environment.NewLine + "{0} ({1})", LocalLanguage.DynamicLanguage("llActuallyNewGame", Variables.Lang), Variables.UpdateTanksVersion.ToString()); }));
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { bPlay.IsEnabled = false; }));
                }
                else
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { bPlay.IsEnabled = true; }));


                if (Variables.UpdateMultipack || Variables.UpdateTanks) // Если есть одно из обновлений
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { bUpdate.IsEnabled = true;})); // Включаем кнопку обновлений
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { lStatusUpdates.Foreground = System.Windows.Media.Brushes.Yellow;}));
                }
                else
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { bUpdate.IsEnabled = false;})); // Выключаем кнопку обновлений
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { lStatusUpdates.Content = LocalLanguage.DynamicLanguage("llActuallyActually", Variables.Lang);}));
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { lStatusUpdates.Foreground = System.Windows.Media.Brushes.GreenYellow; }));
                }
            }
            catch (Exception ex) { Debug.Save("MainForm", "CheckUpdates()", ex.Message); }


            // Если есть новые версии, то выводим уведомление
            try
            {
                if (Variables.UpdateNotify != Variables.UpdateMultipackVersion.ToString() && Variables.UpdateNotify != String.Empty)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        Notify MainNotify = new Notify();
                        MainNotify.lCaption.Content = lStatusUpdates.Content;
                        MainNotify.lCaption.FontSize = 16;
                        MainNotify.tbDescription.Text = new Classes.POST().DataRegex(Variables.UpdateMessage);
                        MainNotify.DownloadLink = Variables.UpdateLink;

                        this.Effect = new System.Windows.Media.Effects.BlurEffect();
                        MainNotify.ShowDialog();

                        if (MainNotify.cbNotNotify.IsChecked.Value)
                        {
                            if (Variables.Doc.Root.Element("info") != null)
                            {
                                if (Variables.Doc.Root.Element("info").Attribute("notification") != null)
                                    Variables.Doc.Root.Element("info").Attribute("notification").SetValue(Variables.UpdateMultipackVersion);
                                else
                                    Variables.Doc.Root.Element("info").Add(new XAttribute("notification", Variables.UpdateMultipackVersion));
                            }
                            else
                                Variables.Doc.Root.Add(new XElement("info", new XAttribute("notification", Variables.UpdateMultipackVersion)));

                            Variables.UpdateNotify = Variables.UpdateMultipackVersion.ToString(); // Обновляем значение
                        }
                        
                        this.Effect = null;
                    }));
                }
            }
            catch (Exception ex) { Debug.Save("MainWindow", "ShowUpdateWindow()", ex.Message); }

            return true;
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
            catch (Exception ex) { Debug.Save("MainWindow", "StartGame()", "File: " + file, ex.Message); }
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
            try
            {
                string select = "0";

                if (Variables.Doc.Root.Element("launcher") != null)
                    if (Variables.Doc.Root.Element("launcher").Attribute("minimize") != null)
                        select = Variables.Doc.Root.Element("launcher").Attribute("minimize").Value;

                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    switch (select)
                    {
                        case "1": Hide(); break;
                        case "2": WindowState = System.Windows.WindowState.Minimized; break;
                        case "3": Close(); break;
                        default: break;
                    }
                }));
            }
            catch (Exception ex) { Debug.Save("MainWindow", "State()", ex.Message); }
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
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        if (topOffset + fontSize + 6 < (int)gGrid.RowDefinitions[3].Height.Value)
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
                            Grid.SetColumn(label, 0);
                            gGrid.Children.Add(label);

                            // News title
                            Label labelT = new Label();
                            //Hyperlink hyperlink = new Hyperlink(new Run(content));
                            Hyperlink hyperlink = new Hyperlink(new Run(TrimText(content, gGrid.ColumnDefinitions[0].ActualWidth - 100, fontSize)));
                            hyperlink.NavigateUri = new Uri(link);
                            hyperlink.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_Open);

                            labelT.Content = hyperlink;
                            labelT.FontSize = fontSize;
                            labelT.ToolTip = content;
                            labelT.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            labelT.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            labelT.Margin = new Thickness(60, topOffset, 0, 0);
                            Grid.SetRow(labelT, 3);
                            Grid.SetColumn(labelT, 0);
                            gGrid.Children.Add(labelT);

                            topOffset += fontSize + 6;
                            //Thread.Sleep(100);
                        }
                    }));
                }
            }
            catch (Exception ex) { Debug.Save("MainWindow", "Youtube()", ex.Message); }
        }

        private void WargamingNews()
        {
            try
            {
                int topOffset = 0,
                    fontSize = 16;

                int height = 100;

                Dispatcher.BeginInvoke(new ThreadStart(delegate { height = (int)gGrid.RowDefinitions[3].Height.Value; }));

                XDocument doc = XDocument.Load(Variables.Lang == "ru" ? Properties.Resources.RssWotRU : Properties.Resources.RssWotEn);

                foreach (XElement el in doc.Root.Element("channel").Elements("item"))
                {
                    if (topOffset + fontSize + 6 < height)
                    {
                        Dispatcher.BeginInvoke(new ThreadStart(delegate
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
                            Hyperlink hyperlink = new Hyperlink(new Run(TrimText(el.Element("title").Value, (int)gGrid.ColumnDefinitions[1].ActualWidth - 100, fontSize)));
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
                        })).Wait();
                    }
                    else break;
                }
            }
            catch (Exception ex) { Debug.Save("MainWindow", "WargamingNews()", ex.Message); }
        }

        private string FormatDateNews(string dt)
        {
            try
            {
                if (dt == null) return "null";
                return DateTime.Parse(dt).ToString("dd/MM").Replace(".", "/");
            }
            catch (Exception ex) { Debug.Save("MainWindow", "FormatDateNews()", "Date: " + dt, ex.Message); return "error"; }
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
            catch (Exception /*ex*/)
            {/* Debug.Save("MainWindow", "TrimText()", ex.Message, text, "Max width: " + maxWidth.ToString(), "Font size: " + fontSize.ToString());*/
                return text;
            }
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
            catch (Exception ex) { Debug.Save("MainWindow", "TextWidth()", ex.Message, text, "Font size: " + fontSize.ToString()); return 0; }
        }

        private void ShowNotify(string text, string caption = null)
        {
            try
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
               {
                   caption = caption != null ? caption : lCaption.Content.ToString();
                   notifyIcon.ShowBalloonTip(5000, caption, text, System.Windows.Forms.ToolTipIcon.Info);
               }));
            }
            catch (Exception ex) { Debug.Save("MainWindow", "ShowNotify()", ex.Message, "Caption: " + caption, text); }
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
            catch (Exception ex) { Debug.Save("MainWindow", "VideoNotify()", ex.Message); }
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
                    try { if (!Variables.ParseDate(Variables.MultipackDate, el.Date)) YoutubeClass.Delete(el.ID); }
                    catch (Exception ex0)
                    { Debug.Save("MainWindow", "DeleteVideo()", "Element ID: " + el.ID, "Element title: " + el.Title, ex0.Message);
                        DeleteVideo();
                    }
            }
            catch (Exception /*ex*/)
            { /*Debug.Save("MainWindow", "DeleteVideo()", ex.Message);*/
                DeleteVideo();
            }
        }

        private void bSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    this.Effect = new System.Windows.Media.Effects.BlurEffect();

                    Settings Settings = new Settings();
                    Settings.doc = Variables.Doc;
                    Settings.Lang = Variables.Lang;
                    Settings.GameVersion = Variables.TanksVersion;
                    Settings.cbDisableWinAero.IsEnabled = !Variables.WinXP;
                    if (Variables.WinXP) Variables.Doc.Root.Element("settings").Attribute("aero").SetValue("False");

                    Settings.ShowDialog();

                    Variables.Doc = Settings.doc;

                    this.Effect = null;
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bSettings_Click()", ex.Message)); }
        }

        private void NotifyClick(object sender, EventArgs e)
        {
            try { OpenLink(Variables.notifyLink); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "NotifyClick()", "Link: " + Variables.notifyLink, ex.Message)); }
        }

        private void Information(string text, MessageBoxButton mbb= MessageBoxButton.OK)
        {
            try { MessageBox.Show(text, Variables.ProductName, mbb, MessageBoxImage.Information); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Information()", ex.Message, text)); }
        }

        private void bExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri("Feedback.xaml", UriKind.Relative));
        }
    }
}