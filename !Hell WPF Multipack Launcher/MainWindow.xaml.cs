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

        public static Frame MainFrame0 { get { return mainFrame; } }
        private static Frame mainFrame;

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

        public static bool Navigator(string page = "General", string from = null)
        {
            try
            {
                StackPanel sp = new StackPanel();
                sp.SetResourceReference(TextBlock.StyleProperty, "LoadingPage");
                Grid.SetRow(sp, 0);
                Grid.SetColumn(sp, 0);
                Grid.SetRowSpan(sp, 5);
            }
            catch (Exception) { }

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
                {
                    if (title.Trim().Length > 0)
                    {
                        tbPreview.Text = title;
                        tbPreview.Visibility = Visibility.Visible;
                    }
                    else { tbPreview.Visibility = Visibility.Hidden; }

                    framePreview.NavigationService.Navigate(new Uri("http://www.youtube.com/embed/" + id + "?rel=0&controls=0&showinfo=0", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    tbPreview.Visibility = Visibility.Hidden;
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

        private void Loading()
        {
            StackPanel sp = new StackPanel();
            sp.SetResourceReference(TextBlock.StyleProperty, "LoadingPage");
            Grid.SetRow(sp, 0);
            Grid.SetColumn(sp, 0);
            Grid.SetRowSpan(sp, 5);
            GridGlobal.Children.Add(sp);
        }

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                MouseDown += delegate { DragMove(); };
                
                Task.Factory.StartNew(() => Navigator("Loading")); // Изменение страницы

                // Делаем общей иконку в трее
                notifier = this.notifyIcon;
                this.Closing += delegate { notifier = null; };

                // Делаем общим фрейм
                mainFrame = this.MainFrame;
                this.Closing += delegate { mainFrame = null; };

                // Готовим превью
                framePreview = this.FramePreview;
                tbPreview = this.TbPreview;
                this.Closing += delegate { framePreview = null; tbPreview = null; };

                // Подгружаем перевод
                
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
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "MainWindow()", ex.Message, ex.StackTrace)); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    Variables.Start();
                    MultipackDate = Variables.MultipackDate;
                    ProductName = Variables.ProductName;

                    // Загружаем настройки из XML-файла
                    if (File.Exists(Variables.SettingsPath))
                        xmlDocument = XDocument.Load(Variables.SettingsPath);
                    this.Closing += delegate { xmlDocument = null; };
                }).Wait();
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded(2)", ex.Message, ex.StackTrace)).Wait();
                Task.Factory.StartNew(() => Debug.Restart());
            }


            Task.Factory.StartNew(() =>
            {
                try { Dispatcher.BeginInvoke(new ThreadStart(delegate { MainFrame.NavigationService.Navigate(new Uri("General.xaml", UriKind.Relative)); })); }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded(4)", ex.InnerException.ToString(), ex.Message, ex.StackTrace)); }
            });


            try { Task.Factory.StartNew(() => Debug.ClearLogs()); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "Debug.ClearLogs()", ex.Message, ex.StackTrace)); }
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
            try { OpenLink(Variables.notifyLink); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "NotifyClick()", "Link: " + Variables.notifyLink, ex.Message, ex.StackTrace)); }
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
                        Optimize.Start(Variables.WinXP);

                        Optimize.Start(
                                Variables.GetElement("settings", "winxp"),
                                Variables.GetElement("settings", "kill"),
                                Variables.GetElement("settings", "force"),
                                Variables.GetElement("settings", "aero"),
                                Variables.GetElement("settings", "video"),
                                Variables.GetElement("settings", "weak"),
                                true
                            );

                        ProcessStart(Variables.PathTanks, "WorldOfTanks.exe");
                    }
                    else
                        MessageBox.Show("Клиент игры не обнаружен!");
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "bPlay_Click()", ex.Message, ex.StackTrace)); }
            });
        }

        private void bAirus_Click(object sender, RoutedEventArgs e)
        {
            try { ProcessStart(Properties.Resources.Developer); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "bAirus_Click()", "Link: " + Properties.Resources.Developer,ex.Message, ex.StackTrace)); }
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
        private void ProcessStart(string path, string filename = "")
        {
            try
            {
                Process process = new Process();
                process.StartInfo.WorkingDirectory = path;
                process.StartInfo.FileName = filename == "" ? path : path + filename;
                process.StartInfo.UseShellExecute = false;

                process.Start();
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("MainWindow", "ProcessStart()", "Path: " + path, "Filename: " + filename, ex.Message, ex.StackTrace)); }
        }


        public void OokiiMessage(string text, string caption = "", MessageBoxButton Mbb = MessageBoxButton.OK, MessageBoxImage Mbi = MessageBoxImage.Information)
        {
            //  MessageBox.Show("text", "caption", MessageBoxButton.OK, MessageBoxImage.Information)
            if (Ookii.Dialogs.Wpf.TaskDialog.OSSupportsTaskDialogs)
            {
                using (Ookii.Dialogs.Wpf.TaskDialog dialog = new Ookii.Dialogs.Wpf.TaskDialog())
                {
                    dialog.WindowTitle = "Task dialog sample";
                    dialog.MainInstruction = "This is an example task dialog.";
                    dialog.Content = "Task dialogs are a more flexible type of message box. Among other things, task dialogs support custom buttons, command links, scroll bars, expandable sections, radio buttons, a check box (useful for e.g. \"don't show this again\"), custom icons, and a footer. Some of those things are demonstrated here.";
                    dialog.ExpandedInformation = "Ookii.org's Task Dialog doesn't just provide a wrapper for the native Task Dialog API; it is designed to provide a programming interface that is natural to .Net developers.";
                    dialog.Footer = "Task Dialogs support footers and can even include <a href=\"http://www.ookii.org\">hyperlinks</a>.";
                    dialog.FooterIcon = Ookii.Dialogs.Wpf.TaskDialogIcon.Information;
                    dialog.EnableHyperlinks = true;
                    Ookii.Dialogs.Wpf.TaskDialogButton customButton = new Ookii.Dialogs.Wpf.TaskDialogButton("A custom button");
                    Ookii.Dialogs.Wpf.TaskDialogButton okButton = new Ookii.Dialogs.Wpf.TaskDialogButton(ButtonType.Ok);
                    Ookii.Dialogs.Wpf.TaskDialogButton cancelButton = new Ookii.Dialogs.Wpf.TaskDialogButton(ButtonType.Cancel);
                    dialog.Buttons.Add(customButton);
                    dialog.Buttons.Add(okButton);
                    dialog.Buttons.Add(cancelButton);
                    dialog.HyperlinkClicked += new EventHandler<HyperlinkClickedEventArgs>(TaskDialog_HyperLinkClicked);
                    TaskDialogButton button = dialog.ShowDialog(this);
                    if (button == customButton)
                        MessageBox.Show(this, "You clicked the custom button", "Task Dialog Sample");
                    else if (button == okButton)
                        MessageBox.Show(this, "You clicked the OK button.", "Task Dialog Sample");
                }
            }
            else
            {
                MessageBox.Show(this, "This operating system does not support task dialogs.", "Task Dialog Sample");
            }
        }
    }
}
