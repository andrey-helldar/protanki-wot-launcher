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
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Page
    {
        public Update()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbNotify.IsChecked == true)
                MainWindow.XmlDocument.Root.Element("info").Attribute("notification").SetValue(new Classes.Variables().VersionFromSharp(newVersion.Content.ToString()));

            MainWindow.Navigator("General", "Update.xaml");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("General", "Update.xaml");
        }
    }
}
