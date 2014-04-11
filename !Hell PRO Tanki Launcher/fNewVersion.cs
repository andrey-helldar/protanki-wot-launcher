using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fNewVersion : Form
    {
        public fNewVersion()
        {
            InitializeComponent();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            if (cbNotification.Checked)
            {
                XDocument doc = XDocument.Load("settings.xml");

                if (doc.Root.Element("notification") != null) { doc.Root.Element("notification").SetValue(llVersion.Text); }
                else { XElement el = new XElement("notification", llVersion.Text); doc.Root.Add(el); }

                doc.Save("settings.xml");
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
