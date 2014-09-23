using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try { Task.Factory.StartNew(() => Processes()); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsProcesses.xaml", "Page_Loaded()", ex.Message, ex.StackTrace)); }
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
                                        gridPanel.Width = double.NaN;
                                        gridPanel.Margin = new Thickness(0);
                                        gridPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                                        ColumnDefinition gridColumn1 = new ColumnDefinition();
                                        gridColumn1.Width = new GridLength(30, GridUnitType.Pixel);
                                        gridPanel.ColumnDefinitions.Add(gridColumn1);

                                        ColumnDefinition gridColumn2 = new ColumnDefinition();
                                        gridColumn2.Width = new GridLength(150, GridUnitType.Pixel);
                                        gridPanel.ColumnDefinitions.Add(gridColumn2);
                                        gridPanel.ColumnDefinitions.Add(new ColumnDefinition());

                                        CheckBox checkBox = new CheckBox();
                                        checkBox.Margin = new Thickness(5, 0, 0, 0);
                                        checkBox.Name = "Cb" + myProcesses[i].ProcessName;
                                        checkBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                                        if (ProccessLibrary.Search(myProcesses[i].ProcessName))
                                        {
                                            checkBox.IsChecked = true;
                                            checkBox.IsEnabled = false;
                                        }
                                        else
                                        {
                                            checkBox.IsChecked = CheckUserProcess(myProcesses[i].ProcessName);
                                            checkBox.IsEnabled = true;
                                        }
                                        checkBox.Click += ProcessChanged;
                                        Grid.SetRow(checkBox, 0);
                                        Grid.SetColumn(checkBox, 0);
                                        gridPanel.Children.Add(checkBox);


                                        Label label = new Label();
                                        label.Content = myProcesses[i].ProcessName;
                                        label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                        Grid.SetRow(label, 0);
                                        Grid.SetColumn(label, 1);
                                        gridPanel.Children.Add(label);

                                        Label label12 = new Label();
                                        label12.Content = myProcesses[i].MainModule.FileVersionInfo.FileDescription.Trim();
                                        label12.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                        Grid.SetRow(label12, 0);
                                        Grid.SetColumn(label12, 2);
                                        gridPanel.Children.Add(label12);

                                        ListBoxItem lbi = new ListBoxItem();
                                        lbi.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                                        lbi.SetResourceReference(TextBlock.StyleProperty, Style(myProcesses[i].ProcessName));
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
                if (MainWindow.XmlDocument.Root.Element("processes") != null)
                    if (MainWindow.XmlDocument.Root.Element("processes").Element(proc) != null)
                        return true;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsProcesses.xaml", "CheckUserProcess()", "Process: " + proc, ex.Message, ex.StackTrace)); }

            return false;
        }


        /// <summary>
        /// Проверка входит ли процесс в список запрещенных к закрытию
        /// </summary>
        /// <param name="process">Имя процесса</param>
        /// <returns>Применяемый визуальный стиль</returns>
        private string Style(string process)
        {
            try
            {
                if (ProccessLibrary.Search(process)) return "ProcessesCheckedGlobal";
                if (CheckUserProcess(process)) return "ProcessesChecked";
                return "ProcessesUnChecked";
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsProcesses.xaml", "Style()", "Process: " + process, ex.Message, ex.StackTrace)); }

            return "ProcessesUnChecked";
        }

        private void ProcessChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListBoxItem el = (((sender as CheckBox).Parent as Grid).Parent as ListBoxItem);

            string processName = cb.Name.Remove(0, 2);

            try
            {
                XElement elem = MainWindow.XmlDocument.Root.Element("processes");

                if (cb.IsChecked == true)
                {
                    if (elem != null)
                        if (elem.Element(processName) == null)
                            elem.Add(new XElement(processName, null));
                }
                else
                {
                    if (elem != null)
                        if (elem.Element(processName) != null)
                            elem.Element(processName).Remove();
                }

                el.SetResourceReference(TextBlock.StyleProperty, Style(cb.Name.Remove(0, 2)));
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("SettingsProcesses.xaml", "ProcessChanged()", ex.Message, ex.StackTrace));
                el.SetResourceReference(TextBlock.StyleProperty, "ProcessesUnChecked");
            }
        }
    }
}
