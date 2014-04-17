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
        string sendText = String.Empty,
            sendStatus = String.Empty;

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

                List<string> myJsonData = new List<string>();
                myJsonData.Clear();

                myJsonData.Add(new Debug().code);
                myJsonData.Add(name);
                myJsonData.Add("PROTanki");
                myJsonData.Add(Application.ProductName + " " + Application.ProductVersion);

                string text = tbTicket.Text.Trim() + "[br][br][hr]";

                sendText = tbTicket.Text.Trim();

                string settings;
                if (File.Exists("settings.xml"))
                {
                    StreamReader sr = new StreamReader("settings.xml");
                    settings = sr.ReadToEnd();
                    sr.Close();
                    settings = settings.Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;").Replace("<", ":lt;").Replace(">", ":gt;");
                }
                else settings = "File settings.xml not found";
                text += "[b]settings.xml[/b][br]" + settings + "[br][br][hr]";

                string tanks;
                if (File.Exists(@"..\version.xml"))
                {
                    StreamReader sr = new StreamReader(@"..\version.xml");
                    tanks = sr.ReadToEnd();
                    sr.Close();
                    tanks = tanks.Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;").Replace("<", ":lt;").Replace(">", ":gt;");
                }
                else tanks = "File version.xml not found";
                text += "[b]version.xml[/b][br]" + tanks;

                myJsonData.Add(text);
                myJsonData.Add(rbBug.Checked ? "bug" : "wish");

                if (myJsonData.Count > 2)
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(myJsonData);

                        string status = "";

                        SendPOST SendPOST = new SendPOST();
                        sendStatus = SendPOST.Send("http://ai-rus.com/wot/ticket/", "data=" + json);
                        switch (sendStatus)
                        {
                            case "OK": status = "Спасибо за обращение!" + Environment.NewLine + "Разработчик рассмотрит Вашу заявку в ближайшее время"; break;
                            case "Hacking attempt!": status = "Ведутся работы на сервере. Попробуйте отправить запрос чуть позже."; break;
                            default: status = "Ошибка отправки сообщения. Попробуйте еще раз."; break;
                        }

                        MessageBox.Show(this, /*answer+Environment.NewLine+Environment.NewLine+ */status, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (WebException ex) { MessageBox.Show(this, ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                else
                {
                    MessageBox.Show(this, "Ошибка отправки данных. Попробуйте чуть познее...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) { }
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            int symbolsCount = 50;
            string mess = String.Empty;

            if (tbTicket.Text.Length < symbolsCount) { mess += "Текст не может быть меньше " + symbolsCount.ToString() + " символов!" + Environment.NewLine + Environment.NewLine; }

            if (sendStatus == "OK" && sendText == tbTicket.Text.Trim()) { mess += "Вы уже отправляли данное сообщение."; }

            if (mess == String.Empty)
            {
                bSend.Text = "Отправка...";
                SendTicket().Wait();
                bSend.Text = "Отправить";
            }
            else
            {
                MessageBox.Show(this, mess, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
