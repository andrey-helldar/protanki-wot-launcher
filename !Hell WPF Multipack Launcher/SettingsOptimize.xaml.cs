using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for SettingsOptimize.xaml
    /// </summary>
    public partial class SettingsOptimize : Page
    {
        Classes.Language Lang = new Classes.Language();

        public SettingsOptimize()
        {
            InitializeComponent();

            /*
             * Загружаем заголовки
             */
            cbPriority.Items.Clear();
            for (int i = 0; i < Convert.ToInt16(Lang.Set("Settings", "priority")); i++)
                cbPriority.Items.Add(Lang.Set("Settings", "priority" + i.ToString(), MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value));

            cbLauncher.Items.Clear();
            for (int i = 0; i < Convert.ToInt16(Lang.Set("Settings", "minimize")); i++)
                cbLauncher.Items.Add(Lang.Set("Settings", "minimize" + i.ToString(), MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value));



                /*
                 *  Блок ОПТИМИЗАЦИЯ
                 */

                /*
                 * <settings kill="False" force="False" aero="False" video="False" weak="False" winxp="False" launcher="0" />
                 */
                cbKill.IsChecked = Check("settings", "kill");
            cbForce.IsChecked = Check("settings", "force");
            cbVideo.IsChecked = Check("settings", "video");
            cbWeak.IsChecked = Check("settings", "weak");
            cbAero.IsChecked = Check("settings", "aero");

            // Устанавливаем значение чекбокса лаунчера
            try
            {
                cbLauncher.SelectedIndex = 0;   // Устанавливаем базу

                if (MainWindow.XmlDocument.Root.Element("settings") != null)
                    if (MainWindow.XmlDocument.Root.Element("settings").Attribute("launcher") != null)
                        cbLauncher.SelectedIndex = Convert.ToInt16(MainWindow.XmlDocument.Root.Element("settings").Attribute("launcher").Value);
            }
            catch (Exception ex) { cbLauncher.SelectedIndex = 0; }


            /*
             *  Блок ДРУГОЕ
             */

            /*
             * <info video="True" news="True" multipack="True" notification="0.0.0.0" language="ru">
             */
            cbNotifyVideo.IsChecked = Check("settings", "video");
            cbNotifyNews.IsChecked = Check("settings", "news");
        }

        /// <summary>
        /// Проверяем значение параметра в настройках XML документа
        /// </summary>
        /// <param name="el">Блок элемента</param>
        /// <param name="attr">Проверяемый аттрибут</param>
        /// <returns>
        ///     Если аттрибут задан, выдаем значение TRUE или FALSE,
        ///     во всех прочих случаях возвращаем FALSE
        /// </returns>
        private bool Check(string el, string attr)
        {
            if (MainWindow.XmlDocument.Root.Element(el) != null)
                if (MainWindow.XmlDocument.Root.Element(el).Attribute(attr) != null)
                    if (MainWindow.XmlDocument.Root.Element(el).Attribute(attr).Value == "True")
                        return true;

            return false;
        }
    }
}
