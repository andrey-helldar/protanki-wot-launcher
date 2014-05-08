using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormatXML
{
    public partial class fFormatXML : Form
    {
        public fFormatXML()
        {
            InitializeComponent();

            this.Text = Application.ProductName + " v" + Application.ProductVersion;
            this.Icon = Properties.Resources.Icon;
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            XDocument doc = new XDocument(
                new XElement("pro",
                    new XElement("version", tbVersion.Text.Trim()),

                    new XElement("extended",
                        new XElement("message", tbFullDesc.Text.Replace(Environment.NewLine, ":;").Trim()),
                        new XElement("download", tbFullLink.Text.Trim())
                        ),

                    new XElement("full",
                        new XElement("message", tbFullDesc.Text.Replace(Environment.NewLine, ":;").Trim()),
                        new XElement("download", tbFullLink.Text.Trim())
                        ),

                    new XElement("base",
                        new XElement("message", tbBaseDesc.Text.Replace(Environment.NewLine, ":;").Trim()),
                        new XElement("download", tbBaseLink.Text.Trim())
                        )
                    )
            );

            folderBrowserDialog.SelectedPath = Application.StartupPath;

            folderBrowserDialog.ShowDialog();
            doc.Save(folderBrowserDialog.SelectedPath + @"\pro.xml");
        }
    }
}
