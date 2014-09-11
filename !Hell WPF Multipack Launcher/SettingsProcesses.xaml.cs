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
    /// Interaction logic for SettingsProcesses.xaml
    /// </summary>
    public partial class SettingsProcesses : Page
    {
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
            gridPanel0.Height = double.NaN;
            gridPanel0.Margin = new Thickness(5, 5, 5, 5);
            gridPanel0.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            ColumnDefinition gridColumn0 = new ColumnDefinition();
            gridColumn0.Width = GridLength.Auto;
            gridPanel0.ColumnDefinitions.Add(gridColumn0);

            ColumnDefinition gridColumn02 = new ColumnDefinition();
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
            lbi0.SetResourceReference(TextBlock.StyleProperty, "ListBoxItemProcesses");
            lbi0.Content = gridPanel0;

            lbProcesses.Items.Add(lbi0);



            for (int i = 0; i < 10; i++)
            {
                Grid gridPanel = new Grid();
                gridPanel.Width = double.MaxValue;
                gridPanel.Margin = new Thickness(5, 5, 5, 5);
                gridPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                ColumnDefinition gridColumn1 = new ColumnDefinition();
                gridColumn1.Width = GridLength.Auto;
                gridPanel.ColumnDefinitions.Add(gridColumn1);

                ColumnDefinition gridColumn2 = new ColumnDefinition();
                gridColumn2.Width = GridLength.Auto;
                gridPanel.ColumnDefinitions.Add(gridColumn2);
                gridPanel.ColumnDefinitions.Add(new ColumnDefinition());

                CheckBox checkBox = new CheckBox();
                checkBox.Margin = new Thickness(0);
                checkBox.Checked += ProcessChanged;
                Grid.SetRow(checkBox, 0);
                Grid.SetColumn(checkBox, 0);
                gridPanel.Children.Add(checkBox);


                Label label = new Label();
                label.Content = "THIS IS A PROCESS!!!";
                Grid.SetRow(label, 0);
                Grid.SetColumn(label, 1);
                gridPanel.Children.Add(label);

                Label label12 = new Label();
                label12.Content = "DESC : " + i.ToString();
                Grid.SetRow(label12, 0);
                Grid.SetColumn(label12, 2);
                gridPanel.Children.Add(label12);

                ListBoxItem lbi = new ListBoxItem();
                //lbi.Width = double.NaN;
                lbi.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                lbi.SetResourceReference(TextBlock.StyleProperty, "ListBoxItemProcesses");
                lbi.Content = gridPanel;

                lbProcesses.Items.Add(lbi);
            }
        }

        private void ProcessChanged(object sender, RoutedEventArgs e)
        {
        }
    }
}
