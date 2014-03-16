﻿using System;
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
using System.Threading;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fIndex : Form
    {
        string xmlTitle = "",
            path = "",
            sVerPack,
            sVerTanks,
            sVerModPack;

        int verPack,
            verTanks,
            verModPack,
            threadSleep = 1000;

        bool updPack = false,
            updTanks = false;


        private void showError(string err)
        {
            MessageBox.Show(this, err, "Обнаружена ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public fIndex()
        {
            try
            {
                // Проверяем установлен ли в системе нужный нам фраймворк
                getFramework();

                InitializeComponent();

                //Проверяем запущен ли процесс
                // Если запущен, то закрываем все предыдущие, оставляя заново открытое окно
                Process[] myProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
                for (int i = 1; i < myProcesses.Length; i++) { myProcesses[i].Kill(); }

                loadSettings();

                this.Text = xmlTitle + " v" + sVerModPack;
                this.Icon = Properties.Resources.myicon;

                notifyIcon.Icon = Properties.Resources.myicon;
                notifyIcon.Text = xmlTitle + " v" + sVerModPack;

                llVersion.Text = sVerModPack;

                setBackground();

                moveForm();

                bwUpdater.WorkerReportsProgress = true;
                bwUpdater.WorkerSupportsCancellation = true;

                bwOptimize.WorkerReportsProgress = true;
                bwOptimize.WorkerSupportsCancellation = true;
            }
            catch (Exception ex0)
            {
                showError(ex0.Message);
            }
        }

        // Скачиваем и устанавливаем необходимую версию .NET Framework
        public void getFramework()
        {
            string query = "";

            Microsoft.Win32.RegistryKey myRegKey = Microsoft.Win32.Registry.LocalMachine;
                // 3.0
            query += !Directory.Exists(Environment.SystemDirectory + @"\..\Microsoft.NET\Framework\v3.0\") ? "3.0:" : "";

            // 4.0
            myRegKey = myRegKey.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4.0\Client\", true);
            try
            {
                query += (string)myRegKey.GetValue("version") != "4.0.0.0" ? "4.0" : "";
            }
            catch (Exception ex1)
            {
                query += (string)myRegKey.GetValue("version") != "4.0.0.0" ? "4.0" : "";
                showError(ex1.Message);
            }

            if (query != "") MessageBox.Show(query);
        }

        // Загружаем настройки
        public void loadSettings()
        {
            if (File.Exists("settings.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("settings.xml");

                //xmlTitle = doc.GetElementsByTagName("title")[0].InnerText;
                //xmlTitle = xmlTitle != "" ? xmlTitle : Application.ProductName;
                xmlTitle = Application.ProductName;

                path = doc.GetElementsByTagName("path")[0].InnerText;

                sVerModPack = doc.GetElementsByTagName("version")[0].InnerText;
                verModPack = Convert.ToInt32(sVerModPack.Replace(".", "")) + 0;

                //if (Directory.Exists(path))
                if (File.Exists(path + "version.xml"))
                {
                    if (!bwUpdater.IsBusy)
                    {
                        this.bwUpdater.RunWorkerAsync();
                    }
                    else
                    {
                        //Thread.Sleep(threadSleep);
                        loadSettings();
                    }
                }
                else
                {
                    checkTanks();
                }
            }
            else
            {
                MessageBox.Show("Файл настроек не обнаружен!" + Environment.NewLine + "Будут применены настройки по-умолчанию.");

                xmlTitle = Application.ProductName;

                sVerModPack = Application.ProductVersion;
                verModPack = Convert.ToInt32(sVerModPack.Replace(".", "")) + 0;

                path = "";

                checkTanks();
            }
        }

        // Если папка с танками не найдена, запускаем рекурсию, пока папка не будет указана верно
        public void checkTanks()
        {
            MessageBox.Show("Папка 'World of Tanks' задана некорректно");

            fSettings fSettings = new fSettings();
            if (fSettings.ShowDialog() == DialogResult.OK)
            {
                loadSettings();

                if (File.Exists(path + "version.xml"))
                {                    
                    if (!bwUpdater.IsBusy) { bwUpdater.RunWorkerAsync(); }
                }
                else
                {
                    //Thread.Sleep(threadSleep);
                    checkTanks();
                }
            }
            else
            {
                //Thread.Sleep(threadSleep);
                checkTanks();
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
            catch (Exception ex) { showError(ex.Message); return -1; }
        }

        // Выбираем изображение для установки фона
        public void setBackground()
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
            //bwUpdater.ReportProgress(0);

            // Парсим сайт танков
            /*string s = getResponse("http://worldoftanks.ru");
            s = s.Remove(0, s.IndexOf("b-game-version") + 16);
            s = s.Remove(s.IndexOf("</span>")).Replace(".","");             
             updates = getVersion() < (Convert.ToInt32(s)+0) ? true : false;*/

            XmlDocument doc0 = new XmlDocument();
            doc0.Load(@"http://ai-rus.com/pro.xml");
            sVerPack = doc0.GetElementsByTagName("version")[0].InnerText;
            sVerTanks = doc0.GetElementsByTagName("tanks")[0].InnerText;

            verPack = Convert.ToInt32(sVerPack.Replace(".", "")) + 0;
            verTanks = Convert.ToInt32(sVerTanks.Replace(".", "")) + 0;

            int v = getVersion();
            updTanks = v < 0 || v < verTanks ? true : false;

            //Проверяем апдейты на модпак
            updPack = verModPack < verPack ? true : false;
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
            try
            {
                //pWork.Visible = false;

                string status = "";

                if (updPack || updTanks)
                {
                    if (updPack)
                    {
                        status += "Обнаружена новая версия!" + Environment.NewLine +
                            "Вы используете устаревшую версию Мультипака" + Environment.NewLine +
                            "Рекомендуем обновить Ваш Мультипак до версии '" + sVerPack.ToString() + "'" + Environment.NewLine + Environment.NewLine;
                        bUpdate.Enabled = true;

                        this.llContent.Location = new Point(22, 55);
                        this.llContent.Size = new Size(638, 377);
                    }

                    if (updTanks)
                    {
                        status += "Обнаружена новая версия клиента игры!" + Environment.NewLine +
                            "Необходимо запустить лаунчер игры для обновления до версии '" + sVerTanks.ToString() + "'";
                        bUpdate.Enabled = true;

                        this.llContent.Location = new Point(22, 55);
                        this.llContent.Size = new Size(638, 377);
                    }
                }
                else
                {
                    status = "Вы используете самые свежие моды." + Environment.NewLine +
                        "Текущая версия Мультипака '" + sVerModPack + "'";
                    bUpdate.Enabled = false;

                    this.llContent.Location = new Point(70, 130);
                    this.llContent.Size = new Size(590, 227);
                }

                this.llContent.Text = status;
                notifyIcon.ShowBalloonTip(2000, xmlTitle, status, ToolTipIcon.Info);
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка. Требуется перезапустить лаунчер модпака.");
                Process.Start("restart.exe", Process.GetCurrentProcess().ProcessName);
                Close();
            }
        }

        private void bLauncher_Click(object sender, EventArgs e)
        {
            Process.Start(path + "WoTLauncher.exe");

            Hide();
            WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = true;
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            Process.Start(path + "WorldOfTanks.exe");

            Hide();
            WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = true;
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void проверитьОбновленияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bwUpdater.IsBusy)
            {
                bwUpdater.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void видеоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://goo.gl/gr6pFl");
        }

        private void llVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!bwUpdater.IsBusy)
            {
                bwUpdater.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void bwUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //pWork.Visible = true;
            //pWork.Location = new Point(4, 25);
            //pWork.Size = new Size(852, 431);
        }

        private void bOptimizePC_Click(object sender, EventArgs e)
        {
            if (!bwOptimize.IsBusy)
            {
                bwOptimize.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(this, "Подождите завершения предыдущей операции", "Оптимизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bwOptimize_DoWork(object sender, DoWorkEventArgs e)
        {
            // Отключаем гибернацию
            Process.Start(Environment.SystemDirectory + @"\powercfg.exe", "-h off");

            // Правим реестр
            Microsoft.Win32.RegistryKey myRegKey = Microsoft.Win32.Registry.CurrentUser;
            myRegKey = myRegKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true);
            myRegKey.SetValue("DisablePreviewDesktop", "dword: 00000001");

        }

        private void llContent_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!bwUpdater.IsBusy)
            {
                bwUpdater.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fSettings fSettings = new fSettings();
            fSettings.ShowDialog();
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }
    }
}
