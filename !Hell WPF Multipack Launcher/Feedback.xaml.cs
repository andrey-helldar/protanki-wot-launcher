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
    /// Interaction logic for Feedback.xaml
    /// </summary>
    public partial class Feedback : Page
    {
        public Feedback()
        {
            InitializeComponent();
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("General", "Settings.xaml");
        }

        private void bAttach_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bSend_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            Classes.POST POST = new Classes.POST();

            json.Add("api", Properties.Resources.API);
            json.Add("youtube", Properties.Resources.YoutubeChannel);
            json.Add("project", new Classes.Variables().ProductName);
            json.Add("project_version", new Classes.Variables().MultipackVersion.ToString());

            string cat = String.Empty;
            if (rbWishMultipack.IsChecked == true)
            {
                cat = "WM";
            }else
                if (rbWishLauncher.IsChecked == true)
                {
                    cat = "WL";
                }else
                    if (rbWishInstaller.IsChecked == true)
                    {
                        cat = "WI";
                    } if (rbErrorMultipack.IsChecked == true)


            json.Add("category", cat);
            json.Add("message",  POST.Shield(tbMessage.Text));
            json.Add("email", POST.Shield(tbEmail.Text));
        }
    }
}
