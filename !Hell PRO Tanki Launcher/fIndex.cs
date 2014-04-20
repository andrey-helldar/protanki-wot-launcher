using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using Processes_Library;
using _Hell_Language_Pack;
using Newtonsoft.Json;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fIndex : Form
    {
        fLanguage languagePack = new fLanguage();
        YoutubeVideo YoutubeVideo = new YoutubeVideo();
        SendPOST SendPOST = new SendPOST();
        Debug Debug = new Debug();
        ProcessList ProcessList = new ProcessList();

        string pathToTanks = "",

            modpackType = "base",
            modpackDate = "1970-1-1",

            youtubeChannel = "PROTankiWoT",
            newVersionMessage,
            newVersionLink = "http://goo.gl/gr6pFl",
            videoLink = "http://goo.gl/gr6pFl",
            updateNotification = "",

            notifyLink = "",

            lang = "ru";

        Version remoteModVersion = new Version("0.0.0.0"),
            remoteTanksVersion = new Version("0.0.0.0"),
            modpackVersion = new Version("0.0.0.0"),
            tanksVersion = new Version("0.0.0.0");

        int maxPercentUpdateStatus = 1,
         showVideoTop = 0 /*110*/,
         showNewsTop = 0 /*110*/;

        bool modpackUpdates = false,
            tanksUpdates = false,
            autoOptimizePC = false,

            showVideoNotify = true,

            playGame = false,

            commonTest = false,

            manualClickUpdate = false,

            autoKill = false,
            autoForceKill = false,
            autoAero = false,
            autoVideo = false,
            autoWeak = false,
            autoCPU = true;

        List<string> newsTitle = new List<string>();
        List<string> newsLink = new List<string>();
        List<string> newsDate = new List<string>();

        ProcessStartInfo psi;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool ShowWindow(int hWnd, int nCmdShow);

        public fIndex()
        {
            //Проверяем запущен ли процесс
            foreach (Process process in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
            {
                if (process.SessionId != Process.GetCurrentProcess().SessionId) process.Kill();
            }

            InitializeComponent();
        }

        // Узнаем разряд системы
        private bool isX64() { return Environment.Is64BitOperatingSystem; }

        /// <summary>
        //// Сперва загружаем настройки из файла "config.ini" - конфиг модпака
        //// Затем загружаем настройки самой программы - "settings.xml"
        /// </summary>
        /// <returns>На возврат ничего нет, функция без возврата</returns>
        public async Task loadSettings()
        {
            try
            {
                if (File.Exists("config.ini"))
                {
                    string pathINI = Directory.GetCurrentDirectory() + @"\config.ini";

                    try
                    {
                        modpackDate = new IniFile(pathINI).IniReadValue("new", "date");
                        modpackType = new IniFile(pathINI).IniReadValue("new", "update_file").Replace("update", "").Replace(".xml", "").ToLower();
                        modpackVersion = new Version(new IniFile(pathINI).IniReadValue("new", "version"));
                        lang = new IniFile(pathINI).IniReadValue("new", "languages");
                    }
                    catch (Exception ex)
                    {
                        Debug.Save("loadSettings()", "Read from config.ini", ex.Message);
                    }
                }
                else
                {
                    Debug.Save("loadSettings()", "File not found \"config.ini\"");
                    MessageBox.Show(this, "Модпак не обнаружен!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Загружаем настройки
                if (File.Exists("settings.xml"))
                {
                    XDocument doc = XDocument.Load("settings.xml");

                    updateNotification = doc.Root.Element("notification") != null ? doc.Root.Element("notification").Value : "";

                    showVideoNotify = doc.Root.Element("info") != null ? (doc.Root.Element("info").Attribute("video") != null ? (doc.Root.Element("info").Attribute("video").Value == "True") : false) : false;

                    if (doc.Root.Element("settings") != null)
                    {
                        autoForceKill = doc.Root.Element("settings").Attribute("force").Value == "True";
                        autoKill = doc.Root.Element("settings").Attribute("kill").Value == "True";
                        autoAero = doc.Root.Element("settings").Attribute("aero").Value == "True";
                        autoVideo = ReadCheckStateBool(doc, "video");
                        autoWeak = doc.Root.Element("settings").Attribute("weak").Value == "True";
                        autoCPU = doc.Root.Element("settings").Attribute("balance").Value == "True";
                    }

                    if (doc.Root.Element("common.test") != null) commonTest = true;
                }
                else
                {
                    MessageBox.Show(this, "Файл настроек не обнаружен!" + Environment.NewLine + "Программа будет автоматически перезапущена. Во время перезапуска будет применен стандартная конфигурация", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Process.Start("restart.exe", "\"" + Process.GetCurrentProcess().ProcessName + "\"");
                    Process.GetCurrentProcess().CloseMainWindow();
                }
            }
            catch (Exception ex)
            {
                Debug.Save("loadSettings()", ex.Message);
            }
        }

        private bool ReadCheckStateBool(XDocument doc, string attr)
        {
            if (doc.Root.Element("settings") != null)
                if (doc.Root.Element("settings").Attribute(attr) != null)
                {
                    switch (doc.Root.Element("settings").Attribute(attr).Value)
                    {
                        case "Checked": return true;
                        case "Indeterminate": return true;
                        default: return false;
                    }
                }
            return false;
        }

        private string GetTanksRegistry()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{1EAC1D02-C6AC-4FA6-9A44-96258C37C812RU}_is1");
            return key != null ? (string)key.GetValue("InstallLocation") : null;
        }

        // Получаем версию танков
        private async Task<Version> GetTanksVersion()
        {
            try
            {
                pathToTanks = File.Exists(@"..\version.xml") ? CorrectPath(Application.StartupPath, -1) : GetTanksRegistry();

                if (pathToTanks != null && File.Exists(pathToTanks + "version.xml"))
                {
                    XDocument doc = XDocument.Load(pathToTanks + "version.xml");

                    if (doc.Root.Element("version").Value.IndexOf("Test") > 0)
                    {
                        commonTest = true;
                        return new Version(doc.Root.Element("version").Value.Trim().Remove(0, 2).Replace(" Common Test #", "."));
                    }
                    else
                    {
                        return new Version(doc.Root.Element("version").Value.Trim().Remove(0, 2).Replace(" ", "").Replace("#", "."));
                    }
                }
                else
                {
                    Debug.Save("tanksVersion()", "Клиент игры не обнаружен в реестре.");
                    //bPlay.Enabled = false;
                    bLauncher.Enabled = false;
                    MessageBox.Show(this, "Клиент игры не обнаружен!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return new Version("0.0.0.0");
                }
            }
            catch (Exception ex)
            {
                Debug.Save("tanksVersion()", "doc.Load(\"" + pathToTanks + "version.xml\");", ex.Message);
                //bPlay.Enabled = false;
                bLauncher.Enabled = false;
                MessageBox.Show(this, "Клиент игры не обнаружен!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return new Version("0.0.0.0");
            }
        }

        private string CorrectPath(string sourcePath, int remove = 0)
        {
            string[] temp = sourcePath.Split('\\');
            string newPath = "";

            for (int i = 0; i < temp.Length + remove; i++)
                newPath += temp[i] + @"\";

            return newPath;
        }

        // Выбираем изображение для установки фона
        public void setBackground()
        {
            try
            {
                switch ("back_" + new Random().Next(1, 7))
                {
                    case "back_1": this.BackgroundImage = Properties.Resources.back_1; break;
                    case "back_2": this.BackgroundImage = Properties.Resources.back_2; break;
                    //case "back_3": this.BackgroundImage = Properties.Resources.back_3; break;
                    case "back_4": this.BackgroundImage = Properties.Resources.back_4; break;
                    case "back_5": this.BackgroundImage = Properties.Resources.back_5; break;
                    case "back_6": this.BackgroundImage = Properties.Resources.back_6; break;
                    default: this.BackgroundImage = Properties.Resources.back_7; break;
                }
            }
            catch (Exception ex)
            {
                this.BackgroundImage = Properties.Resources.back_7;
                Debug.Save("public void setBackground()", ex.Message);
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            psi = new ProcessStartInfo("cmd", @"/c net start uxsms");
            Process.Start(psi);

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
                Debug.Save("private void bVideo_Click()", ex.Message);
            }
        }

        private void bwUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                XDocument docSettings = XDocument.Load("settings.xml");
                if (commonTest)
                    if (docSettings.Root.Element("common.test") == null) docSettings.Root.Add(new XElement("common.test", null));
                    else
                        if (docSettings.Root.Element("common.test") != null) docSettings.Root.Element("common.test").Remove();

                docSettings.Save("settings.xml");

                XDocument doc = XDocument.Load(@"http://ai-rus.com/pro/pro.xml");

                remoteModVersion = new Version(doc.Root.Element("version").Value);
                remoteTanksVersion = new Version(SendPOST.Send("http://ai-rus.com/wot/version/", "code=" + Debug.code + "&user=" + Debug.UserID() + "&version=" + tanksVersion.ToString() + "&test=" + (commonTest ? "1" : "0")));
                                
                tanksUpdates = tanksVersion < remoteTanksVersion; // Сравниваем версии танков
                modpackUpdates = modpackVersion < remoteModVersion; // Сравниваем версии мультипака

                if (modpackUpdates)
                {
                    newVersionMessage = doc.Root.Element(modpackType).Element("message").Value.Replace(":;", Environment.NewLine);
                    newVersionLink = doc.Root.Element(modpackType).Element("download").Value;
                }
            }
            catch (Exception ex)
            {
                Debug.Save("bwUpdater_DoWork()", ex.Message);
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
                    if (count != 0) { sb.Append(Encoding.Default.GetString(buf, 0, count)); }
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

                string status = String.Empty;

                if (modpackUpdates || tanksUpdates)
                {
                    if (modpackUpdates)
                    {
                        //status += "Обнаружена новая версия Мультипака (" + remoteModVersion.ToString() + ")" + Environment.NewLine;
                        status += languagePack.DynamicLanguage("llActuallyNewMods", lang) + "(" + remoteModVersion.ToString() + ")" + Environment.NewLine;
                        bUpdate.Enabled = true;

                        videoLink = newVersionLink;
                    }

                    if (tanksUpdates)
                    {
                        //status += "Обнаружена новая версия клиента игры (" + remoteTanksVersion.ToString() + ")" + Environment.NewLine;
                        status += languagePack.DynamicLanguage("llActuallyNewGame", lang) + "(" + remoteTanksVersion.ToString() + ")" + Environment.NewLine;

                        // Отключаем кнопку запуска игры
                        //bPlay.Enabled = false;
                    }
                    else
                    {
                        // Включаем кнопку запуска игры
                        //bPlay.Enabled = true;
                    }


                    //llActually.Text = modpackUpdates ? "Обнаружена новая версия мультипака!" : "Обнаружена новая версия игры!";
                    llActually.Text = modpackUpdates ?
                        languagePack.DynamicLanguage("llActuallyNewMods", lang) + "!" :
                        languagePack.DynamicLanguage("llActuallyNewGame", lang)+"!";
                    llActually.ForeColor = Color.Yellow;
                    llActually.ActiveLinkColor = Color.Yellow;
                    llActually.LinkColor = Color.Yellow;
                    llActually.VisitedLinkColor = Color.Yellow;
                    llActually.SetBounds(315 + 100 - (int)(llActually.Width / 2), llActually.Location.Y, 10, 10);

                    // Окно статуса обновлений
                    // отображаем если найдены обновы модпака
                    fNewVersion.llCaption.Text = status;

                    fNewVersion.llContent.Text = modpackUpdates ? newVersionMessage : "";
                    fNewVersion.llContent.Links[0].LinkData = videoLink;
                    fNewVersion.llVersion.Text = remoteModVersion.ToString();
                    if (modpackUpdates && (updateNotification != remoteModVersion.ToString() || manualClickUpdate == true))
                    {
                        fNewVersion.cbNotification.Checked = updateNotification == remoteModVersion.ToString();
                        fNewVersion.ShowDialog();
                    }
                }
                else
                {
                    //llActually.Text = "Вы используете самые свежие моды!";
                    llActually.Text = languagePack.DynamicLanguage("llActuallyActually", lang) + "!";
                    llActually.ForeColor = Color.Lime;
                    llActually.ActiveLinkColor = Color.Lime;
                    llActually.LinkColor = Color.Lime;
                    llActually.VisitedLinkColor = Color.Lime;

                    /*status = "Вы используете самые свежие моды." + Environment.NewLine +
                        "Текущая версия Мультипака: " + modpackVersion.ToString() + Environment.NewLine +
                        "Текущая версия клиента игры: " + tanksVersion.ToString();*/
                    status = languagePack.DynamicLanguage("llActuallyActually", lang) + Environment.NewLine +
                        languagePack.DynamicLanguage("llActuallyThisVerMods", lang) + modpackVersion.ToString() + Environment.NewLine +
                        languagePack.DynamicLanguage("llActuallyThisVerGame", lang) + tanksVersion.ToString();
                    bUpdate.Enabled = false;

                    // Окно статуса обновлений
                    fNewVersion.llCaption.Text = status;

                    //fNewVersion.llContent.Text = "Обновления отсутствуют";
                    fNewVersion.llContent.Text = languagePack.DynamicLanguage("noUpdates", lang);
                    fNewVersion.llVersion.Text = modpackVersion.ToString();
                }

                manualClickUpdate = false;
                notifyLink = null;
                if (status != String.Empty) notifyIcon.ShowBalloonTip(2000, Application.ProductName, status, ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                Debug.Save("private void bwUpdater_RunWorkerCompleted()", ex.Message);
            }
        }

        private void bLauncher_Click(object sender, EventArgs e)
        {
            try
            {
                autoOptimizePC = false;

                GetVipProcesses().Wait();
                OptimizePC().Wait();

                Process.Start(pathToTanks + "WoTLauncher.exe");

                //Hide();
                WindowState = FormWindowState.Minimized;
            }
            catch (Exception ex) { Debug.Save("bLauncher_Click()", ex.Message); }
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            try
            {
                autoOptimizePC = false;
                GetVipProcesses().Wait();

                if (!tanksUpdates)
                {
                        playGame = true;
                        OptimizePC().Wait();
                }
                else
                {
                    OptimizePC().Wait();
                    Process.Start(pathToTanks + "WoTLauncher.exe");
                }

                //Hide();
                WindowState = FormWindowState.Minimized;
            }
            catch (Exception ex) { Debug.Save("bPlay_Click()", ex.Message); }
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
                Debug.Save("private void moveForm()", ex.Message);
            }
        }

        private void bUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(newVersionLink);
            }
            catch (Exception ex)
            {
                Debug.Save("private void bUpdate_Click()", "newVersionLink = " + newVersionLink, ex.Message);
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
                Debug.Save("private void linkLabel1_LinkClicked()", "Process.Start(\"http://ai-rus.com\");", ex.Message);
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
                Debug.Save("private void видеоToolStripMenuItem_Click()", "Process.Start(\"http://goo.gl/gr6pFl\");", ex.Message);
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
                Debug.Save("private void toolStripMenuItem4_Click()", "WindowState = FormWindowState.Normal;", ex.Message);
            }
        }

        private void bOptimizePC_Click(object sender, EventArgs e)
        {
            string addMess = autoVideo ? Environment.NewLine + Environment.NewLine + "Также, после применения настроек графики в игре требуется заново ввести логин/пароль!" : "";

            if (DialogResult.Yes == MessageBox.Show(this, "ВНИМАНИЕ!!!" + Environment.NewLine + "При оптимизации ПК на время игры будут завершены некоторые пользовательские приложения." + addMess +
                Environment.NewLine + Environment.NewLine + "Вы хотите продолжить?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                if (!bwOptimize.IsBusy)
                {
                    autoOptimizePC = true;

                    GetVipProcesses().Wait();

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
            int myProgressStatus = 0;

            try
            {
                if (autoOptimizePC || autoAero)
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
                                    Process.Start(new ProcessStartInfo("cmd", @"/c net stop uxsms")); // останавливаем aero
                                    break;

                                case 7:
                                    // Win8
                                    Process.Start(new ProcessStartInfo("cmd", @"/c net stop uxsms")); // останавливаем aero
                                    break;

                                default:
                                    if (osInfo.Version.Major == 5 && osInfo.Version.Minor != 0)
                                    {
                                        // WinXP
                                    }
                                    break;
                            }
                            break;
                    }

                    maxPercentUpdateStatus = 1;
                    bwOptimize.ReportProgress(++myProgressStatus);
                }
            }
            catch (Exception ex) { Debug.Save("bwOptimize_DoWork()", "if (autoOptimizePC || autoAero)", ex.Message); }

            try
            {
                if (autoOptimizePC || autoKill)
                {
                    // Завершаем ненужные процессы путем перебора массива имен с условием отсутствия определенных условий
                    int processCount = Process.GetProcesses().Length;
                    int sessionId = Process.GetCurrentProcess().SessionId;

                    maxPercentUpdateStatus += autoForceKill ? processCount * 2 - 2 : processCount - 1; // Расчитываем значение прогресс бара                    
                    ProcessesLibrary proccessLibrary = new ProcessesLibrary();  // получаем имена процессов, завершать которые НЕЛЬЗЯ

                    bool kill = false;

                    for (int i = 0; i < 2; i++)
                    {
                        foreach (var process in Process.GetProcesses())
                        {
                            try
                            {
                                bwOptimize.ReportProgress(++myProgressStatus); // Инкрименируем значение прогресс бара

                                if (process.SessionId == sessionId &&
                                    Array.IndexOf(proccessLibrary.Processes(), process.ProcessName) == -1 &&
                                    !ProcessList.IndexOf(process.ProcessName))
                                {
                                    if (!kill) process.CloseMainWindow(); else process.Kill();
                                }
                            }
                            catch (Exception ex) { Debug.Save("bwOptimize_DoWork()", "if (autoOptimizePC || autoKill)", process.ProcessName.ToString(), "Kill: " + kill.ToString(), ex.Message); }
                        }

                        if (!autoForceKill || !autoOptimizePC) { break; }
                        else
                        {
                            kill = true;
                            Thread.Sleep(5000); // Ждем 5 секунд завершения, пока приложения нормально завершатся, затем повторяем цикл
                        }
                    }
                }

                ///
                /// Optimize game graphic
                /// 
                try
                {
                    if (autoOptimizePC)
                    {
                        OptimizeGraphic OptimizeGraphic = new OptimizeGraphic();
                        Task.Factory.StartNew(() => OptimizeGraphic.Optimize(commonTest, autoVideo, autoWeak)).Wait();
                    }
                }
                catch (Exception ex1)
                {
                    Debug.Save("bwOptimize_DoWork()", "if (autoOptimizePC || autoVideo)", "commonTest = " + commonTest.ToString(), ex1.Message);
                }
            }
            catch (Exception ex)
            {
                Debug.Save("bwOptimize_DoWork)", ex.Message);
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
                Debug.Save("private void настройкиToolStripMenuItem_Click()", "fSettings fSettings = new fSettings();", ex.Message);
            }
        }

        private void fIndex_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Архивируем папку дебага
            Debug.Archive(Application.StartupPath);

            // Отключаем иконку в трее
            notifyIcon.Dispose();

            try
            {
                // Раньше проверяли выли ли внесены изменения, сейчас просто запускаем aero...
                if (autoOptimizePC)
                {
                    psi = new ProcessStartInfo("cmd", @"/c net start uxsms");
                    Process.Start(psi);
                }
            }
            catch (Exception ex)
            {
                Debug.Save("private void fIndex_FormClosing()", "psi = new ProcessStartInfo(\"cmd\", @\"/c net start uxsms\");", ex.Message);
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

        private void bSettings_Click(object sender, EventArgs e)
        {
            fSettings fSettings = new fSettings();
            if (fSettings.ShowDialog() == DialogResult.OK)
            {
                loadSettings();

                GetVipProcesses();
            }
        }

        private void bwOptimize_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                double i = Convert.ToDouble(e.ProgressPercentage) / Convert.ToDouble(maxPercentUpdateStatus) * 100;
                pbDownload.Value = (int)i <= pbDownload.Value ? (int)i : pbDownload.Value;
            }
            catch (Exception ex)
            {
                Debug.Save("private void bwOptimize_ProgressChanged(object sender, ProgressChangedEventArgs e)", "double i = Convert.ToDouble(e.ProgressPercentage) / Convert.ToDouble(maxPercentUpdateStatus) * 100;", ex.Message);
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
                Debug.Save("private void label_Click()", "Process.Start((sender as LinkLabel).Links[0].LinkData.ToString());", ex.Message);
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
                //label.Text = "Все видео";
                label.Text = "test";
                label.Name = "llVideoAll";
                label.Click += new EventHandler(bVideo_Click);
                this.Controls.Add(label);
                label.Text = languagePack.DynamicLanguage("llVideoAll", lang);

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
                Debug.Save("private void bwVideo_RunWorkerCompleted()", "Добавление ссылки на все видео", ex.Message);
            }

            // Запускаем функцию уведомлений о новых видео
            if (showVideoNotify) Task.Factory.StartNew(() => ShowVideoNotification());
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
                int i = -1;
                XDocument doc = XDocument.Load(@"https://gdata.youtube.com/feeds/api/users/" + youtubeChannel + "/uploads");
                XNamespace ns = "http://www.w3.org/2005/Atom";

                // Загружаем новости на форму
                foreach (XElement el in doc.Root.Elements(ns + "entry"))
                {
                    string link = "";
                    foreach (XElement subEl in el.Elements(ns + "link")) { if (subEl.Attribute("rel").Value == "alternate") { link = subEl.Attribute("href").Value; break; } }

                    YoutubeVideo.Add(
                        el.Element(ns + "id").Value.Remove(0, 42),
                        (el.Element(ns + "title").Value.IndexOf(" / PRO") >= 0 ? el.Element(ns + "title").Value.Remove(el.Element(ns + "title").Value.IndexOf(" / PRO")) : el.Element(ns + "title").Value),
                        el.Element(ns + "content").Value.Remove(256) + (el.Element(ns + "content").Value.Length > 256 ? "..." : ""),
                        link,
                        el.Element(ns + "published").Value.Remove(10)
                    );

                    bwVideo.ReportProgress(++i);
                }

                if (YoutubeVideo.Count() == 0) { bwVideo.ReportProgress(-1); }
            }
            catch (Exception ex)
            {
                Debug.Save("private void bwVideo_DoWork()", ex.Message);
            }
        }

        private void bwVideo_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int maxInForm = 10;

            if (e.ProgressPercentage > 0 && (e.ProgressPercentage < maxInForm || showVideoTop < 290))
            {
                // Так как начали выводить данные, проверяем существует ли контрол с текстом "ПОдождите, идет загрузка данных..."
                try { if (llLoadingVideoData.Text != "") { this.pVideo.Controls.Remove(llLoadingVideoData); } }
                catch { }

                try
                {
                    Label labelDate = new Label();
                    labelDate.SetBounds(10, showVideoTop, 10, 10);
                    labelDate.AutoSize = true;
                    labelDate.BackColor = Color.Transparent;
                    labelDate.ForeColor = Color.Silver;
                    labelDate.Font = new Font("Sochi2014", 11f, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
                    labelDate.Text = formatDate(YoutubeVideo.List[e.ProgressPercentage].Date);
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
                    label.Text = YoutubeVideo.List[e.ProgressPercentage].Title;
                    label.Links[0].LinkData = YoutubeVideo.List[e.ProgressPercentage].Link;
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
                    Debug.Save("private void bwVideo_ProgressChanged()", "Создание динамических полей", ex.Message);
                }
            }
            else
            {
                llLoadingVideoData.Text = "Видео не обнаружено...";
            }
        }

        // Форматируем дату для вывода в список новостей и видео
        private string formatDate(string dt)
        {
            try
            {
                if (dt != null)
                {
                    DateTime nd = DateTime.Parse(dt);
                    return nd.ToString("dd/MM").Replace(".", "/");
                }
                return "null";
            }
            catch (Exception ex)
            {
                Debug.Save("private string formatDate(string dt)", dt, ex.Message);
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
                Debug.Save("private void download_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)", "pbDownload.Value = e.ProgressPercentage;", ex.Message);
            }
        }

        private void download_Completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show(this, "Обнаружена новая версия лаунчера (" + rVerLauncher.ToString() + ")" + Environment.NewLine +
                    "Применить обновление сейчас?", Application.ProductName + " v" + Application.ProductVersion, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    //if (checksum("launcher.update", checksumLauncher)) { MessageBox.Show("checksum: OK"); }

                    Process.Start("restart.exe", "launcher.update \""+Process.GetCurrentProcess().ProcessName+".exe\"");
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception ex)
            {
                Debug.Save("private void download_Completed(object sender, AsyncCompletedEventArgs e)", "Process.Start(\"restart.exe\", \"launcher.update \"\"\" + Process.GetCurrentProcess().ProcessName + \"\"\");", ex.Message);
            }
        }*/

        private void changeContent(bool video = true)
        {
            try
            {
                if (video)
                {
                    if (llBlockCaption.Text != "Видео:" && llBlockCaption.Text != "Video:") // Исключаем повторное нажатие
                    {
                        bShowVideo.BackColor = Color.Black;
                        bShowVideo.FlatAppearance.BorderColor = Color.FromArgb(155, 55, 0);

                        bShowNews.BackColor = Color.FromArgb(28, 28, 28);
                        bShowNews.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 63);

                        llBlockCaption.Text = lang == "ru" ? "Видео:" : "Video:";

                        pNews.Visible = false;
                        pVideo.Visible = true;
                    }
                }
                else
                {
                    if (llBlockCaption.Text != "Новости:" && llBlockCaption.Text != "News:") // Исключаем повторное нажатие
                    {
                        bShowVideo.BackColor = Color.FromArgb(28, 28, 28);
                        bShowVideo.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 63);

                        bShowNews.BackColor = Color.Black;
                        bShowNews.FlatAppearance.BorderColor = Color.FromArgb(155, 55, 0);

                        llBlockCaption.Text = lang == "ru" ? "Новости:" : "News:";

                        pVideo.Visible = false;
                        pNews.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Save("private void changeContent(bool video = true)", ex.Message);
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
                int i = -1;

                XDocument doc = XDocument.Load(@"http://worldoftanks.ru/ru/rss/news/");

                newsTitle.Clear();
                newsLink.Clear();
                newsDate.Clear();

                foreach (XElement el in doc.Root.Element("channel").Elements("item"))
                {
                    if (i > 10 || showNewsTop > 290) { break; }

                    newsDate.Add(el.Element("pubDate").Value);
                    newsTitle.Add(el.Element("title").Value);
                    newsLink.Add(el.Element("link").Value);

                    bwNews.ReportProgress(++i);
                }
            }
            catch (Exception ex)
            {
                Debug.Save("private void bwVideo_DoWork()", "XmlDocument doc = new XmlDocument();", ex.Message);
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
                Debug.Save("private void bwVideo_ProgressChanged(object sender, ProgressChangedEventArgs e)", "Создание динамических полей", ex.Message);
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
                Debug.Save("private void bwVideo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)", "Добавление ссылки на все видео", ex.Message);
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
            /// Запускаем проверку обновлений лаунчера после инициализации приложения
            UpdateLauncher update = new UpdateLauncher();
            update.Check(true).Wait();

            loadSettings().Wait();

            notifyIcon.Icon = Properties.Resources.Icon;
            notifyIcon.Text = Application.ProductName;

            try
            {
                this.Text = Application.ProductName + " v" + modpackVersion.ToString();
                this.Icon = Properties.Resources.Icon;

                llTitle.Text = Application.ProductName + " (" + (modpackType == "full" ? "Расширенная версия" : "Базовая версия") + ")";
                llLauncherVersion.Text = Application.ProductVersion;
            }
            catch (Exception ex)
            {
                Debug.Save("public fIndex()", "Применение заголовков и иконок приложения", ex.Message);
            }

            if (!bwVideo.IsBusy) { bwVideo.RunWorkerAsync(); } // Грузим видео с ютуба
            if (!bwNews.IsBusy) { bwNews.RunWorkerAsync(); } // Грузим новости с WG

            pNews.SetBounds(13, 109, 620, 290); // Так как панель у нас убрана с видимой части, устанавливаем ее расположение динамически

            llVersion.Text = modpackVersion.ToString();

            tanksVersion = GetTanksVersion().Result;

            if (!bwUpdater.IsBusy) { bwUpdater.RunWorkerAsync(); } // Запускаем проверку обновлений модпака и клиента игры

            Task.Factory.StartNew(() =>SendPOST.CountUsers(Application.ProductName, Application.ProductVersion, modpackVersion.ToString(), modpackType, youtubeChannel, lang)); // Отправляем на сайт инфу о запуске лаунчера

            // Главное окно
            languagePack.toolTip(bOptimizePC);

            GetVipProcesses();

            setBackground();
            moveForm();

            SetInterfaceLanguage();

            Task.Factory.StartNew(() => Debug.Send()); // Если имеются какие-либо файлы дебага, то отправляем их на сайт
        }

        private async Task GetVipProcesses()
        {
            try
            {
                if (File.Exists("settings.xml"))
                {
                    //ProcessList.processes;
                    XDocument doc = XDocument.Load("settings.xml");
                    if (doc.Root.Element("processes") != null)
                        foreach (XElement el in doc.Root.Element("processes").Elements("process")) { ProcessList.Add(el.Attribute("name").Value, el.Attribute("description").Value); }
                }
            }
            catch (Exception ex)
            {
                Debug.Save("GetVipProcesses()", ex.Message);
            }
        }

        private void bwOptimize_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbDownload.Visible = false;
            pbDownload.Value = 0;

            if (playGame)
            {
                if (!File.Exists(@"..\s.bat")) { File.WriteAllBytes(@"..\s.bat", Properties.Resources.start); }
                Process.Start(@"..\s.bat");
            }

            Task.Factory.StartNew(() => CheckClosingGame()); // Запускаем утилиту проверки запущен ли клиент игры
        }

        private async Task CheckClosingGame()
        {
            await Task.Delay(playGame ? 5000 : 2000); // Если запускаем танки, ждем 5 сек, если лаунчер - 2

            while (Process.GetProcessesByName("WorldOfTanks").Length > 0 || Process.GetProcessesByName("WoTLauncher").Length > 0)
            {
                await Task.Delay(5000);
            }

            Show();
            WindowState = FormWindowState.Normal;

            psi = new ProcessStartInfo("cmd", @"/c net start uxsms");
            Process.Start(psi);
        }

        private async Task ShowVideoNotification()
        {
            XDocument doc = XDocument.Load("settings.xml");

            if (doc.Root.Element("youtube") != null)
            {
                foreach (var el in doc.Root.Element("youtube").Elements("video")) { YoutubeVideo.Delete(el.Value); }
            }
            else doc.Root.Add(new XElement("youtube", null));

            // Перед выводом уведомлений проверяем даты. Все лишние удаляем
            foreach (var el in YoutubeVideo.List) { if (!ParseDate(modpackDate, el.Date)) { YoutubeVideo.Delete(el.ID); } }

            // Выводим список
            foreach (var el in YoutubeVideo.List)
            {
                await Task.Factory.StartNew(() => ShowVideoPause());

                notifyLink = el.Link;
                notifyIcon.ShowBalloonTip(2000, el.Title, "Посмотреть видео", ToolTipIcon.Info);

                doc.Root.Element("youtube").Add(new XElement("video", el.ID));
                doc.Save("settings.xml");
            }
        }

        private void ShowVideoPause()
        {
            Thread.Sleep(5000);
            while (Process.GetProcessesByName("WorldOfTanks").Length > 0 || Process.GetProcessesByName("WoTLauncher").Length > 0)
            {
                Thread.Sleep(5000);
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
                Debug.Save("private void notifyIcon_Click()", ex.Message);
            }
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            try
            {
                if (notifyLink != null) Process.Start(notifyLink);
            }
            catch (Exception ex)
            {
                Debug.Save("private void notifyIcon_BalloonTipClicked()", "Link: " + notifyLink, ex.Message);
            }
        }

        private bool ParseDate(string packDate = null, string newsDate = null)
        {
            if (packDate != null && newsDate != null)
            {
                /// Если дата новости старее даты выпуска модпака,
                /// то выводим в результат "false" как запрет на вывод.
                /// Во всех иных случаях выводим "true"
                if (DateTime.Parse(newsDate) < DateTime.Parse(packDate)) { return false; }
            }
            return true;
        }

        private void pbWarning_Click(object sender, EventArgs e)
        {
            fWarning fWarning = new fWarning();
            fWarning.ShowDialog();
        }

        /// <summary>
        ////Оптимизируем комп
        /// </summary>
        /// <returns>ждем ответа для решения о запуске клиента игры</returns>
        private async Task OptimizePC()
        {
            int myProgressStatus = 0;

            pbDownload.Value = 0;
            pbDownload.Visible = true;

            try
            {
                if (autoOptimizePC || autoAero)
                {
                    // Для начала определим версию ОС для отключения AERO
                    OperatingSystem osInfo = Environment.OSVersion;

                    switch (osInfo.Platform)
                    {
                        case System.PlatformID.Win32NT:
                            switch (osInfo.Version.Major)
                            {
                                case 5: /* Win XP */break;

                                default: // Win Vista, 7, 8, 8.1
                                    Process.Start(new ProcessStartInfo("cmd", @"/c net stop uxsms"));
                                    break;
                            }
                            break;
                        default: break;
                    }

                    maxPercentUpdateStatus = 1;
                    pbDownload.Value = ++myProgressStatus;
                }
            }
            catch (Exception ex) { Debug.Save("OptimizePC()", "if (autoOptimizePC || autoAero)", ex.Message); }

            try
            {
                if (autoOptimizePC || autoKill)
                {
                    // Завершаем ненужные процессы путем перебора массива имен с условием отсутствия определенных условий
                    int processCount = Process.GetProcesses().Length;
                    int sessionId = Process.GetCurrentProcess().SessionId;

                    maxPercentUpdateStatus += autoForceKill ? processCount * 2 - 2 : processCount - 1; // Расчитываем значение прогресс бара                    
                    ProcessesLibrary proccessLibrary = new ProcessesLibrary();  // получаем имена процессов, завершать которые НЕЛЬЗЯ

                    bool kill = false;

                    for (int i = 0; i < 2; i++)
                    {
                        foreach (var process in Process.GetProcesses())
                        {
                            try
                            {
                                if (process.SessionId == sessionId &&
                                    Array.IndexOf(proccessLibrary.Processes(), process.ProcessName) == -1 &&
                                    !ProcessList.IndexOf(process.ProcessName))
                                {
                                    if (!kill) process.CloseMainWindow(); else process.Kill();
                                }

                                pbDownload.Value = ++myProgressStatus; // Инкрименируем значение прогресс бара
                            }
                            catch (Exception ex) { Debug.Save("bwOptimize_DoWork()", "if (autoOptimizePC || autoKill)", process.ProcessName.ToString(), "Kill: " + kill.ToString(), ex.Message); }
                        }

                        if (!autoForceKill || !autoOptimizePC) { break; }
                        else
                        {
                            kill = true;
                            await Task.Delay(5000); // Ждем 5 секунд завершения, пока приложения нормально завершатся, затем повторяем цикл
                        }
                    }
                }
            }
            catch (Exception ex) { Debug.Save("OptimizePC()", "autoOptimizePC = " + autoOptimizePC.ToString(), "autoKill = " + autoKill.ToString(), ex.Message); }

            ///
            /// Optimize game graphic
            /// 
            if (autoOptimizePC)
            {
                try
                {
                    OptimizeGraphic OptimizeGraphic = new OptimizeGraphic();
                    Task.Factory.StartNew(() => OptimizeGraphic.Optimize(commonTest, autoVideo, autoWeak)).Wait();

                    pbDownload.Value = ++myProgressStatus; // Инкрименируем значение прогресс бара
                }
                catch (Exception ex)
                {
                    Debug.Save("OptimizePC()", "OptimizeGraphic OptimizeGraphic = new OptimizeGraphic();",
                        "commonTest = " + commonTest.ToString(),
                        "autoVideo = " + autoVideo.ToString(),
                        "autoWeak = " + autoWeak.ToString(),
                        ex.Message);
                }
            }

            // Готовим запуск клиента игры
            pbDownload.Visible = false;
            pbDownload.Value = 0;

            if (playGame)
            {
                if (!File.Exists(@"..\s.bat")) { File.WriteAllBytes(@"..\s.bat", Properties.Resources.start); }
                Process.Start(@"..\s.bat");
            }

            Task.Factory.StartNew(() => CheckClosingGame()); // Запускаем утилиту проверки запущен ли клиент игры
        }

        private async Task SetInterfaceLanguage()
        {
            foreach (Control control in this.Controls)
                control.Text = languagePack.InterfaceLanguage("fIndex", control, lang, modpackType);
        }
    }
}
