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
        LocalInterface.Language InterfaceLang = new LocalInterface.Language();

        public XDocument doc = new XDocument();
        public string GameVersion = "0.0.0",
                      lang = "en";

        public Settings()
        {
            InitializeComponent();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void SettingsForm_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeState(cbCloseProcess, "optimize", "kill");
                ChangeState(cbForceCloseProcess, "optimize", "forcekill");
                ChangeState(cbLowGraphic, "optimize", "graphic");
                ChangeState(cbVeryWeakPC, "optimize", "weak");
                ChangeState(cbDisableWinAero, "optimize", "aero");

                ChangeState(cbNotifyVideo, "notify", "video");
                ChangeState(cbNotifyNews, "notify", "news");

                ChangeState(cbChangeBack, "settings", "background");

                ChangeIndex(cbGamePriority, "optimize", "priority");
                ChangeIndex(cbClosingLauncher, "settings", "closing");

                cbCpuLoading.Content = InterfaceLang.DynamicLanguage((File.Exists(@"..\res_mods\" + GameVersion + @"\engine_config.xml") ? "cbCpuLoading0" : "cbCpuLoading1"), lang);

                // Применяем язык

            }
            finally { }
        }

        private void ChangeState(CheckBox cb, string elem, string attr)
        {
            try
            {
                if (doc.Root.Element(elem) != null)
                    if (doc.Root.Element(elem).Attribute(attr) != null)
                        cb.IsChecked = doc.Root.Element(elem).Attribute(attr).Value == "True";
            }
            finally { }
        }

        private void ChangeIndex(ComboBox cb, string elem, string attr)
        {
            try
            {
                if (doc.Root.Element(elem) != null)
                    if (doc.Root.Element(elem).Attribute(attr) != null)
                        cb.SelectedIndex = Convert.ToInt16(doc.Root.Element(elem).Attribute(attr).Value);
            }
            catch (Exception) { cb.SelectedIndex = 0; }
        }
    }
}
