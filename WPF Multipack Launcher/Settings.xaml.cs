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

                ChangeState(cbNotifyVideo, "notify", "video");
                ChangeState(cbNotifyNews, "notify", "news");

                //ChangeState(cbChangeBack, "settings", "background");

                ChangeIndex(cbGamePriority, "settings", "priority");
                ChangeIndex(cbClosingLauncher, "launcher", "minimize");

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
