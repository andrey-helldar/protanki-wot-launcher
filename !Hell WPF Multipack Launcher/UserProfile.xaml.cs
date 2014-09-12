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
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Page
    {
        public UserProfile()
        {
            InitializeComponent();

            if (MainWindow.XmlDocument.Root.Element("info") != null)
                if (MainWindow.XmlDocument.Root.Element("info").Attribute("player") != null)
                    if (MainWindow.XmlDocument.Root.Element("info").Attribute("player").Value != "")
                        runPlayer.Text = MainWindow.XmlDocument.Root.Element("info").Attribute("player").Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("General", "Settings.xaml");
        }
    }
}
