﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fSettings : Form
    {
        string //title,
            version,
            path;

        public fSettings()
        {
            InitializeComponent();

            moveForm();

            if (File.Exists("settings.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("settings.xml");

                version = doc.GetElementsByTagName("version")[0].InnerText;
                path = doc.GetElementsByTagName("path")[0].InnerText;

                try
                {
                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("settings"))
                    {
                        cbAutoOptimize.Checked = xmlNode.Attributes["optimize"].InnerText == "False" ? false : true;
                        cbNews.Checked = xmlNode.Attributes["news"].InnerText == "False" ? false : true;
                        cbVideo.Checked = xmlNode.Attributes["video"].InnerText == "False" ? false : true;
                    }
                }
                catch (Exception)
                {
                    cbAutoOptimize.Checked = true;
                    cbNews.Checked = true;
                    cbVideo.Checked = true;
                }
            }
            else
            {
                //title = Application.ProductName;
                version = Application.ProductVersion;
                path = "";

                cbAutoOptimize.Checked = true;
                cbNews.Checked = true;
                cbVideo.Checked = true;
            }

            tbPath.Text = path;
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            tbPath.Text = fbd.SelectedPath + @"\";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            if (File.Exists("settings.xml")) { File.Delete(Application.StartupPath + "/settings.xml"); }

            XmlTextWriter wr = new XmlTextWriter("settings.xml", Encoding.UTF8);
            wr.Formatting = Formatting.Indented;

            wr.WriteStartDocument();
            wr.WriteStartElement("pro");

           /* wr.WriteStartElement("title", null);
            wr.WriteString(title);
            wr.WriteEndElement();*/

            wr.WriteStartElement("version", null);
            wr.WriteString(version);
            wr.WriteEndElement();

            wr.WriteStartElement("path", null);
            wr.WriteString(tbPath.Text);
            wr.WriteEndElement();

            wr.WriteStartElement("settings", null);
            wr.WriteAttributeString("optimize", cbAutoOptimize.Checked.ToString());
            wr.WriteAttributeString("news", cbNews.Checked.ToString());
            wr.WriteAttributeString("video", cbVideo.Checked.ToString());
            wr.WriteEndElement();

            wr.WriteEndElement();

            wr.Flush();
            wr.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fSettings_Move(object sender, EventArgs e)
        {
        }

        private void moveForm()
        {
            this.MouseDown += delegate
            {
                this.Capture = false;
                var msg = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            };

            groupBox1.MouseDown += delegate
            {
                groupBox1.Capture = false;
                var msg = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            };
        }
    }
}
