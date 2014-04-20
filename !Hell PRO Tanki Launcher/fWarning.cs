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
using System.Diagnostics;
using Newtonsoft.Json;
using _Hell_Language_Pack;

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
                Debug Debug = new Debug();

                List<string> myJsonData = new List<string>();
                myJsonData.Clear();

                myJsonData.Add(Debug.code);
                myJsonData.Add(Debug.UserID());
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
                    StreamReader sr1 = new StreamReader(@"..\version.xml");
                    tanks = sr1.ReadToEnd();
                    sr1.Close();
                    tanks = tanks.Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;").Replace("<", ":lt;").Replace(">", ":gt;");
                }
                else tanks = "File version.xml not found";
                text += "[b]version.xml[/b][br]" + tanks;

                myJsonData.Add(text);
                myJsonData.Add(rbBug.Checked ? "bug" : "wish");
                myJsonData.Add(tbEmail.Text.Trim() != "" ? tbEmail.Text.Trim() : "0");

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

                        MessageBox.Show(this, status, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                bSend.Enabled = false;

                SendTicket().Wait();

                bSend.Text = "Отправить";
                bSend.Enabled = true;

                Close();
            }
            else
            {
                MessageBox.Show(this, mess, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lMessAboutNewVersion_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://vk.com/topic-58816477_29818765");
        }

        private async Task SetInterfaceLanguage()
        {
            XDoc

            LanguagePack LanguagePack = new LanguagePack();

            foreach (Control control in this.Controls)
                control.Text = new LanguagePack().InterfaceLanguage("fWarning", control, lang);
        }
    }
}
