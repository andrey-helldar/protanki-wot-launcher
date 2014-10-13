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

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Error.xaml
    /// </summary>
    public partial class Error : Page
    {
        public Error()
        {
            InitializeComponent();

            bClose.Content = new Classes.Language().Set("PageSettings", "bClose", MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value.Trim());
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {       
            MainWindow.LoadingPanelShow(1);

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate { MainWindow.Navigator(); }));
                }
                catch (Exception ex) { System.Threading.Tasks.Task.Factory.StartNew(() => new Classes.Debug().Save("Feedback.xaml", "bClose_Click()", ex.Message, ex.StackTrace)); }
            });
        }

        private void PageError_Loaded(object sender, RoutedEventArgs e)
        {
            try { MainWindow.LoadingPanelShow(); }
            catch (Exception) { }
        }
    }
}
