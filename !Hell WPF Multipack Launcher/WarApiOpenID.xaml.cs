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
                    Dictionary<string, string> Token = WarAPI.Token(WB.Source.ToString());

                    MessageBox.Show(WarAPI.TokenString(WB.Source.ToString()));

                    if (Token["status"] == "ok")
                    {
                        if (MainWindow.XmlDocument.Root.Element("token") != null)
                        {
                            if (MainWindow.XmlDocument.Root.Element("token").Attribute("access_token") != null)
                                MainWindow.XmlDocument.Root.Element("token").Attribute("access_token").SetValue(Token["access_token"]);

                            if (MainWindow.XmlDocument.Root.Element("token").Attribute("expires_at") != null)
                                MainWindow.XmlDocument.Root.Element("token").Attribute("expires_at").SetValue(Token["expires_at"]);

                            if (MainWindow.XmlDocument.Root.Element("token").Attribute("nickname") != null)
                                MainWindow.XmlDocument.Root.Element("token").Attribute("nickname").SetValue(Token["nickname"]);

                            if (MainWindow.XmlDocument.Root.Element("token").Attribute("account_id") != null)
                                MainWindow.XmlDocument.Root.Element("token").Attribute("account_id").SetValue(Token["account_id"]);
                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(WB.Source.ToString());
                    }
                }
            }
            catch (Exception) { MessageBox.Show(WB.Source.ToString()); }
        }
    }
}
