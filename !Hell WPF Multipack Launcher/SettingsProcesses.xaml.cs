using System;
using System.Collections.Generic;
using System.Linq;
//using System.Xml.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
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

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for SettingsProcesses.xaml
    /// </summary>
    public partial class SettingsProcesses : Page
    {
        Processes.Global ProccessLibrary = new Processes.Global();
        Processes.Listing ProcessList = new Processes.Listing();

        Classes.Debug Debug = new Classes.Debug();

        public SettingsProcesses()
        {
            InitializeComponent();

            gbProcesses.Header = new Classes.Language().Set("PageSettingsProcesses", "lProcesses", (string)MainWindow.JsonSettingsGet("info.language"));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try { Task.Factory.StartNew(() => Processes()).Wait(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsProcesses.xaml", "Page_Loaded()", ex.Message, ex.StackTrace)); }

            try { MainWindow.LoadingPanelShow(); }
            catch (Exception) { }
        }

        private void Processes()
        {
            try
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    lbProcesses.Items.Clear();
                    
                    Process[] myProcesses = Process.GetProcesses();
                    int processID = Process.GetCurrentProcess().SessionId;

                    List<string> DoubleProcess = new List<string>();

                    for (int i = 1; i < myProcesses.Length; i++)
                    {
                        try
                        {
                            if (myProcesses[i].SessionId == processID)
                                if (!ProcessList.IndexOf(myProcesses[i].ProcessName) && myProcesses[i].ProcessName != Process.GetCurrentProcess().ProcessName)
                                {
                                    //  Проверяем список на наличие дубликатов
                                    if (DoubleProcess.IndexOf(myProcesses[i].ProcessName) == -1)
                                    {
                                        DoubleProcess.Add(myProcesses[i].ProcessName);

                                        
                                        //  Заполняем данные
                                        Grid gridPanel = new Grid();
                                        gridPanel.Width = 459;

                                        ColumnDefinition cd1 = new ColumnDefinition();
                                        ColumnDefinition cd2 = new ColumnDefinition();
                                        ColumnDefinition cd3 = new ColumnDefinition();
                                        cd1.Width = new GridLength(10, GridUnitType.Auto);
                                        cd2.Width = new GridLength(140, GridUnitType.Pixel);
                                        cd3.Width = new GridLength(1, GridUnitType.Star);
                                        gridPanel.ColumnDefinitions.Add(cd1);
                                        gridPanel.ColumnDefinitions.Add(cd2);
                                        gridPanel.ColumnDefinitions.Add(cd3);

                                        CheckBox cb = new CheckBox();
                                        cb.Margin = new Thickness(5,0,5,0);
                                        cb.Name = "Cb" + myProcesses[i].ProcessName;
                                        this.RegisterName(cb.Name, cb);
                                        cb.Click += ProcessChanged;
                                        if (ProccessLibrary.Search(myProcesses[i].ProcessName))
                                        {
                                            cb.IsChecked = true;
                                            cb.IsEnabled = false;
                                        }
                                        else
                                        {
                                            cb.IsChecked = CheckUserProcess(myProcesses[i].ProcessName);
                                            cb.IsEnabled = true;
                                        }
                                        Grid.SetColumn(cb, 0);
                                        gridPanel.Children.Add(cb);

                                        TextBlock tb1 = new TextBlock();
                                        tb1.SetResourceReference(TextBlock.StyleProperty, "lbiProcessB");
                                        tb1.Text = myProcesses[i].ProcessName;
                                        Grid.SetColumn(tb1, 1);
                                        gridPanel.Children.Add(tb1);

                                        TextBlock tb2 = new TextBlock();
                                        tb2.Text = myProcesses[i].MainModule.FileVersionInfo.FileDescription.Trim();
                                        Grid.SetColumn(tb2, 2);
                                        gridPanel.Children.Add(tb2);


                                        ListBoxItem lbi = new ListBoxItem();
                                        //lbi.SetResourceReference(ListBoxItem.StyleProperty, i % 2 == 0 ? "lbiProcess2" : "lbiProcess");
                                        lbi.SetResourceReference(ListBoxItem.StyleProperty, "lbiProcess");
                                        lbi.Content = gridPanel;


                                        lbProcesses.Items.Add(lbi);
                                    }
                                }
                        }
                        catch (Exception) { }
                    }

                    DoubleProcess = null;
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsProcesses.xaml", "Processes()", ex.Message, ex.StackTrace)); }
        }

        /// <summary>
        /// Запрещал ли пользователь закрытие процесса при оптимизации ПК?
        /// </summary>
        /// <param name="proc">Имя процесса для проверки</param>
        /// <returns>Если процесс есть в списке, выводим TRUE, иначе - FALSE</returns>
        private bool CheckUserProcess(string proc)
        {
            try
            {
                string processes = MainWindow.JsonSettingsGet("processes").ToString();
                if (processes.IndexOf(proc) != -1) return true;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsProcesses.xaml", "CheckUserProcess()", "Process: " + proc, ex.Message, ex.StackTrace)); }
            return false;
        }


        /// <summary>
        /// Проверка входит ли процесс в список запрещенных к закрытию
        /// </summary>
        /// <param name="process">Имя процесса</param>
        /// <returns>Применяемый визуальный стиль</returns>
        /*private string Style(string process)
        {
            try
            {
                if (ProccessLibrary.Search(process)) return "ProcessesCheckedGlobal";
                if (CheckUserProcess(process)) return "ProcessesChecked";
                return "ProcessesUnChecked";
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsProcesses.xaml", "Style()", "Process: " + process, ex.Message, ex.StackTrace)); }

            return "ProcessesUnChecked";
        }*/

        private void ProcessChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListBoxItem el = (((sender as CheckBox).Parent as Grid).Parent as ListBoxItem);

            string processName = cb.Name.Remove(0, 2);

            try
            {
                if (cb.IsEnabled)
                {
                    if (cb.IsChecked == true)
                    {
                        MainWindow.JsonSettingsSet("processes", processName, "array");
                        el.SetResourceReference(ListBoxItem.StyleProperty, "lbiProcessCheck");
                    }
                    else
                    {
                        MainWindow.JsonSettingsRemove("processes." + processName);
                        el.SetResourceReference(ListBoxItem.StyleProperty, "lbiProcessUnCheck");
                    }
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("SettingsProcesses.xaml", "ProcessChanged()", ex.Message, ex.StackTrace));
            }
        }
    }
}
