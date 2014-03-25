using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Xml;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;

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
            sUpdateLink = "http://goo.gl/gr6pFl",
            videoLink = "http://goo.gl/gr6pFl",
            updateNotification = "";

        int verPack,
            verTanks,
            verModPack,
            maxPercentUpdateStatus = 1,
            showVideoTop = 70;

        bool updPack = false,
            updTanks = false,
            optimized = false,

            autoKill = true,
            autoForceKill = false,
            autoAero = true,
            autoNews = true,
            autoVideo = true;

        List<string> youtubeTitle = new List<string>();
        List<string> youtubeLink = new List<string>();
        List<string> youtubeDate = new List<string>();

        ProcessStartInfo psi;

        // Инициализируем окно статуса обновлений
        fNewVersion fNewVersion = new fNewVersion();

        // Инициализируем класс дебага
        debug debug = new debug();


        private void showError(string err)
        {
            MessageBox.Show(this, err, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public fIndex()
        {
            //Проверяем запущен ли процесс
            // Если запущен, то закрываем все предыдущие, оставляя заново открытое окно
            Process[] myProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            for (int i = 1; i < myProcesses.Length; i++) { myProcesses[i].Kill(); }

            // Проверяем установлен ли в системе нужный нам фраймворк
            getFramework();

            InitializeComponent();

            loadSettings();

            try
            {
                this.Text = xmlTitle + " v" + sVerModPack;
                this.Icon = Properties.Resources.myicon;

                llTitle.Text = xmlTitle + " (" + (sVerType == "full" ? "Расширенная версия" : "Базовая версия") + ")";

                llLauncherVersion.Text = Application.ProductVersion;

                notifyIcon.Icon = Properties.Resources.myicon;
                notifyIcon.Text = xmlTitle + " v" + sVerModPack;
            }
            catch (Exception ex)
            {
                debug.Save("public fIndex()", "Применение заголовков и иконок приложения", ex.Message);
            }

            // Грузим видео с ютуба
            if (!bwVideo.IsBusy) { bwVideo.RunWorkerAsync(); }

            llUpdateStatus.Text = ""; // Убираем текст с метки статуса обновления

            llVersion.Text = sVerModPack;

            setBackground();

            moveForm();

            if (!bwUpdateLauncher.IsBusy) { bwUpdateLauncher.RunWorkerAsync(); }

            bwUpdater.WorkerReportsProgress = true;
            bwUpdater.WorkerSupportsCancellation = true;

            bwOptimize.WorkerReportsProgress = true;
            bwOptimize.WorkerSupportsCancellation = true;
        }

        // Узнаем разряд системы
        private bool isX64()
        {
            return Environment.Is64BitOperatingSystem ? true : false;
        }

        // Скачиваем и устанавливаем необходимую версию .NET Framework
        public void getFramework()
        {
            try
            {
                string mess = "";
                List<string> frameworkLinks = new List<string>();

                // v2.0.50727
                /*var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727");
                if (key) != null)
                {
                    if ((int)key.GetValue("Install") != 1) { 
                        mess += ".NET Framework " + (string)key.GetValue("Version") + " not installed!" + Environment.NewLine;                        
                        frameworkLinks.Add(isX64() ? "http://www.microsoft.com/ru-ru/download/details.aspx?id=6041" : "http://www.microsoft.com/ru-ru/download/details.aspx?id=1639");
                    }
                }
                else
                {
                    mess += ".NET Framework " + (string)key.GetValue("Version") + " not installed!" + Environment.NewLine;
                }*/


                // v3.0.30729
                /*var key30 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0");
                if (key30) != null)
                {
                    if ((int)key30.GetValue("Install") != 1) { mess += ".NET Framework " + (string)key30.GetValue("Version") + " not installed!" + Environment.NewLine; }
                }
                else
                {
                    mess += ".NET Framework " + (string)key30.GetValue("Version") + " not installed!" + Environment.NewLine;
                }*/


                // v3.5
                /*var key35 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5");
                if (key35 != null)
                {
                    if ((int)key35.GetValue("Install") != 1) { mess += ".NET Framework " + (string)key35.GetValue("Version") + " not installed!" + Environment.NewLine; }
                }
                else
                {
                    mess += ".NET Framework " + (string)key35.GetValue("Version") + " not installed!" + Environment.NewLine;
                }*/


                // v4.0
                var key40 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4.0\Client");
                if (key40 != null)
                {
                    if ((int)key40.GetValue("Install") != 1)
                    {
                        mess += ".NET Framework " + (string)key40.GetValue("Version") + " not installed!" + Environment.NewLine;
                        frameworkLinks.Add("http://www.microsoft.com/ru-ru/download/details.aspx?id=24872");
                    }
                }
                else
                {
                    mess += ".NET Framework " + (string)key40.GetValue("Version") + " not installed!" + Environment.NewLine;
                }

                if (mess.Length > 0)
                {
                    MessageBox.Show(this, "Для корректной работы приложения требуется установка следующих пакетов:" + Environment.NewLine + mess + Environment.NewLine +
                    "---------------------------------------------------" + Environment.NewLine +
                    "ВНИМАНИЕ! После закрытия данного окна в Вашем браузере будут открыты ссылки на страницы для скачивания нужных Вам библиотек .NET Framework с сайта microsoft.com", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (string link in frameworkLinks)
                    {
                        Process.Start(link);
                    }
                }
            }
            catch (Exception ex)
            {
                debug.Save("public void getFramework()", "", ex.Message);
            }
        }

        // Загружаем настройки
        public void loadSettings()
        {
            try
            {
                if (File.Exists("settings.xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load("settings.xml");

                    //xmlTitle = doc.GetElementsByTagName("title")[0].InnerText;
                    //xmlTitle = xmlTitle != "" ? xmlTitle : Application.ProductName;
                    xmlTitle = Application.ProductName;

                    //path = @"d:\Games\World_of_Tanks\";
                    path = Application.StartupPath + @"\..\";
                    sVerType = doc.GetElementsByTagName("type")[0].InnerText;

                    try
                    {
                        updateNotification = doc.GetElementsByTagName("notification")[0].InnerText;
                    }
                    catch (Exception)
                    {
                        updateNotification = "";
                    }

                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("settings"))
                    {
                        autoKill = xmlNode.Attributes["kill"].InnerText == "False" ? false : true;
                        autoAero = xmlNode.Attributes["aero"].InnerText == "False" ? false : true;
                        autoNews = xmlNode.Attributes["news"].InnerText == "False" ? false : true;
                        autoVideo = xmlNode.Attributes["video"].InnerText == "False" ? false : true;
                    }

                    sVerModPack = doc.GetElementsByTagName("version")[0].InnerText;
                    verModPack = Convert.ToInt32(sVerModPack.Replace(".", "")) + 0;
                }
                else
                {
                    MessageBox.Show("Файл настроек не обнаружен!" + Environment.NewLine + "Будут применены настройки по-умолчанию.");

                    xmlTitle = Application.ProductName;

                    sVerModPack = Application.ProductVersion;
                    verModPack = Convert.ToInt32(sVerModPack.Replace(".", "")) + 0;

                    path = "";
                }
            }
            catch (Exception ex)
            {
                debug.Save("public fIndex()", "public void loadSettings()", ex.Message);
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
                    debug.Save(" private int getVersion()", "if (File.Exists(path + \"version.xml\"))", "Клиент игры не обнаружен." + Environment.NewLine + "Проверьте правильность установки модпака.");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                debug.Save(" private int getVersion()", "if (File.Exists(path + \"version.xml\"))", ex.Message);
                return -1;
            }
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
            catch (Exception ex)
            {
                debug.Save("public void setBackground()", "", ex.Message);
                this.BackgroundImage = Properties.Resources.back_7;
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bVideo_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(videoLink);
            }
            catch (Exception ex)
            {
                debug.Save("private void bVideo_Click(object sender, EventArgs e)", "", ex.Message);
            }
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
                doc.Load(@"http://ai-rus.com/pro/pro.xml");
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
                debug.Save("private void bwUpdater_DoWork(object sender, DoWorkEventArgs e)", "", ex.Message);
            }
        }


        static string getResponse(string uri)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(fIndex.ActiveForm, ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        private void bwUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                string status = "";

                if (updPack || updTanks)
                {
                    if (updPack)
                    {
                        status += "Обнаружена новая версия Мультипака (" + sVerPack.ToString() + ")!" + Environment.NewLine;
                        bUpdate.Enabled = true;

                        videoLink = sUpdateLink;
                    }

                    if (updTanks)
                    {
                        status += "Обнаружена новая версия клиента игры (" + sVerTanks.ToString() + ")!" + Environment.NewLine;
                        bUpdate.Enabled = true;

                        // Отключаем кнопку запуска игры
                        bPlay.Enabled = false;
                    }
                    else
                    {
                        // Включаем кнопку запуска игры
                        bPlay.Enabled = true;
                    }

                    // Добавляем новость об изменениях в модпаке
                    if (updPack) { status += Environment.NewLine + sUpdateNews; }

                    llActually.Text = "Обнаружена новая версия!";
                    llActually.ForeColor = Color.Yellow;
                    llActually.ActiveLinkColor = Color.Yellow;
                    llActually.LinkColor = Color.Yellow;
                    llActually.VisitedLinkColor = Color.Yellow;
                    llActually.SetBounds(315 - (int)(llActually.Width / 2), llActually.Location.Y, 10, 10);

                    // Изменяем изображение иконки статуса обновлений
                    pbNewVersion.BackgroundImage = Properties.Resources.newVersion;
                    pbNewVersion.Cursor = Cursors.Hand;

                    // Окно статуса обновлений
                    fNewVersion.llContent.Text = status;
                    fNewVersion.llContent.Links[0].LinkData = videoLink;
                    fNewVersion.llVersion.Text = sVerPack.ToString();
                    if (updateNotification != sVerPack.ToString())
                    {
                        fNewVersion.ShowDialog();
                    }
                }
                else
                {
                    llActually.Text = "Вы используете самые свежие моды!";
                    llActually.ForeColor = Color.Lime;
                    llActually.ActiveLinkColor = Color.Lime;
                    llActually.LinkColor = Color.Lime;
                    llActually.VisitedLinkColor = Color.Lime;

                    status = "Вы используете самые свежие моды." + Environment.NewLine +
                        "Текущая версия Мультипака '" + sVerModPack + "'";
                    bUpdate.Enabled = false;

                    //this.llContent.Location = new Point(150, 200);
                    //this.llContent.Size = new Size(477, 100);

                    // Изменяем изображение иконки статуса обновлений
                    pbNewVersion.BackgroundImage = Properties.Resources.good;
                    pbNewVersion.Cursor = Cursors.Default;

                    // Окно статуса обновлений
                    fNewVersion.llContent.Text = status;
                    fNewVersion.llVersion.Text = sVerPack.ToString();
                }

                //this.llContent.Text = status;
                notifyIcon.ShowBalloonTip(2000, xmlTitle, status, ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                debug.Save("private void bwUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)", "Возникла ошибка обновления. Лаунчер модпака будет перезапущен.", ex.Message);
                MessageBox.Show("Возникла ошибка обновления. Лаунчер модпака будет перезапущен.");
                Process.Start("restart.exe", Process.GetCurrentProcess().ProcessName);
                Process.GetCurrentProcess().Kill();
            }
        }

        private void bLauncher_Click(object sender, EventArgs e)
        {
            try
            {
                if (!bwOptimize.IsBusy) { bwOptimize.RunWorkerAsync(); }

                Process.Start(path + "WoTLauncher.exe");

                if (!bwAero.IsBusy) { bwAero.RunWorkerAsync(); }

                Hide();
                WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = true;
            }
            catch (Exception ex)
            {
                debug.Save("private void bLauncher_Click(object sender, EventArgs e)", "Process.Start(path + \"WoTLauncher.exe\");", ex.Message);
            }
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (!bwOptimize.IsBusy) { bwOptimize.RunWorkerAsync(); }

                Process.Start(path + "WorldOfTanks.exe");

                if (!bwAero.IsBusy) { bwAero.RunWorkerAsync(); }

                Hide();
                WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = true;
            }
            catch (Exception ex)
            {
                debug.Save("private void bPlay_Click(object sender, EventArgs e)", "Process.Start(path + \"WorldOfTanks.exe\");", ex.Message);
            }
        }

        private void moveForm()
        {
            try
            {
                this.MouseDown += delegate
                {
                    this.Capture = false;
                    var msg = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                    this.WndProc(ref msg);
                };
            }
            catch (Exception ex)
            {
                debug.Save("private void moveForm()", "", ex.Message);
            }
        }

        private void bUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(sUpdateLink);
            }
            catch (Exception ex)
            {
                debug.Save("private void bUpdate_Click(object sender, EventArgs e)", "Process.Start(sUpdateLink);", ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://ai-rus.com");
            }
            catch (Exception ex)
            {
                debug.Save("private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)", "Process.Start(\"http://ai-rus.com\");", ex.Message);
            }
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
            try
            {
                Process.Start("http://goo.gl/gr6pFl");
            }
            catch (Exception ex)
            {
                debug.Save("private void видеоToolStripMenuItem_Click(object sender, EventArgs e)", "Process.Start(\"http://goo.gl/gr6pFl\");", ex.Message);
            }
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
            try
            {
                Show();
                WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            catch (Exception ex)
            {
                debug.Save("private void toolStripMenuItem4_Click(object sender, EventArgs e)", "WindowState = FormWindowState.Normal;", ex.Message);
            }
        }

        private void bOptimizePC_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "ВНИМАНИЕ!!!" + Environment.NewLine + "При оптимизации ПК на время игры будут завершены некоторые пользовательские приложения." + Environment.NewLine + "Вы хотите продолжить?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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
        }

        private void bwOptimize_DoWork(object sender, DoWorkEventArgs e)
        {
            int myIndex = 0,
                myProgressStatus = 0;

            // Проверяем условие: если процесс оптимизации запущен вручную, или указан в настройках, то:
            try
            {
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
            }
            catch (Exception ex)
            {
                debug.Save("private void bwOptimize_DoWork(object sender, DoWorkEventArgs e)", "if (optimized || autoAero)", ex.Message);
            }

            try
            {
                if (optimized || autoKill)
                {
                    // Завершаем ненужные процессы путем перебора массива имен с условием отсутствия определенных условий
                    Process[] myProcesses = Process.GetProcesses();
                    int processID = Process.GetCurrentProcess().SessionId;

                    // Расчитываем значение прогресс бара
                    maxPercentUpdateStatus += autoKill ? myProcesses.Length * 2 - 2 : myProcesses.Length - 1;

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
                                      "nvxdsync",// NVIDIA
                                      "Skype",
                                      "qip",
                                      "icq",
                                      "fraps"
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


                    if (autoForceKill)
                    {
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
            }
            catch (Exception ex)
            {
                debug.Save("private void bwOptimize_DoWork(object sender, DoWorkEventArgs e)", "", ex.Message);
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
            try
            {
                fSettings fSettings = new fSettings();
                fSettings.ShowDialog();
            }
            catch (Exception ex)
            {
                debug.Save("private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)", "fSettings fSettings = new fSettings();", ex.Message);
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            try
            {
                Show();
                WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            catch (Exception ex)
            {
                debug.Save("private void notifyIcon_Click(object sender, EventArgs e)", "WindowState = FormWindowState.Normal;", ex.Message);
            }
        }

        private void fIndex_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Архивируем папку дебага
            debug.Archive(Application.StartupPath);

            try
            {
                // Раньше проверяли выли ли внесены изменения, сейчас просто запускаем aero...
                if (optimized)
                {
                    psi = new ProcessStartInfo("cmd", @"/c net start uxsms");
                    Process.Start(psi);
                }
            }
            catch (Exception ex)
            {
                debug.Save("private void fIndex_FormClosing(object sender, FormClosingEventArgs e)", "psi = new ProcessStartInfo(\"cmd\", @\"/c net start uxsms\");", ex.Message);
            }
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

        private void saveLog(int index, string param)
        {
            try
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
            catch (Exception ex)
            {
                debug.Save("private void saveLog(int index, string param)", "", ex.Message);
            }
        }

        private void saveLogNotCloseProcess(string param)
        {
            try
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
            catch (Exception ex)
            {
                debug.Save("private void saveLogNotCloseProcess(string param)", "if (!File.Exists(@\"log\not_closed_process.log\"))", ex.Message);
            }
        }

        private void bSettings_Click(object sender, EventArgs e)
        {
            try
            {
                fSettings fSettings = new fSettings();
                if (fSettings.ShowDialog() == DialogResult.OK)
                {
                    loadSettings();
                }
            }
            catch (Exception ex)
            {
                debug.Save("private void bSettings_Click(object sender, EventArgs e)", "fSettings fSettings = new fSettings();", ex.Message);
            }
        }

        private void bwOptimize_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                double i = Convert.ToDouble(e.ProgressPercentage) / Convert.ToDouble(maxPercentUpdateStatus) * 100;
                llUpdateStatus.Text = "Оптимизация завершена на: " + ((int)i).ToString() + "%";
            }
            catch (Exception ex)
            {
                debug.Save("private void bwOptimize_ProgressChanged(object sender, ProgressChangedEventArgs e)", "double i = Convert.ToDouble(e.ProgressPercentage) / Convert.ToDouble(maxPercentUpdateStatus) * 100;", ex.Message);
            }
        }

        private void bwAero_DoWork(object sender, DoWorkEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                debug.Save("private void bwAero_DoWork(object sender, DoWorkEventArgs e)", "", ex.Message);
                showError(ex.Message);
            }
        }

        // Инфа тут:
        //          http://www.cyberforum.ru/windows-forms/thread740428.html

        /* Запрос тут:
         * https://www.google.ru/search?q=%D0%BA%D0%B0%D0%BA+%D0%BF%D1%80%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%BC%D0%BD%D0%BE+%D1%81%D0%BE%D0%B7%D0%B4%D0%B0%D1%82%D1%8C+linklabel+c%23&oq=%D0%BA%D0%B0%D0%BA+%D0%BF%D1%80%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%BC%D0%BD%D0%BE+%D1%81%D0%BE%D0%B7%D0%B4%D0%B0%D1%82%D1%8C+linklabel+c%23&aqs=chrome..69i57.11575j0j7&sourceid=chrome&espv=2&es_sm=93&ie=UTF-8
         * */
        private void label_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start((sender as LinkLabel).Links[0].LinkData.ToString());
            }
            catch (Exception ex)
            {
                debug.Save("private void label_Click(object sender, EventArgs e)", "Process.Start((sender as LinkLabel).Links[0].LinkData.ToString());", ex.Message);
            }
        }

        private void pbNewVersion_Click(object sender, EventArgs e)
        {
            // Если версия актуальна, то перепроверяем ее, иначе сразу открываем окно
            if (pbNewVersion.BackgroundImage == Properties.Resources.good)
            {
                if (!bwUpdater.IsBusy) { bwUpdater.RunWorkerAsync(); }
            }
            else
            {
                fNewVersion.ShowDialog();
            }
        }

        private void bwVideo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // for (int i = 0; i < youtubeTitle.Count; i++)
                // {
                LinkLabel label = new LinkLabel();
                label.SetBounds(190, 426, 100, 20);
                label.AutoSize = true;
                label.ActiveLinkColor = Color.FromArgb(243, 123, 16);
                label.BackColor = Color.Transparent;
                label.ForeColor = Color.FromArgb(243, 123, 16);
                label.VisitedLinkColor = Color.FromArgb(243, 123, 16);
                label.LinkColor = Color.FromArgb(243, 123, 16);
                label.LinkBehavior = LinkBehavior.HoverUnderline;
                label.Font = new System.Drawing.Font("Sochi2014", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                label.Text = "Все видео";
                label.Name = "llVideoAll";
                label.Click += new EventHandler(bVideo_Click);
                this.Controls.Add(label);
                // }


                /*
                 * Теперь двигаем элементы перед выводом новостей
                 
                foreach (Label l in Controls.Cast<Control>().Where(x => x is Label).Select(x => x as Label))
                {
                    if (l.Name.StartsWith("llDateNews")){ l.Location = new Point(l.Location.X, l.Location.Y); }
                }*/
            }
            catch (Exception ex)
            {
                debug.Save("private void bwVideo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)", "Добавление ссылки на все видео", ex.Message);
            }
        }

        private void bwUpdateLauncher_DoWork(object sender, DoWorkEventArgs e)
        {
            var client = new WebClient();
            XmlDocument doc = new XmlDocument();

            try
            {
                // Для работы нам нужна библиотека Ionic.Zip.dll
                //var clientZIP = new WebClient();
                if (!File.Exists("Ionic.Zip.dll"))
                {
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/Ionic.Zip.dll"), "Ionic.Zip.dll");
                }
            }
            catch (Exception ex1)
            {
                debug.Save("private void bwUpdateLauncher_DoWork(object sender, DoWorkEventArgs e)", "if (!File.Exists(\"Ionic.Zip.dll\"))", ex1.Message);
            }


            try
            {
                //XmlDocument doc = new XmlDocument();
                doc.Load(@"http://ai-rus.com/pro/protanks.xml");

                // Process List
                double verProcesses = Convert.ToDouble(doc.GetElementsByTagName("processes")[0].InnerText.Replace(".", ""));
                if (!File.Exists("processes.exe") || getFileVersion("processes.exe") < verProcesses)
                {
                    var client1 = new WebClient();
                    client1.DownloadFile(new Uri(@"http://ai-rus.com/pro/processes.txt"), "processes.exe");
                }

                // Updater
                double verUpdater = Convert.ToDouble(doc.GetElementsByTagName("updater")[0].InnerText.Replace(".", ""));
                if (!File.Exists("updater.exe") || getFileVersion("updater.exe") < verUpdater)
                {
                    var client1 = new WebClient();
                    client1.DownloadFile(new Uri(@"http://ai-rus.com/pro/updater.txt"), "updater.exe");
                }

                // Restarter
                double verRestart = Convert.ToDouble(doc.GetElementsByTagName("restart")[0].InnerText.Replace(".", ""));
                if (!File.Exists("restart.exe") || getFileVersion("restart.exe") < verRestart)
                {
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/restart.txt"), "restart.exe");
                }

                // Версия лаунчера
                double ver = Convert.ToDouble(doc.GetElementsByTagName("version")[0].InnerText.Replace(".", "")),
                    thisVer = Convert.ToDouble(Application.ProductVersion.Replace(".", ""));

                if (thisVer < ver)
                {
                    if (File.Exists("hell-protanks-download")) { File.Delete("hell-protanks-download"); }

                    //var client = new WebClient();
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/launcher.txt"), "hell-protanks-download");

                    //MessageBox.Show(this, "Требуется перезапуск приложения для завершения обновления", "Обновление " + Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Process.Start("updater.exe", "hell-protanks-download \"!Hell PRO Tanki Launcher.exe\"");
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception ex1)
            {
                debug.Save("private void bwUpdateLauncher_DoWork(object sender, DoWorkEventArgs e)", "XmlDocument doc = new XmlDocument();", ex1.Message);
            }
        }

        private double getFileVersion(string filename)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(filename);
            return Convert.ToDouble(fvi.FileVersion.Replace(".", ""));
        }

        private void bwVideo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int i = 0;

                XmlDocument doc = new XmlDocument();
                doc.Load(@"https://gdata.youtube.com/feeds/api/users/" + youtubeChannel + "/uploads");

                youtubeTitle.Clear();
                youtubeLink.Clear();
                youtubeDate.Clear();

                foreach (XmlNode xmlNode in doc.GetElementsByTagName("entry"))
                {
                    if (i >= 11 || showVideoTop > 360) { break; }

                    youtubeDate.Add(xmlNode["published"].InnerText.Remove(10));
                    youtubeTitle.Add((xmlNode["title"].InnerText.IndexOf(" / PRO") >= 0 ? xmlNode["title"].InnerText.Remove(xmlNode["title"].InnerText.IndexOf(" / PRO")) : xmlNode["title"].InnerText));
                    youtubeLink.Add(xmlNode["link"].Attributes["rel"].InnerText == "alternate" ? xmlNode["link"].Attributes["href"].InnerText : "");

                    bwVideo.ReportProgress(i);

                    ++i;
                }
            }
            catch (Exception ex)
            {
                debug.Save("private void bwVideo_DoWork(object sender, DoWorkEventArgs e)", "XmlDocument doc = new XmlDocument();", ex.Message);
            }
        }

        private void bwVideo_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                // Так как начали выводить данные, проверяем существует ли контрол с текстом "ПОдождите, идет загрузка данных..."
                try
                {
                    if (llLoadingVideoData.Text != "")
                    {
                        this.Controls.Remove(llLoadingVideoData);
                    }
                }
                catch { }

                Label labelDate = new Label();
                //labelDate.SetBounds(10, (e.ProgressPercentage + showVideoTop) * topPosition + topOffset, 10, 10);
                labelDate.SetBounds(10, showVideoTop, 10, 10);
                labelDate.AutoSize = true;
                labelDate.BackColor = Color.Transparent;
                labelDate.ForeColor = Color.Silver;
                labelDate.Font = new System.Drawing.Font("Sochi2014", 11f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                labelDate.Text = formatDate(youtubeDate[e.ProgressPercentage]);
                labelDate.Name = "llDateNews" + e.ProgressPercentage.ToString();
                this.Controls.Add(labelDate);

                LinkLabel label = new LinkLabel();
                label.Font = new System.Drawing.Font("Sochi2014", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                label.ActiveLinkColor = Color.FromArgb(243, 123, 16);
                label.BackColor = Color.Transparent;
                label.ForeColor = Color.FromArgb(243, 123, 16);
                label.VisitedLinkColor = Color.FromArgb(243, 123, 16);
                label.LinkColor = Color.FromArgb(243, 123, 16);
                label.LinkBehavior = LinkBehavior.HoverUnderline;
                label.Name = "llNews" + e.ProgressPercentage.ToString();
                label.Text = youtubeTitle[e.ProgressPercentage];
                label.Links[0].LinkData = youtubeLink[e.ProgressPercentage];
                try
                {
                    label.SetBounds(labelDate.Width + 10, showVideoTop, 100, 20);
                }
                catch (Exception) { }
                label.AutoSize = true;
                label.Click += new EventHandler(label_Click);
                this.Controls.Add(label);

                //showVideoTop = 0;

                /*if (label.Width > 250)
                {
                    label.AutoSize = false;
                    label.Size = new Size(250, 40);

                    //label.SetBounds(labelDate.Width + 10, (e.ProgressPercentage + showVideoTop) * topPosition + topOffset, 250, 40);
                    label.SetBounds(labelDate.Width + 10, showVideoTop, 250, 40);
                    labelDate.Location = new Point(10, label.Location.Y);

                    showVideoTop += 50;
                }
                else
                {
                    showVideoTop += 30;
                }*/
                showVideoTop += 30;
            }
            catch (Exception ex)
            {
                debug.Save("private void bwVideo_ProgressChanged(object sender, ProgressChangedEventArgs e)", "Создание динамических полей", ex.Message);
            }
        }

        // Форматируем дату для вывода в список новостей и видео
        private string formatDate(string dt)
        {
            try
            {
                DateTime nd = DateTime.Parse(dt);

                return nd.ToString("dd/MM").Replace(".", "/");
            }
            catch (Exception ex)
            {
                debug.Save("private string formatDate(string dt)", "", ex.Message);
                return "error";
            }
        }

        private void bProcess_Click(object sender, EventArgs e)
        {
            if (File.Exists("processes.exe"))
            {
                Process.Start("processes.exe");
            }
            else
            {
                MessageBox.Show(this, "Исполняемый файл приложения не найден. Попробуйте перезапустить программу для автоматического устранения неисправности.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}