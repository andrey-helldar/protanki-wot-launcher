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
using _Hell_Language_Pack;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fNewVersion : Form
    {
        Debug Debug = new Debug();
        LanguagePack LanguagePack = new LanguagePack();

        string lang = "en";

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

        private void fNewVersion_Load(object sender, EventArgs e)
        {
            string pathINI = Directory.GetCurrentDirectory() + @"\config.ini";
            lang = new IniFile(pathINI).IniReadValue("new", "languages");
            lang = lang != "" ? lang : "en";

            foreach (Control control in this.Controls)
                SetLanguageControl(control);
        }

        private void SetLanguageControl(Control control)
        {
            try
            {
                foreach (Control c in control.Controls)
                {
                    SetLanguageControl(c);
                }

                var cb = control as CheckBox;

                if (cb != null)
                    cb.Text = LanguagePack.InterfaceLanguage("fNewVersion", cb, lang);
                else
                    control.Text = LanguagePack.InterfaceLanguage("fNewVersion", control, lang);
            }
            catch (Exception ex)
            {
                Debug.Save("fNewVersion", "UncheckAllCheckBoxes()", ex.Message);
            }
        }
    }
}
