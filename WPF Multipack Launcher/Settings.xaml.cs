using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Processes_Library;

namespace WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        Classes.Language InterfaceLang = new Classes.Language();
        Classes.Debug Debug = new Classes.Debug();
        ProcessesLibrary ProccessLibrary = new ProcessesLibrary();
        ProcessList ProcessList = new ProcessList();

        public XDocument doc = new XDocument();

        public string Lang = Properties.Resources.Default_Lang;

        public Version GameVersion = new Version("0.0.0.0");
        public Version GameVersionBase = new Version("0.0.0");


        public Settings()
        {
            try { InitializeComponent(); MouseDown += delegate { DragMove(); }; }
            catch (Exception ex) { Debug.Save("MainSettings", "Settings()", ex.Message); Debug.Restart(); }
        }

        private void lClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void MainSettings_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GameVersionBase = new Version(String.Format("{0}.{1}.{2}", GameVersion.Major, GameVersion.Minor, GameVersion.Build));

                ChangeState(cbCloseProcess, "settings", "kill");
                ChangeState(cbForceCloseProcess, "settings", "force");
                ChangeState(cbLowGraphic, "settings", "video");
                ChangeState(cbVeryWeakPC, "settings", "weak");
                ChangeState(cbDisableWinAero, "settings", "aero");

                ChangeState(cbNotifyVideo, "info", "video");
                ChangeState(cbNotifyNews, "info", "news");
                ChangeState(cbNotifyPack, "info", "multipack");

                ChangeIndex(cbGamePriority, "settings", "priority");
                ChangeIndex(cbClosingLauncher, "settings", "launcher");

                // Применяем язык
                //lCaption.Content = InterfaceLang.DynamicLanguage();


                Task.Factory.StartNew(() => LoadModules()); // Загружаем список модулей
                Task.Factory.StartNew(() => LoadProcesses()); // Загружаем список модулей
            }
            catch (Exception ex) { Debug.Save("MainSettings", "MainSettings_Loaded()", ex.Message); }
        }

        private void ChangeState(CheckBox cb, string elem, string attr)
        {
            try
            {
                if (doc.Root.Element(elem) != null)
                {
                    if (doc.Root.Element(elem).Attribute(attr) != null)
                        cb.IsChecked = doc.Root.Element(elem).Attribute(attr).Value == "True";
                    else
                        cb.IsChecked = true;
                }
                else
                    cb.IsChecked = true;
            }
            catch (Exception ex) { Debug.Save("MainSettings", "ChangeState()", ex.Message, "Element name: " + cb.Name, "XML Element: " + elem, "XML Attribute: " + attr); }
        }

        private void ChangeIndex(ComboBox cb, string elem, string attr)
        {
            try
            {
                if (doc.Root.Element(elem) != null)
                    if (doc.Root.Element(elem).Attribute(attr) != null)
                        cb.SelectedIndex = Convert.ToInt16(doc.Root.Element(elem).Attribute(attr).Value);
            }
            catch (Exception ex)
            {
                Debug.Save("MainSettings", "ChangeIndex()", ex.Message, "Element name: " + cb.Name, "XML Element: " + elem, "XML Attribute: " + attr);
                cb.SelectedIndex = 0;
            }
        }

        private void SetXML(string elem, string attr, string value)
        {
            try
            {
                if (doc.Root.Element(elem) != null)
                {
                    if (doc.Root.Element(elem).Attribute(attr) != null)
                        doc.Root.Element(elem).Attribute(attr).SetValue(value);
                    else
                        doc.Root.Element(elem).Add(new XAttribute(attr, value));
                }
                else
                    doc.Root.Add(new XElement(elem, new XAttribute(attr, value)));
            }
            catch (Exception ex) { Debug.Save("MainSettings", "SetXML", ex.Message, "Element: " + elem, "Attribute: " + attr, "Value: " + value); }
        }

        private int GetPriority(int pr, bool save = true)
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

        private void LoadModules()
        {
            try
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    Action<string> addInListAction = (string text) =>
                           {
                               if (System.IO.Path.GetFileName(text).IndexOf(Application.Current.GetType().Assembly.GetName().Name) == -1)
                                   try { lvModules.Items.Add(new { Name = System.IO.Path.GetFileName(text), Version = FileVersionInfo.GetVersionInfo(text).FileVersion }); }
                                   catch (Exception) { lvModules.Items.Add(new { Name = System.IO.Path.GetFileName(text), Version = "---" }); }
                           };

                    SearchFilesInDirectory(Environment.CurrentDirectory, new List<string>() { "*.dll", "*.exe" }, addInListAction);
                }));
            }
            catch (Exception ex) { Debug.Save("MainSettings", "LoadModules()", ex.Message); };
        }

        private void MainSettings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetXML("settings", "kill", cbCloseProcess.IsChecked.ToString()); // Закрывать процессы
            SetXML("settings", "force", cbForceCloseProcess.IsChecked.ToString()); // Принудительно закрывать процессы
            SetXML("settings", "video", cbLowGraphic.IsChecked.ToString()); // Графика
            SetXML("settings", "weak", cbVeryWeakPC.IsChecked.ToString()); // Очень слабый ПК
            SetXML("settings", "aero", cbDisableWinAero.IsChecked.ToString()); // WinAero

            SetXML("settings", "launcher", cbClosingLauncher.SelectedIndex.ToString()); // Действия с лаунчером при запуске игры

            SetXML("info", "video", cbNotifyVideo.IsChecked.ToString()); // Уведомлять о новых видео
            SetXML("info", "news", cbNotifyNews.IsChecked.ToString()); // Уведомлять о новых новостях
            SetXML("info", "multipack", cbNotifyPack.IsChecked.ToString()); // Уведомлять о новых версиях мультипака

            /*******************************
             * Сохраняем приоритет в реестр
             *******************************/
            try
            {
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions", true);
                key.SetValue("CpuPriorityClass", GetPriority(cbGamePriority.SelectedIndex).ToString(), Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (Exception)
            {
                try
                {
                    var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                    key.SetValue("CpuPriorityClass", GetPriority(cbGamePriority.SelectedIndex).ToString(), Microsoft.Win32.RegistryValueKind.DWord);
                }
                catch (Exception ex) { Debug.Save("MainSettings", "bSave_Click()", ex.Message); }
            }

            /*******************************
             * Сохраняем список процессов
             *******************************/
            /*if (lvProcessList.CheckedItems.Count > 0)
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
            }*/



            /*******************************
             * Изменяем файл настроек игры
             *******************************/
            /* OptimizeGraphic OptimizeGraphic = new OptimizeGraphic();
             Task.Factory.StartNew(() => OptimizeGraphic.Optimize(commonTest, cbVideoQuality.Checked, cbVideoQualityWeak.Checked)).Wait();*/



            /*******************************
             * Отправляем данные на сайт
             *******************************/
            /*try
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
            }*/


        }

        private void SearchFilesInDirectory(String directory, IEnumerable<string> searchPatternList, Delegate method)
        {
            try
            {
                if ((searchPatternList == null) || (searchPatternList.Count<string>() == 0))
                    throw new ArgumentNullException("searchPatternList");
                foreach (string searchPattern in searchPatternList)
                    using (var iterator = Directory.EnumerateFiles(directory, searchPattern).GetEnumerator())
                        try
                        { while (iterator.MoveNext()) Application.Current.Dispatcher.BeginInvoke(method, iterator.Current); }
                        finally { }

                using (var iterator = Directory.EnumerateDirectories(directory).GetEnumerator())
                    while (iterator.MoveNext())
                        try { SearchFilesInDirectory(iterator.Current, searchPatternList, method); }
                        finally { }
            }
            catch (Exception ex) { Debug.Save("MainSettings", "SearchFilesInDirectory()", ex.Message, "Directory: " + directory); }
        }

        /// <summary>
        /// http://msdn.microsoft.com/ru-ru/library/ms750972(v=vs.100).aspx
        /// </summary>
        private void LoadProcesses()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    try
                    {
                        lvProcessList.Items.Clear();

                        Process[] myProcesses = Process.GetProcesses();
                        int processID = Process.GetCurrentProcess().SessionId;

                        foreach (var process in myProcesses)
                            if (process.SessionId == processID)
                                if (!ProcessList.IndexOf(process.ProcessName) && process.ProcessName != Process.GetCurrentProcess().ProcessName)
                                    try { lvProcessList.Items.Add(new { Status = "---", Name = process.ProcessName, Description = process.MainModule.FileVersionInfo.FileDescription.Trim() }); }
                                    catch (Exception) { }
                    }
                    finally { }
                }));
        }

    }

    public class CheckBoxListViewItem : INotifyPropertyChanged { private bool isChecked; private string text; public bool IsChecked { get { return isChecked; } set { if (isChecked == value) return; isChecked = value; RaisePropertyChanged("IsChecked"); } } public String Text { get { return text; } set { if (text == value) return; text = value; RaisePropertyChanged("Text"); } } public CheckBoxListViewItem(string t, bool c) { this.Text = t; this.IsChecked = c; } public event PropertyChangedEventHandler PropertyChanged; private void RaisePropertyChanged(string propName) { PropertyChangedEventHandler eh = PropertyChanged; if (eh != null) { eh(this, new PropertyChangedEventArgs(propName)); } } }
}
