using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace _Hell_Processes_to_DLL
{
    public partial class fIndex : Form
    {
        MySqlConnection con = new MySqlConnection(getConString());
        MySqlCommand cmd;
        MySqlDataReader dr;

        public fIndex()
        {
            InitializeComponent();
        }

        private void fIndex_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + " v" + Application.ProductVersion;

            con.Open();
            cmd = new MySqlCommand("SELECT * FROM `dle_wot_processes` ORDER BY `name`", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int i = lvProcesses.Items.Add(dr["id"].ToString()).Index;
                    lvProcesses.Items[i].SubItems.Add(dr["name"].ToString());
                    lvProcesses.Items[i].SubItems.Add(dr["desc"].ToString());

                    if (dr["view"].ToString() == "True")
                    {
                        if (dr["check"].ToString() == "True")
                        {
                            lvProcesses.Items[i].Checked = true;
                            lvProcesses.Items[i].Group = lvProcesses.Groups["lvgChecked"];
                            lvProcesses.Items[i].BackColor = Color.LightGreen;
                        }
                        else
                            lvProcesses.Items[i].Group = lvProcesses.Groups["lvgUnchecked"];
                    }
                    else
                    {
                        lvProcesses.Items[i].Group = lvProcesses.Groups["lvgNew"];
                        lvProcesses.Items[i].BackColor = Color.Azure;
                    }
            }
            con.Close();
        }

        static string getConString()
        {
            XDocument doc = XDocument.Load("settings.xml");

            return "Network Address=" + doc.Root.Element("connect").Attribute("address").Value + "; Initial Catalog='" +
                doc.Root.Element("connect").Attribute("database").Value + "'; Persist Security Info=no; User Name='" +
                doc.Root.Element("connect").Attribute("login").Value + "'; Password='" +
                doc.Root.Element("connect").Attribute("password").Value + "'";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> ToFile = new List<string>();

            string toDB = String.Empty;

            for (int i = 0; i < lvProcesses.Items.Count; i++)
            {
                toDB += String.Format("UDPATE `dle_wot_processes` SET `view`='1', `check`='{0}' WHERE `id`='{1}';", lvProcesses.Items[i].Checked?"1":"0", lvProcesses.Items[i].Text);
                ToFile.Add(String.Format("\"{0}\",", lvProcesses.Items[i].SubItems[1].Text));
            }

            File.WriteAllLines("file.txt", ToFile);

            con.Open();
            cmd = new MySqlCommand(toDB, con);
            dr = cmd.ExecuteReader();
            con.Close();
        }
    }
}
