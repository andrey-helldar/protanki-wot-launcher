using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LocalInterface.LocInterface LocalInterface = new LocalInterface.LocInterface();

        public MainWindow()
        {
            InitializeComponent();
            MouseDown += delegate { DragMove(); };
        }

        private void bExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(()=>SetBack());
        }

        private void SetBack()
        {
            while (true)
            {
                this.Background = LocalInterface.Background().Result;
                Thread.Sleep(5000);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Multipack Launcher;component/Resources/back_2.jpg")));
            Close();
        }
    }
}