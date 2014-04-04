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
using System.Web;
using System.IO;
using System.Xml;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using Processes_Library;
using _Hell_Language_Pack;
using Newtonsoft.Json;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fIndex : Form
    {
        fLanguage languagePack = new fLanguage();   // ПОдгружаем языковую библиотеку

        string xmlTitle = "",
            path = "",
            sVerType = "full",
            sUpdateNews,
            youtubeChannel = "PROTankiWoT",
            sUpdateLink = "http://goo.gl/gr6pFl",
            videoLink = "http://goo.gl/gr6pFl",
            updateNotification = "";

        Version rVerModpack,
            rVerTanks,
            // local version
            lVerModpack,
            lVerTanks;

        int maxPercentUpdateStatus = 1,
         showVideoTop = 0 /*110*/,
         showNewsTop = 0 /*110*/;

        bool updPack = false,
            updTanks = false,
            optimized = false,

            playGame = false,

            optimizeVideo = false,
            optimizeAffinity = false,

            manualClickUpdate = false,

            autoKill = true,
            autoForceKill = false,
            autoAero = true,
            autoNews = true,
            autoVideo = true;

        List<string> youtubeTitle = new List<string>();
        List<string> youtubeLink = new List<string>();
        List<string> youtubeDate = new List<string>();

        List<string> newsTitle = new List<string>();
        List<string> newsLink = new List<string>();
        List<string> newsDate = new List<string>();

        List<string> processesList = new List<string>();

        ProcessStartInfo psi;

        debug debug = new debug();  // Инициализируем класс дебага

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool ShowWindow(int hWnd, int nCmdShow);

        public fIndex()
        {
            //Проверяем запущен ли процесс
            // Если запущен, то закрываем все предыдущие, оставляя заново открытое окно
            Process[] myProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            for (int i = 1; i < myProcesses.Length; i++) { myProcesses[i].Kill(); }

            /// Получаем Handle уже запущенного приложения
            /// и используем его для разворачивания окна
            /*if (myProcesses.Length > 1)
            {
                for (int i = 0; i < myProcesses.Length; i++)
                {
                    IntPtr hWnd = myProcesses[0].MainWindowHandle;
                    ShowWindow((int)hWnd, 1);
                }
            }*/


            // Проверяем установлен ли в системе нужный нам фраймворк
            //framework framework = new framework();
            //framework.Check();

            InitializeComponent();

            loadSettings();

            try
            {
                this.Text = xmlTitle + " v" + lVerModpack.ToString();
                this.Icon = Properties.Resources.myicon;

                llTitle.Text = xmlTitle + " (" + (sVerType == "full" ? "Расширенная версия" : "Базовая версия") + ")";

                llLauncherVersion.Text = Application.ProductVersion;

                notifyIcon.Icon = Properties.Resources.myicon;
                notifyIcon.Text = xmlTitle + " v" + lVerModpack.ToString();
            }
            catch (Exception ex)
            {
                /// Иногда данная ошибка возникает при некорректном файле настроек. Удаляем его
                if (File.Exists("settings.xml")) { File.Delete("settings.xml"); }


                debug.Save("public fIndex()", "Применение заголовков и иконок приложения", ex.Message);
            }

            // Грузим видео с ютуба
            if (!bwVideo.IsBusy) { bwVideo.RunWorkerAsync(); }

            // Грузим новости
            if (!bwNews.IsBusy) { bwNews.RunWorkerAsync(); }

            // Так как панель у нас убрана с видимой части, устанавливаем ее расположение динамически
            pNews.SetBounds(13, 109, 620, 290);

            //llUpdateStatus.Text = ""; // Убираем текст с метки статуса обновления

            llVersion.Text = lVerModpack.ToString();

            setBackground();

            moveForm();

            if (!bwUpdater.IsBusy) { bwUpdater.RunWorkerAsync(); }
        }

        // Узнаем разряд системы
        private bool isX64()
        {
            return Environment.Is64BitOperatingSystem ? true : false;
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
                    path = Application.StartupPath + @"\..\";

                    var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{1EAC1D02-C6AC-4FA6-9A44-96258C37C812RU}_is1");
                    if (key != null)
                    {
                        if ((string)key.GetValue("InstallLocation") != "")
                        {
                            path = (string)key.GetValue("InstallLocation");
                        }
                    }


                    try
                    {
                        sVerType = doc.GetElementsByTagName("type")[0].InnerText;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            sVerType = new IniFile(Directory.GetCurrentDirectory() + @"\config.ini").IniReadValue("new", "update_file").Replace("update", "").Replace(".xml", "").ToLower();
                            debug.Save("public void loadSettings()", "sVerType = doc.GetElementsByTagName(\"type\")[0].InnerText;", ex.Message);
                        }
                        catch (Exception ex1)
                        {
                            sVerType = "full";
                            debug.Save("public void loadSettings()", "Read from INI", ex1.Message);
                        }
                    }

                    try
                    {
                        updateNotification = doc.GetElementsByTagName("notification")[0].InnerText;
                    }
                    catch (Exception)
                    {
                        updateNotification = "";
                    }


                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("game"))
                    {
                        optimizeVideo = xmlNode.Attributes["video"].InnerText == "False" ? false : true;
                        optimizeAffinity = xmlNode.Attributes["affinity"].InnerText == "False" ? false : true;
                    }

                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("settings"))
                    {
                        autoKill = xmlNode.Attributes["kill"].InnerText == "False" ? false : true;
                        autoAero = xmlNode.Attributes["aero"].InnerText == "False" ? false : true;
                        autoNews = xmlNode.Attributes["news"].InnerText == "False" ? false : true;
                        autoVideo = xmlNode.Attributes["video"].InnerText == "False" ? false : true;
                    }

                    try
                    {
                        lVerModpack = new Version(new IniFile(Directory.GetCurrentDirectory() + @"\config.ini").IniReadValue("new", "version"));
                        //lVerModpack = new Version(doc.GetElementsByTagName("version")[0].InnerText);
                    }
                    catch (Exception ex)
                    {
                        lVerModpack = new Version(doc.GetElementsByTagName("version")[0].InnerText);
                        debug.Save("public void loadSettings()", "IniFile ini = new IniFile(\"config.ini\");", ex.Message);
                    }
                }
                else
                {
                    //MessageBox.Show("Файл настроек не обнаружен!" + Environment.NewLine + "Будут применены настройки по-умолчанию и программа будет перезапущена.");

                    //var client = new WebClient();
                    //client.DownloadFile(new Uri(@"http://ai-rus.com/pro/settings.xml"), "settings.xml");

                    debug.Save("public void loadSettings()", "Файл настроек не обнаружен. Перезапускаем ПО", "");

                    Process.Start("restart.exe", "\"" + Process.GetCurrentProcess().ProcessName + "\"");
                    Process.GetCurrentProcess().Kill();

                    xmlTitle = Application.ProductName;

                    lVerModpack = new Version("0.0.0.0");

                    path = @"\..";
                }
            }
            catch (Exception ex)
            {
                debug.Save("public fIndex()", "public void loadSettings()", ex.Message);
            }
        }


        // Получаем версию танков
        private Version getTanksVersion()
        {
            try
            {
                if (File.Exists(path + "version.xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + "version.xml");

                    return new Version(doc.GetElementsByTagName("version")[0].InnerText.Trim().Remove(0, 2).Replace(" ", "").Replace("#", "."));
                }
                else
                {
                    var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{1EAC1D02-C6AC-4FA6-9A44-96258C37C812RU}_is1");
                    if (key != null)
                    {
                        if ((string)key.GetValue("InstallLocation") != "")
                        {
                            path = (string)key.GetValue("InstallLocation");

                            if (File.Exists(path + "version.xml"))
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(path + "version.xml");

                                return new Version(doc.GetElementsByTagName("version")[0].InnerText.Trim().Remove(0, 2).Replace(" ", "").Replace("#", "."));
                            }
                            else
                            {
                                debug.Save("private int getTanksVersion()", "if (File.Exists(path + \"version.xml\"))", "Клиент игры не обнаружен." + Environment.NewLine + "Проверьте правильность установки модпака.");
                                return new Version("0.0.0.0");
                            }
                        }
                        else
                        {
                            debug.Save("private int getTanksVersion()", "if (File.Exists(path + \"version.xml\"))", "Клиент игры не обнаружен." + Environment.NewLine + "Проверьте правильность установки модпака.");
                            return new Version("0.0.0.0");
                        }
                    }

                    debug.Save("private int getTanksVersion()", "if (File.Exists(path + \"version.xml\"))", "Клиент игры не обнаружен." + Environment.NewLine + "Проверьте правильность установки модпака.");
                    return new Version("0.0.0.0");
                }
            }
            catch (Exception ex)
            {
                debug.Save("private int getTanksVersion()", "doc.Load(path + \"version.xml\");", ex.Message);
                return new Version("0.0.0.0");
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
                // Парсим сайт PROТанки
                XmlDocument doc = new XmlDocument();
                doc.Load(@"http://ai-rus.com/pro/pro.xml");

                rVerModpack = new Version(doc.GetElementsByTagName("version")[0].InnerText);
                rVerTanks = new Version(doc.GetElementsByTagName("tanks")[0].InnerText);

                lVerTanks = getTanksVersion();

                // Отправляем данные на сайт
                // В ответ присваиваем переменной verTanksServer значение с сайта
                if (lVerTanks > rVerTanks)
                {
                    rVerTanks = new Version(getResponse("http://ai-rus.com/wot/micro/" + lVerTanks.ToString()));

                    updTanks = true;
                }
                else
                {
                    updTanks = lVerTanks > new Version("0.0.0.0") && lVerTanks < rVerTanks ? true : false;
                }

                //Проверяем апдейты на модпак
                updPack = lVerModpack < rVerModpack ? true : false;

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
                fNewVersion fNewVersion = new fNewVersion();

                string status = "";

                if (updPack || updTanks)
                {
                    if (updPack)
                    {
                        status += "Обнаружена новая версия Мультипака (" + rVerModpack.ToString() + ")" + Environment.NewLine;
                        bUpdate.Enabled = true;

                        videoLink = sUpdateLink;
                    }

                    if (updTanks)
                    {
                        status += "Обнаружена новая версия клиента игры (" + rVerTanks.ToString() + ")" + Environment.NewLine;
                        bUpdate.Enabled = true;

                        // Отключаем кнопку запуска игры
                        bPlay.Enabled = false;
                    }
                    else
                    {
                        // Включаем кнопку запуска игры
                        bPlay.Enabled = true;
                    }


                    llActually.Text = "Обнаружена новая версия мультипака!";
                    llActually.ForeColor = Color.Yellow;
                    llActually.ActiveLinkColor = Color.Yellow;
                    llActually.LinkColor = Color.Yellow;
                    llActually.VisitedLinkColor = Color.Yellow;
                    llActually.SetBounds(315 + 100 - (int)(llActually.Width / 2), llActually.Location.Y, 10, 10);

                    // Окно статуса обновлений
                    fNewVersion.llCaption.Text = status;

                    fNewVersion.llContent.Text = updPack ? sUpdateNews : "";
                    fNewVersion.llContent.Links[0].LinkData = videoLink;
                    fNewVersion.llVersion.Text = rVerModpack.ToString();
                    if (updateNotification != rVerModpack.ToString() || manualClickUpdate == true)
                    {
                        fNewVersion.cbNotification.Checked = updateNotification == rVerModpack.ToString() ? true : false;
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
                        "Текущая версия Мультипака '" + lVerModpack.ToString() + "'";
                    bUpdate.Enabled = false;

                    // Окно статуса обновлений
                    fNewVersion.llCaption.Text = status;

                    fNewVersion.llContent.Text = "Обновления отсутствуют";
                    fNewVersion.llVersion.Text = lVerModpack.ToString();
                }

                manualClickUpdate = false;

                //this.llContent.Text = status;
                notifyIcon.ShowBalloonTip(2000, xmlTitle, status, ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                /*debug.Save("private void bwUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)", "Возникла ошибка обновления. Лаунчер модпака будет перезапущен.", ex.Message);
                MessageBox.Show("Возникла ошибка обновления. Лаунчер модпака будет перезапущен.");
                Process.Start("restart.exe", Process.GetCurrentProcess().ProcessName);
                Process.GetCurrentProcess().Kill();*/

                debug.Save("private void bwUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)", "", ex.Message);
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
                if (!bwOptimize.IsBusy) { playGame = true; bwOptimize.RunWorkerAsync(); }
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
                manualClickUpdate = true;
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

                    pbDownload.Visible = true;
                    pbDownload.Value = 0;

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
                    maxPercentUpdateStatus += autoForceKill ? myProcesses.Length * 2 - 2 : myProcesses.Length - 1;

                    // устанавливаем имена процессов, завершать которые НЕЛЬЗЯ
                    // Грузим библиотеку со списком процессов
                    ProcessesLibrary proccessLibrary = new ProcessesLibrary();

                    // Передаем процессам инфу, что приложение должно закрыться
                    for (int i = 1; i < myProcesses.Length; i++)
                    {
                        try
                        {
                            // Расчитываем значение прогресс бара
                            bwOptimize.ReportProgress(++myProgressStatus);

                            if (myProcesses[i].SessionId == processID && Array.IndexOf(proccessLibrary.Processes(), myProcesses[i].ProcessName.ToString()) == -1 && processesList.IndexOf(myProcesses[i].ProcessName.ToString()) == -1)
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

                                if (myProcesses[i].SessionId == processID && Array.IndexOf(proccessLibrary.Processes(), myProcesses[i].ProcessName.ToString()) == -1 && processesList.IndexOf(myProcesses[i].ProcessName.ToString()) == -1)
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

                ///
                /// Optimize game graphic
                /// 
                try
                {
                    if (optimizeVideo)
                    {
                        /// http://mirtankov.su/fps-test
                        /// c:\Users\user\AppData\Roaming\Wargaming.net\WorldOfTanks\                        

                        string str = string.Empty;

                        using (StreamReader reader = File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\preferences.xml"))
                        {
                            str = reader.ReadToEnd();
                        }

                        str = str.Replace("<label>	SHADER_VERSION_CAP	</label>" + Environment.NewLine + "			<activeOption>	0	</activeOption>",
                            "<label>	SHADER_VERSION_CAP	</label>" + Environment.NewLine + "			<activeOption>	1	</activeOption>");

                        using (StreamWriter file = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\preferences.xml"))
                        {
                            file.Write(str);
                        }
                    }
                    else
                    {
                        string str = string.Empty;

                        using (StreamReader reader = File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\preferences.xml"))
                        {
                            str = reader.ReadToEnd();
                        }

                        str = str.Replace("<label>	SHADER_VERSION_CAP	</label>" + Environment.NewLine + "			<activeOption>	1	</activeOption>",
                            "<label>	SHADER_VERSION_CAP	</label>" + Environment.NewLine + "			<activeOption>	0	</activeOption>");

                        using (StreamWriter file = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\preferences.xml"))
                        {
                            file.Write(str);
                        }
                    }
                }
                catch (Exception ex1)
                {
                    debug.Save("private void bwOptimize_DoWork(object sender, DoWorkEventArgs e)", "if (optimizeVideo)", ex1.Message);
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
                manualClickUpdate = true;
                bwUpdater.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void saveLog(int index, string param)
        {
            /*  try
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
              }*/
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

                    if (!bwGetVipProcesses.IsBusy) { bwGetVipProcesses.RunWorkerAsync(); }
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
                pbDownload.Value = (int)i;
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

        private void bwVideo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                LinkLabel label = new LinkLabel();
                label.SetBounds(190, 426, 100, 20);
                label.AutoSize = true;
                label.ActiveLinkColor = Color.FromArgb(243, 123, 16);
                label.BackColor = Color.Transparent;
                label.ForeColor = Color.FromArgb(243, 123, 16);
                label.VisitedLinkColor = Color.FromArgb(243, 123, 16);
                label.LinkColor = Color.FromArgb(243, 123, 16);
                label.LinkBehavior = LinkBehavior.HoverUnderline;
                label.Font = new Font("Sochi2014", 12f, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
                label.Text = "Все видео";
                label.Name = "llVideoAll";
                label.Click += new EventHandler(bVideo_Click);
                this.Controls.Add(label);

                bShowVideo.Enabled = true;

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

        private Version getFileVersion(string filename)
        {
            return new Version(FileVersionInfo.GetVersionInfo(filename).FileVersion);
        }

        /// <summary>
        /// Читаем RSS-канал на ютубе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    if (i >= 10 || showVideoTop > 290) { break; }

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
            // Так как начали выводить данные, проверяем существует ли контрол с текстом "ПОдождите, идет загрузка данных..."
            try
            {
                if (llLoadingVideoData.Text != "")
                {
                    this.pVideo.Controls.Remove(llLoadingVideoData);
                }
            }
            catch { }

            try
            {

                Label labelDate = new Label();
                labelDate.SetBounds(10, showVideoTop, 10, 10);
                labelDate.AutoSize = true;
                labelDate.BackColor = Color.Transparent;
                labelDate.ForeColor = Color.Silver;
                labelDate.Font = new Font("Sochi2014", 11f, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
                labelDate.Text = formatDate(youtubeDate[e.ProgressPercentage]);
                labelDate.Name = "llDateVideo" + e.ProgressPercentage.ToString();
                this.pVideo.Controls.Add(labelDate);

                LinkLabel label = new LinkLabel();
                label.Font = new Font("Sochi2014", 12f, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
                label.ActiveLinkColor = Color.FromArgb(243, 123, 16);
                label.BackColor = Color.Transparent;
                label.ForeColor = Color.FromArgb(243, 123, 16);
                label.VisitedLinkColor = Color.FromArgb(243, 123, 16);
                label.LinkColor = Color.FromArgb(243, 123, 16);
                label.LinkBehavior = LinkBehavior.HoverUnderline;
                label.Name = "llVideo" + e.ProgressPercentage.ToString();
                label.Text = youtubeTitle[e.ProgressPercentage];
                label.Links[0].LinkData = youtubeLink[e.ProgressPercentage];
                try
                {
                    label.SetBounds(labelDate.Width + 10, showVideoTop, 100, 20);
                }
                catch (Exception) { }
                label.AutoSize = true;
                label.Click += new EventHandler(label_Click);
                this.pVideo.Controls.Add(label);

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

        /*private void download_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                pbDownload.Value = e.ProgressPercentage;
            }
            catch (Exception ex)
            {
                debug.Save("private void download_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)", "pbDownload.Value = e.ProgressPercentage;", ex.Message);
            }
        }

        private void download_Completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show(this, "Обнаружена новая версия лаунчера (" + rVerLauncher.ToString() + ")" + Environment.NewLine +
                    "Применить обновление сейчас?", Application.ProductName + " v" + Application.ProductVersion, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    //if (checksum("launcher.update", checksummLauncher)) { MessageBox.Show("Checksumm: OK"); }

                    Process.Start("updater.exe", "launcher.update \""+Process.GetCurrentProcess().ProcessName+".exe\"");
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception ex)
            {
                debug.Save("private void download_Completed(object sender, AsyncCompletedEventArgs e)", "Process.Start(\"updater.exe\", \"launcher.update \"\"\" + Process.GetCurrentProcess().ProcessName + \"\"\");", ex.Message);
            }
        }*/

        private void changeContent(bool video = true)
        {
            try
            {
                if (video)
                {
                    if (llBlockCaption.Text != "Видео:") // Исключаем повторное нажатие
                    {
                        bShowVideo.BackColor = Color.Black;
                        bShowVideo.FlatAppearance.BorderColor = Color.FromArgb(155, 55, 0);

                        bShowNews.BackColor = Color.FromArgb(28, 28, 28);
                        bShowNews.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 63);

                        llBlockCaption.Text = "Видео:";

                        pNews.Visible = false;
                        pVideo.Visible = true;
                    }
                }
                else
                {
                    if (llBlockCaption.Text != "Новости:") // Исключаем повторное нажатие
                    {
                        bShowVideo.BackColor = Color.FromArgb(28, 28, 28);
                        bShowVideo.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 63);

                        bShowNews.BackColor = Color.Black;
                        bShowNews.FlatAppearance.BorderColor = Color.FromArgb(155, 55, 0);

                        llBlockCaption.Text = "Новости:";

                        pVideo.Visible = false;
                        pNews.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.Save("private void changeContent(bool video = true)", "", ex.Message);
            }
        }

        private void bShowVideo_Click(object sender, EventArgs e)
        {
            changeContent();
        }

        private void bShowNews_Click(object sender, EventArgs e)
        {
            changeContent(false);
        }

        private void bwNews_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int i = 0;

                XmlDocument doc = new XmlDocument();
                doc.Load(@"http://worldoftanks.ru/ru/rss/news/");

                newsTitle.Clear();
                newsLink.Clear();
                newsDate.Clear();

                foreach (XmlNode xmlNode in doc.GetElementsByTagName("item"))
                {
                    if (i >= 10 || showNewsTop > 290) { break; }

                    newsDate.Add(xmlNode["pubDate"].InnerText);
                    newsTitle.Add(xmlNode["title"].InnerText);
                    newsLink.Add(xmlNode["link"].InnerText);

                    bwNews.ReportProgress(i);

                    ++i;
                }
            }
            catch (Exception ex)
            {
                debug.Save("private void bwVideo_DoWork(object sender, DoWorkEventArgs e)", "XmlDocument doc = new XmlDocument();", ex.Message);
            }
        }

        private void bwNews_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                Label labelNewsDate = new Label();
                labelNewsDate.SetBounds(10, showNewsTop, 10, 10);
                labelNewsDate.AutoSize = true;
                labelNewsDate.BackColor = Color.Transparent;
                labelNewsDate.ForeColor = Color.Silver;
                labelNewsDate.Font = new Font("Sochi2014", 11f, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
                labelNewsDate.Text = formatDate(newsDate[e.ProgressPercentage]);
                labelNewsDate.Name = "llDateNews" + e.ProgressPercentage.ToString();
                this.pNews.Controls.Add(labelNewsDate);

                LinkLabel labelNews = new LinkLabel();
                labelNews.Font = new Font("Sochi2014", 12f, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
                labelNews.ActiveLinkColor = Color.FromArgb(243, 123, 16);
                labelNews.BackColor = Color.Transparent;
                labelNews.ForeColor = Color.FromArgb(243, 123, 16);
                labelNews.VisitedLinkColor = Color.FromArgb(243, 123, 16);
                labelNews.LinkColor = Color.FromArgb(243, 123, 16);
                labelNews.LinkBehavior = LinkBehavior.HoverUnderline;
                labelNews.Name = "llNews" + e.ProgressPercentage.ToString();
                labelNews.Text = newsTitle[e.ProgressPercentage];
                labelNews.Links[0].LinkData = newsLink[e.ProgressPercentage];
                try
                {
                    labelNews.SetBounds(labelNewsDate.Width + 10, showNewsTop, 100, 20);
                }
                catch (Exception) { }
                labelNews.AutoSize = true;
                labelNews.Click += new EventHandler(label_Click);
                this.pNews.Controls.Add(labelNews);

                showNewsTop += 30;
            }
            catch (Exception ex)
            {
                debug.Save("private void bwVideo_ProgressChanged(object sender, ProgressChangedEventArgs e)", "Создание динамических полей", ex.Message);
            }
        }

        private void bwNews_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                /*LinkLabel label = new LinkLabel();
                label.SetBounds(190, 426, 100, 20);
                label.AutoSize = true;
                label.ActiveLinkColor = Color.FromArgb(243, 123, 16);
                label.BackColor = Color.Transparent;
                label.ForeColor = Color.FromArgb(243, 123, 16);
                label.VisitedLinkColor = Color.FromArgb(243, 123, 16);
                label.LinkColor = Color.FromArgb(243, 123, 16);
                label.LinkBehavior = LinkBehavior.HoverUnderline;
                label.Font = new Font("Sochi2014", 12f, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
                label.Text = "Все новости";
                label.Name = "llNewsAll";
                label.Click += new EventHandler(bVideo_Click);
                this.Controls.Add(label);*/

                //pNews.Visible = true;
                bShowNews.Enabled = true;
            }
            catch (Exception ex)
            {
                debug.Save("private void bwVideo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)", "Добавление ссылки на все видео", ex.Message);
            }
        }

        private void llActually_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!bwUpdater.IsBusy)
            {
                manualClickUpdate = true;
                bwUpdater.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void fIndex_Load(object sender, EventArgs e)
        {
            // Главное окно
            languagePack.toolTip(bOptimizePC);

            if (!bwGetVipProcesses.IsBusy) { bwGetVipProcesses.RunWorkerAsync(); }

            //update_launcher update = new update_launcher();
            //update.CheckUpdates();

            //debug.Send(); // Если имеются какие-либо файлы дебага, то отправляем их на сайт
        }

        private void bwGetVipProcesses_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string tmp = "";

                if (File.Exists("settings.xml"))
                {
                    processesList.Clear();

                    XmlDocument doc = new XmlDocument();
                    doc.Load("settings.xml");


                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("process"))
                    {
                        tmp = xmlNode.Attributes["name"].InnerText;

                        processesList.Add(tmp.Remove(tmp.IndexOf("     (")));
                    }
                }
            }
            catch (Exception ex)
            {
                debug.Save("private void bwGetVipProcesses_DoWork(object sender, DoWorkEventArgs e)", "", ex.Message);
            }
        }

        private void bwOptimize_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (playGame)
            {
                //Process.Start(path + "WorldOfTanks.exe");
                if (File.Exists(@"..\s.bat")) { File.Delete(@"..\s.bat"); }
                File.WriteAllBytes(@"..\s.bat", Properties.Resources.start);
                Process.Start(@"..\s.bat");

                // Устанавливаем соответствие процессов
                if (optimizeAffinity)
                {
                    Process[] myProcesses = Process.GetProcessesByName("WorldOfTanks");
                    for (int i = 1; i < myProcesses.Length; i++) { myProcesses[i].ProcessorAffinity = (IntPtr)2; }
                }
            }
        }
    }
}
