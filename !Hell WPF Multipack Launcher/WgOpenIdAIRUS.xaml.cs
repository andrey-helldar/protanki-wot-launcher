using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using mshtml;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for WgOpenIdAIRUS.xaml
    /// </summary>
    public partial class WgOpenIdAIRUS : Page
    {
        Classes.WargamingAPI WarAPI = new Classes.WargamingAPI();
        Classes.Debugging Debugging = new Classes.Debugging();
        Classes.Language Lang = new Classes.Language();

        private bool pageParsed = false;

        string
            resultCsrf = "",
            resultNext = "",
            resultCaptcha = "",
            resultErrors = "";


        public WgOpenIdAIRUS()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
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
                        Debugging.Save("WgOpenIdAIRUS.xaml", "Page_Loaded()", "WarAPI.OpenID() == null");
                        ClosePage();
                    }
                }
                catch (Exception) { }
            }));

            Task.Factory.StartNew(() =>
            {
                try
                {
                    MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden;
                    Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));
                }
                catch (Exception) { }
            });
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

        private void InjectSubmit()
        {
            try
            {
                string sendConfirm =
                                        "$(document).ready(function() {" +
                                        "$('#confirm_form').submit();" +
                                        "sendConfirm();" +
                                        "}); " +
                                        "sendConfirm();";

                var doc = WB.Document as HTMLDocument;

                if (doc != null)
                {
                    var scriptErrorSuppressed = (IHTMLScriptElement)doc.createElement("SCRIPT");
                    scriptErrorSuppressed.type = "text/javascript";
                    scriptErrorSuppressed.text = sendConfirm;
                    IHTMLElementCollection nodes = doc.getElementsByTagName("head");
                    foreach (IHTMLElement elem in nodes)
                        (elem as HTMLHeadElement).appendChild((IHTMLDOMNode)scriptErrorSuppressed);
                }
            }
            catch (Exception) { }
        }

        private void WB_Navigated(object sender, NavigationEventArgs e)
        {
            Task.Factory.StartNew(() => InjectDisableScript()).Wait();

            Task.Factory.StartNew(() => InjectSubmit()).Wait();
        }

        private void bEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CaptionStatus.Text = "";

                if (tbEmail.Text.Trim() == "" || pbPassword.Password == "" || tbCaptcha.Text.Trim() == "")
                    MessageBox.Show("Заполнены не все поля!");
                else
                    Task.Factory.StartNew(() => SendData());
            }
            catch (Exception) { }
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            ClosePage();
        }

        private void WB_LoadCompleted(object sender, NavigationEventArgs e)
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
                                    Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden; /* Visible*/ }));
                                    Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

                                    Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator("UserProfile"); }));
                                }
                                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WgOpenIdAIRUS.xaml", "WB_LoadCompleted()", ex.Message, ex.StackTrace)); }

                                ClosePage("UserProfile");
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
                                        Debugging.Save("WgOpenIdAIRUS.xaml", "WB_LoadCompleted()", "WarAPI.OpenID() == null");
                                        ClosePage();
                                    }
                                }
                                else
                                    ClosePage();
                            }


                            if (WB.Source.ToString().IndexOf("confirm") > -1)
                                InjectSubmit();
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
                         *           
                         *   errors
                         *          <P class=js-form-errors-content>(.*)</P>
                         */

                        try
                        {
                            if (!pageParsed)
                            {
                                Regex regexForm = new Regex("<form id=js-auth-form(.*)form>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                Regex regexCsrf = new Regex("value=(.*) name=csrfmiddlewaretoken", RegexOptions.IgnoreCase);
                                Regex regexNext = new Regex("value=(.*) name=next", RegexOptions.IgnoreCase);

                                var doc = WB.Document as HTMLDocument;

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

                                        match = match.NextMatch();
                                    }
                                }

                                // Подгружаем изображение
                                UpdateCaptcha();

                                pageParsed = true;

                                Dispatcher.BeginInvoke(new ThreadStart(delegate
                                {
                                    try
                                    {
                                        MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden;
                                        Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));
                                    }
                                    catch (Exception) { }
                                }));
                            }
                        }
                        catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WgOpenIdAIRUS.xaml", "WB_LoadCompleted()", "Parse form", ex.Message, ex.StackTrace)); }
                    }));
                }).Wait();
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WgOpenIdAIRUS.xaml", "WB_LoadCompleted()", ex.Message, ex.StackTrace)); }
        }

        private void ClosePage(string page = "")
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                MainWindow.LoadPage.Content = Lang.Set("PageLoading", "lLoading", (string)MainWindow.JsonSettingsGet("info.language"));
                MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden; // Visible
            }));
            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(page); }));
        }

        private void UpdateCaptcha()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    Regex regexCaptcha = new Regex("<img class=js-captcha-image src=\"(.*)\"> </div>", RegexOptions.IgnoreCase);

                    var doc = WB.Document as HTMLDocument;

                    IHTMLElementCollection nodes = doc.getElementsByTagName("body");
                    foreach (IHTMLElement elem in nodes)
                    {
                        var body = (HTMLHeadElement)elem;


                        Match matchCaptcha = regexCaptcha.Match(body.innerHTML);
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
                    }

                    // Компилируем изображение
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.None;
                    bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bi.UriSource = new Uri("https://ru.wargaming.net" + resultCaptcha, UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    imgCaptcha.Source = bi;
                }
                catch (Exception ex)
                {
                    imgCaptcha.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/reload.jpg", (string)MainWindow.JsonSettingsGet("info.ProductName"))));
                    Task.Factory.StartNew(() => Debugging.Save("WgOpenIdAIRUS.xaml", "UpdateCaptcha()", ex.Message, ex.StackTrace));
                }
            }));
        }

        private void imgCaptcha_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UpdateCaptcha();
        }

        private void SendData()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden; // Visible
                    Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));
                }
                catch (Exception) { }
            }));

            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.JsonSettingsSet("info.user_email", tbEmail.Text.Trim()); }));
            pageParsed = false;

            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                string inputData =
                    "function inputData() {" +
                    "$('#id_login').val('" + tbEmail.Text.Trim() + "');" +
                    "$('#id_password').val('" + pbPassword.Password + "');" +
                    "$('#id_captcha').val('" + tbCaptcha.Text.Trim() + "');" +
                    "$('#js-auth-form').submit();" +
                    "} " +
                    "inputData();";

                var doc = WB.Document as HTMLDocument;

                if (doc != null)
                {
                    var scriptErrorSuppressed = (IHTMLScriptElement)doc.createElement("SCRIPT");
                    scriptErrorSuppressed.type = "text/javascript";
                    scriptErrorSuppressed.text = inputData;
                    IHTMLElementCollection nodes = doc.getElementsByTagName("head");
                    foreach (IHTMLElement elem in nodes)
                        (elem as HTMLHeadElement).appendChild((IHTMLDOMNode)scriptErrorSuppressed);
                }
            }));

            Thread.Sleep(3000);

            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    /*
                     *  Проверка корректности ввода капчи
                     */
                    var doc = WB.Document as HTMLDocument;
                    IHTMLElementCollection nodes = doc.getElementsByTagName("body");
                    foreach (IHTMLElement elem in nodes)
                    {
                        var body = (HTMLHeadElement)elem;

                        if (body.innerHTML.IndexOf("Символы указаны неверно") > -1)
                        {
                            UpdateCaptcha();

                            Dispatcher.BeginInvoke(new ThreadStart(delegate
                            {
                                try
                                {
                                    CaptionStatus.Text = "Error";
                                    tbCaptcha.Text = "";
                                    resultErrors = "error";

                                    MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden;
                                    Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));
                                }
                                catch (Exception) { }
                            }));
                        }


                        /*
                         *  Проверка не вышла ли другая ошибка
                         */
                        if (resultErrors == "")
                        {
                            Regex regexErrors = new Regex("<P class=js-form-errors-content>(.*)</P>", RegexOptions.IgnoreCase);

                            Match matchErrors = regexErrors.Match(body.innerHTML);
                            while (matchErrors.Success)
                            {
                                resultErrors = matchErrors.Value.Trim();
                                resultErrors = resultErrors
                                    .Replace("<P class=js-form-errors-content>", "")
                                    .Replace("<p class=js-form-errors-content>", "")
                                    .Replace("</P>", "")
                                    .Replace("</p>", "");

                                if (resultErrors == "")
                                    matchErrors = matchErrors.NextMatch();
                                else
                                {
                                    CaptionStatus.Text = "Error";
                                    tbCaptcha.Text = "";

                                    UpdateCaptcha();

                                    break;
                                }
                            }

                            System.IO.File.WriteAllText(@"c:\Users\Helldar\AppData\Roaming\Wargaming.net\WorldOfTanks\multipack_launcher\6.txt", body.innerHTML);
                        }

                        /*
                         *      Если ошибка найдена
                         */
                        if (resultErrors != "")
                        {

                        }
                    }
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("WgOpenIdAIRUS.xaml", "CheckCaptcha()", ex.Message, ex.StackTrace)); }
            }));
        }

        private void bReloadCaptcha_Click(object sender, RoutedEventArgs e)
        {
            UpdateCaptcha();
        }
    }
}
