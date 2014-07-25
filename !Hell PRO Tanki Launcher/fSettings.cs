using System;
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

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fSettings : Form
    {
        ProcessesLibrary ProccessLibrary = new ProcessesLibrary();
        ProcessList ProcessList = new ProcessList();
        Language Language = new Language();
        Debug Debug = new Debug();

        public string //title,
            version = "0.0.0.0",
            type = "full",
            notification,
            lang = "en",
            patoToSettings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";

        bool commonTest = false;

        List<string> userProcesses = new List<string>();

        public fSettings()
        {
            InitializeComponent();
        }

        private void loadLang()
        {
            foreach (Control control in this.Controls)
                SetLanguageControl(control);

            // Нагрузка на ЦП
            if (!File.Exists(@"..\res_mods\" + Properties.Resources.ModPackVersion + @"\engine_config.xml"))
            {
                bBalanceCPU.Text = Language.DynamicLanguage("bBalanceCPU1", lang);
                bBalanceCPU.BackgroundImage = Properties.Resources.lamp_on;
            }
            else
            {
                bBalanceCPU.Text = Language.DynamicLanguage("bBalanceCPU0", lang);
                bBalanceCPU.BackgroundImage = Properties.Resources.lamp_off;
            }
        }

        private void SetLanguageControl(Control control)
        {
            try
            {
                if (control.Name != "lvProcessesUser")
                {
                    if (control.Name != "cbPriority" && control.Name != "cbMinimize")
                    {
                        foreach (Control c in control.Controls)
                        {
                            SetLanguageControl(c);
                        }

                        var cb = control as CheckBox;

                        if (cb != null)
                        {
                            cb.Text = Language.InterfaceLanguage("fSettings", cb, lang);
                            Language.toolTip(cb, lang);
                        }
                        else
                        {
                            control.Text = Language.InterfaceLanguage("fSettings", control, lang);
                            Language.toolTip(control, lang);
                        }
                    }
                    else
                    {
                        if (control.Name == "cbPriority")
                        {
                            cbPriority.Items.Clear();
                            for (int i = 0; i < 5; i++)
                                cbPriority.Items.Add(Language.DynamicLanguage("priority" + i.ToString(), lang));
                        }

                        if (control.Name == "cbMinimize")
                        {
                            cbMinimize.Items.Clear();
                            for (int i = 0; i < 4; i++)
                                cbMinimize.Items.Add(Language.DynamicLanguage("minimize" + i.ToString(), lang));
                        }
                    }
                }
                else
                {
                    if (control.Name == "lvProcessesUser")
                    {
                        lvProcessesUser.Columns[0].Text = Language.DynamicLanguage("lvProcessesUser0", lang);
                        lvProcessesUser.Columns[1].Text = Language.DynamicLanguage("lvProcessesUser1", lang);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Save("fSettings", "UncheckAllCheckBoxes()", ex.Message);
            }
        }

        private CheckState ReadCheckState(XDocument doc, string attr)
        {
            if (doc.Root.Element("settings") != null)
                if (doc.Root.Element("settings").Attribute(attr) != null)
                    switch (doc.Root.Element("settings").Attribute(attr).Value)
                    {
                        case "Checked": return CheckState.Checked;
                        case "Indeterminate": return CheckState.Indeterminate;
                        default: return CheckState.Unchecked;
                    }
            return CheckState.Unchecked;
        }

        private int ReadIntStatus(XDocument doc, string el, string attr)
        {
            if (doc.Root.Element(el) != null)
                if (doc.Root.Element(el).Attribute(attr) != null)
                    return Convert.ToInt16(doc.Root.Element(el).Attribute(attr).Value);

            return 0;
        }

        private bool ReadSettingsStatus(XDocument doc, string root, string attr, bool back = false)
        {
            if (!back)
            {
                if (doc.Root.Element(root) != null)
                    if (doc.Root.Element(root).Attribute(attr) != null)
                        return doc.Root.Element(root).Attribute(attr).Value == "True";
                return false;
            }
            else
            {
                if (doc.Root.Element(root) != null)
                    if (doc.Root.Element(root).Attribute(attr) != null)
                        return doc.Root.Element(root).Attribute(attr).Value == "True";
                return true;
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (cbVideoQuality.Checked)
                MessageBox.Show(this, Language.DynamicLanguage("reEnterLoginPass", lang),
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
            try
            {
                ProcessList.Clear();

                Process[] myProcesses = Process.GetProcesses();
                int processID = Process.GetCurrentProcess().SessionId;

                for (int i = 1; i < myProcesses.Length; i++)
                {
                    try
                    {
                        if (myProcesses[i].SessionId == processID)
                            if (!ProcessList.IndexOf(myProcesses[i].ProcessName) && myProcesses[i].ProcessName != Process.GetCurrentProcess().ProcessName)
                                ProcessList.Add(myProcesses[i].ProcessName, myProcesses[i].MainModule.FileVersionInfo.FileDescription.Trim());
                    }
                    catch (Exception ex) { Debug.Save("bwUserProcesses_DoWork()", myProcesses[i].ProcessName, ex.Message); }
                }
            }
            catch (Exception ex0)
            {
                Debug.Save("bwUserProcesses_DoWork()", ex0.Message);
            }
        }

        private void bwUserProcesses_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 1; i < ProcessList.Count(); i++)
            {
                try
                {
                    int pos = lvProcessesUser.Items.Add(ProcessList.List[i].Name).Index;
                    lvProcessesUser.Items[pos].SubItems.Add(ProcessList.List[i].Description);

                    // Процессы юзера
                    if (userProcesses.IndexOf(ProcessList.List[i].Name) > -1)
                    {
                        lvProcessesUser.Items[pos].Checked = true;
                        lvProcessesUser.Items[pos].BackColor = Color.LightGreen;
                    }

                    // Глобальные процессы
                    if (Array.IndexOf(ProccessLibrary.Processes(), ProcessList.List[i].Name) > -1)
                    {
                        lvProcessesUser.Items[pos].Checked = true;
                        lvProcessesUser.Items[pos].BackColor = Color.Plum;
                    }
                }
                catch (Exception ex) { Debug.Save("bwUserProcesses_RunWorkerCompleted()", ex.Message); }
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

        private XDocument AddAttributeSettings(XDocument doc, string root, string attr, string value)
        {
            if (doc.Root.Element(root) != null)
            {
                if (doc.Root.Element(root).Attribute(attr) != null)
                    doc.Root.Element(root).Attribute(attr).SetValue(value);
                else
                    doc.Root.Element(root).Add(new XAttribute(attr, value));
            }
            else
            {
                doc.Root.Add(new XElement(root, new XAttribute(attr, value)));
            }

            return doc;
        }

        private async Task SaveSettings()
        {
            try
            {
                bSave.Text = "Сохранение";

                XDocument doc = XDocument.Load(patoToSettings);

                if (doc.Root.Element("info") != null) { doc.Root.Element("info").Attribute("video").SetValue(cbVideo.Checked.ToString()); }
                else { XElement el = new XElement("info", new XAttribute("video", cbVideo.Checked.ToString())); doc.Root.Add(el); }

                AddAttributeSettings(doc, "settings", "kill", cbKillProcesses.Checked.ToString());
                AddAttributeSettings(doc, "settings", "force", cbForceClose.Checked.ToString());
                AddAttributeSettings(doc, "settings", "aero", cbAero.Checked.ToString());
                AddAttributeSettings(doc, "settings", "video", cbVideoQuality.CheckState.ToString());
                AddAttributeSettings(doc, "settings", "weak", cbVideoQualityWeak.Checked.ToString());
                //AddAttributeSettings(doc, "balance", cbBalanceCPU.Checked.ToString());

                AddAttributeSettings(doc, "launcher", "minimize", cbMinimize.SelectedIndex.ToString());
                AddAttributeSettings(doc, "launcher", "background", cbChangeBack.Checked.ToString());

                if (lvProcessesUser.CheckedItems.Count > 0)
                {
                    // Удаляем элемент
                    if (doc.Root.Element("processes") != null) doc.Root.Element("processes").Remove();

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

                doc.Save(patoToSettings);
            }
            catch (Exception ex) { Debug.Save("SaveSettings()", ex.Message); }

            // Сохраняем приоритет в реестр
            try
            {
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions", true);
                key.SetValue("CpuPriorityClass", getPriority(cbPriority.SelectedIndex).ToString(), Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (Exception)
            {
                try
                {
                    var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                    key.SetValue("CpuPriorityClass", getPriority(cbPriority.SelectedIndex).ToString(), Microsoft.Win32.RegistryValueKind.DWord);
                }
                catch (Exception)
                {
                    MessageBox.Show(this, Language.DynamicLanguage("admin", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // Изменяем файл настроек игры
            OptimizeGraphic OptimizeGraphic = new OptimizeGraphic();
            Task.Factory.StartNew(() => OptimizeGraphic.Optimize(commonTest, cbVideoQuality.Checked, cbVideoQualityWeak.Checked)).Wait();


            // Отправляем данные на сайт
            try
            {
                if (lvProcessesUser.CheckedItems.Count > 0)
                {
                    List<string> myJsonData = new List<string>();
                    myJsonData.Add(Debug.Code);

                    foreach (ListViewItem obj in lvProcessesUser.CheckedItems)
                    {
                        if (obj.BackColor != Color.Plum) // Если процесс не является глобальным, то добавляем данные для вывода
                            myJsonData.Add(obj.Text + "|" + obj.SubItems[1].Text);
                    }

                    string json = JsonConvert.SerializeObject(myJsonData);

                    //ProcessList.Send(Properties.Resources.de json);
                }
            }
            catch (Exception ex)
            {
                Debug.Save("bwSave_RunWorkerCompleted()", "Send processes", ex.Message);
            }

            bSave.Text = "Сохранить";
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
            moveForm();

            // Если винда старее Висты, то отключаем пункт
            cbAero.Enabled = EnableAero();
            if (!EnableAero()) cbAero.Checked = false;

            string pathINI = Directory.GetCurrentDirectory() + @"\config.ini";
            lang = new IniFile(pathINI).IniReadValue("protanki", "language");
            lang = lang != "" ? lang : "en";

            loadLang();

            if (!File.Exists(patoToSettings)) new UpdateLauncher().SaveFromResources();
            if (File.Exists(patoToSettings))
            {
                XDocument doc = XDocument.Load(patoToSettings);

                commonTest = doc.Root.Element("common.test") != null;

                try { version = Properties.Resources.ModPackVersion + "." + new IniFile(Directory.GetCurrentDirectory() + @"\config.ini").IniReadValue("protanki", "version"); }
                catch (Exception) { version = doc.Root.Element("version") != null ? doc.Root.Element("version").Value : "0.0.0.0"; }

                type = doc.Root.Element("type") != null ? doc.Root.Element("type").Value : "full";
                notification = doc.Root.Element("notification") != null ? doc.Root.Element("notification").Value : "";

                try
                {
                    var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                    cbPriority.SelectedIndex = getPriority((int)key.GetValue("CpuPriorityClass"), false);
                }
                catch (Exception ex) { cbPriority.SelectedIndex = 2; Debug.Save("public fSettings()", "Priority", ex.Message); }

                cbVideo.Checked = doc.Root.Element("info") != null ? (doc.Root.Element("info").Attribute("video").Value == "True") : false;

                cbKillProcesses.Checked = ReadSettingsStatus(doc, "settings", "kill");
                cbForceClose.Checked = ReadSettingsStatus(doc, "settings", "force");
                cbAero.Checked = ReadSettingsStatus(doc, "settings", "aero");
                cbVideoQuality.CheckState = ReadCheckState(doc, "video");
                cbVideoQualityWeak.Checked = ReadSettingsStatus(doc, "settings", "weak");
                //cbBalanceCPU.Checked = ReadSettingsStatus(doc, "balance");

                cbMinimize.SelectedIndex = ReadIntStatus(doc, "launcher", "minimize");
                cbChangeBack.Checked = ReadSettingsStatus(doc, "launcher", "background", true);

                userProcesses.Clear();
                if (doc.Root.Element("processes") != null) foreach (XElement el in doc.Root.Element("processes").Elements("process")) { userProcesses.Add(el.Attribute("name").Value); }

                if (!bwUserProcesses.IsBusy) { bwUserProcesses.RunWorkerAsync(); }
            }

            try
            {
                //bSave.Text = "Сохранить";
                bSave.Text = Language.DynamicLanguage("save", lang);

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

        private void bBalanceCPU_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(@"..\res_mods\" + Properties.Resources.ModPackVersion + @"\engine_config.xml"))
                {
                    bBalanceCPU.Text = Language.DynamicLanguage("bBalanceCPU1", lang);
                    bBalanceCPU.BackgroundImage = Properties.Resources.lamp_on;
                    File.Delete(@"..\res_mods\" + Properties.Resources.ModPackVersion + @"\engine_config.xml");
                }
                else
                {
                    bBalanceCPU.Text = Language.DynamicLanguage("bBalanceCPU0", lang);
                    bBalanceCPU.BackgroundImage = Properties.Resources.lamp_off;
                    File.WriteAllText(@"..\res_mods\" + Properties.Resources.ModPackVersion + @"\engine_config.xml", Properties.Resources.engine_config);
                }
            }
            catch (Exception ex) { Debug.Save("bBalanceCPU_Click", "Возможно, файл балансировки не найден", ex.Message); }
            finally { }
        }
    }
}
