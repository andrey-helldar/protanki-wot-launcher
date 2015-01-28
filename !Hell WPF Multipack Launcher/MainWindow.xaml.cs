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

        // Путь к файлу настроек
        public static string SettingsDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\";
        public static string SettingsPath = SettingsDir + "settings.json";

        // Главное окно для передачи статуса
        public static Window State { get { return state; } }
        private static Window state;

        public System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public static System.Windows.Forms.NotifyIcon Notifier { get { return notifier; } }
        private static System.Windows.Forms.NotifyIcon notifier;

        public static Frame MainFrame0 { get { return mainFrame; } }
        private static Frame mainFrame;

        public static TextBlock PlayBtn { get { return playBtn; } }
        private static TextBlock playBtn;

        public static Image Flag { get { return flag; } }
        private static Image flag;

        public static ProgressBar OptimizeProgress { get { return optimizeProgress; } }
        private static ProgressBar optimizeProgress;

        /// <summary>
        /// Готовим контрол для отображения превью видео
        /// </summary>
        //public static Frame framePreviewM { get { return framePreview; } }
        private static Frame framePreview;

        // Глобальная переменная настроек ПО
        public static JObject jSettings;

        private string game_path = String.Empty;

        // Задаем переменную слоя загрузки
        public static Button LoadPage { get { return loadPage; } }
        private static Button loadPage;



        public static void Navigator(string page = "General")
        {
            try { mainFrame.NavigationService.Navigate(new Uri(page + ".xaml", UriKind.Relative)); }
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
                {
                    framePreview.Visibility = Visibility.Visible;
                    framePreview.NavigationService.Navigate(new Uri(String.Format(Properties.Resources.Youtube_Preview, id), UriKind.RelativeOrAbsolute));
                }
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
                    framePreview.Visibility = Visibility.Hidden;
                    new Classes.Debugging().Save("MainWindow", "PreviewVideo(3)", "ID: " + id, "Title: " + title, "Show: " + show.ToString(), ex.Message, ex.StackTrace);
                });
            }

            /*try { Process.Start("http://www.youtube.com/watch?v=" + id); }
            catch (Exception) { MessageBox.Show(new Classes.Language().Set("MainProject", "Preview_NoData", (string)JsonSettingsGet("info.language"))); }*/
        }

        public MainWindow()
        {
            InitializeComponent();

            // Загружаем настройки из JSON
            Task.Factory.StartNew(() => JsonSettingsLoad()).Wait();
            this.Closing += delegate { jSettings = null; };

            Task.Factory.StartNew(() => Variables.Start()).Wait();
            Task.Factory.StartNew(() => GetInfo()).Wait();

            Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    try
                    {
                        loadPage = LoadingPanel;
                        loadPage.SetResourceReference(Button.StyleProperty, "LoadingPanel");
                        loadPage.Content = Lang.Set("PageLoading", "lLoading", (string)JsonSettingsGet("info.language"));
                        loadPage.Visibility = System.Windows.Visibility.Visible;
                    }
                    catch (Exception ex) { new Classes.Debugging().Save("MainWindow", "MainWindow()", ex.Message, ex.StackTrace); }
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
                string decrypt = "";

                if (File.Exists(SettingsPath))
                {
                    using (StreamReader reader = File.OpenText(SettingsPath))
                    {
                        decrypt = reader.ReadToEnd();

                        if (Properties.Resources.Default_Crypt_Settings == "1")
                        {
                            try
                            {
                                Classes.Crypt Crypt = new Classes.Crypt();
                                decrypt = Crypt.Decrypt(decrypt, Variables.GetUserID());
                            }
                            catch (Exception)
                            {
                                if (File.Exists(SettingsPath)) File.Delete(SettingsPath);
                                Thread.Sleep(300);
                                File.WriteAllBytes(SettingsPath, Properties.Resources.Settings_Encoded);

                                Classes.Crypt Crypt = new Classes.Crypt();
                                decrypt = Crypt.Decrypt(decrypt, Variables.GetUserID());
                            }
                        }

                        jSettings = JObject.Parse(decrypt);

                        // Проверяем настройки
                        CheckSettings();
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
                //string settings = "settings.json";
                string encrypt = String.Empty;

                if (File.Exists(SettingsPath)) File.Delete(SettingsPath);

                if (Properties.Resources.Default_Crypt_Settings == "1")
                {
                    Classes.Crypt Crypt = new Classes.Crypt();
                    encrypt = Crypt.Encrypt(jSettings.ToString(), Variables.GetUserID());
                }
                else
                    encrypt = jSettings.ToString();

                File.WriteAllText(SettingsPath, encrypt);
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
            catch (Exception /*ex*/) {/* File.WriteAllText(@"temp\log.debug", ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);*/ }
        }

        public static bool JsonSettingsRemove(string path, string type = "string")
        {
            try
            {
                if (path.IndexOf('.') == -1)
                    //jSettings.Remove(path);
                    jSettings.Property(path).Remove();
                else
                {
                    string[] str = path.Split('.');

                    switch (type)
                    {
                        case "array":
                            foreach (var item in (JArray)jSettings.SelectToken(str[0]))
                                if ((string)item == str[1]) { item.Remove(); break; }
                            break;

                        default:
                            ((JObject)jSettings[str[0]]).Property(str[1]).Remove();
                            break;
                    }
                }

                return true;
            }
            catch (Exception /*ex*/) { /* File.WriteAllText(@"temp\log2.debug", ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);*/ }

            return false;
        }

        private void Loading()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    try { MouseDown += delegate { DragMove(); }; }
                    catch (Exception) { }

                    // Главное окно
                    state = this.MainProject;
                    this.Closing += delegate { state = null; };

                    // Делаем общей иконку в трее
                    notifier = this.notifyIcon;
                    this.Closing += delegate { notifier = null; };

                    playBtn = this.bPlayTb;
                    this.Closing += delegate { playBtn = null; };

                    // Делаем общим фрейм
                    mainFrame = this.MainFrame;
                    this.Closing += delegate { mainFrame = null; };

                    // Переменная флага
                    flag = this.rectLang;
                    this.Closing += delegate { flag = null; };

                    // Готовим превью
                    framePreview = this.FramePreview;
                    this.Closing += delegate { framePreview = null; };

                    // Прогресс-бар функции оптимизации
                    optimizeProgress = this.pbOptimize;
                    this.Closing += delegate { optimizeProgress = null; };


                    try
                    {
                        Stream iconStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/" + (string)JsonSettingsGet("info.ProductName") + ";component/Resources/WOT.ico")).Stream;
                        if (iconStream != null) notifyIcon.Icon = new System.Drawing.Icon(iconStream);
                        notifyIcon.Visible = true;
                        notifyIcon.Text = (string)JsonSettingsGet("info.ProductName");
                        notifyIcon.BalloonTipClicked += new EventHandler(NotifyClick);
                        notifyIcon.Click +=
                            delegate(object sender, EventArgs args)
                            {
                                this.Show();
                                this.WindowState = WindowState.Normal;
                            };
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
            try { if ((bool)JsonSettingsGet("settings.aero_disable")) { Process.Start(new ProcessStartInfo("cmd", @"/c net start uxsms")); } }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "Window_Closing()", "aero_disable", ex.Message, ex.StackTrace)).Wait(); }

            try { notifyIcon.Dispose(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "Window_Closing()", "notifyIcon.Dispose();", ex.Message, ex.StackTrace)).Wait(); }

            try { JsonSettingsSave(); }
            catch (Exception ex) { Debugging.Save("MainWindow", "Window_Closing()", "JsonSettingsSave()", ex.Message, ex.StackTrace); }
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
            try
            {
                if (File.Exists(game_path + "WorldOfTanks.exe"))
                {
                    Task.Factory.StartNew(() => Optimize.Start(
                            (bool)JsonSettingsGet("settings.winxp"),
                            (bool)JsonSettingsGet("settings.kill"),
                            (bool)JsonSettingsGet("settings.force"),
                            (bool)JsonSettingsGet("settings.aero"),
                            (bool)JsonSettingsGet("settings.video"),
                            (bool)JsonSettingsGet("settings.weak"),
                            false
                        ));

                    if ((bool)JsonSettingsGet("game.update"))
                        Task.Factory.StartNew(() => ProcessStart(game_path, "WoTLauncher.exe")).Wait();
                    else
                        Task.Factory.StartNew(() => ProcessStart(game_path, "WorldOfTanks.exe")).Wait();

                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        switch ((int)JsonSettingsGet("settings.launcher"))
                        {
                            case 1: this.Hide(); break;
                            case 2: this.WindowState = System.Windows.WindowState.Minimized; break;
                            case 3: this.Close(); break;
                            default: break;
                        }
                    }));
                }
                else
                    MessageBox.Show(Lang.Set("MainProject", "Game_Not_Found", (string)JsonSettingsGet("info.language")));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "bPlay_Click()", ex.Message, ex.StackTrace)); }
        }

        private void bAirus_Click(object sender, RoutedEventArgs e)
        {
            try { ProcessStart(Properties.Resources.Developer_Link_Site); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "bAirus_Click()", "Link: " + Properties.Resources.Developer_Link_Site, ex.Message, ex.StackTrace)); }
        }

        /// <summary>
        /// Запуск приложения или ссылки
        /// </summary>
        /// <param name="path">Пусть к файлу или ссылка</param>
        /// <param name="filename">Если используется запуск приложения, то обязательно указать имя файла</param>
        public static void ProcessStart(string path, string filename = null)
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
                    Process.Start(path);
            }
            catch (Exception ex) { new Classes.Debugging().Save("MainWindow", "ProcessStart()", "Path: " + path, "Filename: " + filename, ex.Message, ex.StackTrace); }
        }


        /// <summary>
        /// Костыль в виде установки значений интерфейса
        /// </summary>
        public void SetInterface()
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

        public static MessageBoxResult MessageShow(string text, string caption = "", MessageBoxButton mbb = MessageBoxButton.OK)
        {
            try
            {
                caption = caption != "" ? caption : (string)JsonSettingsGet("info.ProductName");
                return MessageBox.Show(text, caption, mbb, MessageBoxImage.Information);
            }
            catch (Exception) { return MessageBoxResult.OK; }
        }

        public static void Progress(int value, int max = 0, bool resetValue = false, bool resetMax = false)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    OptimizeProgress.Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        if (max > 0)
                            if (resetMax)
                                OptimizeProgress.Maximum = max;
                            else
                                OptimizeProgress.Maximum = OptimizeProgress.Maximum + max;

                        if (resetValue)
                            OptimizeProgress.Value = value;
                        else
                            if (OptimizeProgress.Value > OptimizeProgress.Maximum)
                                OptimizeProgress.Value = OptimizeProgress.Maximum;
                            else
                                OptimizeProgress.Value = OptimizeProgress.Value + value;

                        MessageBox.Show("Setted: " + value.ToString() + " / " + max.ToString() + Environment.NewLine + OptimizeProgress.Value.ToString() + " / " + OptimizeProgress.Maximum.ToString());
                    }));
                });
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace); /*Debugging.Save("Optimize.Class", "Progress()", ex.Message, ex.StackTrace);*/ }
        }


        private void GetInfo()
        {
            string ans = String.Empty;

            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply reply = ping.Send(Properties.Resources.API_DEV_Address.Split('/').Last());

                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    Classes.POST POST = new Classes.POST();

                    ans = POST.Send(Properties.Resources.API_DEV_Address + Properties.Resources.API_DEV_Info,
                        new JObject(
                                    new JProperty("code", Properties.Resources.API),
                                    new JProperty("user_id", (string)JsonSettingsGet("info.user_id")),
                                    new JProperty("user_name", (string)JsonSettingsGet("info.user_name")),
                                    new JProperty("user_email", (string)JsonSettingsGet("info.user_email")),
                                    new JProperty("modpack_type", (string)JsonSettingsGet("multipack.type")),
                                    new JProperty("modpack_ver", (string)JsonSettingsGet("multipack.version")),
                                    new JProperty("launcher", Application.Current.GetType().Assembly.GetName().Version.ToString()),
                                    new JProperty("game", (string)JsonSettingsGet("game.version")),
                                    new JProperty("game_test", (bool)JsonSettingsGet("game.test")),
                                    new JProperty("youtube", Properties.Resources.YoutubeChannel),
                                    new JProperty("lang", (string)JsonSettingsGet("info.language")),
                                    new JProperty("os", Environment.OSVersion.Version.ToString())
                                ));
                    JObject answer = JObject.Parse(ans);

                    /*
                     * code
                     * status
                     * version
                     */

                    if ((string)answer["status"] == "OK" && (string)answer["code"] == Properties.Resources.API)
                    {
                        // Проверяем корректность получения версии
                        // Ticket #106 Bitbucket
                        Version verRgame;
                        try { verRgame = new Version((string)answer["version"]); }
                        catch (Exception) { verRgame = new Version("0.0.0.0"); }

                        if (new Version((string)JsonSettingsGet("game.version")) < verRgame)
                        {
                            JsonSettingsSet("game.update", true, "bool");
                            JsonSettingsSet("game.new_version", (string)answer["version"]);
                        }
                        else JsonSettingsSet("game.update", false, "bool");

                        verRgame = null;
                    }
                    else JsonSettingsSet("game.update", false, "bool");
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "GetInfo(0)", "Developer", ans, ex.Message, ex.StackTrace)); }


            /*
             *      Проверяем обновления мультипака
             */
            JObject json_upd = null;
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply reply = ping.Send(Properties.Resources.Multipack_Address.Split('/').Last());

                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    json_upd = new Classes.POST().JsonResponse(Properties.Resources.Multipack_Address + Properties.Resources.Multipack_Updates);
                    json_upd["version"] = (string)json_upd.SelectToken("path") + "." + (string)json_upd.SelectToken((string)JsonSettingsGet("multipack.type") + ".version");

                    if (json_upd != null && (string)json_upd.SelectToken("version") != null)
                    {
                        if (new Version((string)JsonSettingsGet("multipack.version")) <
                            new Version((string)json_upd.SelectToken("version")))
                        {
                            string path = (string)JsonSettingsGet("multipack.type") + ".";

                            JsonSettingsSet("multipack.link", (string)json_upd.SelectToken(path + "download"));
                            JsonSettingsSet("multipack.changelog", (string)json_upd.SelectToken(path + "changelog." + (string)JsonSettingsGet("info.language")));
                            JsonSettingsSet("multipack.new_version", (string)json_upd.SelectToken("version"));
                            JsonSettingsSet("multipack.update", true, "bool");

                            try
                            {
                                if ((string)JsonSettingsGet("info.notification") != (string)json_upd.SelectToken("path"))
                                {
                                    if (JsonSettingsGet("info.session") == null)
                                    {
                                        Dispatcher.BeginInvoke(new ThreadStart(delegate
                                        {
                                            LoadingPanel.Visibility = System.Windows.Visibility.Visible;
                                            MainFrame.NavigationService.Navigate(new Uri("Update.xaml", UriKind.Relative));
                                        }));
                                    }
                                    else
                                        if ((int)JsonSettingsGet("info.session") != Process.GetCurrentProcess().Id)
                                        {
                                            Dispatcher.BeginInvoke(new ThreadStart(delegate
                                            {
                                                LoadingPanel.Visibility = System.Windows.Visibility.Visible;
                                                MainFrame.NavigationService.Navigate(new Uri("Update.xaml", UriKind.Relative));
                                            }));
                                        }
                                }
                            }
                            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "GetInfo(0)", "OpenPage(Update)", ex.Message, ex.StackTrace)); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("MainWindow.xaml", "GetInfo(1)",
                "This version: " + (string)JsonSettingsGet("multipack.version"),
                "New version: " + (json_upd != null ? (string)json_upd["version"] : "null"),
                ex.Message, ex.StackTrace));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(jSettings.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string file = @"settings.json";
                string settings_enc = @"settings_enc.json";

                if (File.Exists(file))
                    using (StreamReader reader = File.OpenText(file))
                    {
                        Classes.Crypt Crypt = new Classes.Crypt();

                        if (File.Exists(settings_enc)) File.Delete(settings_enc);
                        File.WriteAllText(settings_enc, Crypt.Encrypt(Crypt.Decrypt(reader.ReadToEnd(), Variables.GetUserID()), Variables.GetUserID()));
                    }


            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "JsonSaveSettings()", ex.Message, ex.StackTrace, jSettings.ToString())); }
        }

        private void rectLang_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate { LoadingPanel.Visibility = System.Windows.Visibility.Visible; }));
            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

            Dispatcher.BeginInvoke(new ThreadStart(delegate { Navigator("ChangeLocale"); }));
        }

        /// <summary>
        /// Check settings tokens
        /// </summary>
        private void CheckSettings()
        {
            try
            {
                // Проверяем разрешал ли юзер обработку данных
                if (jSettings.SelectToken("info.user_accept") == null) JsonSettingsSet("info.user_accept", false, "bool");

                //  Info
                if (jSettings.SelectToken("info.video") == null) JsonSettingsSet("info.video", true, "bool");
                if (jSettings.SelectToken("info.news") == null) JsonSettingsSet("info.news", true, "bool");
                if (jSettings.SelectToken("info.multipack") == null) JsonSettingsSet("info.multipack", true, "bool");
                if (jSettings.SelectToken("info.notification") == null) JsonSettingsSet("info.notification", "0.0.0.0");
                if (jSettings.SelectToken("info.language") == null) JsonSettingsSet("info.language", Properties.Resources.Default_Lang);
                if (jSettings.SelectToken("info.locale") == null) JsonSettingsSet("info.locale", 0, "int");
                if (jSettings.SelectToken("info.user_email") == null) JsonSettingsSet("info.user_email", "");
                if (jSettings.SelectToken("info.user_id") == null) JsonSettingsSet("info.user_id", "");
                if (jSettings.SelectToken("info.notify_link") == null) JsonSettingsSet("info.notify_link", "");
                if (jSettings.SelectToken("info.ProductName") == null) JsonSettingsSet("info.ProductName", "");


                //  Settings
                if (jSettings.SelectToken("settings.admin") == null) JsonSettingsSet("settings.admin", false, "bool");
                if (jSettings.SelectToken("settings.kill") == null) JsonSettingsSet("settings.kill", false, "bool");
                if (jSettings.SelectToken("settings.force") == null) JsonSettingsSet("settings.force", false, "bool");
                if (jSettings.SelectToken("settings.aero") == null) JsonSettingsSet("settings.aero", false, "bool");
                if (jSettings.SelectToken("settings.video") == null) JsonSettingsSet("settings.video", false, "bool");
                if (jSettings.SelectToken("settings.weak") == null) JsonSettingsSet("settings.weak", false, "bool");
                if (jSettings.SelectToken("settings.winxp") == null) JsonSettingsSet("settings.winxp", false, "bool");
                if (jSettings.SelectToken("settings.launcher") == null) JsonSettingsSet("settings.launcher", 0, "int");
                if (jSettings.SelectToken("settings.aero_disable") == null) JsonSettingsSet("settings.aero_disable", false, "bool");


                //  Multipack
                if (jSettings.SelectToken("multipack.language") == null) JsonSettingsSet("multipack.language", Properties.Resources.Default_Lang);
                if (jSettings.SelectToken("multipack.type") == null) JsonSettingsSet("multipack.type", Properties.Resources.Default_Multipack_Type);
                if (jSettings.SelectToken("multipack.version") == null) JsonSettingsSet("multipack.version", "0.0.0.0");
                if (jSettings.SelectToken("multipack.date") == null) JsonSettingsSet("multipack.date", "");
                if (jSettings.SelectToken("multipack.link") == null) JsonSettingsSet("multipack.link", "");
                if (jSettings.SelectToken("multipack.changelog") == null) JsonSettingsSet("multipack.changelog", "");
                if (jSettings.SelectToken("multipack.new_version") == null) JsonSettingsSet("multipack.new_version", "0.0.0.0");
                if (jSettings.SelectToken("multipack.update") == null) JsonSettingsSet("multipack.update", false, "bool");


                //  Game
                if (jSettings.SelectToken("game.version") == null) JsonSettingsSet("game.version", "0.0.0.0");
                if (jSettings.SelectToken("game.new_version") == null) JsonSettingsSet("game.new_version", "0.0.0.0");
                if (jSettings.SelectToken("game.path") == null) JsonSettingsSet("game.path", "");
                if (jSettings.SelectToken("game.test") == null) JsonSettingsSet("game.test", false, "bool");
                if (jSettings.SelectToken("game.language") == null) JsonSettingsSet("game.language", Properties.Resources.Default_Lang);
                if (jSettings.SelectToken("game.update") == null) JsonSettingsSet("game.update", false, "bool");

                // Создаем блок статистики
                if (jSettings.SelectToken("stats.donate") == null) JsonSettingsSet("stats.donate", 0, "int");
                if (jSettings.SelectToken("stats.donate_link") == null) JsonSettingsSet("stats.donate_link", 0, "int");
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "CheckSettings()", ex.Message, ex.StackTrace)); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (File.Exists("settings_open.json")) File.Delete("settings_open.json");
            File.WriteAllText("settings_open.json", jSettings.ToString());
        }
    }
}
