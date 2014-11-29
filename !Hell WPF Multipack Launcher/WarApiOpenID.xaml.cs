using System;
using System.Collections.Generic;
using System.Linq;
//using System.Xml.Linq;
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
using System.Threading;
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
        Classes.Debugging Debugging = new Classes.Debugging();

        public WarApiOpenID()
        {
            InitializeComponent();
        }

        private void WB_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                if (WB.Source.ToString().IndexOf("status=ok") > -1)
                {
                    JObject Token = WarAPI.Token(WB.Source.ToString());

                    MainWindow.JsonSettingsSet("token.access_token", (string)Token.SelectToken("access_token"));
                    MainWindow.JsonSettingsSet("token.expires_at", (string)Token.SelectToken("expires_at"));
                    MainWindow.JsonSettingsSet("token.nickname", (string)Token.SelectToken("nickname"));
                    MainWindow.JsonSettingsSet("token.account_id", (string)Token.SelectToken("account_id"));

                    MainWindow.JsonSettingsSet("info.user_name", (string)Token.SelectToken("nickname"));

                    try
                    {
                        Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible; }));
                        Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

                        Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator("UserProfile"); }));
                    }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WarApiOpenID.xaml", "WB_LoadCompleted()", ex.Message, ex.StackTrace)); }

                    this.Close();
                }

                if (WB.Source.ToString().IndexOf("AUTH_CANCEL") > -1)
                {
                    Classes.Language Lang = new Classes.Language();
                    MessageBoxResult mbr = MessageBox.Show(Lang.Set("PageUser", "ActivateWarID", (string)MainWindow.JsonSettingsGet("info.language")) +
                        Environment.NewLine +
                        Lang.Set("PageUser", "RepeatActivation", (string)MainWindow.JsonSettingsGet("info.language")),
                        (string)MainWindow.JsonSettingsGet("info.ProductName"), MessageBoxButton.YesNo, MessageBoxImage.Information);

                    if (mbr == MessageBoxResult.Yes)
                        WB.Source = new Uri(WarAPI.OpenID());
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WarApiOpenID.xaml", "WebBrowser_LoadCompleted()", ex.Message, ex.StackTrace)); }
        }

        /// <summary>
        /// Устанавливаем значение в ключ токена XML
        /// </summary>
        /// <param name="attr">Ключ токена</param>
        /// <param name="val">Значение</param>
        /*private void SetValue(string attr, string val)
        {
            try { MainWindow.JsonSettingsSet("token." + attr, val); }
            catch (Exception ex) { System.Threading.Tasks.Task.Factory.StartNew(() => Debug.Save("WarApiOpenID.xaml", "SetValue()", "Attribute: " + attr, "Value: " + val, ex.Message, ex.StackTrace)); }
        }*/

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
                            try { MainWindow.JsonSettingsSet("info.user_email", match.Value.Trim()); }
                            catch (Exception) { }
                            match = match.NextMatch();
                        }
                    }
                }
            }
            catch (Exception ex) { Debugging.Save("WarApiOpenID.xaml", "WB_Navigating()", WB.Source.ToString(), ex.Message, ex.StackTrace); }
        }

        private void WarApiOpenID1_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try { MainWindow.LoadPage.Visibility = Visibility.Hidden; }
                catch (Exception) { }
            }));
        }
    }
}
