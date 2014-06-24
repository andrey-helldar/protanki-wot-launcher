﻿using System;
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

            lang = "en",
            
            patoToSettings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";

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
                //NameValueCollection nvc = new NameValueCollection();
                Dictionary<string, string> nvc = new Dictionary<string, string>();

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
                 * Цепляем конфиг
                 * **********************/
                if (File.Exists("config.ini"))
                {
                    try
                    {
                        string pathINI = Directory.GetCurrentDirectory() + @"\config.ini";
                        nvc.Add("modpackVersion", Properties.Resources.ModPackVersion + "." + new IniFile(pathINI).IniReadValue("protanki", "version"));
                        nvc.Add("modpackType", (new IniFile(pathINI).IniReadValue("protanki", "type")));
                        nvc.Add("modpackLang", new IniFile(pathINI).IniReadValue("protanki", "languages"));
                    }
                    finally { }
                }

                /*************************
                 * Отправляем файлы
                 * **********************/
                if (!File.Exists(patoToSettings)) new UpdateLauncher().SaveFromResources();

                string settings;
                if (File.Exists(patoToSettings))
                {
                    StreamReader sr = new StreamReader(patoToSettings);
                    settings = sr.ReadToEnd();
                    sr.Close();
                    settings = ReplaceSymbols(settings, true);
                }
                else settings = "File settings.xml not found";
                nvc.Add(patoToSettings, settings);

                // Танки
                string tanks;
                if (File.Exists(@"..\version.xml"))
                {
                    StreamReader sr1 = new StreamReader(@"..\version.xml");
                    tanks = sr1.ReadToEnd();
                    sr1.Close();
                    tanks = ReplaceSymbols(tanks, true);
                }
                else tanks = "File version.xml not found";
                nvc.Add("version.xml", tanks);

                // CONFIG MULTIPACK
                string config;
                if (File.Exists("config.ini"))
                {
                    StreamReader sr1 = new StreamReader("config.ini");
                    config = sr1.ReadToEnd();
                    sr1.Close();
                    config = ReplaceSymbols(config, true);
                }
                else config = "File config.ini not found";
                nvc.Add("config.ini", config);

                // XVM
                string xvm;
                if (File.Exists(@"..\xvm.log"))
                {
                    if (new FileInfo(@"..\xvm.log").Length > 0)
                    {
                        StreamReader sr1 = new StreamReader(@"..\xvm.log");
                        xvm = sr1.ReadToEnd();
                        sr1.Close();
                        xvm = ReplaceSymbols(xvm, true);
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
                        python = ReplaceSymbols(python, true);
                    }
                    else python = "File python.log not found";
                }
                else python = "File python.log not found";
                nvc.Add("python.log", python);

                // INSTALL LOG
                string loginstall;
                if (File.Exists("loginstall.inf"))
                {
                    if (new FileInfo("loginstall.inf").Length > 0)
                    {
                        StreamReader sr1 = new StreamReader("loginstall.inf");
                        loginstall = sr1.ReadToEnd();
                        sr1.Close();
                        loginstall = ReplaceSymbols(loginstall, true);
                    }
                    else loginstall = "File loginstall.inf not found";
                }
                else loginstall = "File loginstall.inf not found";
                nvc.Add("loginstall.inf", loginstall);

                if (nvc.Count > 2)
                {
                    try
                    {
                        string status = String.Empty;

                        SendPOST SendPOST = new SendPOST();
                        Dictionary<string, string> sendStatus = SendPOST.FromJson(SendPOST.Send(Properties.Resources.PostTicket, "data=" + SendPOST.Json(nvc)));
                        switch (sendStatus["status"])
                        {
                            case "OK": status = Language.DynamicLanguage("thanks", lang, sendStatus["id"]); break;
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
                    MessageBox.Show(this, Language.DynamicLanguage("error", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) { new Debug().Save("ticket", ex.Message); }
        }

        private string ReplaceSymbols(string s, bool files = false)
        {
            if (!files)
                return s.Replace("\\", "::-;;").Replace("\"", ":-:").Replace("'", ":-;").Replace("\r\n", ";-;").Replace(Environment.NewLine, ";-;").Replace("<", ":lt;").Replace(">", ":gt;");
            else
                return s.Replace("\\", "::-;;").Replace("\"", ":-:").Replace("'", ":-;").Replace("<", ":lt;").Replace(">", ":gt;");
        }

        private string Category()
        {
            switch (cbCaption.SelectedIndex)
            {
                /***********************
                 * 0 Выберите категорию
                 * 1 Пожелания к мультипаку
                 * 2 Пожелания к лаунчеру
                 * 3 Найдена ошибка в мультипаке
                 * 4 Найдена ошибка в лаунчере
                 * ********************/
                case 1: return "wish:Multipack";
                case 2: return "wish:Launcher";
                case 3: return "bug:Multipack";
                default: return "bug:Launcher";
            }
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            if (cbCaption.SelectedIndex != 0)
            {
                // Если юзер хочет привязать лаунчер к сайту
                if (cbCaption.SelectedIndex == 5)
                {
                    if (tbTicket.Text.Trim() != "" && tbEmail.Text.Trim() != "")
                    {
                        if (tbEmail.Text.Trim().IndexOf(" ") > -1)
                            MessageBox.Show(this, "Неверный email!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                        {
                            try
                            {
                                Debug Debug = new Debug();
                                Dictionary<string, string> nvc = new Dictionary<string, string>();

                                // Готовим отправку сообщения
                                nvc.Add("code", Debug.Code);
                                nvc.Add("userid", Debug.UserID());
                                nvc.Add("login", tbTicket.Text.Trim());
                                nvc.Add("email", tbEmail.Text.Trim());

                                SendPOST SendPOST = new SendPOST();

                                Dictionary<string, string> sendStatus = SendPOST.FromJson(SendPOST.Send(Properties.Resources.PostLink, "data=" + SendPOST.Json(nvc)));
                                switch (sendStatus["status"])
                                {
                                    case "LINK":
                                        if (DialogResult.Yes == MessageBox.Show(this, Language.DynamicLanguage("link", lang, tbTicket.Text.Trim()), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                                            Process.Start(Properties.Resources.LinkTicket);
                                        break;
                                    case "Hacking attempt!": MessageBox.Show(this, Language.DynamicLanguage("hacking", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); break;
                                    case "BANNED": MessageBox.Show(this, Language.DynamicLanguage("banned", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); break;
                                    case "LINKED": MessageBox.Show(this, Language.DynamicLanguage("linked", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); break;
                                    default: MessageBox.Show(this, Language.DynamicLanguage("error", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); break;
                                }
                            }
                            catch (Exception) { MessageBox.Show(this, Language.DynamicLanguage("error", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Обязательное условие - заполнение обоих полей!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    Close();
                }
                else
                {
                    int minSymbolsCount = 50,
                        maxWordLength = 20;

                    string mess = String.Empty;


                    if (tbTicket.Text.Trim().Length < minSymbolsCount) { mess += Language.DynamicLanguage("symbolLength", lang, minSymbolsCount.ToString()); }
                    if (sendStatus == "OK" && sendText == tbTicket.Text.Trim()) { mess += Environment.NewLine + Environment.NewLine + Language.DynamicLanguage("messAreSended", lang); }
                    if (!CheckLengthWord(tbTicket.Text.Trim(), maxWordLength)) mess += Environment.NewLine + Environment.NewLine + Language.DynamicLanguage("veryLongWord", lang, maxWordLength.ToString());

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
            }
            else
            {
                MessageBox.Show(this, Language.DynamicLanguage("NotSelectCategory", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckLengthWord(string s, int wordLength = 0)
        {
            if (wordLength == 0) return true;

            try
            {
                s = s.Replace("\n", " ").Replace("\r", " ").Replace("\r\n", " ").Replace(Environment.NewLine, " ");

                string[] arr = s.Split(' ');

                foreach (string str in arr)
                    if (str.Length > wordLength) return false;

                return true;
            }
            catch (Exception) { return true; }
        }

        private void lMessAboutNewVersion_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.LinkLauncherVK);
        }

        private async Task SetInterfaceLanguage()
        {
            foreach (Control control in this.Controls)
                control.Text = new Language().InterfaceLanguage("fWarning", control, lang);
        }

        private void fWarning_Load(object sender, EventArgs e)
        {
            string pathINI = Directory.GetCurrentDirectory() + @"\config.ini";
            lang = new IniFile(pathINI).IniReadValue("protanki", "language");
            lang = lang != "" ? lang : "en";

            SetInterfaceLanguage();


            /*********************************
             * Пожелания к мультипаку
             * Пожелания к лаунчеру
             * Найдена ошибка в мультипаке
             * Найдена ошибка в лаунчере
             * *******************************/
            cbCaption.Items.Clear(); // Очищаем перед загрузкой языка

            for (int i = 0; i <= 5; i++)
                if (Language.DynamicLanguage("cbCaption" + i.ToString(), lang) != "null")
                    cbCaption.Items.Add(Language.DynamicLanguage("cbCaption" + i.ToString(), lang));

            cbCaption.SelectedIndex = 0;
        }

        private void cbCaption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (cbCaption.SelectedIndex)
                {
                    case 5:
                        tbTicket.Height = 159;
                        lLinkWithSite.Visible = true;
                        break;

                    default:
                        tbTicket.Height = 207;
                        lLinkWithSite.Visible = false;
                        break;
                }
            }
            catch (Exception) { tbTicket.Height = 207; }
        }
    }
}
