﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Processes_Library;
using _Hell_Language_Pack;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fSettings : Form
    {
        ProcessesLibrary ProccessLibrary = new ProcessesLibrary();
        ProcessList ProcessList = new ProcessList();

        string //title,
            version = "0.0.0.0",
            type = "full",
            notification;

        bool commonTest = false;

        List<string> userProcesses = new List<string>();

        Debug Debug = new Debug();

        public fSettings()
        {
            InitializeComponent();

            loadLang();
            moveForm();

            // Если винда старее Висты, то отключаем пункт
            cbAero.Enabled = EnableAero();
            if (!EnableAero()) cbAero.Checked = false;

            if (File.Exists("settings.xml"))
            {
                XDocument doc = XDocument.Load("settings.xml");

                commonTest = doc.Root.Element("common.test") != null;

                try { version = new IniFile(Directory.GetCurrentDirectory() + @"\config.ini").IniReadValue("new", "version"); }
                catch (Exception) { version = doc.Root.Element("version") != null ? doc.Root.Element("version").Value : "0.0.0.0"; }

                type = doc.Root.Element("type") != null ? doc.Root.Element("type").Value : "full";
                notification = doc.Root.Element("notification") != null ? doc.Root.Element("notification").Value : "";

                try
                {
                    var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                    cbPriority.SelectedIndex = getPriority((int)key.GetValue("CpuPriorityClass"), false);
                }
                catch (Exception ex) { cbPriority.SelectedIndex = 2; Debug.Save("public fSettings()", "Priority", ex.Message); }

                cbVideo.Checked = doc.Root.Element("info") == null || doc.Root.Element("info").Attribute("video").Value == "True";

                cbKillProcesses.Checked = ReadSettingsStatus(doc, "kill");
                cbForceClose.Checked = ReadSettingsStatus(doc, "force");
                cbAero.Checked = ReadSettingsStatus(doc, "aero");
                cbVideoQuality.CheckState = ReadCheckState(doc, "video");
                cbVideoQualityWeak.Checked = ReadSettingsStatus(doc, "weak");

                userProcesses.Clear();
                if (doc.Root.Element("processes") != null) foreach (XElement el in doc.Root.Element("processes").Elements("process")) { userProcesses.Add(el.Attribute("name").Value); }

                if (!bwUserProcesses.IsBusy) { bwUserProcesses.RunWorkerAsync(); }
            }
        }

        private void loadLang()
        {
            try
            {
                fLanguage languagePack = new fLanguage();

                // Окно настроек
                languagePack.toolTip(llUserProcesses);
                languagePack.toolTip(llGlobalProcesses);
            }
            catch (Exception ex)
            {
                Debug.Save("loadLang()", ex.Message);
            }
        }

        private CheckState ReadCheckState(XDocument doc, string attr)
        {
            if (doc.Root.Element("settings") != null)
                if (doc.Root.Element("settings").Attribute(attr) != null)
                {
                    switch (doc.Root.Element("settings").Attribute(attr).Value)
                    {
                        case "Checked": return CheckState.Checked; 
                        case "Indeterminate": return CheckState.Indeterminate;
                        default: return CheckState.Unchecked;
                    }
                }
            return CheckState.Unchecked;
        }

        private bool ReadSettingsStatus(XDocument doc, string attr)
        {
            if (doc.Root.Element("settings") != null)
                if (doc.Root.Element("settings").Attribute(attr) != null)
                    if (doc.Root.Element("settings").Attribute(attr).Value == "True")
                        return true;
            return false;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (cbVideoQuality.Checked)
                MessageBox.Show(this, "ВНИМАНИЕ!!!" + Environment.NewLine + "После применения настроек графики в игре требуется заново ввести логин/пароль!" + Environment.NewLine + Environment.NewLine +
                "Настройки графики применяются только при сохранении информации в окне настоек, либо при нажатии на кнопку \"Оптимизировать\" на главном окне программы." + Environment.NewLine +
                "При автоматической оптимизации настройки графики не изменяются.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            SaveSettings().Wait();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
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

        private void bwUserProcesses_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessList.Clear();

            Process[] myProcesses = Process.GetProcesses();
            int processID = Process.GetCurrentProcess().SessionId;

            for (int i = 1; i < myProcesses.Length; i++)
            {
                try
                {
                    if (myProcesses[i].SessionId == processID)
                    {
                        if (ProcessList.IndexOf(myProcesses[i].ProcessName) < 0 && myProcesses[i].ProcessName != Process.GetCurrentProcess().ProcessName)
                        {
                            ProcessList.Add(myProcesses[i].ProcessName, myProcesses[i].MainModule.FileVersionInfo.FileDescription.Trim());
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        private void bwUserProcesses_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 1; i < ProcessList.Count(); i++)
            {
                try
                {
                    int pos = lvProcessesUser.Items.Add(ProcessList.Range[i].Process).Index;
                    lvProcessesUser.Items[pos].SubItems.Add(ProcessList.Range[i].Description);

                    // Процессы юзера
                    if (userProcesses.IndexOf(ProcessList.Range[i].Process) > -1)
                    {
                        lvProcessesUser.Items[pos].Checked = true;
                        lvProcessesUser.Items[pos].BackColor = Color.LightGreen;
                    }

                    // Глобальные процессы
                    if (Array.IndexOf(ProccessLibrary.Processes(), ProcessList.Range[i].Process) > -1)
                    {
                        lvProcessesUser.Items[pos].Checked = true;
                        lvProcessesUser.Items[pos].BackColor = Color.Plum;
                    }
                }
                catch (Exception) { }
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

        private int getPriority(int pr, bool save = true)
        {
            if (save)
            {
                switch (pr)
                {
                    case 0: return 3; //Высокий
                    case 1: return 6; // Выше среднего
                    case 3: return 5; // Ниже среднего
                    case 4: return 1; // Низкий
                    default: return 2; // Средний
                }
            }
            else
            {
                switch (pr)
                {
                    case 3: return 0; //Высокий
                    case 6: return 1; // Выше среднего
                    case 5: return 3; // Ниже среднего
                    case 1: return 4; // Низкий
                    default: return 2; // Средний
                }
            }
        }

        private XDocument AddAttributeSettings(XDocument doc, string attr, string value)
        {
            if (doc.Root.Element("settings").Attribute(attr) != null)
                doc.Root.Element("settings").Attribute(attr).SetValue(value);
            else
                doc.Root.Element("settings").Add(new XAttribute(attr, value));

            return doc;
        }

        private async Task SaveSettings()
        {
            try
            {
                XDocument doc = XDocument.Load("settings.xml");

                if (doc.Root.Element("info") != null) { doc.Root.Element("info").Attribute("video").SetValue(cbVideo.Checked.ToString()); }
                else { XElement el = new XElement("info", new XAttribute("video", cbVideo.Checked.ToString())); doc.Root.Add(el); }

                if (doc.Root.Element("settings") != null)
                {
                    AddAttributeSettings(doc, "kill", cbKillProcesses.Checked.ToString());
                    AddAttributeSettings(doc, "force", cbForceClose.Checked.ToString());
                    AddAttributeSettings(doc, "aero", cbAero.Checked.ToString());
                    AddAttributeSettings(doc, "video", cbVideoQuality.CheckState.ToString());
                    AddAttributeSettings(doc, "weak", cbVideoQualityWeak.Checked.ToString());
                }
                else
                {
                    XElement el = new XElement("settings",
                        new XAttribute("kill", cbKillProcesses.Checked.ToString()),
                        new XAttribute("force", cbForceClose.Checked.ToString()),
                        new XAttribute("aero", cbAero.Checked.ToString()),
                        new XAttribute("video", cbVideoQuality.CheckState.ToString()),
                        new XAttribute("weak", cbVideoQualityWeak.Checked.ToString())
                        );
                    doc.Root.Add(el);
                }

                if (lvProcessesUser.CheckedItems.Count > 0)
                {
                    // Удаляем элемент
                    doc.Root.Element("processes").Remove();

                    if (doc.Root.Element("processes") == null) { XElement el = new XElement("processes", null); doc.Root.Add(el); }

                    foreach (ListViewItem obj in lvProcessesUser.CheckedItems)
                    {
                        if (obj.BackColor != Color.Plum)
                            doc.Root.Element("processes").Add(
                                new XElement("process",
                                    new XAttribute("name", obj.Text),
                                    new XAttribute("description", obj.SubItems[1].Text)
                            ));
                    }
                }

                doc.Save("settings.xml");
            }
            catch (Exception ex) { Debug.Save("bwSave_DoWork()", ex.Message); }

            // Сохраняем приоритет в реестр
            try
            {
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions", true);
                key.SetValue("CpuPriorityClass", getPriority(cbPriority.SelectedIndex).ToString(), Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (Exception)
            {
                var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                key.SetValue("CpuPriorityClass", getPriority(cbPriority.SelectedIndex).ToString(), Microsoft.Win32.RegistryValueKind.DWord);
            }

            // Изменяем файл настроек игры
            OptimizeGraphic OptimizeGraphic = new OptimizeGraphic();
            Task.Factory.StartNew(() => OptimizeGraphic.Optimize(commonTest, cbVideoQualityWeak.Checked)).Wait();


            // Отправляем данные на сайт
            try
            {
                if (lvProcessesUser.CheckedItems.Count > 0)
                {
                    List<string> myJsonData = new List<string>();

                    string name = Environment.MachineName +
                        Environment.UserName +
                        Environment.UserDomainName +
                        Environment.Version.ToString() +
                        Environment.OSVersion.ToString();

                    using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(name));
                        StringBuilder sBuilder = new StringBuilder();
                        for (int i = 0; i < data.Length; i++) { sBuilder.Append(data[i].ToString("x2")); }

                        name = sBuilder.ToString();
                    }

                    myJsonData.Clear();
                    myJsonData.Add(name);
                    myJsonData.Add("TIjgwJYQyUyC2E3BRBzKKdy54C37dqfYjyInFbfMeYed0CacylTK3RtGaedTHRC6");

                    foreach (ListViewItem obj in lvProcessesUser.CheckedItems)
                    {
                        if (obj.BackColor != Color.Plum) // Если процесс не является глобальным, то добавляем данные для вывода
                            myJsonData.Add(obj.Text + "::" + obj.SubItems[1].Text);
                    }

                    if (myJsonData.Count > 2)
                    {
                        string json = JsonConvert.SerializeObject(myJsonData);

                        string answer = getResponse("http://ai-rus.com/wot/process/" + json);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Save("bwSave_RunWorkerCompleted()", "Send processes", ex.Message);
            }

            this.DialogResult = DialogResult.OK;
        }

        private void llRecoverySettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (File.Exists("preferences" + (commonTest ? "_ct" : "") + ".xml"))
            {
                if (DialogResult.Yes == MessageBox.Show(this, "Вы действительно хотите восстановить файл прежний файл настроек игры?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    File.Copy("preferences" + (commonTest ? "_ct" : "") + ".xml", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\preferences" + (commonTest ? "_ct" : "") + ".xml", true);
                    MessageBox.Show(this, "Настройки успешно восстановлены", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void fSettings_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("preferences" + (commonTest ? "_ct" : "") + ".xml"))
                    llRecoverySettings.Enabled = true;
                else
                    File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\preferences" + (commonTest ? "_ct" : "") + ".xml", "preferences" + (commonTest ? "_ct" : "") + ".xml");
            }
            catch (Exception ex) { Debug.Save("fSettings_Load()", ex.Message); }
        }

        private bool EnableAero()
        {
            return Environment.OSVersion.Version.Major > 5;
        }

        private void cbVideoQuality_Click(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.CheckState == CheckState.Checked)
                cbVideoQualityWeak.Checked = true;
            else
                cbVideoQualityWeak.Checked = false;
        }

        private void cbVideoQualityVeryLow_Click(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked)
                cbVideoQuality.CheckState = CheckState.Checked;
            else
                cbVideoQuality.CheckState = CheckState.Indeterminate;
        }
    }
}
