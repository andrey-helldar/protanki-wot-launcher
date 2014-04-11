﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace _Hell_MD5files
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Text = Application.ProductName;
            this.Icon = Properties.Resources.Icon;

            GetFiles().Wait();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async Task GetFiles()
        {
            if (File.Exists(@"files\Multipack Launcher.exe")) { File.Move(@"files\Multipack Launcher.exe", @"files\launcher.exe"); }

            lvMD5.Items.Clear();

            if (!Directory.Exists("files")) { Directory.CreateDirectory("files"); }

            var info = new DirectoryInfo("files");

            foreach (FileInfo file in info.GetFiles())
            {
                int i = lvMD5.Items.Add(file.Name).Index;
                lvMD5.Items[i].SubItems.Add(FileVersionInfo.GetVersionInfo(file.FullName).FileVersion);
                lvMD5.Items[i].SubItems.Add(file.Length.ToString());
                lvMD5.Items[i].SubItems.Add(file.Extension.Replace(".",""));
                lvMD5.Items[i].SubItems.Add(Checksum(file.FullName));
            }
        }

        private string Checksum(string filename)
        {
            if (File.Exists(filename))
                using (FileStream fs = File.OpenRead(filename))
                {
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] fileData = new byte[fs.Length];
                    fs.Read(fileData, 0, (int)fs.Length);
                    byte[] exp = md5.ComputeHash(fileData);
                    return BitConverter.ToString(exp).Replace("-", "");
                }
            else
                return null;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            XDocument doc = new XDocument(new XElement("ai.rus", null));

            foreach(ListViewItem file in lvMD5.Items){
                doc.Root.Add(new XElement(file.Text.Remove(file.Text.Length - 4),
                    file.SubItems[1].Text,
                    new XAttribute("ext", file.SubItems[3].Text),
                    new XAttribute("checksum", file.SubItems[4].Text),
                    new XAttribute("important", file.Checked.ToString())
                ));
            }

            doc.Save("version.xml");
        }
    }
}