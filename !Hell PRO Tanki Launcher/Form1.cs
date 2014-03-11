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
        string path = "";
        bool updates = false;

        public fIndex()
        {
            InitializeComponent();

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

                llContent.Text = path;

                if (!bwUpdater.IsBusy)
                {
                    bwUpdater.RunWorkerAsync();
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
            XmlDocument doc = new XmlDocument();
            doc.Load(path + "version.xml");

            string ver = doc.GetElementsByTagName("version")[0].InnerText;
            ver = ver.Trim().Remove(0, 2).Remove(ver.IndexOf("#") - 4).Replace(".","");

            return Convert.ToInt16(ver)+0;
        }

        // Выбираем изображение для установки фона
        private void setBackground()
        {
            Random rand = new Random();

            string r = "back_"+rand.Next(1,7);

            switch(r){
                case "back_1": this.BackgroundImage = Properties.Resources.back_1; break;
                case "back_2": this.BackgroundImage = Properties.Resources.back_2; break;
                case "back_3": this.BackgroundImage = Properties.Resources.back_3; break;
                case "back_4": this.BackgroundImage = Properties.Resources.back_4; break;
                case "back_5": this.BackgroundImage = Properties.Resources.back_5; break;
                case "back_6": this.BackgroundImage = Properties.Resources.back_6; break;
                default: this.BackgroundImage = Properties.Resources.back_7; break;
            }
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
            string version = doc.GetElementsByTagName("version")[0].InnerText;
            string tanks = doc.GetElementsByTagName("tanks")[0].InnerText;

            version = version.Replace(".", "");
            tanks = tanks.Replace(".", "");

            updates = getVersion() < (Convert.ToInt16(tanks) + 0) ? true : false;
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
            llContent.Text = updates ? "Обнаружена новая версия" : "Вы используете актуальную версию";
            bUpdate.Enabled = updates;
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
