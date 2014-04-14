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

            this.Text = Application.ProductName + " v" + Application.ProductVersion;
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
                myJsonData.Add("PROTanki");
                myJsonData.Add(Application.ProductVersion);

                string settings;
                if (File.Exists("settings.xml"))
                {
                    StreamReader sr = new StreamReader("settings.xml");
                    settings = sr.ReadToEnd();
                    sr.Close();
                    settings = settings.Replace("\"", ":-:").Replace("'", ":-;");
                }
                else settings = "File settings.xml not found";
                myJsonData.Add(settings);

                string tanks;
                if (File.Exists(@"..\version.xml"))
                {
                    StreamReader sr = new StreamReader(@"..\version.xml");
                    tanks = sr.ReadToEnd();
                    sr.Close();
                    tanks = settings.Replace("\"", ":-:").Replace("'", ":-;");
                }
                else tanks = "File version.xml not found";
                myJsonData.Add(tanks);

                if (myJsonData.Count > 2)
                {
                    try
                    {
                        string status = "";
                        string json = JsonConvert.SerializeObject(myJsonData);

                        switch (getResponse("http://ai-rus.com/wot/ticket/" + json))
                        {
                            case "OK": status="Спасибо за обращение!" + Environment.NewLine + "Разработчик рассмотрит Вашу заявку в ближайшее время"; break;
                            case "Hacking attempt!": status = "Ведутся работы на сервере. Попробуйте отправить запрос чуть позже."; break;
                            default: status="Ошибка отправки сообщения. Попробуйте еще раз."; break;
                        }

                        MessageBox.Show(this, status, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (WebException ex) { MessageBox.Show(this, "Ошибка отправки сообщения. Попробуйте еще раз." + Environment.NewLine + Environment.NewLine + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
            catch (Exception) { }
        }

        private static string getResponse(string uri)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                int count = 0;
                do
                {
                    count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        sb.Append(Encoding.Default.GetString(buf, 0, count));
                    }
                }
                while (count > 0);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(fIndex.ActiveForm, ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            if (tbTicket.Text.Length >= 50)
            {
                bSend.Text = "Отправка данных...";
                SendTicket().Wait();
                bSend.Text = "Отправить";
            }
            else
            {
                MessageBox.Show(this, "Текст не может быть меньше 50 символов!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
