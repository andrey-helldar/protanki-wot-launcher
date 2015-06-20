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
        public WgOpenIdAIRUS()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden;
                Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));
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
    }
}
