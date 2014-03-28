using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fNewVersion : Form
    {
        string version,
            type,
            kill,
            forceKill,
            aero,
            news,
            video;

        List<string> processes = new List<string>();

        public fNewVersion()
        {
            InitializeComponent();

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
                        kill = xmlNode.Attributes["kill"].InnerText == "False" ? "False" : "True";
                        forceKill = xmlNode.Attributes["force"].InnerText == "False" ? "False" : "True";
                        aero = xmlNode.Attributes["aero"].InnerText == "False" ? "False" : "True";
                        news = xmlNode.Attributes["news"].InnerText == "False" ? "False" : "True";
                        video = xmlNode.Attributes["video"].InnerText == "False" ? "False" : "True";
                    }
                }
                catch (Exception)
                {
                    kill = "False";
                    forceKill = "False";
                    aero = "False";
                    news = "True";
                    video = "True";
                }

                try
                {
                    processes.Clear();

                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("process"))
                    {
                        processes.Add(xmlNode.Attributes["name"].InnerText + "::" + xmlNode.Attributes["description"].InnerText);
                    }
                }
                catch (Exception) { }
            }
            else
            {
                kill = "False";
                forceKill = "False";
                aero = "False";
                news = "True";
                video = "True";
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            if (cbNotification.Checked)
            {
                XmlDocument doc = new XmlDocument();
                if (File.Exists("settings.xml")) { File.Delete("settings.xml"); }

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

                wr.WriteStartElement("notification", null);
                wr.WriteString(llVersion.Text);
                wr.WriteEndElement();

                wr.WriteStartElement("settings", null);
                wr.WriteAttributeString("kill", kill);
                wr.WriteAttributeString("force", forceKill);
                wr.WriteAttributeString("aero", aero);
                wr.WriteAttributeString("news", news);
                wr.WriteAttributeString("video", video);
                wr.WriteEndElement();

                if (processes.Count > 0)
                {
                    wr.WriteStartElement("processes", null);
                    foreach (string str in processes)
                    {
                        wr.WriteStartElement("process", null);
                        wr.WriteAttributeString("name", str.Remove(str.IndexOf("::")));
                        wr.WriteAttributeString("description", str.Remove(0, str.IndexOf("::")+2));
                        wr.WriteEndElement();
                    }
                    wr.WriteEndElement();
                }

                wr.WriteEndElement();

                wr.Flush();
                wr.Close();
            }

            Close();
        }

        private void bDownload_Click(object sender, EventArgs e)
        {
            Process.Start(llContent.Links[0].LinkData.ToString());
            Close();
        }
    }
}
