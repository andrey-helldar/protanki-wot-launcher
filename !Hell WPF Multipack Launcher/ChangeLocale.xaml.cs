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
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for ChangeLocale.xaml
    /// </summary>
    public partial class ChangeLocale : Page
    {
        Classes.Language Lang = new Classes.Language();
        Classes.Debugging Debugging = new Classes.Debugging();

        public ChangeLocale()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                JObject obj = Lang.Translated();

                Dispatcher.BeginInvoke(new ThreadStart(delegate { lbLocales.Items.Clear(); }));

                if (obj != null)
                    foreach (var lang in obj)
                    {
                        Dispatcher.BeginInvoke(new ThreadStart(delegate
                        {
                            Grid grid = new Grid();
                            grid.Width = double.NaN;
                            grid.Margin = new Thickness(0);
                            grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                            ColumnDefinition cd1 = new ColumnDefinition();
                            ColumnDefinition cd2 = new ColumnDefinition();
                            cd1.Width = new GridLength(1, GridUnitType.Auto);
                            cd2.Width = new GridLength(1, GridUnitType.Star);
                            grid.ColumnDefinitions.Add(cd1);
                            grid.ColumnDefinitions.Add(cd2);

                            Image img = new Image();
                            img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            img.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            img.Height = 20;
                            img.Width = 20;
                            img.Margin = new Thickness(10, 10, 5, 10);
                            img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/flag_{1}.png", (string)MainWindow.JsonSettingsGet("info.ProductName"), (string)lang.Key)));

                            TextBlock tb = new TextBlock();
                            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            tb.Margin = new Thickness(5, 10, 10, 10);
                            tb.TextWrapping = TextWrapping.NoWrap;
                            tb.FontWeight = FontWeights.Bold;
                            tb.FontSize = 14;
                            tb.Text = ((string)lang.Value).Remove(0, 3);
                            Grid.SetColumn(tb, 1);

                            grid.Children.Add(img);
                            grid.Children.Add(tb);

                            ListBoxItem lbi = new ListBoxItem();
                            lbi.IsSelected = (string)lang.Key == (string)MainWindow.JsonSettingsGet("info.language");
                            lbi.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lbi.Cursor = Cursors.Hand;
                            lbi.Content = grid;
                            lbi.Name = (string)lang.Key;
                            this.RegisterName(lbi.Name, lbi);

                            lbLocales.Items.Add(lbi);
                        }));
                }
            }
            catch (Exception ex) { Debugging.Save("ChangeLocale.xaml", "Page_Loaded", ex.Message, ex.StackTrace); }

            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try { MainWindow.LoadPage.Visibility = Visibility.Hidden; }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("ChangeLocale.xaml", "Page_Loaded", ex.Message, ex.StackTrace)); }
            }));
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                MainWindow.LoadPage.Content = Lang.Set("PageLoading", "lLoading", (string)MainWindow.JsonSettingsGet("info.language"));
                MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible;

                MainWindow.Flag.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/flag_{1}.png", (string)MainWindow.JsonSettingsGet("info.ProductName"), (string)MainWindow.JsonSettingsGet("info.language"))));
                MainWindow.Flag.ToolTip = Lang.Set("Tooltip", "rectLang", (string)MainWindow.JsonSettingsGet("info.language"));

                Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

                Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
            }));
        }

        private void lbLocales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);

                MainWindow.JsonSettingsSet("info.language", lbi.Name);
                MainWindow.JsonSettingsSet("info.locale", 2, "int");
                
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    try
                    {
                        bClose.Content = Lang.Set("PageSettings", "bClose", lbi.Name);
                        MainWindow.PlayBtn.Text = Lang.Set("MainProject", "bPlay", lbi.Name);
                        MainWindow.Flag.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/flag_{1}.png", (string)MainWindow.JsonSettingsGet("info.ProductName"), lbi.Name)));

                        MainWindow.LoadPage.Visibility = Visibility.Hidden;
                    }
                    catch (Exception ex) { Debugging.Save("ChangeLocale.xaml", "lbLocales_SelectionChanged", "Innter Catch", ex.Message, ex.StackTrace); }
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("ChangeLocale.xaml", "lbLocales_SelectionChanged", ex.Message, ex.StackTrace)); }
        }
    }
}
