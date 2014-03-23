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
using System.Threading;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fIndex : Form
    {
        string xmlTitle = "",
            path = "",
            sVerType = "full",
            sVerPack,
            sVerTanks,
            sVerModPack,
            sUpdateNews,
            youtubeChannel = "PROTankiWoT",
            youtubeLink = "",
            sUpdateLink = "http://goo.gl/gr6pFl",
            videoLink = "http://goo.gl/gr6pFl";

        int verPack,
            verTanks,
            verModPack,
            threadSleep = 1000,
            maxPercentUpdateStatus = 1;

        bool updPack = false,
            updTanks = false,
            optimized = false,
            
            autoKill = true,
            autoAero = true,
            autoNews = true,
            autoVideo = true;

        ProcessStartInfo psi;


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

                llTitle.Text = xmlTitle + " (" + sVerType + ")";

                notifyIcon.Icon = Properties.Resources.myicon;
                notifyIcon.Text = xmlTitle + " v" + sVerModPack;

                llUpdateStatus.Text = ""; // Убираем текст с метки статуса обновления

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
                sVerType = doc.GetElementsByTagName("type")[0].InnerText;

                foreach (XmlNode xmlNode in doc.GetElementsByTagName("settings"))
                {
                    autoKill = xmlNode.Attributes["kill"].InnerText == "False" ? false : true;
                    autoAero = xmlNode.Attributes["aero"].InnerText == "False" ? false : true;
                    autoNews = xmlNode.Attributes["news"].InnerText == "False" ? false : true;
                    autoVideo = xmlNode.Attributes["video"].InnerText == "False" ? false : true;
                }

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
                        Thread.Sleep(500);
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
            Process.Start(videoLink);
        }

        private void bwUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //bwUpdater.ReportProgress(0);

                // Парсим сайт танков
                /*string s = getResponse("http://worldoftanks.ru");
                s = s.Remove(0, s.IndexOf("b-game-version") + 16);
                s = s.Remove(s.IndexOf("</span>")).Replace(".","");             
                 updates = getVersion() < (Convert.ToInt32(s)+0) ? true : false;*/

                // Парсим сайт PRO Танки
                XmlDocument doc = new XmlDocument();
                //doc.Load(@"http://file.theaces.ru/mods/proupdate/updateFull.xml");
                //doc.Load(@"pro.xml");
                doc.Load(@"http://ai-rus.com/pro.xml");
                sVerPack = doc.GetElementsByTagName("version")[0].InnerText;
                sVerTanks = doc.GetElementsByTagName("tanks")[0].InnerText;

                verPack = Convert.ToInt32(sVerPack.Replace(".", "")) + 0;
                verTanks = Convert.ToInt32(sVerTanks.Replace(".", "")) + 0;

                int v = getVersion();
                updTanks = v < 0 || v < verTanks ? true : false;

                //Проверяем апдейты на модпак
                updPack = verModPack < verPack ? true : false;

                if (updPack)
                {
                    foreach (XmlNode xmlNode in doc.GetElementsByTagName(sVerType))
                    {
                        sUpdateNews = xmlNode["message"].InnerText.Replace(":;", Environment.NewLine);
                        sUpdateLink = xmlNode["download"].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
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
                        status += "Обнаружена новая версия Мультипака (" + sVerPack.ToString() + ")!" + Environment.NewLine;
                        bUpdate.Enabled = true;

                        this.llContent.Location = new Point(30, 40);
                        this.llContent.Size = new Size(595, 370);

                        videoLink = sUpdateLink;
                    }

                    if (updTanks)
                    {
                        status += "Обнаружена новая версия клиента игры (" + sVerTanks.ToString() + ")!" + Environment.NewLine;
                        bUpdate.Enabled = true;

                        this.llContent.Location = new Point(30, 40);
                        this.llContent.Size = new Size(595, 370);
                    }

                    // Добавляем новость об изменениях в модпаке
                    if (updPack) { status += Environment.NewLine + sUpdateNews; }
                }
                else
                {
                    status = "Вы используете самые свежие моды." + Environment.NewLine +
                        "Текущая версия Мультипака '" + sVerModPack + "'";
                    bUpdate.Enabled = false;

                    this.llContent.Location = new Point(150, 200);
                    this.llContent.Size = new Size(477, 100);
                }

                this.llContent.Text = status;
                notifyIcon.ShowBalloonTip(2000, xmlTitle, status, ToolTipIcon.Info);
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка. Требуется перезапустить лаунчер модпака.");
                Process.Start("restart.exe", Process.GetCurrentProcess().ProcessName);
                this.Close();
            }
        }

        private void bLauncher_Click(object sender, EventArgs e)
        {
            if (!bwOptimize.IsBusy) { bwOptimize.RunWorkerAsync(); }

            Process.Start(path + "WoTLauncher.exe");

            if (!bwAero.IsBusy) { bwAero.RunWorkerAsync(); }

            Hide();
            WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = true;
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            if (!bwOptimize.IsBusy) { bwOptimize.RunWorkerAsync(); }

            Process.Start(path + "WorldOfTanks.exe");

            if (!bwAero.IsBusy) { bwAero.RunWorkerAsync(); }

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
            Process.Start(sUpdateLink);
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
                optimized = true;
                bwOptimize.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(this, "Подождите завершения предыдущей операции", "Оптимизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bwOptimize_DoWork(object sender, DoWorkEventArgs e)
        {
            int myIndex = 0,
                myProgressStatus = 0;

            // Проверяем условие: если процесс оптимизации запущен вручную, или указан в настройках, то:
            if (optimized || autoAero)
            {
                // Для начала определим версию ОС для отключения AERO
                OperatingSystem osInfo = Environment.OSVersion;

                switch (osInfo.Platform)
                {
                    case System.PlatformID.Win32NT:
                        switch (osInfo.Version.Major)
                        {
                            case 6:
                                // Win7
                                saveLog(++myIndex, @"Start -- net stop uxsms");
                                psi = new ProcessStartInfo("cmd", @"/c net stop uxsms"); // останавливаем aero
                                Process.Start(psi);
                                saveLog(myIndex, @"End -- net stop uxsms");
                                //addData("cmd", @"Win7 :: /c /q net stop uxsms");
                                break;

                            case 7:
                                // Win8
                                saveLog(++myIndex, @"Start -- net stop uxsms");
                                psi = new ProcessStartInfo("cmd", @"/c net stop uxsms"); // останавливаем aero
                                Process.Start(psi);
                                saveLog(myIndex, @"End -- net stop uxsms");
                                //addData("cmd", @"Win8 :: /c /q net stop uxsms");
                                break;

                            /* default:
                                 if (osInfo.Version.Major == 5 && osInfo.Version.Minor != 0)
                                 {
                                     // WinXP
                                 }
                                 break;*/
                        }
                        break;
                }

                maxPercentUpdateStatus = 1;
                bwOptimize.ReportProgress(++myProgressStatus);
            }

            if (optimized || autoKill)
            {
                // Завершаем ненужные процессы путем перебора массива имен с условием отсутствия определенных условий
                Process[] myProcesses = Process.GetProcesses();
                int processID = Process.GetCurrentProcess().SessionId;

                // Расчитываем значение прогресс бара
                maxPercentUpdateStatus += myProcesses.Length * 2 - 2;

                // устанавливаем имена процессов, завершать которые НЕЛЬЗЯ
                string[] vipProcess = {
                                      Process.GetCurrentProcess().ProcessName.ToString(),
                                      "restart",
                                      "WorldOfTanks", 
                                      "WoTLauncher",
                                      "devenv", // Visual Studio
                                      "CCC", // ATI
                                      "atieclxx", // ATI
                                      "avpui", // Kaspersky
                                      "conhost",
                                      "explorer",
                                      "MOM",
                                      "taskeng",
                                      "taskhost",
                                      "RtkNGUI64",  // Realtek
                                      "csrss",
                                      "dwm",
                                      "winlogon",
                                      "iusb3mon",
                                      "U3BoostSvr64",
                                      "U3BoostSvr32",
                                      "MKey",
                                      "NvBackend", // NVIDIA
                                      "nvstreamsvc",// NVIDIA
                                      "NvTmru",// NVIDIA
                                      "nvtray",// NVIDIA
                                      "nvvsvc",// NVIDIA
                                      "nvxdsync"// NVIDIA
                                      };

                // Передаем процессам инфу, что приложение должно закрыться
                for (int i = 1; i < myProcesses.Length; i++)
                {
                    try
                    {
                        // Расчитываем значение прогресс бара
                        bwOptimize.ReportProgress(++myProgressStatus);

                        if (myProcesses[i].SessionId == processID && Array.IndexOf(vipProcess, myProcesses[i].ProcessName.ToString()) == -1)
                        {
                            saveLog(++myIndex, @"Start close normally -- " + myProcesses[i].ProcessName.ToString());
                            myProcesses[i].CloseMainWindow();
                            saveLog(myIndex, @"End close normally -- " + myProcesses[i].ProcessName.ToString());
                            //addData(myProcesses[i].ProcessName.ToString(), "Closed normally");
                        }
                        else
                        {
                            saveLogNotCloseProcess(myProcesses[i].ProcessName.ToString() + "   ||   SessionID : " + myProcesses[i].SessionId.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        saveLog(myIndex, @"ERROR EXCEPTION close normally -- " + myProcesses[i].ProcessName.ToString());
                    }
                }

                Thread.Sleep(5000); // Ждем 5 секунд завершения, пока приложения нормально завершатся

                // Кто не успел - тот опоздал! Принудительно убиваем процесс
                for (int i = 1; i < myProcesses.Length; i++)
                {
                    try
                    {
                        // Расчитываем значение прогресс бара
                        bwOptimize.ReportProgress(++myProgressStatus);

                        if (myProcesses[i].SessionId == processID && Array.IndexOf(vipProcess, myProcesses[i].ProcessName.ToString()) == -1)
                        {
                            saveLog(++myIndex, @"Start Kill -- " + myProcesses[i].ProcessName.ToString());
                            myProcesses[i].Kill();
                            saveLog(myIndex, @"End Kill -- " + myProcesses[i].ProcessName.ToString());
                            //addData(myProcesses[i].ProcessName.ToString(), "Killed");
                        }
                        else
                        {
                            saveLogNotCloseProcess(myProcesses[i].ProcessName.ToString() + " (kill)   ||   SessionID : " + myProcesses[i].SessionId.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        saveLog(myIndex, @"ERROR EXCEPTION Kill -- " + myProcesses[i].ProcessName.ToString());
                    }
                }
            }
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

        private void fIndex_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Раньше проверяли выли ли внесены изменения, сейчас просто запускаем aero...
            //if (optimized)
            //{
                psi = new ProcessStartInfo("cmd", @"/c net start uxsms"); // останавливаем aero
                Process.Start(psi);
                //addData("cmd", @"/c net start uxsms");
            //}
        }

        private void llTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void addData(string attr, string param)
        {
            //int i = listView1.Items.Add(attr).Index;
            //listView1.Items[i].SubItems.Add(param);
        }

        private void saveLog(int index, string param)
        {
            bool z = false;

            if (z)
            {
                if (!Directory.Exists("log")) { Directory.CreateDirectory("log"); }

                string myFile = @"log\" + index.ToString() + "__" + param + ".log";

                if (!File.Exists(myFile))
                {
                    File.WriteAllText(myFile, param, Encoding.UTF8);
                }
                else
                {
                    File.AppendAllText(myFile, Environment.NewLine + param, Encoding.UTF8);
                }
            }
        }

        private void saveLogNotCloseProcess(string param)
        {
            bool z = false;

            if (z)
            {
                if (!File.Exists(@"log\not_closed_process.log"))
                {
                    File.WriteAllText(@"log\not_closed_process.log", param, Encoding.UTF8);
                }
                else
                {
                    File.AppendAllText(@"log\not_closed_process.log", Environment.NewLine + param, Encoding.UTF8);
                }
            }
        }

        private void bwOptimize_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Успешно завершено!");
            //llUpdateStatus.Text = "Оптимизация завершена";
        }

        private void bSettings_Click(object sender, EventArgs e)
        {
            fSettings fSettings = new fSettings();
            if (fSettings.ShowDialog() == DialogResult.OK)
            {
                loadSettings();
            }
        }

        private void bwOptimize_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double i = Convert.ToDouble(e.ProgressPercentage) / Convert.ToDouble(maxPercentUpdateStatus) * 100;
            llUpdateStatus.Text = "Оптимизация завершена на: " + ((int)i).ToString() + "%";
        }

        private void bwAero_DoWork(object sender, DoWorkEventArgs e)
        {
            bool t = true;

            Thread.Sleep(5000);

            while (t)
            {
                Process[] myProcessL = Process.GetProcessesByName("WoTLauncher");
                Process[] myProcessW = Process.GetProcessesByName("WorldOfTanks");

                if (myProcessW.Length < 1 && myProcessL.Length < 1)
                {
                    psi = new ProcessStartInfo("cmd", @"/c net start uxsms");
                    Process.Start(psi);
                    t = false;
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                int i = 0;

                XmlDocument doc = new XmlDocument();
                doc.Load(@"https://gdata.youtube.com/feeds/api/users/"+youtubeChannel+"/uploads");

                foreach (XmlNode xmlNode in doc.GetElementsByTagName("entry"))
                {
                    /*str += xmlNode["title"].InnerText + "   :::   ";
                    if (xmlNode["link"].Attributes["rel"].InnerText == "alternate")
                    {
                        str += xmlNode["link"].Attributes["href"].InnerText + Environment.NewLine;
                    }*/

                    LinkLabel lebel = new LinkLabel();
                    lebel.SetBounds(10,i*20,100,20);
                    lebel.AutoSize = true;
                    lebel.ActiveLinkColor = Color.FromArgb(243, 123, 16);
                    lebel.ForeColor = Color.FromArgb(243, 123, 16);
                    lebel.VisitedLinkColor = Color.FromArgb(243, 123, 16);
                    lebel.LinkColor = Color.FromArgb(243, 123, 16);
                    lebel.Text = xmlNode["title"].InnerText;
                    lebel.Name = "llNews" + (++i).ToString();
                    if (xmlNode["link"].Attributes["rel"].InnerText == "alternate"){
                        lebel.Links[0].LinkData = xmlNode["link"].Attributes["href"].InnerText;
                        lebel.Click += new EventHandler(lebel_Click);
                    }
                    this.gbVideo.Controls.Add(lebel);
                }

               // MessageBox.Show(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Инфа тут:
        //          http://www.cyberforum.ru/windows-forms/thread740428.html
        private void lebel_Click(object sender, EventArgs e)
        {
            Process.Start((sender as LinkLabel).Links[0].LinkData.ToString());
        }

        /* Запрос тут:
         * https://www.google.ru/search?q=%D0%BA%D0%B0%D0%BA+%D0%BF%D1%80%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%BC%D0%BD%D0%BE+%D1%81%D0%BE%D0%B7%D0%B4%D0%B0%D1%82%D1%8C+linklabel+c%23&oq=%D0%BA%D0%B0%D0%BA+%D0%BF%D1%80%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%BC%D0%BD%D0%BE+%D1%81%D0%BE%D0%B7%D0%B4%D0%B0%D1%82%D1%8C+linklabel+c%23&aqs=chrome..69i57.11575j0j7&sourceid=chrome&espv=2&es_sm=93&ie=UTF-8
         * */
    }
}
