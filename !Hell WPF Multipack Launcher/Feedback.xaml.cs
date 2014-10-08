using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Feedback.xaml
    /// </summary>
    public partial class Feedback : Page
    {
        Classes.Debug Debug = new Classes.Debug();
        Classes.Language Lang = new Classes.Language();


        private string filepath = String.Empty;
        private string lang = Properties.Resources.Default_Lang;


        public Feedback()
        {
            InitializeComponent();

            Task.Factory.StartNew(() =>
            {
                try { lang = MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value; }
                catch (Exception ex)
                {
                    Debug.Save("Feedback.xaml", "Feedback()", ex.Message, ex.StackTrace);
                    lang = Properties.Resources.Default_Lang;
                }
            }).Wait();

            Task.Factory.StartNew(() => SetInterface()); // Загрузка языка
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            try { MainWindow.Navigator("General", "Feedback.xaml"); }
            catch (Exception ex3) { Task.Factory.StartNew(() => Debug.Save("Feedback.xaml", "bClose_Click()", ex3.Message, ex3.StackTrace)); }
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
                            Dictionary<string, string> json = new Dictionary<string, string>();
                            Classes.POST POST = new Classes.POST();

                            json.Add("api", Properties.Resources.API);
                            json.Add("youtube", Properties.Resources.YoutubeChannel);
                            json.Add("project", Application.Current.GetType().Assembly.GetName().Name);
                            json.Add("project_version", new Classes.Variables().MultipackVersion.ToString());
                            json.Add("language", lang);

                            string cat = String.Empty;
                            string status = string.Empty;

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


                            json.Add("category", cat);
                            json.Add("message", POST.Shield(tbMessage.Text.Trim()));
                            json.Add("email", POST.Shield(tbEmail.Text.Trim()));

                            //  http://ai-rus.com/api/2.0/feedback
                            //  {0}/api/{1}/{2}

                            Dictionary<string, string> answer = POST.FromJson(POST.Send(String.Format(Properties.Resources.Developer_API_Format,
                                Properties.Resources.Developer,
                                Properties.Resources.Developer_API,
                                "feedback"
                                ), POST.Json(json)));

                            switch (answer["status"])
                            {
                                case "OK":
                                    status = Lang.Set("PageFeedback", "statusOK", lang, answer["id"]);
                                    tbMessage.Text = String.Empty;
                                    tbEmail.Text = String.Empty;
                                    break;
                                case "ANSWER": status = Lang.Set("PageFeedback", "statusANSWER", lang, answer["content"]); break;
                                case "Hacking attempt!": status = Lang.Set("PageFeedback", "statusHacking", lang); break;
                                case "BANNED":
                                    status = Lang.Set("PageFeedback", "statusBANNED", lang);
                                    tbMessage.Text = String.Empty;
                                    tbEmail.Text = String.Empty;
                                    break;
                                default: status = Lang.Set("PageFeedback", "statusDEFAULT", lang); break;
                            }
                            MessageBox.Show(status);
                        }
                    }
                    else
                        MessageBox.Show(Lang.Set("PageFeedback", "MinimumSymbols", lang, Properties.Resources.Developer_Feedback_Symbols));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Feedback.xaml", "bSend_Click()", ex.Message, ex.StackTrace)); }
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
                    bAttach.Content = Lang.Set("PageFeedback", "bAttach", lang);
                    bSend.Content = Lang.Set("PageFeedback", "bSend", lang);
                    bClose.Content = Lang.Set("PageFeedback", "bClose", lang);

                    SymbolsTitle.Text = Lang.Set("PageFeedback", "SymbolsTitle", lang);
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("PageFeedback", "SetInterface()", ex.Message, ex.StackTrace)); }
            }));
        }
    }
}
