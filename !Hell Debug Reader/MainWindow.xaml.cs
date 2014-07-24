using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;

namespace _Hell_Debug_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MouseDown += delegate { DragMove(); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvDebugs.SelectedIndex > -1)
            {
                Dictionary<string, string> result = Json(File.ReadAllText(tbPath.Text + @"\" + lvDebugs.SelectedItem.ToString()));
                lParams.Text = String.Empty;

                string param = "{0}:\n----------\n{1}\n----------\n\n";

                lModule.Content = result["module"];
                lFunction.Content = result["function"];
                lUserID.Content = result["uid"];
                lVersion.Content = result["version"];
                lDate.Content = result["date"];

                foreach (var res in result)
                    if (res.Key.IndexOf("param") > -1)
                        lParams.Text += String.Format(param, res.Key, res.Value);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Выберите папку с дебагами:";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
                MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            if ((bool)dialog.ShowDialog(this))
            {
                tbPath.Text = dialog.SelectedPath;
                lvDebugs.ItemsSource = new DirectoryInfo(dialog.SelectedPath).GetFiles("*.debug");
            }
        }

        public Dictionary<string, string> Json(string json)
        {
            try { return JsonConvert.DeserializeObject<Dictionary<string, string>>(json); }
            catch (Exception) { return null; }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить файл дебага \"" + lvDebugs.SelectedItem.ToString() + "\"?", "Удаление дебага", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                File.Delete(tbPath.Text + @"\" + lvDebugs.SelectedItem.ToString());

                lvDebugs.ItemsSource = new DirectoryInfo(tbPath.Text).GetFiles("*.debug");

                lModule.Content = "---";
                lFunction.Content = "---";
                lUserID.Content = "---";
                lVersion.Content = "---";
                lDate.Content = "---";
                tbPath.Text = "---";

                MessageBox.Show("Файл успешно удален!");
            }
        }
    }
}
