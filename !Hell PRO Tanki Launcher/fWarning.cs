using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fWarning : Form
    {
        public fWarning()
        {
            InitializeComponent();

            this.Text = Application.ProductName + " v"+Application.ProductVersion;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async Task SendTicket()
        {
            try
            {
                string code = new Debug().code;

                List<string> myJsonData = new List<string>();
                myJsonData.Clear();

                string name = Environment.MachineName +
                        Environment.UserName +
                        Environment.UserDomainName +
                        Environment.Version.ToString() +
                        Environment.OSVersion.ToString();

                using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
                {
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(name));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++) { sBuilder.Append(data[i].ToString("x2")); }
                    name = sBuilder.ToString();
                }

                myJsonData.Add(code);
                myJsonData.Add(name);
                myJsonData.Add(Application.ProductVersion);
                myJsonData.Add();

                if (myJsonData.Count > 2)
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(myJsonData);

                        WebRequest req = WebRequest.Create("http://ai-rus.com/wot/ticket/" + json);
                        WebResponse resp = req.GetResponse();
                    }
                    catch (WebException) { }
                }
            }
            catch (Exception) { }
        }
    }
}
