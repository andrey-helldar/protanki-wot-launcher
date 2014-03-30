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

            Process[] myProcesses = Process.GetProcesses();
            int processID = Process.GetCurrentProcess().SessionId;
            
            for (int i = 1; i < myProcesses.Length; i++)
            {
                try
                {
                    //if (myProcesses[i].SessionId == processID)
                    {

                        string tmp = myProcesses[i].ProcessName + "      ::      " + myProcesses[i].MainModule.FileVersionInfo.FileDescription.Trim();

                        if (clbProcesses.Items.IndexOf(tmp) < 0) clbProcesses.Items.Add(tmp);
                    }
                }
                catch (Exception) { }
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!bwSave.IsBusy) { bwSave.RunWorkerAsync(); } else { MessageBox.Show("Подождите завершения предыдущей операции"); }
        }

        private void bwSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (File.Exists("processes.log"))
                    Process.Start("processes.log");
                else
                    MessageBox.Show("Файл \"processes.log\" не существует!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bwSave_DoWork(object sender, DoWorkEventArgs e)
        {

            string str = "";

            //foreach (object item in clbProcesses.CheckedItems)
            foreach (object item in clbProcesses.Items)
            {
                str += item.ToString() + Environment.NewLine;
            }

            if (File.Exists("processes.log")) { File.Delete("processes.log"); }

            File.WriteAllText("processes.log", str);
        }
    }
}
