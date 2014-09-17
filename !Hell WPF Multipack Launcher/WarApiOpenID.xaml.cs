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
using System.Windows.Shapes;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for WarApiOpenID.xaml
    /// </summary>
    public partial class WarApiOpenID : Window
    {
        public WarApiOpenID()
        {
            InitializeComponent();
        }

        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (MainWindow.XmlDocument.Root.Element("account") != null)
                if (MainWindow.XmlDocument.Root.Element("account").Element("token") != null)
                    if (MainWindow.XmlDocument.Root.Element("account").Element("token").Attribute("access_token") != null)
                        MainWindow.XmlDocument.Root.Element("account").Element("token").Attribute("access_token").SetValue(WB.Source.ToString());
            /* else
                 MainWindow.XmlDocument.Root.Element("account").Element("token").SetValue(WB.Source.ToString());*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            eeeew.Text = WB.Source.ToString() + Environment.NewLine + Environment.NewLine +
                new Classes.WargamingAPI().Token(MainWindow.XmlDocument.Root.Element("account").Element("token").Attribute("access_token").Value)["access_token"] + Environment.NewLine + Environment.NewLine +
                MainWindow.XmlDocument.Root.Element("account").Element("token").Attribute("access_token").Value + Environment.NewLine + Environment.NewLine;
        }
    }
}
