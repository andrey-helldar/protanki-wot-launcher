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
                string aw = "";

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
                    settings = settings.Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;");
                }
                else settings = "File settings.xml not found";
                myJsonData.Add(settings);

                string tanks;
                if (File.Exists(@"..\version.xml"))
                {
                    StreamReader sr = new StreamReader(@"..\version.xml");
                    tanks = sr.ReadToEnd();
                    sr.Close();
                    tanks = settings.Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;");
                }
                else tanks = "File version.xml not found";
                myJsonData.Add(tanks);

                if (myJsonData.Count > 2)
                {
                    try
                    {
                        string status = "";
                        string json = JsonConvert.SerializeObject(myJsonData);

                        aw = getResponse("http://ai-rus.com/wot/ticket/",json);
                        switch (aw)
                        {
                            case "OK": status="Спасибо за обращение!" + Environment.NewLine + "Разработчик рассмотрит Вашу заявку в ближайшее время"; break;
                            case "Hacking attempt!": status = "Ведутся работы на сервере. Попробуйте отправить запрос чуть позже."; break;
                            default: status="Ошибка отправки сообщения. Попробуйте еще раз."; break;
                        }

                        MessageBox.Show(this, status, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show(this, aw, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (WebException ex) { MessageBox.Show(this, "Ошибка отправки сообщения. Попробуйте еще раз." + Environment.NewLine + Environment.NewLine + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
            catch (Exception) { }
        }

        private static string getResponse(string uri, string data=null)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(data);
                var request = (HttpWebRequest)WebRequest.Create(uri);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    response.Close();
                    return "check site";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(fIndex.ActiveForm, ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            if (tbTicket.Text.Length >= 0)
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
