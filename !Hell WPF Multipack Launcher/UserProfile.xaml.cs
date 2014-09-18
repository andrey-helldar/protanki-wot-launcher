using System;
using System.Collections.Generic;
using System.Linq;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Page
    {
        public UserProfile()
        {
            InitializeComponent();

            Task.Factory.StartNew(() => { AccountInfo(); });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("General", "Settings.xaml");
        }

        /// <summary>
        /// Получаем информацию по аккаунту.
        /// Если токен активен, сразу выводим.
        /// Если нет, вначале запрашиваем авторизацию, после чего получаем информацию.
        /// </summary>
        private void AccountInfo()
        {
            Classes.WargamingAPI WarAPI = new Classes.WargamingAPI();

            bool active = CheckElement("access_token") && CheckElement("expires_at") && CheckElement("nickname") && CheckElement("account_id");

            if (active)
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(MainWindow.XmlDocument.Root.Element("token").Attribute("expires_at").Value)).ToLocalTime();

                active = dtDateTime > DateTime.UtcNow;
            }

            if (!active)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    try
                    {
                        WarApiOpenID WarApiOpenID = new WarApiOpenID();
                        WarApiOpenID.WB.Source = new Uri(WarAPI.OpenID());
                        WarApiOpenID.ShowDialog();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace); }
                }));
            }
            else
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                   {
                       try
                       {
                           string toMess = "null";
                           /*
                            *      Читаем информацию о пользователе
                            */

                           PlayerName.Text = GetElement("nickname");

                           JObject obj = JObject.Parse(WarAPI.AccountInfo(GetElement("account_id"), GetElement("access_token")));

                           if (obj.SelectToken("status").ToString() =="ok")
                           {

                               MessageBox.Show("data[" + GetElement("account_id") + "].clan_id");

                               PlayerClan.Text = obj.SelectToken("data[" + GetElement("account_id") + "].clan_id").ToString();
                           }

                           MessageBox.Show(toMess);
                       }
                       catch (Exception ex) { MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace); }
                   }));
            }
        }

        /// <summary>
        /// Проверяем существует ли аттрибут
        /// </summary>
        /// <param name="attr">Аттрибут для проверки</param>
        /// <returns>TRUE - существует, FALSE - не существует</returns>
        private bool CheckElement(string attr)
        {
            if (MainWindow.XmlDocument.Root.Element("token") != null)
                if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr) != null)
                    if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value != "")
                        return true;
            return false;
        }

        /// <summary>
        /// Получаем значение аттрибута
        /// </summary>
        /// <param name="attr">Аттрибут</param>
        /// <returns>Значение</returns>
        private string GetElement(string attr)
        {
            if (MainWindow.XmlDocument.Root.Element("token") != null)
                if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr) != null)
                    if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value != "")
                        return MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value;
            return "NULL";
        }
    }
}
