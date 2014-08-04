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

                //MainFrame.NavigationService.Navigate(new Uri("Loading.xaml", UriKind.Relative));
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
                Task.Factory.StartNew(() => ShowNotify("Добро пожаловать!"));
                Task.Factory.StartNew(() => VideoNotify());

                MainFrame.NavigationService.Navigate(new Uri("General.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("MainWindow", "Window_Loaded()", ex.Message)).Wait();
                Task.Factory.StartNew(() => Debug.Restart());
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIcon.Dispose();
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

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OK");
        }
    }
}
