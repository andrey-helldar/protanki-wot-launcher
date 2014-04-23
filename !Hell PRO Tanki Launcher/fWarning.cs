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
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fWarning : Form
    {
        Language Language = new Language();

        string sendText = String.Empty,
            sendStatus = String.Empty,

            lang = "en";

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

                // Если имеются какие-либо файлы дебага, то отправляем их
                // и если юзер разрешил нам отправку
                if (cbAttachDebug.Checked) Debug.Send();
                
                // Готовим отправку сообщения
                List<string> myJsonData = new List<string>();
                myJsonData.Clear();

                myJsonData.Add(Debug.Code);
                myJsonData.Add(Debug.UserID());
                myJsonData.Add(Debug.Youtube);
                myJsonData.Add(Application.ProductName + " " + Application.ProductVersion);
                
                string text = tbTicket.Text.Trim() + "[br][br][hr]";

                sendText = tbTicket.Text.Trim();

                string settings;
                if (File.Exists("settings.xml"))
                {
                    StreamReader sr = new StreamReader("settings.xml");
                    settings = sr.ReadToEnd();
                    sr.Close();
                    settings = ReplaceSymbols(settings);
                }
                else settings = "File settings.xml not found";
                text += "[b]settings.xml[/b][br]" + settings + "[br][br][hr]";

                string tanks;
                if (File.Exists(@"..\version.xml"))
                {
                    StreamReader sr1 = new StreamReader(@"..\version.xml");
                    tanks = sr1.ReadToEnd();
                    sr1.Close();
                    tanks = ReplaceSymbols(tanks);
                }
                else tanks = "File version.xml not found";
                text += "[b]version.xml[/b][br]" + tanks;

                myJsonData.Add(text);
                myJsonData.Add(rbBug.Checked ? "bug" : "wish");
                myJsonData.Add(tbEmail.Text.Trim() != "" ? tbEmail.Text.Trim() : "0");

                if (File.Exists("config.ini"))
                {
                    try
                    {
                        string pathINI = Directory.GetCurrentDirectory() + @"\config.ini";
                        myJsonData.Add("Multipack version: " + new IniFile(pathINI).IniReadValue("new", "version"));
                    }
                    catch (Exception) { text = String.Empty; }
                }

                if (myJsonData.Count > 2)
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(myJsonData);

                        string status = "";

                        SendPOST SendPOST = new SendPOST();
                        sendStatus = SendPOST.Send("http://ai-rus.com/wot/ticket/", "data=" + json);
                        string[] statusTSN = sendStatus.Split(':');
                        switch (statusTSN[0])
                        {
                            /*case "OK": status = "Спасибо за обращение!" + Environment.NewLine + "Разработчик рассмотрит Вашу заявку в ближайшее время"; break;
                            case "Hacking attempt!": status = "Ведутся работы на сервере. Попробуйте отправить запрос чуть позже."; break;
                            default: status = "Ошибка отправки сообщения. Попробуйте еще раз."; break;*/
                            case "OK": status = Language.DynamicLanguage("thanks", lang, statusTSN[1]); break;
                            case "Hacking attempt!": status = Language.DynamicLanguage("hacking", lang); break;
                            case "BANNED": status = Language.DynamicLanguage("banned", lang); break;
                            default: status = Language.DynamicLanguage("error", lang); break;
                        }

                        MessageBox.Show(this, status, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (WebException ex) { MessageBox.Show(this, ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                else
                {
                    //MessageBox.Show(this, "Ошибка отправки данных. Попробуйте чуть познее...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(this, Language.DynamicLanguage("error", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) { }
        }

        private string ReplaceSymbols(string s)
        {
            return s.Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;").Replace(Environment.NewLine, ";-;").Replace("<", ":lt;").Replace(">", ":gt;");
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            int minSymbolsCount = 50,
                maxWordLength = 20;
            string mess = String.Empty;
            

            //if (tbTicket.Text.Length < symbolsCount) { mess += "Текст не может быть меньше " + symbolsCount.ToString() + " символов!" + Environment.NewLine + Environment.NewLine; }
            if (tbTicket.Text.Trim().Length < minSymbolsCount) { mess += Language.DynamicLanguage("symbolLength", lang, minSymbolsCount.ToString()); }

            //if (sendStatus == "OK" && sendText == tbTicket.Text.Trim()) { mess += "Вы уже отправляли данное сообщение."; }
            if (sendStatus == "OK" && sendText == tbTicket.Text.Trim()) { mess += Environment.NewLine + Environment.NewLine + Language.DynamicLanguage("messAreSended", lang); }

            if (!ChechLengthWord(tbTicket.Text.Trim(), maxWordLength)) mess += Environment.NewLine + Environment.NewLine + Language.DynamicLanguage("veryLongWord", lang, maxWordLength.ToString()); 

            if (mess == String.Empty)
            {
                //bSend.Text = "Отправка...";
                bSend.Text = Language.DynamicLanguage("sending", lang);
                bSend.Enabled = false;

                SendTicket().Wait();

                //bSend.Text = "Отправить";
                bSend.Text = Language.DynamicLanguage("send", lang);
                bSend.Enabled = true;

                Close();
            }
            else
            {
                MessageBox.Show(this, mess, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ChechLengthWord(string s, int wordLength = 255)
        {
            try
            {
                string[] arr = s.Split(' ');

                foreach (string str in arr)
                    if (str.Length > wordLength) return false;

                return true;
            }
            catch (Exception) { return true; }
        }

        private void lMessAboutNewVersion_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://vk.com/topic-58816477_29818765");
        }

        private async Task SetInterfaceLanguage()
        {
            foreach (Control control in this.Controls)
                control.Text = new Language().InterfaceLanguage("fWarning", control, lang);
        }

        private void fWarning_Load(object sender, EventArgs e)
        {
            string pathINI = Directory.GetCurrentDirectory() + @"\config.ini";
            lang = new IniFile(pathINI).IniReadValue("new", "languages");
            lang = lang != "" ? lang : "en";

            SetInterfaceLanguage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Debug Debug = new Debug();
                Debug.Send();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
