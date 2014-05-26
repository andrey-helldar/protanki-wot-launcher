using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Notify.xaml
    /// </summary>
    public partial class Notify : Window
    {
        public Notify()
        {
            InitializeComponent();
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            if (cbNotNotify.IsChecked.Value)
            {
                    Match vers = Regex.Match(lCaption.Content.ToString(), @"\((.*)\)");


                XDocument doc = XDocument.Load("settings.xml");
                if (doc.Root.Element("notification") == null)
                    doc.Root.Add(new XElement("notification", vers.Groups[1].Value));
                else
                    doc.Root.Element("notification").SetValue(vers.Groups[1].Value);

                doc.Save("settings.xml");
            }

            Close();
        }

        private void bDownload_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
