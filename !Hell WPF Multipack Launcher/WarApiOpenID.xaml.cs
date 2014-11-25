using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using mshtml;

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

        private void WB_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                //if (WB.Source.ToString().IndexOf(Properties.Resources.Developer) > -1 && WB.Source.ToString().IndexOf("access_token") > 0)
                if (WB.Source.ToString().IndexOf("status=ok") > -1)
                {
                    JObject Token = WarAPI.Token(WB.Source.ToString());

                    SetValue("access_token", Token.SelectToken("access_token").ToString());
                    SetValue("expires_at", Token.SelectToken("expires_at").ToString());
                    SetValue("nickname", Token.SelectToken("nickname").ToString());
                    SetValue("account_id", Token.SelectToken("account_id").ToString());

                    this.Close();
                }

                if (WB.Source.ToString().IndexOf("AUTH_CANCEL") > -1)
                {
                    Classes.Language Lang = new Classes.Language();
                    MessageBoxResult mbr = MessageBox.Show(Lang.Set("PageUser", "ActivateWarID", MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value) +
                        Environment.NewLine +
                        Lang.Set("PageUser", "RepeatActivation", MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value),
                        MainWindow.ProductName, MessageBoxButton.YesNo, MessageBoxImage.Information);

                    if (mbr == MessageBoxResult.Yes)
                        WB.Source = new Uri(WarAPI.OpenID());
                    else
                    {
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

        private void WB_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            InjectDisableScript();
        }

        private void InjectDisableScript()
        {
            var doc = WB.Document as HTMLDocument;

            if (doc != null)
            {
                //Create the sctipt element 
                var scriptErrorSuppressed = (IHTMLScriptElement)doc.createElement("SCRIPT");
                scriptErrorSuppressed.type = "text/javascript";
                scriptErrorSuppressed.text = DisableScriptError;
                //Inject it to the head of the page 
                IHTMLElementCollection nodes = doc.getElementsByTagName("head");
                foreach (IHTMLElement elem in nodes)
                    (elem as HTMLHeadElement).appendChild((IHTMLDOMNode)scriptErrorSuppressed);
            }
        }

        private const string DisableScriptError =
            @"function noError() {return true;}
              window.onerror = noError;";

        /// <summary>
        /// Считываем email при подтверждении отправки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WB_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            try
            {
                var doc = WB.Document as HTMLDocument;

                if (
                    doc != null &&
                    WB.Source.ToString() != Properties.Resources.API_DEV_Address + Properties.Resources.API_DEV_OpenID &&
                    WB.Source.ToString().IndexOf("AUTH_CANCEL") == -1
                    )
                {
                    Regex regex = new Regex(@"\w+[a-zA-Z0-9-_.]+@+\w+[a-zA-Z0-9-]+.[a-zA-Z]{2,10}");

                    IHTMLElementCollection nodes = doc.getElementsByTagName("html");
                    foreach (IHTMLElement elem in nodes)
                    {
                        var body = (HTMLHeadElement)elem;

                        Match match = regex.Match(body.innerHTML);
                        while (match.Success)
                        {
                            Debug.Save("WarApiOpenID.xaml", "SetValue()", WB.Source.ToString(), match.Value);

                            try
                            {
                                if (MainWindow.XmlDocument.Root.Element("info").Attribute("user_email") != null)
                                    MainWindow.XmlDocument.Root.Element("info").Attribute("user_email").SetValue(match.Value.Trim());
                            }
                            catch (Exception) { }
                            match = match.NextMatch();
                        }
                    }
                }
            }
            catch (Exception ex) { Debug.Save("WarApiOpenID.xaml", "WB_Navigating()", WB.Source.ToString(), ex.Message, ex.StackTrace); }
        }
    }
}
