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
        string filename = "";

        public fIndex()
        {
            InitializeComponent();

            this.Text = Application.ProductName + " v" + Application.ProductVersion;

            if (!bwLoad.IsBusy) { bwLoad.RunWorkerAsync(); }
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clbProcesses.Items.Clear();

            int processID = Process.GetCurrentProcess().SessionId;

            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Process");

            foreach (ManagementObject queryObj in searcher1.Get())
            {
                if (processID == Convert.ToInt32(queryObj["SessionId"].ToString()))
                {
                    clbProcesses.Items.Add(queryObj["Name"].ToString().Replace(".exe", ""));
                }
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!bwSave.IsBusy) { bwSave.RunWorkerAsync(); } else { MessageBox.Show("Подождите завершения предыдущей операции"); }
        }

        private void bwSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Данные успешно сохранены в файл " + filename + " папке с данной программой");
        }

        private void bwSave_DoWork(object sender, DoWorkEventArgs e)
        {

            string str = "";

            foreach (object item in clbProcesses.CheckedItems)
            {
                str += item.ToString() + Environment.NewLine;
            }

            //if (File.Exists(".log")) { File.Delete(".log"); }

            filename = DateTime.Now.ToString("yyyy-MM-dd h-m-s") + ".log";

            File.WriteAllText(filename, str);
        }
    }
}
