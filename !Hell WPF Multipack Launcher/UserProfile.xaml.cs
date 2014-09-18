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
            bool active = false;

            active = GetElement("access_token") && GetElement("expires_at") && GetElement("nickname") && GetElement("account_id");
            //if (active) active = DateTime.UtcNow.Subtract(DateTime.Parse(MainWindow.XmlDocument.Root.Element("token").Attribute("expires_at").Value)) <= DateTime.UtcNow;

            if (active)
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(MainWindow.XmlDocument.Root.Element("token").Attribute("expires_at").Value)).ToLocalTime();

                active = dtDateTime > DateTime.UtcNow;
            }

            if (!active)
            {
                Classes.WargamingAPI WarAPI = new Classes.WargamingAPI();

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
                /*
                 *      Читаем информацию о пользователе
                 */
            }
        }

        private bool GetElement(string attr)
        {
            if (MainWindow.XmlDocument.Root.Element("token") != null)
                if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr) != null)
                    if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value != "")
                        return true;
            return false;
        }
    }
}
