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

        private string filepath = String.Empty;

        public Feedback()
        {
            InitializeComponent();
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            try { MainWindow.Navigator("General", "Feedback.xaml"); }
            catch (Exception ex3) { Task.Factory.StartNew(() => Debug.Save("Feedback.xaml", "bClose_Click()", ex3.Message, ex3.StackTrace)); }
        }

        private void bAttach_Click(object sender, RoutedEventArgs e)
        {

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
                if (tbMessage.Text.Length >= Convert.ToInt16(Properties.Resources.Developer_Feedback_Symbols))
                {
                    Dictionary<string, string> json = new Dictionary<string, string>();
                    Classes.POST POST = new Classes.POST();

                    json.Add("api", Properties.Resources.API);
                    json.Add("youtube", Properties.Resources.YoutubeChannel);
                    json.Add("project", new Classes.Variables().ProductName);
                    json.Add("project_version", new Classes.Variables().MultipackVersion.ToString());

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
                    json.Add("message", POST.Shield(tbMessage.Text));
                    json.Add("email", POST.Shield(tbEmail.Text));

                    //  http://ai-rus.com/api/2.0/feedback
                    //  {0}/api/{1}/{2}

                    Dictionary<string, string> answer = POST.FromJson(POST.Send(String.Format(Properties.Resources.Developer_API_Format,
                        Properties.Resources.Developer,
                        Properties.Resources.Developer_API,
                        "feedback"
                        ), POST.Json(json)));

                    switch (answer["status"])
                    {
                        case "OK": status = "Ваше сообщение принято под номером " + answer["id"]; break;
                        case "ANSWER": status = "Сервер ответил: " + answer["content"]; break;
                        case "Hacking attempt!": status = "Ошибка доступа к сервису"; break;
                        case "BANNED": status = "Ваше сообщение принято к обработке"; break;
                        default: status = "Ошибка отправки сообщения. Попробуйте еще раз."; break;
                    }
                    MessageBox.Show(status);
                }
                else
                    MessageBox.Show("Сообщение должно быть не менее " + Properties.Resources.Developer_Feedback_Symbols + " символов");
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Feedback.xaml", "bSend_Click()", ex.Message, ex.StackTrace)); }
        }
    }
}
