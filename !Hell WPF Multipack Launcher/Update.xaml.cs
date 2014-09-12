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
        Classes.POST POST = new Classes.POST();


        public Update()
        {
            InitializeComponent();

            MultipackUpdate();
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbNotify.IsChecked == true)
                MainWindow.XmlDocument.Root.Element("info").Attribute("notification").SetValue(new Classes.Variables().VersionFromSharp(newVersion.Content.ToString()));

            MainWindow.Navigator("General", "Update.xaml");
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("General", "Update.xaml");
        }

        public bool MultipackUpdate()
        {
            try
            {
                var json = POST.JsonResponse(Properties.Resources.JsonUpdates);

                string mType = "base";
                if (MainWindow.XmlDocument.Root.Element("multipack") != null)
                    if (MainWindow.XmlDocument.Root.Element("multipack").Attribute("type") != null)
                        if (MainWindow.XmlDocument.Root.Element("multipack").Attribute("type").Value != "")
                            mType = MainWindow.XmlDocument.Root.Element("multipack").Attribute("type").Value;

                string thisVersion = "0.0.0.0";
                if (MainWindow.XmlDocument.Root.Element("multipack") != null)
                    if (MainWindow.XmlDocument.Root.Element("multipack").Attribute("version") != null)
                        if (MainWindow.XmlDocument.Root.Element("multipack").Attribute("version").Value != "")
                            thisVersion = MainWindow.XmlDocument.Root.Element("multipack").Attribute("version").Value;
                
                string lang = Properties.Resources.Default_Lang;
                if (MainWindow.XmlDocument.Root.Element("info") != null)
                    if (MainWindow.XmlDocument.Root.Element("info").Attribute("language") != null)
                        if (MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value != "")
                            lang = MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value;

                Version remoteVersion = new Version(new Classes.Variables().VersionPrefix(new Version(thisVersion)) + json[mType]["version"]);

                newVersion.Content = remoteVersion.ToString();
                tbContent.Text = json[mType]["changelog"][lang].ToString();

                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
