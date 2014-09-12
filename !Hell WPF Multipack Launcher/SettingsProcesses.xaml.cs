using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
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

        public SettingsProcesses()
        {
            InitializeComponent();

            Processes();
        }

        private void Processes()
        {
            lbProcesses.Items.Clear();
            /*
             * 
             */
            Grid gridPanel0 = new Grid();
            gridPanel0.Width = double.NaN;
            gridPanel0.Margin = new Thickness(0);
            gridPanel0.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            ColumnDefinition gridColumn0 = new ColumnDefinition();
            gridColumn0.Width = new GridLength(30, GridUnitType.Pixel);
            gridPanel0.ColumnDefinitions.Add(gridColumn0);

            ColumnDefinition gridColumn02 = new ColumnDefinition();
            gridColumn02.Width = new GridLength(150, GridUnitType.Pixel);
            gridPanel0.ColumnDefinitions.Add(gridColumn02);
            gridPanel0.ColumnDefinitions.Add(new ColumnDefinition());

            Label label0 = new Label();
            label0.Margin = new Thickness(0);
            label0.Content = "#";
            label0.Margin = new Thickness(5);
            label0.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            label0.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Grid.SetRow(label0, 0);
            Grid.SetColumn(label0, 0);
            gridPanel0.Children.Add(label0);

            Label label1 = new Label();
            label1.Content = "Process";
            label1.Margin = new Thickness(5);
            label1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            label1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Grid.SetRow(label1, 0);
            Grid.SetColumn(label1, 1);
            gridPanel0.Children.Add(label1);

            Label label2 = new Label();
            label2.Content = "Description";
            label2.Margin = new Thickness(5);
            label2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            label2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Grid.SetRow(label2, 0);
            Grid.SetColumn(label2, 2);
            gridPanel0.Children.Add(label2);

            ListBoxItem lbi0 = new ListBoxItem();
            //lbi0.Width = double.NaN;
            lbi0.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            lbi0.SetResourceReference(TextBlock.StyleProperty, "ListBoxItemProcessesCaption");
            lbi0.Content = gridPanel0;

            lbProcesses.Items.Add(lbi0);


            /*
             * Загружаем список процессов
             */
            Process[] myProcesses = Process.GetProcesses();
            int processID = Process.GetCurrentProcess().SessionId;

            for (int i = 1; i < myProcesses.Length; i++)
            {
                try
                {
                    if (myProcesses[i].SessionId == processID)
                        if (!ProcessList.IndexOf(myProcesses[i].ProcessName) && myProcesses[i].ProcessName != Process.GetCurrentProcess().ProcessName)
                        {
                            Grid gridPanel = new Grid();
                            gridPanel.Width = double.NaN;
                            gridPanel.Margin = new Thickness(0);
                            gridPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                            ColumnDefinition gridColumn1 = new ColumnDefinition();
                            //gridColumn1.Width = GridLength.Auto;
                            gridColumn1.Width = new GridLength(30, GridUnitType.Pixel);
                            gridPanel.ColumnDefinitions.Add(gridColumn1);

                            ColumnDefinition gridColumn2 = new ColumnDefinition();
                            gridColumn2.Width = new GridLength(150, GridUnitType.Pixel);
                            gridPanel.ColumnDefinitions.Add(gridColumn2);
                            gridPanel.ColumnDefinitions.Add(new ColumnDefinition());

                            CheckBox checkBox = new CheckBox();
                            checkBox.Margin = new Thickness(5, 0, 0, 0);
                            checkBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            checkBox.IsChecked = CheckUserProcess(myProcesses[i].ProcessName);
                            checkBox.Checked += ProcessChanged;
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
                            //lbi.Width = double.NaN;
                            lbi.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                            lbi.SetResourceReference(TextBlock.StyleProperty, CheckUserProcess(myProcesses[i].ProcessName) ? "ProcessesChecked" : "ProcessesUnChecked");
                            lbi.Content = gridPanel;

                            lbProcesses.Items.Add(lbi);
                        }
                }
                catch (Exception ex) { /*Debug.Save("bwUserProcesses_DoWork()", myProcesses[i].ProcessName, ex.Message);*/ }
            }
        }

        /// <summary>
        /// Запрещал ли пользователь закрытие процесса при оптимизации ПК?
        /// </summary>
        /// <param name="proc">Имя процесса для проверки</param>
        /// <returns>Если процесс есть в списке, выводим TRUE, иначе - FALSE</returns>
        private bool CheckUserProcess(string proc)
        {
            if (MainWindow.XmlDocument.Root.Element("processes") != null)
                if (MainWindow.XmlDocument.Root.Element("processes").Element(proc) != null)
                    return true;

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
            catch (Exception) { return "ProcessesUnChecked"; }
        }

        private void ProcessChanged(object sender, RoutedEventArgs e)
        {
        }
    }
}
