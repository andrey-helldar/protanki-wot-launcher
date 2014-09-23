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
using System.Diagnostics;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Page
    {
        Classes.POST POST = new Classes.POST();
        Classes.Debug Debug = new Classes.Debug();

        private string downloadLink = String.Empty;


        public Update()
        {
            InitializeComponent();

            Task.Factory.StartNew(() => MultipackUpdate());
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbNotify.IsChecked == true)
                    MainWindow.XmlDocument.Root.Element("info").Attribute("notification").SetValue(new Classes.Variables().VersionFromSharp(newVersion.Content.ToString()));

                if (downloadLink!=String.Empty)
                    Process.Start(downloadLink);

                MainWindow.Navigator("General", "Update.xaml");
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Update.xaml", "bUpdate_Click()", ex.Message, ex.StackTrace)); }
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            try { MainWindow.Navigator("General", "Update.xaml"); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Update.xaml", "bCancel_Click()", ex.Message, ex.StackTrace)); }
        }

        public void MultipackUpdate()
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

                if (MainWindow.XmlDocument.Root.Element("multipack") != null)
                    if (MainWindow.XmlDocument.Root.Element("multipack").Element("link") != null)
                        if (MainWindow.XmlDocument.Root.Element("multipack").Element("link").Value != "")
                            downloadLink = MainWindow.XmlDocument.Root.Element("multipack").Element("link").Value;


                newVersion.Content = new Version(new Classes.Variables().VersionPrefix(new Version(thisVersion)) + json[mType]["version"]).ToString();
                tbContent.Text = json[mType]["changelog"][lang].ToString();
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Update.xaml", "MultipackUpdate()", ex.Message, ex.StackTrace)); }
        }
    }
}
