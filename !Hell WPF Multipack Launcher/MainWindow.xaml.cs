﻿using System;
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
        /*********************
         * Variables
         * *******************/
        Classes.Variables Variables = new Classes.Variables();
        Classes.Debug Debug = new Classes.Debug();
        Classes.Optimize Optimize = new Classes.Optimize();
        Classes.Language Lang = new Classes.Language();
        

        public System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public static System.Windows.Forms.NotifyIcon Notifier { get { return notifier; } }
        private static System.Windows.Forms.NotifyIcon notifier;
        public static string NotifyLink = String.Empty;

        public static Frame MainFrame0 { get { return mainFrame; } }
        private static Frame mainFrame;

        public static Button PlayBtn { get { return playBtn; } }
        private static Button playBtn;

        /// <summary>
        /// Готовим контрол для отображения превью видео
        /// </summary>
        private static Frame framePreview;
        private static TextBlock tbPreview;

        public static XDocument XmlDocument { get { return xmlDocument; } }
        private static XDocument xmlDocument;


        public static string MultipackDate = "1970-1-1";
        public static string ProductName = String.Empty;


        /*********************
         * Functions
         * *******************/
        private static int loadingPanel { get { return loadingpanel; } }
        private static int loadingpanel = 0;

        public static bool LoadingPanelShow(int id = 2)
        {
            loadingpanel = id;
            return true;
        }

        public static bool Navigator(string page = "General")
        {
            loadingpanel = 1;

            try { MainWindow.mainFrame.NavigationService.Navigate(new Uri(page + ".xaml", UriKind.Relative)); }
            catch (Exception) { }
            return true;
        }

        /// <summary>
        /// Превью видео
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <param name="title">Заголовок</param>
        /// <param name="show">Отображать ли запись</param>
        public static void PreviewVideo(string id, string title = "", bool show = true)
        {
            try
            {
                if (show)
                    framePreview.NavigationService.Navigate(new Uri(String.Format(Properties.Resources.Youtube_Preview, id), UriKind.RelativeOrAbsolute));
                else
                {
                    framePreview.Visibility = Visibility.Hidden;
                    MessageBox.Show(new Classes.Language().Set("MainProject", "Preview_NoData", XmlDocument.Root.Element("info").Attribute("language").Value));
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() =>
                {
                    tbPreview.Visibility = Visibility.Hidden;
                    framePreview.Visibility = Visibility.Hidden;
                    new Classes.Debug().Save("MainWindow", "PreviewVideo(3)", "ID: " + id, "Title: " + title, "Show: " + show.ToString(), ex.Message, ex.StackTrace);
                });
            }
        }

        public MainWindow()
        {
            InitializeComponent();


            // Загружаем настройки из XML-файла
            if (File.Exists(Variables.SettingsPath))
                xmlDocument = XDocument.Load(Variables.SettingsPath);
            this.Closing += delegate { xmlDocument = null; };
            //}).Wait();

            Variables.Start();
            MultipackDate = Variables.MultipackDate;
            ProductName = Variables.ProductName;

            loadingpanel = 1;
            Task.Factory.StartNew(() => LoadingPanel());    // LoadingPanel

            Task.Factory.StartNew(() => Loading()).Wait();  // Loading data
        }

        private void LoadingPanel()
        {
            while (true)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    /*
                     *  loadingPanel
                     *      0   -   обычное состояние
                     *      1   -   показывать
                     *      2   -   скрыть
                     */
                    try
                    {
                        switch (loadingPanel)
                        {
                            case 1:
                                Button sp1 = FindLoadingPanel(GridGlobal);
                                if (sp1 == null)
                                {
                                    Button sp = new Button();
                                    sp.SetResourceReference(Button.StyleProperty, "LoadingPanel");
                                    sp.Content = Lang.Set("PageLoading", "lLoading", XmlDocument.Root.Element("info").Attribute("language").Value);
                                    sp.Name = "LoadingPanel";
                                    GridGlobal.Children.Add(sp);
                                    this.RegisterName(sp.Name, sp); // Register name of panel
                                }
                                else
                                    sp1.Visibility = System.Windows.Visibility.Visible;

                                loadingpanel = 0;
                                break;

                            case 2:
                                Button sp2 = FindLoadingPanel(GridGlobal);
                                if (sp2 != null)
                                    sp2.Visibility = System.Windows.Visibility.Hidden;
                                loadingpanel = 0;
                                break;

                            default: break;
                        }
                    }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "LoadingPanel()", ex.Message, ex.StackTrace)); }
                }));

                Thread.Sleep(100);
            }
        }

        private void Loading()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    MouseDown += delegate { DragMove(); };

                    // Делаем общей иконку в трее
                    notifier = this.notifyIcon;
                    this.Closing += delegate { notifier = null; };

                    playBtn = this.bPlay;
                    this.Closing += delegate { playBtn = null; };

                    // Делаем общим фрейм
                    mainFrame = this.MainFrame;
                    this.Closing += delegate { mainFrame = null; };

                    // Готовим превью
                    framePreview = this.FramePreview;
                    tbPreview = this.TbPreview;
                    this.Closing += delegate { framePreview = null; tbPreview = null; };


                    try
                    {
                        Stream iconStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + Variables.ProductName + ";component/Resources/WOT.ico")).Stream;
                        if (iconStream != null) notifyIcon.Icon = new System.Drawing.Icon(iconStream);
                        notifyIcon.Visible = true;
                        notifyIcon.Text = Variables.ProductName;
                        notifyIcon.BalloonTipClicked += new EventHandler(NotifyClick);
                    }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded(3)", "iconStream", ex.Message, ex.StackTrace)); }

                    try
                    {
                        Stream cursorStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + Variables.ProductName + ";component/Resources/cursor_chrome.cur")).Stream;
                        MainProject.Cursor = new Cursor(cursorStream);
                    }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded(5)", "cursorStream", ex.Message, ex.StackTrace)); }


                    try
                    {
                        SetInterface(); // Загрузка языка 
                    }
                    catch (Exception ex)
                    {
                        Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded(2)", ex.Message, ex.StackTrace)).Wait();
                        Task.Factory.StartNew(() => Debug.Restart());
                    }
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "MainWindow()", ex.Message, ex.StackTrace)); }
            }));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try { Task.Factory.StartNew(() => Debug.ClearLogs()); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Debug.ClearLogs()", ex.Message, ex.StackTrace)); }

            Task.Factory.StartNew(() =>
            {
                try { Dispatcher.BeginInvoke(new ThreadStart(delegate { MainFrame.NavigationService.Navigate(new Uri("General.xaml", UriKind.Relative)); })); }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded(4)", ex.InnerException.ToString(), ex.Message, ex.StackTrace)); }
            }).Wait();

            // Запускаем функцию автоматической отправки неотправленных тикетов
            Task.Factory.StartNew(() => new Classes.POST().AutosendTicket());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { notifyIcon.Dispose(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Closing(0)", "notifyIcon.Dispose();", ex.Message, ex.StackTrace)); }

            try { xmlDocument.Save(Variables.SettingsPath); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Closing(1)", "xmlDocument.Save();", ex.Message, ex.StackTrace)); }
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            try { Close(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bClose_Click()", ex.Message, ex.StackTrace)); }
        }

        private void bMinimize_Click(object sender, RoutedEventArgs e)
        {
            try { this.WindowState = WindowState.Minimized; }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bMinimize_Click()", ex.Message, ex.StackTrace)); }
        }

        private void NotifyClick(object sender, EventArgs e)
        {
            try { OpenLink(NotifyLink); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "NotifyClick()", "Link: " + NotifyLink, ex.Message, ex.StackTrace)); }
        }

        private void OpenLink(string url)
        {
            try { ProcessStart(url); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "OpenLink()", "URL = " + url, ex.Message, ex.StackTrace)); }
        }

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (File.Exists(Variables.PathTanks + "WorldOfTanks.exe"))
                    {
                        Optimize.Start(
                                XmlDocument.Root.Element("settings").Attribute("winxp").Value == "True",
                                XmlDocument.Root.Element("settings").Attribute("kill").Value == "True",
                                XmlDocument.Root.Element("settings").Attribute("force").Value == "True",
                                XmlDocument.Root.Element("settings").Attribute("aero").Value == "True",
                                XmlDocument.Root.Element("settings").Attribute("video").Value == "True",
                                XmlDocument.Root.Element("settings").Attribute("weak").Value == "True",
                                true
                            );

                        ProcessStart(Variables.PathTanks, "WorldOfTanks.exe");
                    }
                    else
                        MessageBox.Show(Lang.Set("MainProject", "Game_Not_Found", XmlDocument.Root.Element("info").Attribute("language").Value));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bPlay_Click()", ex.Message, ex.StackTrace)); }
            });
        }

        private void bAirus_Click(object sender, RoutedEventArgs e)
        {
            try { ProcessStart(Properties.Resources.Developer_Link_Site); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "bAirus_Click()", "Link: " + Properties.Resources.Developer_Link_Site, ex.Message, ex.StackTrace)); }
        }

        private void bLauncherWOT_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (File.Exists(Variables.PathTanks + "WoTLauncher.exe"))
                        ProcessStart(Variables.PathTanks, "WoTLauncher.exe");
                    else
                        MessageBox.Show(Lang.Set("MainProject", "Game_Not_Found", xmlDocument.Root.Element("info").Attribute("language").Value));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bLauncherWOT_Click()", ex.Message, ex.StackTrace)); }
            });
        }

        /// <summary>
        /// Запуск приложения или ссылки
        /// </summary>
        /// <param name="path">Пусть к файлу или ссылка</param>
        /// <param name="filename">Если используется запуск приложения, то обязательно указать имя файла</param>
        private void ProcessStart(string path, string filename = null)
        {
            try
            {
                if (filename != null)
                {
                    Process process = new Process();
                    process.StartInfo.WorkingDirectory = path;
                    process.StartInfo.FileName = path + filename;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();
                }
                else
                {
                    Process.Start(path);
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "ProcessStart()", "Path: " + path, "Filename: " + filename, ex.Message, ex.StackTrace)); }
        }


        /// <summary>
        /// Костыль в виде установки значений интерфейса
        /// </summary>
        private void SetInterface()
        {
            //Dispatcher.BeginInvoke(new ThreadStart(delegate
            //{
                // Images & other
                try { rectLang.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/flag_{1}.png", Variables.ProductName, XmlDocument.Root.Element("info").Attribute("language").Value))); }
                catch (Exception ex)
                {
                    Task.Factory.StartNew(() => Debug.Save("MainWindow", "SetInterface()", ex.Message, ex.StackTrace));
                    MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
                }

                // Text
                try
                {
                    bPlayTb.Text = Lang.Set("MainProject", "bPlay", Variables.Lang.Trim());
                }
                catch (Exception ex)
                {
                    Task.Factory.StartNew(() => Debug.Save("MainWindow", "SetInterface()", ex.Message, ex.StackTrace));
                    MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
                }
            //}));
        }

        private Button FindLoadingPanel(Grid sender)
        {
            try
            {
                object wantedNode = sender.FindName("LoadingPanel");
                if (wantedNode is Button)
                    return wantedNode as Button;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("MainWindow.xaml", "FindLoadingPanel()", "Find LoadingPanel", ex.Message, ex.StackTrace));
                return null;
            }
        }
    }
}
