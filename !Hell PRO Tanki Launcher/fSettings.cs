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

        List<string> userProcesses = new List<string>();

        Debug Debug = new Debug();

        public fSettings()
        {
            InitializeComponent();

            loadLang();

            moveForm();


            if (File.Exists("settings.xml"))
            {
                XDocument doc = XDocument.Load("settings.xml");

                try { version = new IniFile(Directory.GetCurrentDirectory() + @"\config.ini").IniReadValue("new", "version"); }
                catch (Exception)
                { version = doc.Root.Element("version").Value; }

                try { type = doc.Root.Element("type").Value; }
                catch (Exception ex) { type = "full"; Debug.Save("public fSettings()", "type", ex.Message); }

                try { notification = doc.Root.Element("notification").Value; }
                catch (Exception) { notification = ""; }

                try
                {
                    var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                    cbPriority.SelectedIndex = getPriority((int)key.GetValue("CpuPriorityClass"), false);
                }
                catch (Exception ex) { cbPriority.SelectedIndex = 2; Debug.Save("public fSettings()", "Priority", ex.Message); }

                try { cbVideoQuality.Checked = doc.Root.Element("game").Attribute("video").Value == "True"; }
                catch (Exception) { cbVideoQuality.Checked = false; }

                try
                {
                    cbKillProcesses.Checked = doc.Root.Element("settings").Attribute("kill").Value == "True";
                    cbForceClose.Checked = doc.Root.Element("settings").Attribute("force").Value == "True";
                    cbAero.Checked = doc.Root.Element("settings").Attribute("aero").Value == "True";
                    cbVideo.Checked = doc.Root.Element("settings").Attribute("video").Value == "True";
                }
                catch (Exception ex)
                {
                    cbKillProcesses.Checked = false;
                    cbAero.Checked = false;
                    cbVideo.Checked = true;

                    Debug.Save("public fSettings()", "cbKillProcesses.Checked", ex.Message);
                }

                try
                { userProcesses.Clear(); foreach (XElement el in doc.Root.Element("processes").Elements("process")) { userProcesses.Add(el.Attribute("name").Value); } }
                catch (Exception ex) { Debug.Save("public fSettings()", "foreach (XElement el in doc.Root.Element(\"processes\"))", ex.Message); }

                if (!bwUserProcesses.IsBusy) { bwUserProcesses.RunWorkerAsync(); }
            }
            else
            {
                version = Application.ProductVersion;

                cbKillProcesses.Checked = false;
                cbAero.Checked = false;
                cbVideo.Checked = true;

                cbVideoQuality.Checked = false;
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
                Debug.Save("private void loadLang()", "", ex.Message);
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
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
                Debug.Save("private void moveForm()", "", ex.Message);
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

        private async Task SaveSettings()
        {
            try
            {
                if (!File.Exists("preferences.xml"))
                    File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\preferences.xml", "preferences.xml");
            }
            catch (Exception ex) { Debug.Save("private void bwSave_DoWork(object sender, DoWorkEventArgs e)", "if (!File.Exists(\"preferences.xml\"))", ex.Message); }


            try
            {
                XDocument doc = new XDocument(

                    new XElement("pro",

                        new XElement("version", version),
                        new XElement("type", type),
                        new XElement("notification", notification != "" ? notification : null),

                        new XElement("game",
                            new XAttribute("video", cbVideoQuality.Checked.ToString())
                            ),

                        new XElement("settings",
                            new XAttribute("kill", cbKillProcesses.Checked.ToString()),
                            new XAttribute("force", cbForceClose.Checked.ToString()),
                            new XAttribute("aero", cbAero.Checked.ToString()),
                            new XAttribute("video", cbVideo.Checked.ToString())
                            ),

                        new XElement("processes", null)
                     )
              );

                if (lvProcessesUser.CheckedItems.Count > 0)
                    foreach (ListViewItem obj in lvProcessesUser.CheckedItems)
                    {
                        doc.Root.Element("processes").Add(
                            new XElement("process",
                                new XAttribute("name", obj.Text),
                                new XAttribute("description", obj.SubItems[1].Text)
                        ));
                    }

                doc.Save("settings.xml");

                if (cbVideoQuality.Checked)
                {
                    //  c:\Users\Helldar\AppData\Roaming\Wargaming.net\WorldOfTanks\
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
            }
            catch (Exception ex) { Debug.Save("private void bwSave_DoWork()", ex.Message); }

            // Сохраняем приоритет в реестр
            try
            {
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions", true);
                key.SetValue("CpuPriorityClass", getPriority(cbPriority.SelectedIndex).ToString(), Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                key.SetValue("CpuPriorityClass", getPriority(cbPriority.SelectedIndex).ToString(), Microsoft.Win32.RegistryValueKind.DWord);

                Debug.Save("private void bwSave_RunWorkerCompleted()", "Create registry key", ex.Message);
            }


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

                    if (myJsonData.Count > 0)
                    {
                        string json = JsonConvert.SerializeObject(myJsonData);

                        string answer = getResponse("http://ai-rus.com/wot/process/" + json);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Save("private void bwSave_RunWorkerCompleted()", "Send processes", ex.Message);
            }

            await Task.Delay(1);

            this.DialogResult = DialogResult.OK;
        }
    }
}
