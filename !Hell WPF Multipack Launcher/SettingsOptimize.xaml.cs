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

            if(Check("settings", "winxp")){
                cbAero.IsChecked = false;
                cbAero.IsEnabled = false;
            }

            // Определяем приоритет игры в системе
            try
            {
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                cbPriority.SelectedIndex = getPriority((int)key.GetValue("CpuPriorityClass"), false);
            }
            catch (Exception ex) { cbPriority.SelectedIndex = 2;/* Debug.Save("public fSettings()", "Priority", ex.Message);*/ }
            finally { cbPriority.SelectionChanged += cbPriority_SelectionChanged; }

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
            cbNotifyVideo.IsChecked = Check("info", "video");
            cbNotifyNews.IsChecked = Check("info", "news");
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

        /// <summary>
        /// Изменяем значения параметров
        /// </summary>
        /// <param name="el">Блок элемента</param>
        /// <param name="attr">Проверяемый аттрибут</param>
        /// <param name="val">Значение</param>
        private void Set(string el, string attr, string val)
        {
            try
            {
                if (MainWindow.XmlDocument.Root.Element(el) != null)
                    if (MainWindow.XmlDocument.Root.Element(el).Attribute(attr) != null)
                        MainWindow.XmlDocument.Root.Element(el).Attribute(attr).SetValue(val);
            }
            catch (Exception ex) { }
        }

        /// <summary>
        ///  Загрузка и сохранение приоритета в системе
        /// </summary>
        /// <param name="pr">Компонент для считывания/сохранения результата</param>
        /// <param name="save">Сохранять или загружать?</param>
        /// <returns>Возвращает идентификатор приоритета</returns>
        private int getPriority(int pr, bool save = true)
        {
            /*
             * case "priority0": return "Высокий";
             * case "priority1": return "Выше среднего";
             * case "priority2": return "Средний";
             * case "priority3": return "Ниже среднего";
             * case "priority4": return "Низкий";
             */ 

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

        private void cbKill_Click(object sender, RoutedEventArgs e)
        {
            Set("settings", "kill", cbKill.IsChecked.ToString());
        }

        private void cbForce_Click(object sender, RoutedEventArgs e)
        {
            Set("settings", "force", cbForce.IsChecked.ToString());
        }

        private void cbVideo_Click(object sender, RoutedEventArgs e)
        {
            Set("settings", "video", cbVideo.IsChecked.ToString());
        }

        private void cbWeak_Click(object sender, RoutedEventArgs e)
        {
            Set("settings", "weak", cbWeak.IsChecked.ToString());
        }

        private void cbAero_Click(object sender, RoutedEventArgs e)
        {
            Set("settings", "aero", cbAero.IsChecked.ToString());
        }

        private void cbNotifyVideo_Click(object sender, RoutedEventArgs e)
        {
            Set("info", "video", cbNotifyVideo.IsChecked.ToString());
        }

        private void cbNotifyNews_Click(object sender, RoutedEventArgs e)
        {
            Set("info", "news", cbNotifyNews.IsChecked.ToString());
        }

        private void cbPriority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                    
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
                    //MessageBox.Show(this, Language.DynamicLanguage("admin", lang), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void cbLauncher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Set("settings", "launcher", cbLauncher.SelectedIndex.ToString());
        }
    }
}
