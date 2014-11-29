using System;
using System.Collections.Generic;
using System.Linq;
//using System.Xml.Linq;
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
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Classes.Variables Variables = new Classes.Variables();
        Classes.Debugging Debugging = new Classes.Debugging();
        Classes.Optimize Optimize = new Classes.Optimize();
        Classes.Language Lang = new Classes.Language();


        public System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public static System.Windows.Forms.NotifyIcon Notifier { get { return notifier; } }
        private static System.Windows.Forms.NotifyIcon notifier;

        public static Frame MainFrame0 { get { return mainFrame; } }
        private static Frame mainFrame;

        public static Button PlayBtn { get { return playBtn; } }
        private static Button playBtn;

        /// <summary>
        /// Готовим контрол для отображения превью видео
        /// </summary>
        private static Frame framePreview;
        private static TextBlock tbPreview;

        // Глобальная переменная настроек ПО
        public static JObject jSettings;

        private string game_path = String.Empty;

        // Задаем переменную слоя загрузки
        public static Button LoadPage { get { return loadPage; } }
        private static Button loadPage;



        public static void Navigator(string page = "General")
        {
            try
            {
                MainWindow.mainFrame.NavigationService.Navigate(new Uri(page + ".xaml", UriKind.Relative));
            }
            catch (Exception) { }
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
                    MessageBox.Show(new Classes.Language().Set("MainProject", "Preview_NoData", (string)JsonSettingsGet("info.language")));
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() =>
                {
                    tbPreview.Visibility = Visibility.Hidden;
                    framePreview.Visibility = Visibility.Hidden;
                    new Classes.Debugging().Save("MainWindow", "PreviewVideo(3)", "ID: " + id, "Title: " + title, "Show: " + show.ToString(), ex.Message, ex.StackTrace);
                });
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Загружаем настройки из JSON
            Task.Factory.StartNew(() => JsonSettingsLoad()).Wait();
            this.Closing += delegate { jSettings = null; };

            Task.Factory.StartNew(() => Variables.Start()).Wait();

            Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    try
                    {
                        loadPage = LoadingPanel;
                        loadPage.SetResourceReference(Button.StyleProperty, "LoadingPanel");
                        loadPage.Content = Lang.Set("PageLoading", "lLoading", (string)JsonSettingsGet("info.language"));
                        loadPage.Visibility = System.Windows.Visibility.Visible;
                    }
                    catch (Exception ex) {  new Classes.Debugging().Save("MainWindow", "MainWindow()", ex.Message, ex.StackTrace); }
                    finally
                    {
                        this.Closing += delegate { loadPage = null; };
                    }
                }));

            Task.Factory.StartNew(() => Loading()).Wait();  // Loading data
        }

        /// <summary>
        /// Загружаем настройки из файла JSON
        /// </summary>
        private void JsonSettingsLoad()
        {
            try
            {
                string settings = "settings.json";
                string decrypt = "";

                if (File.Exists(settings))
                {
                    using (StreamReader reader = File.OpenText(settings))
                    {
                        decrypt = reader.ReadToEnd();

                        if (Properties.Resources.Default_Crypt_Settings == "1")
                        {
                            Classes.Crypt Crypt = new Classes.Crypt();
                            decrypt = Crypt.Decrypt(decrypt, Variables.GetUserID());
                        }
                        else
                            jSettings = JObject.Parse(decrypt);
                    }
                }
                else
                {// "Файл настроек не обнаружен"
                    MessageShow(Lang.Set("MainWindow", "Settings_Not_Found", (string)JsonSettingsGet("info.language")));
                    jSettings = null;
                }
            }
            catch (Exception ex) { Debugging.Save("MainWindow", "JsonLoadSettings()", ex.Message, ex.StackTrace); }
        }

        /// <summary>
        /// Сохранение настроек в файл
        /// </summary>
        private void JsonSettingsSave()
        {
            try
            {
                string settings = "settings.json";
                string encrypt = String.Empty;

                if (File.Exists(settings)) File.Delete(settings);

                if (Properties.Resources.Default_Crypt_Settings == "1")
                {
                    Classes.Crypt Crypt = new Classes.Crypt();
                    encrypt = Crypt.Encrypt(jSettings.ToString(), Variables.GetUserID());
                }
                else
                    encrypt = jSettings.ToString();

                File.WriteAllText(settings, encrypt);
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "JsonSaveSettings()", ex.Message, ex.StackTrace, jSettings.ToString())); }
        }

        /// <summary>
        /// Получение параметра настроек по ключу
        /// </summary>
        /// <param name="path">Передаем ссылку на параметр</param>
        /// <returns>Значение параметра</returns>
        public static JToken JsonSettingsGet(string path, string type = "token")
        {
            try
            {
                switch (type)
                {
                    case "JArray": return (JArray)jSettings[path];
                    default: return jSettings.SelectToken(path);
                }
            }
            catch (Exception) { }
            return null;
        }

        /// <summary>
        /// Записываем значение в массив
        /// </summary>
        /// <param name="path">Ключ массива. Максимум 2-х уровневый</param>
        /// <param name="value">Значение для записи</param>
        public static void JsonSettingsSet(string path, JToken key, string type = "string")
        {
            try
            {
                string key_s = String.Empty;

                switch (type)
                {
                    case "int":
                        if (path.IndexOf('.') == -1)
                            jSettings[path] = (int)key;
                        else
                        {
                            string[] str = path.Split('.');

                            if (jSettings[str[0]][str[1]] != null)
                                jSettings[str[0]][str[1]] = (int)key;
                            else
                                jSettings[str[0]][str[1]] = (int)key;
                        }
                        break;

                    case "bool":
                        if (path.IndexOf('.') == -1)
                            jSettings[path] = (bool)key;
                        else
                        {
                            string[] str = path.Split('.');

                            if (jSettings[str[0]][str[1]] != null)
                                jSettings[str[0]][str[1]] = (bool)key;
                            else
                                jSettings[str[0]][str[1]] = (bool)key;
                        }
                        break;

                    case "array":
                        key_s = ((string)key).Replace(@"\", Properties.Resources.Default_JSON_Splitter);
                        JArray ja;

                        if (path.IndexOf('.') == -1)
                        {
                            if (jSettings[path] != null && jSettings[path].ToString().Length > 0)
                                ja = JArray.Parse(jSettings[path].ToString());
                            else
                                ja = new JArray();

                            ja.Add(key_s);
                            jSettings[path] = ja;
                        }
                        else
                        {
                            string[] str = path.Split('.');

                            if (jSettings[str[0]][str[1]] == null)
                                ja = new JArray();
                            else
                                ja = JArray.Parse(jSettings[str[0]][str[1]].ToString());

                            ja.Add(key_s);
                            jSettings[str[0]][str[1]] = ja;
                        }
                        break;

                    default:
                        key_s = ((string)key).Replace(@"\", Properties.Resources.Default_JSON_Splitter);

                        if (path.IndexOf('.') == -1)
                            jSettings[path] = key_s;
                        else
                        {
                            string[] str = path.Split('.');

                            if (jSettings[str[0]] == null)
                                jSettings[str[0]] = new JObject(new JProperty(str[1], (string)key_s));
                            else
                                jSettings[str[0]][str[1]] = (string)key_s;
                        } break;
                }
            }
            catch (Exception ex) { File.WriteAllText(@"temp\log.debug", ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace); }
        }

        public static void JsonSettingsRemove(string path)
        {
            try
            {
                if (path.IndexOf('.') == -1)
                    //jSettings.Remove(path);
                    jSettings.Property(path).Remove();
                else
                {
                    string[] str = path.Split('.');
                    ((JObject)jSettings[str[0]]).Property(str[1]).Remove();
                }
            }
            catch (Exception) {  }
        }

        private void Loading()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    try { MouseDown += delegate { DragMove(); }; }
                    catch (Exception) { }

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
                        Stream iconStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + (string)JsonSettingsGet("info.ProductName") + ";component/Resources/WOT.ico")).Stream;
                        if (iconStream != null) notifyIcon.Icon = new System.Drawing.Icon(iconStream);
                        notifyIcon.Visible = true;
                        notifyIcon.Text = (string)JsonSettingsGet("info.ProductName");
                        notifyIcon.BalloonTipClicked += new EventHandler(NotifyClick);
                    }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "Window_Loaded(3)", "iconStream", ex.Message, ex.StackTrace)); }

                    try
                    {
                        Stream cursorStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + (string)JsonSettingsGet("info.ProductName") + ";component/Resources/cursor_chrome.cur")).Stream;
                        MainProject.Cursor = new Cursor(cursorStream);
                    }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "Window_Loaded(5)", "cursorStream", ex.Message, ex.StackTrace)); }


                    try { SetInterface(); }
                    catch (Exception ex)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Debugging.Save("MainWindow", "Window_Loaded(2)", ex.Message, ex.StackTrace);
                            Debugging.Restart();
                        }).Wait();
                    }
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "MainWindow()", ex.Message, ex.StackTrace)); }
            }));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try { game_path = ((string)JsonSettingsGet("game.path")).Replace(Properties.Resources.Default_JSON_Splitter, @"\"); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "Window_Loaded(0)", ex.Message, ex.StackTrace)); }
            
            try { Task.Factory.StartNew(() => Debugging.ClearLogs()); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "Window_Loaded(1)", ex.Message, ex.StackTrace)); }
            
            Task.Factory.StartNew(() =>
            {
                try { Dispatcher.BeginInvoke(new ThreadStart(delegate { MainFrame.NavigationService.Navigate(new Uri("General.xaml", UriKind.Relative)); })); }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "Window_Loaded(2)", ex.Message, ex.StackTrace)); }
            }).Wait();

            //LoadPage.Visibility = System.Windows.Visibility.Hidden;
            // Запускаем функцию автоматической отправки неотправленных тикетов
            Task.Factory.StartNew(() => new Classes.POST().AutosendTicket());
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { notifyIcon.Dispose(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "Window_Closing(0)", "notifyIcon.Dispose();", ex.Message, ex.StackTrace)); }

            //try { xmlDocument.Save(Variables.SettingsPath); }
            //catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "Window_Closing(1)", "xmlDocument.Save();", ex.Message, ex.StackTrace)); }

            try { JsonSettingsSave(); }
            catch (Exception ex) { Debugging.Save("MainWindow", "Window_Closing(1)", "JSON save settings", ex.Message, ex.StackTrace); }
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            try { Close(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "bClose_Click()", ex.Message, ex.StackTrace)); }
        }

        private void bMinimize_Click(object sender, RoutedEventArgs e)
        {
            try { this.WindowState = WindowState.Minimized; }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "bMinimize_Click()", ex.Message, ex.StackTrace)); }
        }

        private void NotifyClick(object sender, EventArgs e)
        {
            try { OpenLink((string)JsonSettingsGet("info.notify_link")); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "NotifyClick()", "Link: " + (string)JsonSettingsGet("info.notify_link"), ex.Message, ex.StackTrace)); }
        }

        private void OpenLink(string url)
        {
            try { ProcessStart(url); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "OpenLink()", "URL = " + url, ex.Message, ex.StackTrace)); }
        }

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (File.Exists(game_path + "WorldOfTanks.exe"))
                    {
                        Optimize.Start(
                                (bool)JsonSettingsGet("settings.winxp"),
                                (bool)JsonSettingsGet("settings.kill"),
                                (bool)JsonSettingsGet("settings.force"),
                                (bool)JsonSettingsGet("settings.aero"),
                                (bool)JsonSettingsGet("settings.video"),
                                (bool)JsonSettingsGet("settings.weak"),
                                true
                            );

                        ProcessStart(game_path, "WorldOfTanks.exe");
                    }
                    else
                        MessageBox.Show(Lang.Set("MainProject", "Game_Not_Found", (string)JsonSettingsGet("info.language")));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "bPlay_Click()", ex.Message, ex.StackTrace)); }
            });
        }

        private void bAirus_Click(object sender, RoutedEventArgs e)
        {
            try { ProcessStart(Properties.Resources.Developer_Link_Site); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "bAirus_Click()", "Link: " + Properties.Resources.Developer_Link_Site, ex.Message, ex.StackTrace)); }
        }

        private void bLauncherWOT_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (File.Exists(game_path + "WoTLauncher.exe"))
                        ProcessStart(game_path, "WoTLauncher.exe");
                    else
                        MessageBox.Show(Lang.Set("MainProject", "Game_Not_Found", (string)JsonSettingsGet("info.language")));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "bLauncherWOT_Click()", ex.Message, ex.StackTrace)); }
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
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "ProcessStart()", "Path: " + path, "Filename: " + filename, ex.Message, ex.StackTrace)); }
        }


        /// <summary>
        /// Костыль в виде установки значений интерфейса
        /// </summary>
        private void SetInterface()
        {
            //Dispatcher.BeginInvoke(new ThreadStart(delegate
            //{
            // Images & other
            try { rectLang.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/flag_{1}.png", (string)JsonSettingsGet("info.ProductName"), (string)JsonSettingsGet("info.language")))); }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("MainWindow", "SetInterface()", ex.Message, ex.StackTrace));
                MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
            }

            // Text
            try
            {
                bPlayTb.Text = Lang.Set("MainProject", "bPlay", (string)JsonSettingsGet("info.language"));
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("MainWindow", "SetInterface()", ex.Message, ex.StackTrace));
                MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
            }
            //}));
        }

        /*private static Button FindLoadingPanel(Grid sender)
        {
            try
            {
                object wantedNode = sender.FindName("LoadingPanel");
                if (wantedNode is Button)
                {
                    (wantedNode as Button).Visibility = System.Windows.Visibility.Visible;
                }
                else if (wantedNode == null)
                {
                    Button sp = new Button();
                    sp.SetResourceReference(Button.StyleProperty, "LoadingPanel");
                    sp.Content = Lang.Set("PageLoading", "lLoading", (string)JsonSettingsGet("info.language"));
                    sp.Name = "LoadingPanel";
                    sender.Children.Add(sp);
                    this.RegisterName(sp.Name, sp); // Register name of panel
                }
            }
            catch (Exception) { }
        }*/

        public static void MessageShow(string text, string caption = "", MessageBoxButton mbb = MessageBoxButton.OK)
        {
            try
            {
                caption = caption != "" ? caption : (string)JsonSettingsGet("info.ProductName");
                MessageBox.Show(text, caption, mbb, MessageBoxImage.Information);
            }
            catch (Exception) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(jSettings.ToString());
        }
    }
}
