using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Net;
using System.Diagnostics;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Processes_Library;
using _Hell_Language_Pack;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fSettings : Form
    {
        ProcessesLibrary proccessLibrary = new ProcessesLibrary();
        ProcessList processList = new ProcessList();

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
                XmlDocument doc = new XmlDocument();
                doc.Load("settings.xml");

                try
                {
                    version = doc.GetElementsByTagName("version")[0].InnerText;
                }
                catch (Exception ex)
                {
                    Debug.Save("public fSettings()", "get VERSION", ex.Message);
                }

                try
                {
                    type = doc.GetElementsByTagName("type")[0].InnerText;
                }
                catch (Exception ex)
                {
                    Debug.Save("public fSettings()", "get TYPE", ex.Message);
                }

                try
                {
                    notification = doc.GetElementsByTagName("notification")[0].InnerText;
                }
                catch (Exception) { }

                try
                {
                    // priority
                    var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                    cbPriority.SelectedIndex = getPriority((int)key.GetValue("CpuPriorityClass"), false);                        
                }
                catch (Exception ex)
                {
                    cbPriority.SelectedIndex = 2;
                    Debug.Save("public fSettings()", "priority", ex.Message);
                }

                try
                {
                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("game"))
                    {
                        cbVideoQuality.Checked = xmlNode.Attributes["video"].InnerText == "False" ? false : true;
                        
                        // Проверяем количество процессоров
                        if (Environment.ProcessorCount > 1)
                        {
                            cbCPUAffinity.Checked = xmlNode.Attributes["affinity"].InnerText == "False" ? false : true;
                        }
                        else
                        {
                            cbCPUAffinity.Checked = false;
                            cbCPUAffinity.Enabled = false;
                        }
                    }
                }
                catch (Exception) { }

                try
                {
                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("settings"))
                    {
                        cbKillProcesses.Checked = xmlNode.Attributes["kill"].InnerText == "False" ? false : true;
                        cbForceClose.Checked = xmlNode.Attributes["force"].InnerText == "False" ? false : true;
                        cbAero.Checked = xmlNode.Attributes["aero"].InnerText == "False" ? false : true;
                        //cbNews.Checked = xmlNode.Attributes["news"].InnerText == "False" ? false : true;
                        cbVideo.Checked = xmlNode.Attributes["video"].InnerText == "False" ? false : true;
                    }

                    userProcesses.Clear();

                    foreach (XmlNode xmlNode in doc.GetElementsByTagName("process"))
                    {
                        userProcesses.Add(xmlNode.Attributes["name"].InnerText);
                    }

                    if (!bwUserProcesses.IsBusy) { bwUserProcesses.RunWorkerAsync(); }
                }
                catch (Exception ex)
                {
                    cbKillProcesses.Checked = false;
                    cbAero.Checked = false;
                    //cbNews.Checked = true;
                    cbVideo.Checked = true;

                    Debug.Save("public fSettings()", "foreach (XmlNode xmlNode in doc.GetElementsByTagName(\"settings\"))", ex.Message);
                }
            }
            else
            {
                //title = Application.ProductName;
                version = Application.ProductVersion;

                cbKillProcesses.Checked = false;
                cbAero.Checked = false;
                //cbNews.Checked = true;
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
            if (!bwSave.IsBusy) { bwSave.RunWorkerAsync(); }
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
            processList.Clear();

            Process[] myProcesses = Process.GetProcesses();
            int processID = Process.GetCurrentProcess().SessionId;

            for (int i = 1; i < myProcesses.Length; i++)
            {
                try
                {
                    if (myProcesses[i].SessionId == processID)
                    {
                        if (processList.IndexOf(myProcesses[i].ProcessName) < 0 && myProcesses[i].ProcessName != Process.GetCurrentProcess().ProcessName)
                        {
                            processList.Add(myProcesses[i].ProcessName, myProcesses[i].MainModule.FileVersionInfo.FileDescription.Trim());
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        private void bwUserProcesses_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 1; i < processList.Count(); i++)
            {
                try
                {
                    int pos = lvProcessesUser.Items.Add(processList.Range[i].Process).Index;
                    lvProcessesUser.Items[pos].SubItems.Add(processList.Range[i].Description);

                    // Процессы юзера
                    if (userProcesses.IndexOf(processList.Range[i].Process) > -1)
                    {
                        lvProcessesUser.Items[pos].Checked = true;
                        lvProcessesUser.Items[pos].BackColor = Color.LightGreen;
                    }

                    // Глобальные процессы
                    if (Array.IndexOf(proccessLibrary.Processes(), processList.Range[i].Process) > -1)
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

        private void bwSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                if (File.Exists("settings.xml")) { File.Delete(Application.StartupPath + "/settings.xml"); }

                XmlTextWriter wr = new XmlTextWriter("settings.xml", Encoding.UTF8);
                wr.Formatting = System.Xml.Formatting.Indented;

                wr.WriteStartDocument();
                wr.WriteStartElement("pro");

                wr.WriteStartElement("version", null);
                wr.WriteString(version);
                wr.WriteEndElement();

                wr.WriteStartElement("type", null);
                wr.WriteString(type);
                wr.WriteEndElement();

                wr.WriteStartElement("notification", null);
                wr.WriteString(notification);
                wr.WriteEndElement();

                wr.WriteStartElement("game", null);
                wr.WriteAttributeString("video", cbVideoQuality.Checked.ToString());
                wr.WriteAttributeString("affinity", cbCPUAffinity.Checked.ToString());
                wr.WriteEndElement();               

                wr.WriteStartElement("settings", null);
                wr.WriteAttributeString("kill", cbKillProcesses.Checked.ToString());
                wr.WriteAttributeString("force", cbForceClose.Checked.ToString());
                wr.WriteAttributeString("aero", cbAero.Checked.ToString());
                //wr.WriteAttributeString("news", cbNews.Checked.ToString());
                wr.WriteAttributeString("news", "False");
                wr.WriteAttributeString("video", cbVideo.Checked.ToString());
                wr.WriteEndElement();

                if (lvProcessesUser.CheckedItems.Count > 0)
                {
                    wr.WriteStartElement("processes", null);

                    foreach (ListViewItem obj in lvProcessesUser.CheckedItems)
                    {
                        if (obj.BackColor != Color.Plum)
                        {
                            wr.WriteStartElement("process", null);
                            wr.WriteAttributeString("name", obj.Text);
                            wr.WriteAttributeString("description", obj.SubItems[1].Text);
                            wr.WriteEndElement();
                        }
                    }

                    wr.WriteEndElement();
                }

                wr.WriteEndElement();

                wr.Flush();
                wr.Close();

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
            catch (Exception ex)
            {
                Debug.Save("private void bwSave_DoWork(object sender, DoWorkEventArgs e)", "", ex.Message);
            }

            // Сохраняем приоритет в реестр
            var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions", true);
            key.SetValue("CpuPriorityClass", getPriority(cbPriority.SelectedIndex).ToString(), Microsoft.Win32.RegistryValueKind.DWord);


                // Отправляем данные на сайт
                try
                {
                    List<string> myJsonData = new List<string>();

                    string name = Environment.MachineName +
                        Environment.UserName +
                        Environment.UserDomainName +
                        Environment.Version.ToString() +
                        Environment.OSVersion.ToString();

                    using (MD5 md5Hash = MD5.Create())
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
                catch (Exception ex)
                {
                    Debug.Save("private void bwSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)", "Send process", ex.Message);
                }

            this.DialogResult = DialogResult.OK;
        }

        private int getPriority(int pr, bool save=true)
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

        private void bwSave_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!File.Exists("preferences.xml"))
                    File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\preferences.xml", "preferences.xml");
            }
            catch (Exception ex)
            {
                Debug.Save("private void bwSave_DoWork(object sender, DoWorkEventArgs e)", "if (!File.Exists(\"preferences.xml\"))", ex.Message);
            }
        }
    }
}
