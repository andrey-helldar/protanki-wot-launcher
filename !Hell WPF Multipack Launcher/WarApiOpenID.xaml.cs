using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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
        Classes.Language Lang = new Classes.Language();

        private bool loaded = false;
        private bool pageParsed = false;

        public WarApiOpenID()
        {
            InitializeComponent();

            try
            {
                Task.Factory.StartNew(() =>
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        WOIloading.Height = WarApiOpenID1.ActualHeight;
                        WOIloading.Content = Lang.Set("PageLoading", "lLoading", (string)MainWindow.JsonSettingsGet("info.language"));
                    }));
                }).Wait();
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WarApiOpenID.xaml", "WarApiOpenID()", ex.Message, ex.StackTrace)); }
        }

        private void WB_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
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
                                       MessageBoxResult mbr = MessageBox.Show(Lang.Set("PageUser", "ActivateWarID", (string)MainWindow.JsonSettingsGet("info.language")) +
                                           Environment.NewLine +
                                           Lang.Set("PageUser", "RepeatActivation", (string)MainWindow.JsonSettingsGet("info.language")),
                                           (string)MainWindow.JsonSettingsGet("info.ProductName"), MessageBoxButton.YesNo, MessageBoxImage.Information);

                                       if (mbr == MessageBoxResult.Yes)
                                       {
                                           string uri = WarAPI.OpenID();
                                           if (uri != null)
                                               WB.Source = new Uri(WarAPI.OpenID());
                                           else
                                           {
                                               Debugging.Save("WarApiOpenID.xaml", "WB_LoadCompleted()", "WarAPI.OpenID() == null");
                                               this.Close();
                                           }
                                       }
                                       else
                                           this.Close();
                                   }
                               }
                               catch (Exception) { }


                               /*
                                *   Парсим форму
                                *   
                                *   
                                *   form | IgnoreCase | Singleline
                                *           <form id=js-auth-form(.*)form>
                                * 
                                *   csrfmiddlewaretoken | IgnoreCase
                                *           value=(.*) name=csrfmiddlewaretoken
                                *           
                                *   next | IgnoreCase
                                *           value=(.*) name=next
                                *           
                                *   captcha | IgnoreCase
                                *           <img class=js-captcha-image src="(.*)">
                                */

                               try
                               {
                                   if (!pageParsed)
                                   {
                                       Regex regexForm = new Regex("<form id=js-auth-form(.*)form>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                       Regex regexCsrf = new Regex("value=(.*) name=csrfmiddlewaretoken", RegexOptions.IgnoreCase);
                                       Regex regexNext = new Regex("value=(.*) name=next", RegexOptions.IgnoreCase);
                                       Regex regexCaptcha = new Regex("<img class=js-captcha-image src=\"(.*)\"> </div>", RegexOptions.IgnoreCase);

                                       var doc = WB.Document as HTMLDocument;
                                       string
                                           resultCsrf = "",
                                           resultNext = "",
                                           resultCaptcha = "";

                                       IHTMLElementCollection nodes = doc.getElementsByTagName("html");
                                       foreach (IHTMLElement elem in nodes)
                                       {
                                           var form = (HTMLHeadElement)elem;

                                           // Ищем форму
                                           Match match = regexForm.Match(form.innerHTML);
                                           while (match.Success)
                                           {
                                               // Ищем Csrf
                                               Match matchCsrf = regexCsrf.Match(match.Value.Trim());
                                               while (matchCsrf.Success)
                                               {
                                                   resultCsrf = matchCsrf.Value.Trim();
                                                   resultCsrf = resultCsrf
                                                       .Replace("value=", "")
                                                       .Replace(" name=csrfmiddlewaretoken", "");

                                                   matchCsrf = matchCsrf.NextMatch();
                                               }


                                               // Ищем Next
                                               Match matchNext = regexNext.Match(match.Value.Trim());
                                               while (matchNext.Success)
                                               {
                                                   resultNext = matchNext.Value.Trim();
                                                   resultNext = resultNext
                                                       .Replace("value=", "")
                                                       .Replace(" name=next", "");

                                                   matchNext = matchNext.NextMatch();
                                               }


                                               // Ищем Captcha
                                               Match matchCaptcha = regexCaptcha.Match(match.Value.Trim());
                                               while (matchCaptcha.Success)
                                               {
                                                   resultCaptcha = matchCaptcha.Value.Trim();
                                                   resultCaptcha = resultCaptcha
                                                       .Replace("<IMG class=js-captcha-image src=\"", "")
                                                       .Replace("<img class=js-captcha-image src=\"", "")
                                                       .Replace("\"> </div>", "")
                                                       .Replace("\"> </DIV>", "");

                                                   if (resultCaptcha == "")
                                                       matchCaptcha = matchCaptcha.NextMatch();
                                                   else
                                                       break;
                                               }

                                               match = match.NextMatch();
                                           }
                                       }

                                       string resourcesForm = Properties.Resources.index;


                                       /*
                                       if (!System.IO.File.Exists(@"c:\Users\Helldar\AppData\Roaming\Wargaming.net\WorldOfTanks\multipack_launcher\2.txt"))
                                           System.IO.File.WriteAllText(@"c:\Users\Helldar\AppData\Roaming\Wargaming.net\WorldOfTanks\multipack_launcher\2.txt", WB.Source.ToString());
                                       */
                                       
                                       resourcesForm = resourcesForm
                                           .Replace("{{TOKEN}}", resultCsrf)
                                           .Replace("{{NEXT}}", resultNext)
                                           .Replace("{{CAPTCHA}}", resultCaptcha);
                                       WB.NavigateToString(resourcesForm);

                                       pageParsed = true;
                                   }
                               }
                               catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WarApiOpenID.xaml", "WB_LoadCompleted()", "Parse form", ex.Message, ex.StackTrace)); }
                           }));
                }).Wait();

                if (!loaded)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        try
                        {
                            DoubleAnimation da = new DoubleAnimation();
                            da.From = WOIloading.Height;
                            da.To = 0;
                            da.Duration = TimeSpan.FromSeconds(1);
                            WOIloading.BeginAnimation(Button.HeightProperty, da);
                        }
                        catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WarApiOpenID.xaml", "WB_LoadCompleted()", "DoubleAnimation da = new DoubleAnimation();", ex.Message, ex.StackTrace)); }
                    }));

                    loaded = true;
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WarApiOpenID.xaml", "WebBrowser_LoadCompleted()", ex.Message, ex.StackTrace)); }
        }

        private void WB_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Task.Factory.StartNew(() => InjectDisableScript()).Wait();
        }

        private void InjectDisableScript()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
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
                           }));
        }

        private const string DisableScriptError =
            @"function noError() {return true;}
              noError();
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
                Task.Factory.StartNew(() =>
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
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
                            catch (Exception) { }
                        }));
                }).Wait();
            }
            catch (Exception ex) { Debugging.Save("WarApiOpenID.xaml", "WB_Navigating()", WB.Source.ToString(), ex.Message, ex.StackTrace); }
        }

        private void WarApiOpenID1_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    string uri = WarAPI.OpenID();
                    if (uri != null)
                        WB.Source = new Uri(WarAPI.OpenID());
                    else
                    {
                        Debugging.Save("WarApiOpenID.xaml", "WarApiOpenID1_Loaded()", "WarAPI.OpenID() == null");
                        this.Close();
                    }
                }
                catch (Exception) { }
            }));
        }
    }
}
