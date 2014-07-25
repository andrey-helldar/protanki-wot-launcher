using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        Classes.Language InterfaceLang = new Classes.Language();
        Classes.Debug Debug = new Classes.Debug();

        public XDocument doc = new XDocument();

        public string Lang = "en";
        public Version GameVersion = new Version("0.0.0.0");
        public Version GameVersionBase = new Version("0.0.0");

        public bool SaveData = true;


        public Settings()
        {
            try { InitializeComponent(); }
            catch (Exception ex) { Debug.Save("MainSettings", "Settings()", ex.Message); Close(); }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveData = false;
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

                //ChangeState(cbChangeBack, "settings", "background");

                ChangeIndex(cbGamePriority, "settings", "priority");
                ChangeIndex(cbClosingLauncher, "settings", "launcher");

                cbCpuLoading.Content = InterfaceLang.DynamicLanguage((Directory.Exists(@"..\res_mods\" + GameVersionBase) ? (File.Exists(@"..\res_mods\" + GameVersionBase + @"\engine_config.xml") ? "cbCpuLoading0" : "cbCpuLoading1") : "cbCpuLoading1"), Lang);

                // Применяем язык
                //lCaption.Content = InterfaceLang.DynamicLanguage();
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

        private void cbCpuLoading_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(@"..\res_mods\" + GameVersionBase))
                    Directory.CreateDirectory(@"..\res_mods\" + GameVersionBase);


                if (File.Exists(@"..\res_mods\" + GameVersionBase + @"\engine_config.xml"))
                {
                    cbCpuLoading.Content = InterfaceLang.DynamicLanguage("cbCpuLoading1", Lang);
                    File.Delete(@"..\res_mods\" + GameVersionBase + @"\engine_config.xml");
                }
                else
                {
                    cbCpuLoading.Content = InterfaceLang.DynamicLanguage("cbCpuLoading0", Lang);
                    File.WriteAllText(@"..\res_mods\" + GameVersionBase + @"\engine_config.xml", Properties.Resources.engine_config);
                }
            }
            catch (Exception ex) { Debug.Save("MainSettings", "cbCpuLoading_Click()", ex.Message); }
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
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


            Close();
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
        }
    }
}
