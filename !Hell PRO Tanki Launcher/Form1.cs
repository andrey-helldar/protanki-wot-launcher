using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Xml;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fIndex : Form
    {
        string path = "",
            sVerPack,
            sVerTanks;

        int verPack,
            verTanks;

        bool updPack = false,
            updTanks = false;


        public fIndex()
        {
            InitializeComponent();

            this.Text = Application.ProductName + " v" + Application.ProductVersion;
            this.Icon = Properties.Resources.myicon;

            llVersion.Text = Application.ProductVersion;

            setBackground();

            moveForm();

            bwUpdater.WorkerReportsProgress = true;
            bwUpdater.WorkerSupportsCancellation = true;

            // Загружаем настройки
            if (File.Exists("settings.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("settings.xml");
                path = doc.GetElementsByTagName("path")[0].InnerText;

                if (Directory.Exists(path))
                {
                    llContent.Text = path;

                    if (!bwUpdater.IsBusy)
                    {
                        bwUpdater.RunWorkerAsync();
                    }
                }
                else
                {
                    MessageBox.Show("Папка с танками не обнаружена");
                }
            }
            else
            {
                MessageBox.Show("Файл настроек не обнаружен!");
            }
        }

        // Получаем версию танков
        private int getVersion()
        {
            try
            {
                if (File.Exists(path + "version.xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + "version.xml");

                    string ver = doc.GetElementsByTagName("version")[0].InnerText;
                    ver = ver.Trim().Remove(0, 2).Remove(ver.IndexOf("#") - 4).Replace(".", "");

                    return Convert.ToInt16(ver) + 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception) { return -1; }
        }

        // Выбираем изображение для установки фона
        private void setBackground()
        {
            try
            {
                Random rand = new Random();
                string r = "back_" + rand.Next(1, 7);
                switch (r)
                {
                    case "back_1": this.BackgroundImage = Properties.Resources.back_1; break;
                    case "back_2": this.BackgroundImage = Properties.Resources.back_2; break;
                    case "back_3": this.BackgroundImage = Properties.Resources.back_3; break;
                    case "back_4": this.BackgroundImage = Properties.Resources.back_4; break;
                    case "back_5": this.BackgroundImage = Properties.Resources.back_5; break;
                    case "back_6": this.BackgroundImage = Properties.Resources.back_6; break;
                    default: this.BackgroundImage = Properties.Resources.back_7; break;
                }
            }
            catch (Exception) { }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bVideo_Click(object sender, EventArgs e)
        {
            Process.Start("http://goo.gl/gr6pFl");
        }

        private void bwUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            // Парсим сайт танков
            /*string s = getResponse("http://worldoftanks.ru");
            s = s.Remove(0, s.IndexOf("b-game-version") + 16);
            s = s.Remove(s.IndexOf("</span>")).Replace(".","");             
             updates = getVersion() < (Convert.ToInt16(s)+0) ? true : false;*/

            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://ai-rus.com/pro.xml");
            sVerPack = doc.GetElementsByTagName("version")[0].InnerText;
            sVerTanks = doc.GetElementsByTagName("tanks")[0].InnerText;

            verPack = Convert.ToInt32(sVerPack.Replace(".", "")) + 0;
            verTanks = Convert.ToInt32(sVerTanks.Replace(".", "")) + 0;

            int v = getVersion();
            updTanks = v < 0 || v < verTanks ? true : false;

            //Проверяем апдейты на модпак
            updPack = Convert.ToInt32(Application.ProductVersion.Replace(".", "")) < verPack ? true : false;
        }


        static string getResponse(string uri)
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

        private void bwUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string status = "";

            if (updPack || updTanks)
            {
                if (updPack)
                {
                    status += "Обнаружена новая версия!" + Environment.NewLine +
                        "Вы используете устаревшую версию Мультипака" + Environment.NewLine +
                        "Рекомендуем обновить Ваш Мультипак до версии '" + sVerPack.ToString() + "'";
                    bUpdate.Enabled = true;
                    llContent.Location = new Point(22, 55);
                    llContent.Size = new Size(638, 377);
                }

                if (updTanks)
                {
                    status += "Обнаружена новая версия клиента игры!" + Environment.NewLine +
                        "Необходимо запустить лаунчер игры для обновления до версии '" + sVerTanks.ToString() + "'";
                    bUpdate.Enabled = true;
                    llContent.Location = new Point(22, 55);
                    llContent.Size = new Size(638, 377);
                }
            }
            else
            {
                status = "Поздравляю!" + Environment.NewLine +
                    "Вы используете самые свежие моды." + Environment.NewLine +
                    "Текущая версия Мультипака '" + Application.ProductVersion + "'";
                bUpdate.Enabled = false;

                llContent.Location = new Point(70, 130);
                llContent.Size = new Size(590, 227);
            }

            llContent.Text = status;
        }

        private void bLauncher_Click(object sender, EventArgs e)
        {
            Process.Start(path + "WoTLauncher.exe");
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            Process.Start(path + "WorldOfTanks.exe");
        }

        private void moveForm()
        {
            this.MouseDown += delegate
            {
                this.Capture = false;
                var msg = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            };

            llContent.MouseDown += delegate
            {
                llContent.Capture = false;
                var msg = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            };
        }

        private void bUpdate_Click(object sender, EventArgs e)
        {
            Process.Start("http://goo.gl/gr6pFl");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ai-rus.com");
        }
    }
}
