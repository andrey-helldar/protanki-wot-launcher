using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace _Hell_Process_List
{
    public partial class fIndex : Form
    {
        public fIndex()
        {
            InitializeComponent();

            if (!bwLoad.IsBusy) { bwLoad.RunWorkerAsync(); }
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clbProcesses.Items.Clear();

            string s = "";

            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Process");

            foreach (ManagementObject queryObj in searcher1.Get())
            {
                clbProcesses.Items.Add(queryObj["Name"].ToString());
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!bwSave.IsBusy) { bwSave.RunWorkerAsync(); } else { MessageBox.Show("Подождите завершения предыдущей операции"); }
        }

        private void bwSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string str = "";

            foreach(object item in clbProcesses.CheckedItems){
                str += item.ToString().Replace(".exe", "") + Environment.NewLine;
            }

            if (File.Exists("log.txt")) { File.Delete("log.txt"); }

            File.WriteAllText("log.txt", str);

            MessageBox.Show("Данные успешно сохранены в файл log.txt папке с данной программой");
        }
    }
}
