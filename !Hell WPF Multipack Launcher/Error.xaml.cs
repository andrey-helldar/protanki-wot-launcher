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
    /// Interaction logic for Error.xaml
    /// </summary>
    public partial class Error : Page
    {
        public Error()
        {
            InitializeComponent();
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            try { MainWindow.Navigator("General", "Error.xaml"); }
            catch (Exception ex) { System.Threading.Tasks.Task.Factory.StartNew(() => new Classes.Debug().Save("Feedback.xaml", "bClose_Click()", ex.Message, ex.StackTrace)); }
        }
    }
}
