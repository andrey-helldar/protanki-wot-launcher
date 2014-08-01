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
        System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();


        /*********************
         * Variables
         * *******************/
        Classes.Variables Variables = new Classes.Variables();
        Classes.Debug Debug = new Classes.Debug();
        Classes.Optimize Optimize = new Classes.Optimize();
        Classes.YoutubeVideo YoutubeClass = new Classes.YoutubeVideo();



        /*********************
         * Functions
         * *******************/

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                MouseDown += delegate { DragMove(); };
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "MainWindow()", ex.Message)); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Stream iconStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + Variables.ProductName + ";component/Resources/WOT.ico")).Stream;
                if (iconStream != null)
                    notifyIcon.Icon = new System.Drawing.Icon(iconStream);
                notifyIcon.Visible = true;
                notifyIcon.Text = Variables.ProductName;
                notifyIcon.BalloonTipClicked += new EventHandler(NotifyClick);
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded()", "iconStream", ex.Message)); }

            try
            {
                Task.Factory.StartNew(() => Variables.Start()).Wait();
                Task.Factory.StartNew(() => CheckUpdates());
                Task.Factory.StartNew(() => ShowNotify("Добро пожаловать!"));
                Task.Factory.StartNew(() => VideoNotify());

                lType.Content = Variables.MultipackType;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded()", ex.Message)).Wait();
                Task.Factory.StartNew(() => Debug.Restart());
            }
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
                json.Add("youtube", Properties.Resources.YoutubeChannel);
                json.Add("test", Variables.CommonTest ? "1" : "0");
                json.Add("version", Variables.TanksVersion.ToString());
                json.Add("lang", Variables.Lang);
                json.Add("resolution", SystemParameters.PrimaryScreenWidth.ToString() + "x" + SystemParameters.PrimaryScreenHeight.ToString());

                try // Check updates tanks version
                {
                    Dictionary<string, string> postStatus = POST.FromJson(POST.Send(Properties.Resources.Developer + Properties.Resources.DeveloperWotVersion, "data=" + POST.Json(json)));
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
                    /*Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        lStatusUpdates.Content = String.Format("{0} ({1})", LocalLanguage.DynamicLanguage("llActuallyNewMods", Variables.Lang), Variables.VersionToSharp(Variables.UpdateMultipackVersion));
                    }));*/
                }

                if (Variables.UpdateTanks)
                {
                    //Dispatcher.BeginInvoke(new ThreadStart(delegate { lStatusUpdates.Content += String.Format(Environment.NewLine + "{0} ({1})", LocalLanguage.DynamicLanguage("llActuallyNewGame", Variables.Lang), Variables.UpdateTanksVersion.ToString()); }));
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { bPlay.IsEnabled = false; }));
                }
                else
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { bPlay.IsEnabled = true; }));


                if (Variables.UpdateMultipack || Variables.UpdateTanks) // Если есть одно из обновлений
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { bUpdate.IsEnabled = true; })); // Включаем кнопку обновлений
                    // Dispatcher.BeginInvoke(new ThreadStart(delegate { lStatusUpdates.Foreground = System.Windows.Media.Brushes.Yellow;}));
                }
                else
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { bUpdate.IsEnabled = false; })); // Выключаем кнопку обновлений
                    //Dispatcher.BeginInvoke(new ThreadStart(delegate { lStatusUpdates.Content = LocalLanguage.DynamicLanguage("llActuallyActually", Variables.Lang);}));
                    //Dispatcher.BeginInvoke(new ThreadStart(delegate { lStatusUpdates.Foreground = System.Windows.Media.Brushes.GreenYellow; }));
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainForm", "CheckUpdates()", ex.Message)); }


            // Если есть новые версии, то выводим уведомление
            try
            {
                /* if (Variables.UpdateNotify != Variables.UpdateMultipackVersion.ToString() && Variables.UpdateNotify != String.Empty)
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
                 }*/
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "ShowUpdateWindow()", ex.Message)); }

            return true;
        }

        private void ShowNotify(string text, string caption = null)
        {
            try
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    caption = caption != null ? caption : /*lCaption.Content.ToString()*/"BBB";
                    notifyIcon.ShowBalloonTip(5000, caption, text, System.Windows.Forms.ToolTipIcon.Info);
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "ShowNotify()", ex.Message, "Caption: " + caption, text)); }
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
                    ShowNotify(/*LocalLanguage.DynamicLanguage("viewVideo", Variables.Lang), el.Title*/"Смотреть видео");

                    Variables.Doc.Root.Element("youtube").Add(new XElement("video", el.ID));
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "VideoNotify()", ex.Message)); }
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
                    catch (Exception /*ex0*/)
                    {
                        /*Task.Factory.StartNew(() => Debug.Save("MainWindow", "DeleteVideo()", "Element ID: " + el.ID, "Element title: " + el.Title, ex0.Message));*/
                        DeleteVideo();
                    }
            }
            catch (Exception) { DeleteVideo(); }
        }
    }
}
