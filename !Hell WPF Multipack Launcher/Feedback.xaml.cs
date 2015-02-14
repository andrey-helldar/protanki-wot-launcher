using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Feedback.xaml
    /// </summary>
    public partial class Feedback : Page
    {
        Classes.Debugging Debugging = new Classes.Debugging();
        Classes.Language Lang = new Classes.Language();


        private string filepath = String.Empty;
        private string lang = Properties.Resources.Default_Lang;


        public Feedback()
        {
            InitializeComponent();

            Task.Factory.StartNew(() =>
            {
                try { lang = (string)MainWindow.JsonSettingsGet("info.language"); }
                catch (Exception ex) { Debugging.Save("Feedback.xaml", "Feedback()", ex.Message, ex.StackTrace); }
            }).Wait();

            Task.Factory.StartNew(() => SetInterface()); // Загрузка языка
        }

        private void PageFeedback_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)MainWindow.JsonSettingsGet("info.user_email") != null &&
                    (string)MainWindow.JsonSettingsGet("info.user_email") != "")
                {
                    tbEmail.Text = (string)MainWindow.JsonSettingsGet("info.user_email");
                    tbEmail.IsEnabled = false;
                }
                else
                {
                    tbEmail.Text = "";
                    tbEmail.IsEnabled = true;
                }
            }
            catch (Exception) { tbEmail.IsEnabled = true; }

            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try { MainWindow.LoadPage.Visibility = Visibility.Hidden; }
                catch (Exception) { }
            }));
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible; }));
            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
        }

        /// <summary>
        /// Формирование и отправка сообщения на сайт разработчика
        /// 
        /// Категории ответов:
        ///     OK - Сервер дал добро на добавление записи
        ///     ANSWER - Сервер вернул текстовый результат (ответил)
        ///     Hacking attempt! - Неавторизованный доступ к серверу
        ///     BANNED - Идентификатор пользователя заблокирован
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbEmail.Text.Trim().Length > 0 && tbEmail.Text.Trim().IndexOf("@") == -1)
                {
                    MessageBox.Show(Lang.Set("PageFeedback", "UncorrectEmail", lang));
                }
                else
                    if (tbMessage.Text.Length >= Convert.ToInt16(Properties.Resources.Developer_Feedback_Symbols))
                    {
                        if (MessageBox.Show(Lang.Set("PageFeedback", "SendNow", lang), Application.Current.GetType().Assembly.GetName().Name, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            Task.Factory.StartNew(() =>
                                Dispatcher.BeginInvoke(new ThreadStart(delegate
                                {
                                    string cat = String.Empty;
                                    string status = string.Empty;

                                    try
                                    {
                                        Classes.POST POST = new Classes.POST();

                                        /*
                                         *  Получаем тикет
                                         * 
                                         *  api             код API
                                         *  user_id         идентификатор пользователя
                                         *  user_name       имя юзера, если авторизован
                                         *  user_email      мыло юзера, если авторизован
                                         *  modpack_type    тип мультипака
                                         *  modpack_ver     версия мультипака
                                         *  launcher        версия лаунчера
                                         *  youtube         канал ютуба (идентификатор мододела)
                                         *  lang            язык запроса
                                         *  os              версия ОС
                                         */

                                        if (rbWishMultipack.IsChecked == true)
                                            cat = "WM";
                                        else if (rbWishLauncher.IsChecked == true)
                                            cat = "WL";
                                        else if (rbWishInstaller.IsChecked == true)
                                            cat = "WI";
                                        else if (rbErrorMultipack.IsChecked == true)
                                            cat = "EM";
                                        else if (rbErrorLauncher.IsChecked == true)
                                            cat = "EL";
                                        else if (rbErrorInstaller.IsChecked == true)
                                            cat = "EI";

                                        JObject json = new JObject(
                                            new JProperty("code", Properties.Resources.API),
                                            new JProperty("user_id", (string)MainWindow.JsonSettingsGet("info.user_id")),
                                            new JProperty("user_name", (string)MainWindow.JsonSettingsGet("info.user_name")),
                                            new JProperty("user_email", POST.Shield(tbEmail.Text.Trim())),
                                            new JProperty("modpack_type", (string)MainWindow.JsonSettingsGet("multipack.type")),
                                            new JProperty("modpack_ver", (string)MainWindow.JsonSettingsGet("multipack.version")),
                                            new JProperty("launcher", (string)MainWindow.JsonSettingsGet("info.ProductName")),
                                            new JProperty("youtube", Properties.Resources.YoutubeChannel),
                                            new JProperty("lang", lang),
                                            new JProperty("os", "disabled"),
                                            new JProperty("category", cat),
                                            new JProperty("message", POST.Shield(tbMessage.Text.Trim()))
                                        );

                                        JObject answer = JObject.Parse(POST.Send(Properties.Resources.API_DEV_Address + Properties.Resources.API_DEV_Ticket, json));

                                        if (answer["status"].ToString() != "FAIL" && answer["code"].ToString() == Properties.Resources.API)
                                        {
                                            switch (answer["status"].ToString())
                                            {
                                                case "OK":
                                                    status = Lang.Set("PageFeedback", "OK", lang, answer["id"].ToString());
                                                    tbMessage.Text = String.Empty;
                                                    if (tbEmail.IsEnabled) tbEmail.Text = String.Empty;
                                                    break;
                                                case "BANNED":
                                                    status = Lang.Set("PageFeedback", "BANNED", lang);
                                                    tbMessage.Text = String.Empty;
                                                    if (tbEmail.IsEnabled) tbEmail.Text = String.Empty;
                                                    break;
                                                default:
                                                    SaveTicket(json); // Сохранение тикета для последующей отправки
                                                    status = Lang.Set("PageFeedback", answer["status"].ToString(), lang) +
                                                    Lang.Set("PageFeedback", answer["content"].ToString(), lang, answer["message"].ToString());
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            SaveTicket(json); // Сохранение тикета для последующей отправки
                                            status = Lang.Set("PageFeedback", "FAIL", lang, Lang.Set("PageFeedback", answer["content"].ToString(), lang));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Task.Factory.StartNew(() => Debugging.Save("Feedback.xaml", "bSend_Click()", "Intro function", ex.Message, ex.StackTrace));
                                        status = Lang.Set("PageFeedback", "TICKET_ADD_ERROR", lang);
                                    }

                                    MessageBox.Show(status);
                                })));
                        }
                    }
                    else
                        MessageBox.Show(Lang.Set("PageFeedback", "MinimumSymbols", lang, Properties.Resources.Developer_Feedback_Symbols));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Feedback.xaml", "bSend_Click()", ex.Message, ex.StackTrace)); }
        }

        private void tbMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            SymbolsCount.Text = tbMessage.Text.Trim().Length.ToString();

            if (tbMessage.Text.Trim().Length < Convert.ToInt16(Properties.Resources.Developer_Feedback_Symbols))
                SymbolsCount.Foreground = new SolidColorBrush(Color.FromRgb(255, 98, 98));
            else
                SymbolsCount.Foreground = new SolidColorBrush(Color.FromRgb(0, 102, 153));
        }


        /// <summary>
        /// Костыль в виде установки значений интерфейса
        /// </summary>
        private void SetInterface()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    gbFeedback.Header = Lang.Set("PageFeedback", "gbFeedback", lang);
                    tbComment.Text = Lang.Set("PageFeedback", "tbComment", lang);
                    lSelectCategory.Content = Lang.Set("PageFeedback", "lSelectCategory", lang);

                    rbWishMultipack.Content = Lang.Set("PageFeedback", "rbWishMultipack", lang);
                    rbWishLauncher.Content = Lang.Set("PageFeedback", "rbWishLauncher", lang);
                    rbWishInstaller.Content = Lang.Set("PageFeedback", "rbWishInstaller", lang);
                    rbErrorMultipack.Content = Lang.Set("PageFeedback", "rbErrorMultipack", lang);
                    rbErrorLauncher.Content = Lang.Set("PageFeedback", "rbErrorLauncher", lang);
                    rbErrorInstaller.Content = Lang.Set("PageFeedback", "rbErrorInstaller", lang);

                    lSetEmail.Content = Lang.Set("PageFeedback", "lSetEmail", lang);
                    //bAttach.Content = Lang.Set("PageFeedback", "bAttach", lang);
                    bSend.Content = Lang.Set("PageFeedback", "bSend", lang);
                    bClose.Content = Lang.Set("PageFeedback", "bClose", lang);

                    SymbolsTitle.Text = Lang.Set("PageFeedback", "SymbolsTitle", lang);
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("PageFeedback", "SetInterface()", ex.Message, ex.StackTrace)); }
            }));
        }

        /// <summary>
        /// Сохранение неотправленных тикетов
        /// </summary>
        /// <param name="json"></param>
        private void SaveTicket(JObject json)
        {
            try
            {
                if (!Directory.Exists(MainWindow.SettingsDir + "tickets")) { Directory.CreateDirectory(MainWindow.SettingsDir + "tickets"); }

                string filename = String.Format("{0}_{1}.ticket", (string)MainWindow.JsonSettingsGet("info.user_id"), DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff"));

                if (Properties.Resources.API_DEV_CRYPT == "1")
                {
                    string encoded = new Classes.Crypt().Encrypt(json.ToString(), (string)MainWindow.JsonSettingsGet("info.user_id"), true);
                    if (encoded != "FAIL") File.WriteAllText(MainWindow.SettingsDir + @"tickets\" + filename, encoded, Encoding.UTF8);
                }
                else
                    File.WriteAllText(MainWindow.SettingsDir + @"tickets\" + filename, json.ToString(), Encoding.UTF8);

                // Очищаем поля
                tbMessage.Text = String.Empty;
                if (tbEmail.IsEnabled) tbEmail.Text = String.Empty;

                // Выдаем сообщение о сохранении тикета
                MainWindow.Notifier.ShowBalloonTip(5000,
                    Lang.Set("PostClass", "AutoTicketWait", lang),
                    Lang.Set("PageFeedback", "TicketSaved", lang),
                    System.Windows.Forms.ToolTipIcon.Info);
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("PageFeedback", "SaveTicket()", ex.Message, ex.StackTrace)); }
        }
    }
}
