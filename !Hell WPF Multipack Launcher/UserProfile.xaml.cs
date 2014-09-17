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

        private void AccountInfo()
        {
            /*if (MainWindow.XmlDocument.Root.Element("info") != null)
                if (MainWindow.XmlDocument.Root.Element("info").Element("token") != null)
                    if (MainWindow.XmlDocument.Root.Element("info").Element("token").Attribute("expires_at") != null)
                        if (MainWindow.XmlDocument.Root.Element("info").Element("token").Attribute("expires_at").Value != "")
                            if (DateTime.Parse(MainWindow.XmlDocument.Root.Element("info").Element("token").Attribute("expires_at").Value) <= DateTime.Now)
                    {*/
                        Classes.WargamingAPI WarAPI = new Classes.WargamingAPI();
                                
                        Dispatcher.BeginInvoke(new ThreadStart(delegate
                        {
                            WarApiOpenID WarApiOpenID = new WarApiOpenID();
                            WarApiOpenID.WB.Source = new Uri(WarAPI.OpenID());
                            WarApiOpenID.ShowDialog();
                        }));
                    /*}*/
        }
    }
}
