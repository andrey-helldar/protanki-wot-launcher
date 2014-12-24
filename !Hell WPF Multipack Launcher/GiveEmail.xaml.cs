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

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for GiveEmail.xaml
    /// </summary>
    public partial class GiveEmail : Page
    {
        Classes.Debugging Debugging = new Classes.Debugging();

        public GiveEmail()
        {
            InitializeComponent();

            Task.Factory.StartNew(() => SetText());
        }

        private void SetText()
        {
            try
            {
                Classes.Language Lang = new Classes.Language();

                string lang = (string)MainWindow.JsonSettingsGet("info.language");

                tbCaption.Text = Lang.Set("GiveEmail", "Caption", lang);
                tbMessage.Text = Lang.Set("GiveEmail", "Message", lang);

                Iagree.Content = Lang.Set("Button", "Iagree", lang);
                Idisagree.Content = Lang.Set("Button", "Idisagree", lang);
            }
            catch (Exception ex) { Debugging.Save("GiveEmail.xaml", "SetText()", ex.Message, ex.StackTrace); }
        }

        private void Iagree_Click(object sender, RoutedEventArgs e)
        {
            try { MainWindow.JsonSettingsSet("info.user_accept", true, "bool"); }
            catch (Exception ex) { Debugging.Save("GiveEmail.xaml", "Iagree_Click()", ex.Message, ex.StackTrace); }
        }

        private void Idisagree_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.JsonSettingsSet("info.user_accept", false, "bool");
                MainWindow.Navigator();
            }
            catch (Exception ex) { Debugging.Save("GiveEmail.xaml", "Idisagree_Click()", ex.Message, ex.StackTrace); }
        }
    }
}
