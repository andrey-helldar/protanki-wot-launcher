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
                string text = "";

                myJsonData.Add(new Debug().code);
                myJsonData.Add(name);
                myJsonData.Add("PROTanki");
                myJsonData.Add(Application.ProductName + " " + Application.ProductVersion);

                text += tbTicket.Text+"[br][br][hr]";

                string settings;
                if (File.Exists("settings.xml"))
                {
                    StreamReader sr = new StreamReader("settings.xml");
                    settings = sr.ReadToEnd();
                    sr.Close();
                    settings = settings.Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;").Replace("<", ":lt;").Replace(">", ":gt;");
                }
                else settings = "File settings.xml not found";
                text += "[b]settings.xml[/b][br]"+settings+"[br][br][hr]";

                 string tanks;
                 if (File.Exists(@"..\version.xml"))
                 {
                     StreamReader sr = new StreamReader(@"..\version.xml");
                     tanks = sr.ReadToEnd();
                     sr.Close();
                     tanks = settings.Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;").Replace("<", ":lt;").Replace(">", ":gt;");
                 }
                 else tanks = "File version.xml not found";
                  text += "[b]version.xml[/b][br]"+ tanks;

                  myJsonData.Add(text);

                 if (myJsonData.Count > 2)
                 {
                     try
                     {
                         string json = JsonConvert.SerializeObject(myJsonData);

                         string answer = POST("http://ai-rus.com/wot/ticket/", "data=" + json);
                         string status = "";

                         switch (answer)
                         {
                             case "OK": status = "Спасибо за обращение!" + Environment.NewLine + "Разработчик рассмотрит Вашу заявку в ближайшее время"; break;
                             case "Hacking attempt!": status = "Ведутся работы на сервере. Попробуйте отправить запрос чуть позже."; break;
                             default: status = "Ошибка отправки сообщения. Попробуйте еще раз."; break;
                         }

                         MessageBox.Show(this, answer+Environment.NewLine+ status, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private static string POST(string Url, string Data)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            req.Method = "POST";
            req.Timeout = 100000;
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] sentData = Encoding.GetEncoding(1251).GetBytes(Data);
            req.ContentLength = sentData.Length;
            System.IO.Stream sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            System.Net.WebResponse res = req.GetResponse();
            System.IO.Stream ReceiveStream = res.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8);
            //Кодировка указывается в зависимости от кодировки ответа сервера
            Char[] read = new Char[256];
            int count = sr.Read(read, 0, 256);
            string Out = String.Empty;
            while (count > 0)
            {
                String str = new String(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
            }
            return Out;
        }

        static string getResponse(string uri, string json)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(json);

                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

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
