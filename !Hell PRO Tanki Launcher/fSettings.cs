using System;
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
            type = "full";

        public fSettings()
        {
            InitializeComponent();

            moveForm();

            if (File.Exists("settings.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("settings.xml");

                version = doc.GetElementsByTagName("version")[0].InnerText;
                type = doc.GetElementsByTagName("type")[0].InnerText;

                try
                {
                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("settings"))
                    {
                        cbKillProcesses.Checked = xmlNode.Attributes["kill"].InnerText == "False" ? false : true;
                        cbForceClose.Checked = xmlNode.Attributes["force"].InnerText == "False" ? false : true;
                        cbAero.Checked = xmlNode.Attributes["aero"].InnerText == "False" ? false : true;
                        cbNews.Checked = xmlNode.Attributes["news"].InnerText == "False" ? false : true;
                        cbVideo.Checked = xmlNode.Attributes["video"].InnerText == "False" ? false : true;
                    }
                }
                catch (Exception)
                {
                    cbKillProcesses.Checked = false;
                    cbAero.Checked = false;
                    cbNews.Checked = true;
                    cbVideo.Checked = true;
                }
            }
            else
            {
                //title = Application.ProductName;
                version = Application.ProductVersion;

                cbKillProcesses.Checked = false;
                cbAero.Checked = false;
                cbNews.Checked = true;
                cbVideo.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            if (File.Exists("settings.xml")) { File.Delete(Application.StartupPath + "/settings.xml"); }

            XmlTextWriter wr = new XmlTextWriter("settings.xml", Encoding.UTF8);
            wr.Formatting = Formatting.Indented;

            wr.WriteStartDocument();
            wr.WriteStartElement("pro");

            wr.WriteStartElement("version", null);
            wr.WriteString(version);
            wr.WriteEndElement();

            wr.WriteStartElement("type", null);
            wr.WriteString(type);
            wr.WriteEndElement();

            wr.WriteStartElement("settings", null);
            wr.WriteAttributeString("kill", cbKillProcesses.Checked.ToString());
            wr.WriteAttributeString("force", cbForceClose.Checked.ToString());
            wr.WriteAttributeString("aero", cbAero.Checked.ToString());
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

        private void moveForm()
        {
            this.MouseDown += delegate
            {
                this.Capture = false;
                var msg = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            };
        }
    }
}
