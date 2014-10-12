using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for WarApiOpenID.xaml
    /// </summary>
    public partial class WarApiOpenID : Window
    {
        Classes.WargamingAPI WarAPI = new Classes.WargamingAPI();
        Classes.Debug Debug = new Classes.Debug();

        public WarApiOpenID()
        {
            InitializeComponent();
        }

        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                if (WB.Source.ToString().IndexOf(Properties.Resources.Developer) > -1 && WB.Source.ToString().IndexOf("access_token") > 0)
                {
                    JObject Token = WarAPI.Token(WB.Source.ToString());

                    if (Token.SelectToken("status").ToString() == "ok")
                    {

                        SetValue("access_token", Token.SelectToken("access_token").ToString());
                        SetValue("expires_at", Token.SelectToken("expires_at").ToString());
                        SetValue("nickname", Token.SelectToken("nickname").ToString());
                        SetValue("account_id", Token.SelectToken("account_id").ToString());

                        this.Close();
                    }
                }
            }
            catch (Exception ex) { System.Threading.Tasks.Task.Factory.StartNew(() => Debug.Save("WarApiOpenID.xaml", "WebBrowser_LoadCompleted()", ex.Message, ex.StackTrace)); }
        }

        /// <summary>
        /// Устанавливаем значение в ключ токена XML
        /// </summary>
        /// <param name="attr">Ключ токена</param>
        /// <param name="val">Значение</param>
        private void SetValue(string attr, string val)
        {
            try
            {
                XElement el = MainWindow.XmlDocument.Root.Element("token");

                if (el != null)
                    if (el.Attribute(attr) != null)
                        el.Attribute(attr).SetValue(val);
                    else
                        el.Add(new XAttribute(attr, val));
                else
                    MainWindow.XmlDocument.Root.Add(new XElement("token",
                        new XAttribute(attr, val)));
            }
            catch (Exception ex) { System.Threading.Tasks.Task.Factory.StartNew(() => Debug.Save("WarApiOpenID.xaml", "SetValue()", "Attribute: " + attr, "Value: " + val, ex.Message, ex.StackTrace)); }
        }

        private void WarApiOpenID1_Loaded(object sender, RoutedEventArgs e)
        {
            try { MainWindow.LoadingPanelShow(); }
            catch (Exception) { }
        }
    }
}
