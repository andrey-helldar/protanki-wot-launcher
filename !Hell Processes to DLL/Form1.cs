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

        List<string> ToDB = new List<string>();
        string progress = String.Empty;


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

            lStatus.Text = lvProcesses.Items.Count.ToString();
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
            bStart.Enabled = false;

            List<string> ToFile = new List<string>();

            for (int i = 0; i < lvProcesses.Items.Count; i++)
            {
                ToDB.Add(String.Format("UPDATE `dle_wot_processes` SET `view`='1', `check`='{0}' WHERE `id`='{1}';", lvProcesses.Items[i].Checked ? "1" : "0", lvProcesses.Items[i].Text));

                if (lvProcesses.Items[i].Checked)
                    ToFile.Add(String.Format("\"{0}\",", lvProcesses.Items[i].SubItems[1].Text));
            }

            File.WriteAllLines("db.sql", ToDB);
            File.WriteAllLines("file.txt", ToFile);

            try
            {
                timer1.Enabled = true;
                Task.Factory.StartNew(() => UpdateBD());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task UpdateBD()
        {
            try
            {
                con.Open();

                decimal max = (decimal)ToDB.Count;

                for (int i = 0; i < ToDB.Count; i++)
                {
                    progress = Math.Round(((decimal)i / (decimal)max * 100), 2).ToString() + "% (" + i.ToString() + "/" + max.ToString() + ")";
                    new MySqlCommand(ToDB[i], con).ExecuteNonQuery();
                }
                con.Close();

                MessageBox.Show("Database updated sucessfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                timer1.Enabled = false;
                bStart.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lStatus.Text = progress;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1.Text = timer1.Enabled ? "true" : "false";
        }
    }
}
