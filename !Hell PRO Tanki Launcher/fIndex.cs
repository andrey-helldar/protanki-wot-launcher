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
using Newtonsoft.Json;
using Ionic.Zip;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fIndex : Form
    {
        Language Language = new Language();
        YoutubeVideo YoutubeVideo = new YoutubeVideo();
        SendPOST SendPOST = new SendPOST();
        Debug Debug = new Debug();
        ProcessList ProcessList = new ProcessList();

        string pathToTanks = "",

            modpackType = "base",
            modpackDate = "1970-1-1",

            newVersionMessage,
            newVersionLink = "http://goo.gl/gr6pFl",
            videoLink = "http://goo.gl/gr6pFl",
            updateNotification = "",

            notifyLink = "",

            lang = "en";

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
                if (process.SessionId != Process.GetCurrentProcess().SessionId) process.Kill();

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
                        Debug.Save("fIndex", "loadSettings()", "Read from config.ini", ex.Message);
                    }
                }
                else
                {
                    Debug.Save("fIndex", "loadSettings()", "File not found \"config.ini\"");
                    //MessageBox.Show(this, "Модпак не обнаружен!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(this, Language.DynamicLanguage("noMods", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Загружаем настройки
                if (File.Exists("settings.xml"))
                {
                    XDocument doc = XDocument.Load("settings.xml");

                    updateNotification = doc.Root.Element("notification") != null ? doc.Root.Element("notification").Value : "";

                    showVideoNotify = doc.Root.Element("info") != null ? (doc.Root.Element("info").Attribute("video") != null ? (doc.Root.Element("info").Attribute("video").Value == "True") : true) : true;

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
                    /* MessageBox.Show(this, "Файл настроек не обнаружен!" + Environment.NewLine +
                         "Программа будет автоматически перезапущена. Во время перезапуска будет применена стандартная конфигурация",
                         Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);*/
                    MessageBox.Show(this, Language.DynamicLanguage("noSettings", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Process.Start("restart.exe", "\"" + Process.GetCurrentProcess().ProcessName + "\"");
                    Process.GetCurrentProcess().CloseMainWindow();
                }
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "loadSettings()", ex.Message);
            }
        }

        private bool ReadCheckStateBool(XDocument doc, string attr)
        {
            try
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
            catch (Exception) { return false; }
        }

        private string GetTanksRegistry()
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{1EAC1D02-C6AC-4FA6-9A44-96258C37C812RU}_is1");
                return key != null ? (string)key.GetValue("InstallLocation") : null;
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "GetTanksRegistry()", ex.Message);
                //MessageBox.Show(this, Language.DynamicLanguage("admin", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
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
                        return new Version(doc.Root.Element("version").Value.Trim().Remove(0, 2).Replace(" ", "").Replace("#", "."));
                }
                else
                {
                    //Debug.Save("fIndex","tanksVersion()", "Клиент игры не обнаружен в реестре.");
                    //bPlay.Enabled = false;
                    bLauncher.Enabled = false;
                    //MessageBox.Show(this, "Клиент игры не обнаружен!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(this, Language.DynamicLanguage("noTanks", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return new Version("0.0.0.0");
                }
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "tanksVersion()", "doc.Load(\"" + pathToTanks + "version.xml\");", ex.Message);
                //bPlay.Enabled = false;
                bLauncher.Enabled = false;
                //MessageBox.Show(this, "Клиент игры не обнаружен!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(this, Language.DynamicLanguage("noTanks", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return new Version("0.0.0.0");
            }
        }

        private string CorrectPath(string sourcePath, int remove = 0)
        {
            string newPath = "";

            try
            {
                string[] temp = sourcePath.Split('\\');

                for (int i = 0; i < temp.Length + remove; i++)
                    newPath += temp[i] + @"\";

                return newPath;
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "CorrectPath",
                    "sourcePath = " + sourcePath,
                    "remove = " + remove.ToString(),
                    "newPath = " + newPath,
                    ex.Message);
                return sourcePath;
            }
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
                Debug.Save("fIndex", "setBackground()", ex.Message);
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
                Debug.Save("fIndex", "bVideo_Click()", videoLink, ex.Message);
                MessageBox.Show(this, Language.DynamicLanguage("badLink", lang, videoLink), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                remoteTanksVersion = new Version(SendPOST.Send("http://ai-rus.com/wot/version/", "code=" + Debug.Code + "&user=" + Debug.UserID() + "&version=" + tanksVersion.ToString() + "&test=" + (commonTest ? "1" : "0") + "&lang=" + lang));

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
                Debug.Save("fIndex", "bwUpdater_DoWork()", ex.Message);
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
                        status += Language.DynamicLanguage("llActuallyNewMods", lang) + ": " + remoteModVersion.ToString() + Environment.NewLine;

                        videoLink = newVersionLink;
                    }

                    if (tanksUpdates)
                    {
                        //status += "Обнаружена новая версия клиента игры (" + remoteTanksVersion.ToString() + ")" + Environment.NewLine;
                        status += Language.DynamicLanguage("llActuallyNewGame", lang) + ": " + remoteTanksVersion.ToString() + Environment.NewLine;

                        // Отключаем кнопку запуска игры
                        bPlay.Enabled = false;
                    }
                    else
                    {
                        // Включаем кнопку запуска игры
                        bPlay.Enabled = true;
                    }


                    //llActually.Text = modpackUpdates ? "Обнаружена новая версия мультипака!" : "Обнаружена новая версия игры!";
                    llActually.Text = modpackUpdates ?
                        Language.DynamicLanguage("llActuallyNewMods", lang) :
                        Language.DynamicLanguage("llActuallyNewGame", lang);
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

                    bUpdate.Enabled = true; // Включаем кнопку обновлений
                }
                else
                {
                    //llActually.Text = "Вы используете самые свежие моды!";
                    llActually.Text = Language.DynamicLanguage("llActuallyActually", lang);
                    llActually.ForeColor = Color.Lime;
                    llActually.ActiveLinkColor = Color.Lime;
                    llActually.LinkColor = Color.Lime;
                    llActually.VisitedLinkColor = Color.Lime;

                    /*status = "Вы используете самые свежие моды." + Environment.NewLine +
                        "Текущая версия Мультипака: " + modpackVersion.ToString() + Environment.NewLine +
                        "Текущая версия клиента игры: " + tanksVersion.ToString();*/
                    status = Language.DynamicLanguage("llActuallyActually", lang) + Environment.NewLine +
                        Language.DynamicLanguage("llActuallyThisVerMods", lang) + modpackVersion.ToString() + Environment.NewLine +
                        Language.DynamicLanguage("llActuallyThisVerGame", lang) + tanksVersion.ToString();

                    bUpdate.Enabled = false; // Выключаем кнопку обновлений

                    // Окно статуса обновлений
                    fNewVersion.llCaption.Text = status;

                    //fNewVersion.llContent.Text = "Обновления отсутствуют";
                    fNewVersion.llContent.Text = Language.DynamicLanguage("noUpdates", lang);
                    fNewVersion.llVersion.Text = modpackVersion.ToString();
                }

                manualClickUpdate = false;
                notifyLink = null;
                if (status != String.Empty) notifyIcon.ShowBalloonTip(2000, Application.ProductName, status, ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                 Debug.Save("fIndex","bwUpdater_RunWorkerCompleted()", ex.Message);
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
            catch (Exception ex) { Debug.Save("fIndex", "bLauncher_Click()", ex.Message); }
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            try
            {
                autoOptimizePC = false;
                GetVipProcesses().Wait();

                playGame = true;
                OptimizePC().Wait();

                //Hide();
                WindowState = FormWindowState.Minimized;
            }
            catch (Exception ex) { Debug.Save("fIndex", "bPlay_Click()", ex.Message); }
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
                Debug.Save("fIndex", "moveForm()", ex.Message);
            }
        }

        private void bUpdate_Click(object sender, EventArgs e)
        {
            /// Tanks updates
            try
            {
                if (tanksUpdates)
                {
                    autoOptimizePC = false;
                    GetVipProcesses().Wait();

                    OptimizePC().Wait();
                    Process.Start(pathToTanks + "WoTLauncher.exe");

                    WindowState = FormWindowState.Minimized;
                }
            }
            catch (Exception ex) { Debug.Save("fIndex", "bUpdate_Click()", "tanksUpdates = " + tanksUpdates.ToString(), ex.Message); }

            /// Multipack updates
            try
            {
                if (modpackUpdates)
                    Process.Start(newVersionLink);
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "bUpdate_Click()", "newVersionLink = " + newVersionLink, ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://goo.gl/5PR4ma"); // Link to http://ai-rus.com with goo.gl
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "linkLabel1_LinkClicked()", "Process.Start(\"http://goo.gl/5PR4ma\");", ex.Message);
                MessageBox.Show(this, Language.DynamicLanguage("badLink", lang, "http://goo.gl/5PR4ma"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsCheckUpdates_Click(object sender, EventArgs e)
        {
            if (!bwUpdater.IsBusy)
            {
                bwUpdater.RunWorkerAsync();
            }
            else
            {
                //MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(this, Language.DynamicLanguage("checkUpdates", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsVideo_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(videoLink);
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "tsVideo_Click()", "Process.Start(\"" + videoLink + "\");", ex.Message);
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
                // MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(this, Language.DynamicLanguage("checkUpdates", lang), Language.DynamicLanguage("updatingTitle", lang), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsShow_Click(object sender, EventArgs e)
        {
            try
            {
                //Show();
                WindowState = FormWindowState.Normal;
                //this.ShowInTaskbar = true;
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "tsShow_Click()", "WindowState = FormWindowState.Normal;", ex.Message);
            }
        }

        private void bOptimizePC_Click(object sender, EventArgs e)
        {
            //string addMess = autoVideo ? Environment.NewLine + Environment.NewLine + "Также, после применения настроек графики в игре требуется заново ввести логин/пароль!" : "";
            string addMess = autoVideo ? Environment.NewLine + Environment.NewLine + Language.DynamicLanguage("reEnterPass", lang) : "";

            /*if (DialogResult.Yes == MessageBox.Show(this, "ВНИМАНИЕ!!!" + Environment.NewLine + "При оптимизации ПК на время игры будут завершены некоторые пользовательские приложения." + addMess +
                Environment.NewLine + Environment.NewLine + "Вы хотите продолжить?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information))*/
            if (DialogResult.Yes == MessageBox.Show(this, Language.DynamicLanguage("optimize", lang), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
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
                    //MessageBox.Show(this, "Подождите завершения предыдущей операции", "Оптимизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(this, Language.DynamicLanguage("wait", lang), Language.DynamicLanguage("optimizeTitle", lang), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                case 5:/* THIS IS WINDOWS XP!!! AAAAAAAAAAAA!!!!!! */ break;

                                default:
                                    Process.Start(new ProcessStartInfo("cmd", @"/c net stop uxsms")); // останавливаем aero
                                    break;
                            }
                            break;
                    }

                    maxPercentUpdateStatus = 1;
                    bwOptimize.ReportProgress(++myProgressStatus);
                }
            }
            catch (Exception ex) {  Debug.Save("fIndex","bwOptimize_DoWork()", "if (autoOptimizePC || autoAero)", ex.Message); }

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
                            catch (Exception ex) { Debug.Save("fIndex", "bwOptimize_DoWork()", "if (autoOptimizePC || autoKill)", process.ProcessName.ToString(), "Kill: " + kill.ToString(), ex.Message); }
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
                    Debug.Save("fIndex", "bwOptimize_DoWork()", "if (autoOptimizePC || autoVideo)", "commonTest = " + commonTest.ToString(), ex1.Message);
                }
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "bwOptimize_DoWork)", ex.Message);
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
                // MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(this, Language.DynamicLanguage("checkUpdates", lang), Language.DynamicLanguage("updatingTitle", lang), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsSettings_Click(object sender, EventArgs e)
        {
            try
            {
                fSettings fSettings = new fSettings();
                fSettings.ShowDialog();
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "tsSettings_Click()", "fSettings fSettings = new fSettings();", ex.Message);
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
                Debug.Save("fIndex", "fIndex_FormClosing()", "psi = new ProcessStartInfo(\"cmd\", @\"/c net start uxsms\");", ex.Message);
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
                // MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(this, Language.DynamicLanguage("checkUpdates", lang), Language.DynamicLanguage("updatingTitle", lang), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Debug.Save("fIndex", "bwOptimize_ProgressChanged(object sender, ProgressChangedEventArgs e)", "double i = Convert.ToDouble(e.ProgressPercentage) / Convert.ToDouble(maxPercentUpdateStatus) * 100;", ex.Message);
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
                Debug.Save("fIndex", "label_Click()", "Link : " + (sender as LinkLabel).Links[0].LinkData.ToString(), ex.Message);
                MessageBox.Show(this, Language.DynamicLanguage("badLink", lang, videoLink), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bwVideo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                //label.Text = "Все видео";
                label.Text = "test";
                label.Name = "llVideoAll";
                label.Click += new EventHandler(bVideo_Click);
                this.Controls.Add(label);
                label.Text = Language.DynamicLanguage("llVideoAll", lang);*/

                llVideoAll.Links[0].LinkData = "http://goo.gl/LXaU7T";
                llVideoAll.Text = Language.DynamicLanguage("llVideoAllVideo", lang);

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
                Debug.Save("fIndex", "bwVideo_RunWorkerCompleted()", "Добавление ссылки на все видео", ex.Message);
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
                XDocument doc = XDocument.Load(@"https://gdata.youtube.com/feeds/api/users/" + Debug.Youtube + "/uploads");
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
                Debug.Save("fIndex", "bwVideo_DoWork()", ex.Message);
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
                    Debug.Save("fIndex", "bwVideo_ProgressChanged()", "Создание динамических полей", ex.Message);
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
                Debug.Save("fIndex", "private string formatDate(string dt)", dt, ex.Message);
                return "error";
            }
        }

        private void changeContent(bool video = true)
        {
            try
            {
                if (video)
                {
                    //if (llBlockCaption.Text != "Видео:" && llBlockCaption.Text != "Video:") // Исключаем повторное нажатие
                    if (llBlockCaption.Text != Language.DynamicLanguage("video", lang)) // Исключаем повторное нажатие
                    {
                        bShowVideo.BackColor = Color.Black;
                        bShowVideo.FlatAppearance.BorderColor = Color.FromArgb(155, 55, 0);

                        bShowNews.BackColor = Color.FromArgb(28, 28, 28);
                        bShowNews.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 63);

                        llBlockCaption.Text = Language.DynamicLanguage("video", lang);

                        llVideoAll.Links[0].LinkData = "http://goo.gl/LXaU7T";
                        llVideoAll.Text = Language.DynamicLanguage("llVideoAllVideo", lang);

                        pNews.Visible = false;
                        pVideo.Visible = true;
                    }
                }
                else
                {
                    //if (llBlockCaption.Text != "Новости:" && llBlockCaption.Text != "News:") // Исключаем повторное нажатие
                    if (llBlockCaption.Text != Language.DynamicLanguage("news", lang)) // Исключаем повторное нажатие
                    {
                        bShowVideo.BackColor = Color.FromArgb(28, 28, 28);
                        bShowVideo.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 63);

                        bShowNews.BackColor = Color.Black;
                        bShowNews.FlatAppearance.BorderColor = Color.FromArgb(155, 55, 0);

                        llBlockCaption.Text = Language.DynamicLanguage("news", lang);

                        llVideoAll.Links[0].LinkData = "http://goo.gl/Wlrh9F";
                        llVideoAll.Text = Language.DynamicLanguage("llVideoAllNews", lang);

                        pVideo.Visible = false;
                        pNews.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "changeContent(bool video = true)", ex.Message);
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
                Debug.Save("fIndex", "bwVideo_DoWork()", "XmlDocument doc = new XmlDocument();", ex.Message);
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
                Debug.Save("fIndex", "bwVideo_ProgressChanged(object sender, ProgressChangedEventArgs e)", "Создание динамических полей", ex.Message);
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
                Debug.Save("fIndex", "bwVideo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)", "Добавление ссылки на все видео", ex.Message);
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
                //MessageBox.Show(this, "Подождите, предыдущая проверка обновлений не завершена", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(this, Language.DynamicLanguage("checkUpdates", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void fIndex_Load(object sender, EventArgs e)
        {
            loadSettings().Wait();

            notifyIcon.Icon = Properties.Resources.Icon;
            notifyIcon.Text = Application.ProductName;

            try
            {
                this.Text = Application.ProductName + " v" + modpackVersion.ToString();
                this.Icon = Properties.Resources.Icon;

                //llTitle.Text = Application.ProductName + " (" + (modpackType == "full" ? "Расширенная версия" : "Базовая версия") + ")";
                llLauncherVersion.Text = Application.ProductVersion;
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "public fIndex()", "Применение заголовков и иконок приложения", ex.Message);
            }

            if (!bwVideo.IsBusy) { bwVideo.RunWorkerAsync(); } // Грузим видео с ютуба
            if (!bwNews.IsBusy) { bwNews.RunWorkerAsync(); } // Грузим новости с WG

            pNews.SetBounds(13, 109, 620, 290); // Так как панель у нас убрана с видимой части, устанавливаем ее расположение динамически

            llVersion.Text = modpackVersion.ToString();

            tanksVersion = GetTanksVersion().Result;

            if (!bwUpdater.IsBusy) { bwUpdater.RunWorkerAsync(); } // Запускаем проверку обновлений модпака и клиента игры

            Task.Factory.StartNew(() => SendPOST.CountUsers(Application.ProductName, Application.ProductVersion, modpackVersion.ToString(), modpackType, Debug.Youtube, lang)); // Отправляем на сайт инфу о запуске лаунчера

            // Главное окно
            Language.toolTip(bOptimizePC);

            GetVipProcesses();

            setBackground();
            moveForm();

            SetInterfaceLanguage();
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
                Debug.Save("fIndex", "GetVipProcesses()", ex.Message);
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

            //Task.Factory.StartNew(() => CheckClosingGame()); // Запускаем утилиту проверки запущен ли клиент игры
            CheckClosingGame();
        }

        private async Task CheckClosingGame()
        {
            await Task.Delay(playGame ? 10000 : 5000); // Если запускаем танки, ждем 10 сек, если лаунчер - 5 сек

            for (int i = 0; i < 2; i++)
            {
                while (Process.GetProcessesByName("WorldOfTanks").Length > 0 || Process.GetProcessesByName("WoTLauncher").Length > 0)
                {
                    await Task.Delay(5000);
                }

                await Task.Delay(7000); // Если цикл прерван случайно, то выжидаем еще 7 секунд перед повторным запуском
            }

            //Show();
            WindowState = FormWindowState.Normal;

            psi = new ProcessStartInfo("cmd", @"/c net start uxsms");
            Process.Start(psi);
        }

        private async Task ShowVideoNotification()
        {
            try
            {
                XDocument doc = XDocument.Load("settings.xml");

                if (doc.Root.Element("youtube") != null)
                {
                    foreach (var el in doc.Root.Element("youtube").Elements("video")) { YoutubeVideo.Delete(el.Value); }
                }
                else doc.Root.Add(new XElement("youtube", null));

                DeleteVideo(); // Перед выводом уведомлений проверяем даты. Все лишние удаляем

                // Выводим список
                foreach (var el in YoutubeVideo.List)
                {
                    ShowVideoPause().Wait();

                    notifyLink = el.Link;
                    //notifyIcon.ShowBalloonTip(5000, el.Title, "Посмотреть видео", ToolTipIcon.Info);
                    notifyIcon.ShowBalloonTip(5000, el.Title, Language.DynamicLanguage("viewVideo", lang), ToolTipIcon.Info);

                    doc.Root.Element("youtube").Add(new XElement("video", el.ID));
                    doc.Save("settings.xml");
                }
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex", "ShowVideoNotification()", ex.Message);
            }
        }

        /// <summary>
        /// Если мы удалили 1 пункт из списка, то дальнейший перебор невозможен.
        /// Но используя рекурсию мы повторяем перебор до тех пор, пока все ненужные
        /// элементы не будут удалены из списка. Profit!
        /// </summary>
        /// <returns>Функция как таковая ничего не возвращает</returns>
        private async Task DeleteVideo()
        {
            try
            {
                foreach (var el in YoutubeVideo.List)
                    if (!ParseDate(modpackDate, el.Date))
                        YoutubeVideo.Delete(el.ID);
            }
            catch (Exception) { DeleteVideo(); }
        }

        private async Task ShowVideoPause()
        {
            await Task.Delay(5000);

            for (int i = 0; i < 2; i++) // Если цикл прерван случайно, то выжидаем еще 7 секунд перед повторным запуском
            {
                while (Process.GetProcessesByName("WorldOfTanks").Length > 0 || Process.GetProcessesByName("WoTLauncher").Length > 0)
                {
                    await Task.Delay(5000);
                }

                await Task.Delay(7000);
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
                Debug.Save("fIndex", "notifyIcon_Click()", ex.Message);
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
                Debug.Save("fIndex", "notifyIcon_BalloonTipClicked()", "Link: " + notifyLink, ex.Message);
            }
        }

        /// <summary>
        /// Если дата новости старее даты выпуска модпака,
        /// то выводим в результат "false" как запрет на вывод.
        /// </summary>
        /// <param name="packDate"></param>
        /// <param name="newsDate"></param>
        /// <returns>Во всех иных случаях выводим "true",
        /// то есть дата валидная</returns>
        private bool ParseDate(string packDate = null, string newsDate = null)
        {
            try
            {
                if (packDate != null && newsDate != null)
                {
                    if (DateTime.Parse(newsDate) < DateTime.Parse(packDate)) { return false; }
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
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
            catch (Exception ex)
            {
                Debug.Save("fIndex", "OptimizePC()",
                "if (autoOptimizePC || autoAero)",
                "autoOptimizePC = " + autoOptimizePC.ToString(),
                "autoAero = " + autoAero.ToString(),
                ex.Message);
            }

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
                            catch (Exception ex) { Debug.Save("fIndex", "bwOptimize_DoWork()", "if (autoOptimizePC || autoKill)", process.ProcessName.ToString(), "Kill: " + kill.ToString(), ex.Message); }
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
            catch (Exception ex) { Debug.Save("fIndex", "OptimizePC()", "autoOptimizePC = " + autoOptimizePC, "autoKill = " + autoKill, ex.Message); }

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
                    Debug.Save("fIndex", "OptimizePC()", "autoOptimizePC = " + autoOptimizePC,
                        "commonTest = " + commonTest,
                        "autoVideo = " + autoVideo,
                        "autoWeak = " + autoWeak,
                        ex.Message);
                }
            }

            // Готовим запуск клиента игры
            pbDownload.Visible = false;
            pbDownload.Value = 0;

            if (playGame)
            {
                //if (!File.Exists(@"..\s.bat")) { File.WriteAllBytes(@"..\s.bat", Properties.Resources.start); }
                //Process.Start(@"..\s.bat");
                if (!File.Exists(pathToTanks + "s.bat")) { File.WriteAllBytes(pathToTanks + "s.bat", Properties.Resources.start); }
                Process.Start(pathToTanks + "s.bat");                
            }

            //Task.Factory.StartNew(() => CheckClosingGame()); // Запускаем утилиту проверки запущен ли клиент игры
            CheckClosingGame(); // Запускаем утилиту проверки запущен ли клиент игры
            
        }

        private async Task SetInterfaceLanguage()
        {
            /*foreach (Control control in this.Controls)
                control.Text = Language.InterfaceLanguage("fIndex", control, lang, modpackType);*/
            foreach (Control control in this.Controls)
                SetLanguageControl(control);

            tsShow.Text = Language.DynamicLanguage("tsShow", lang);
            tsVideo.Text = Language.DynamicLanguage("tsVideo", lang);
            tsCheckUpdates.Text = Language.DynamicLanguage("tsCheckUpdates", lang);
            tsSettings.Text = Language.DynamicLanguage("tsSettings", lang);
            tsExit.Text = Language.DynamicLanguage("tsExit", lang);
        }

        private void SetLanguageControl(Control control)
        {
            try
            {
                foreach (Control c in control.Controls)
                {
                    SetLanguageControl(c);
                }

                var cb = control as CheckBox;

                if (cb != null)
                {
                    cb.Text = Language.InterfaceLanguage("fIndex", cb, lang, modpackType);
                    Language.toolTip(cb, lang);
                }
                else
                {
                    control.Text = Language.InterfaceLanguage("fIndex", control, lang, modpackType);
                    Language.toolTip(control, lang);
                }
            }
            catch (Exception ex)
            {
                Debug.Save("fIndex","UncheckAllCheckBoxes()", ex.Message);
            }
        }
    }
}
