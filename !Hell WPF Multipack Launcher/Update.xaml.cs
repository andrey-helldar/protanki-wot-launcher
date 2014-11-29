using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Page
    {
        Classes.Debugging Debugging = new Classes.Debugging();

        public Update()
        {
            InitializeComponent();

            Task.Factory.StartNew(() => MultipackUpdate());
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    string link = (string)MainWindow.JsonSettingsGet("multipack.link");
                    if (link != String.Empty) Process.Start(link);
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Update.xaml", "bUpdate_Click()", ex.Message, ex.StackTrace)); }
            }));

            Task.Factory.StartNew(() => ClosingPage());
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => ClosingPage());
        }

        private void MultipackUpdate()
        {
           try
            {
                Classes.Language Lang = new Classes.Language();
                string lang = (string)MainWindow.JsonSettingsGet("info.language");

                 Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                       gbCaption.Header = Lang.Set("PageUpdate", "gbCaption", lang);
                     lDownloadFromLink.Content = Lang.Set("PageUpdate", "lDownloadFromLink", lang);
                     cbNotify.Content = Lang.Set("PageUpdate", "cbNotify", lang);
                     bUpdate.Content = Lang.Set("PageUpdate", "bUpdate", lang);
                     bCancel.Content = Lang.Set("PageUpdate", "bCancel", lang);

                     newVersion.Content = new Classes.Variables().VersionSharp((string)MainWindow.JsonSettingsGet("multipack.new_version"), false);
                     tbContent.Text = ParseChangelog((string)MainWindow.JsonSettingsGet("multipack.changelog"));
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Update.xaml", "MultipackUpdate()", ex.Message, ex.StackTrace)); }
        }

        private string ParseChangelog(string log)
        {
            string export = String.Empty;

            try
            {
                if (log.Length > 0)
                {
                    log = log.Remove(log.IndexOf("</font>"));

                    // Доп обработки строк
                    log = log.Replace("<ul>", Environment.NewLine + Environment.NewLine);
                    log = log.Replace("</ul>", "");

                    // Удаляем все теги <font>
                    Regex regex = new Regex(@"<font(.*)>");
                    Match match = regex.Match(log);
                    while (match.Success)
                    {
                        try { log = log.Replace(match.Value, ""); }
                        catch (Exception) { }
                        match = match.NextMatch();
                    }

                    /*
                     *  Обрабатываем элементы списка
                     */
                    log = log.Replace("<li>", " * ").Replace("</li>", Environment.NewLine);
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Update.xaml", "ParseChangelog()", ex.Message, ex.StackTrace)); }
            return log;
        }

        private void PageUpdate_Loaded(object sender, RoutedEventArgs e)
        {
            try { MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden; }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClosingPage()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    if (cbNotify.IsChecked == true)
                        MainWindow.JsonSettingsSet("info.notification", MainWindow.JsonSettingsGet("multipack.new_version"));

                    MainWindow.JsonSettingsSet("info.session", System.Diagnostics.Process.GetCurrentProcess().Id, "int");
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Update.xaml", "bUpdate_Click()", ex.Message, ex.StackTrace)); }
            }));
        }
    }
}
