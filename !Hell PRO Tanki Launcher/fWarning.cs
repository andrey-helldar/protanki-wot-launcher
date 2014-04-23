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
using System.Collections.Specialized;
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

            this.Text = Application.ProductName;
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
                NameValueCollection nvc = new NameValueCollection();

                // Если имеются какие-либо файлы дебага, то отправляем их
                // и если юзер разрешил нам отправку
                if (cbAttachDebug.Checked) Debug.Send();
                
                // Готовим отправку сообщения
                nvc.Add("code", Debug.Code);
                nvc.Add("userid", Debug.UserID());
                nvc.Add("youtube", Debug.Youtube);
                nvc.Add("product", Application.ProductName + " " + Application.ProductVersion);

                nvc.Add("text", tbTicket.Text.Trim());
                sendText = tbTicket.Text.Trim();

                //myJsonData.Add(rbBug.Checked ? "bug" : "wish");
                nvc.Add("alert", Category());
                nvc.Add("email", tbEmail.Text.Trim() != "" ? tbEmail.Text.Trim() : "0");

                /*************************
                 * Отправляем файлы
                 * **********************/
                string settings;
                if (File.Exists("settings.xml"))
                {
                    StreamReader sr = new StreamReader("settings.xml");
                    settings = sr.ReadToEnd();
                    sr.Close();
                    settings = ReplaceSymbols(settings);
                }
                else settings = "File settings.xml not found";
                nvc.Add("settings.xml", settings);

                // Танки
                string tanks;
                if (File.Exists(@"..\version.xml"))
                {
                    StreamReader sr1 = new StreamReader(@"..\version.xml");
                    tanks = sr1.ReadToEnd();
                    sr1.Close();
                    tanks = ReplaceSymbols(tanks);
                }
                else tanks = "File version.xml not found";
                nvc.Add("version.xml", tanks);

                // XVM
                string xvm;
                if (File.Exists(@"..\xvm.log"))
                {
                    if (new FileInfo(@"..\xvm.log").Length > 0)
                    {
                        StreamReader sr1 = new StreamReader(@"..\xvm.log");
                        xvm = sr1.ReadToEnd();
                        sr1.Close();
                        xvm = ReplaceSymbols(xvm);
                    }
                    else xvm = "File xvm.log not found";
                }
                else xvm = "File xvm.log not found";
                nvc.Add("xvm.log", xvm);

                // PYTHON
                string python;
                if (File.Exists(@"..\python.log"))
                {
                    if (new FileInfo(@"..\python.log").Length > 0)
                    {
                        StreamReader sr1 = new StreamReader(@"..\python.log");
                        python = sr1.ReadToEnd();
                        sr1.Close();
                        python = ReplaceSymbols(xvm);
                    }
                    else python = "File python.log not found";
                }
                else python = "File python.log not found";
                nvc.Add("python.log", python);

                // INSTALL LOG
                string loginstall;
                if (File.Exists(@"..\loginstall.inf"))
                {
                    if (new FileInfo(@"..\loginstall.inf").Length > 0)
                    {
                        StreamReader sr1 = new StreamReader(@"..\loginstall.inf");
                        loginstall = sr1.ReadToEnd();
                        sr1.Close();
                        loginstall = ReplaceSymbols(xvm);
                    }
                    else loginstall = "File loginstall.inf not found";
                }
                else loginstall = "File loginstall.inf not found";
                nvc.Add("loginstall.inf", loginstall);

                /*************************
                 * Цепляем конфиг
                 * **********************/
                if (File.Exists("config.ini"))
                {
                    try
                    {
                        string pathINI = Directory.GetCurrentDirectory() + @"\config.ini";
                        nvc.Add("modpackVersion", new IniFile(pathINI).IniReadValue("new", "version"));
                        nvc.Add("modpackType", (new IniFile(pathINI).IniReadValue("new", "update_file")).Replace("update", "").Replace(".xml", ""));
                        nvc.Add("modpackLang", new IniFile(pathINI).IniReadValue("new", "languages"));
                    }
                    finally { }
                }

                if (nvc.Count > 2)
                {
                    try
                    {
                        string status = String.Empty;

                        SendPOST SendPOST = new SendPOST();
                        sendStatus = SendPOST.Send("http://ai-rus.com/wot/ticket/", "data=" + SendPOST.Json(nvc));
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
            catch (Exception ex) { new Debug().Save("ticket", ex.Message); }
        }

        private string ReplaceSymbols(string s)
        {
            return s.Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;").Replace(Environment.NewLine, ";-;").Replace("<", ":lt;").Replace(">", ":gt;");
        }

        private string Category()
        {
            switch (cbCaption.SelectedIndex)
            {
                    /***********************
                     * 0 Пожелания к мультипаку
                     * 1 Пожелания к лаунчеру
                     * 2 Найдена ошибка в мультипаке
                     * 3 Найдена ошибка в лаунчере
                     * ********************/
                case 0: return "wish:Multipack";
                case 1: return "wish:Launcher";
                case 2: return "bug:Multipack";
                default: return "bug:Launcher";
            }
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            int minSymbolsCount = 50,
                maxWordLength = 20;

            string mess = String.Empty;
            

            if (tbTicket.Text.Trim().Length < minSymbolsCount) { mess += Language.DynamicLanguage("symbolLength", lang, minSymbolsCount.ToString()); }

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
    }
}
