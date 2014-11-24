using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for SettingsOptimize.xaml
    /// </summary>
    public partial class SettingsOptimize : Page
    {
        Classes.Language Lang = new Classes.Language();
        Classes.Debug Debug = new Classes.Debug();

        string lang = Properties.Resources.Default_Lang;


        public SettingsOptimize()
        {
            InitializeComponent();

            Task.Factory.StartNew(() => LoadingPage()).Wait();
        }

        /// <summary>
        /// Асинхронная загрузка параметров формы
        /// </summary>
        private void LoadingPage()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                // Определяем язык интерфейса
                try
                {
                    if (MainWindow.XmlDocument.Root.Element("info") != null)
                        if (MainWindow.XmlDocument.Root.Element("info").Attribute("language") != null)
                            lang = MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value.Trim();
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", ex.Message, ex.StackTrace)); }

                /*
                 * Загружаем заголовки
                 */
                try
                {
                    gbOptimize.Header = Lang.Set("PageSettingsGeneral", "gbOptimize", lang);

                    cbKill.Content = Lang.Set("PageSettingsGeneral", "cbKill", lang);
                    cbForce.Content = Lang.Set("PageSettingsGeneral", "cbForce", lang);
                    cbVideo.Content = Lang.Set("PageSettingsGeneral", "cbVideo", lang);
                    cbWeak.Content = Lang.Set("PageSettingsGeneral", "cbWeak", lang);
                    cbAero.Content = Lang.Set("PageSettingsGeneral", "cbAero", lang);
                    cbKill.Content = Lang.Set("PageSettingsGeneral", "cbKill", lang);

                    gbOther.Header = Lang.Set("PageSettingsGeneral", "gbOther", lang);
                    cbNotifyVideo.Content = Lang.Set("PageSettingsGeneral", "cbNotifyVideo", lang);
                    cbNotifyNews.Content = Lang.Set("PageSettingsGeneral", "cbNotifyNews", lang);

                    gbInterface.Header = Lang.Set("PageSettingsGeneral", "gbInterface", lang);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message+Environment.NewLine+Environment.NewLine+ex.StackTrace); }

                try
                {
                    cbPriority.Items.Clear();
                    for (int i = 0; i < Convert.ToInt16(Lang.Set("PageSettingsGeneral", "priority")); i++)
                        cbPriority.Items.Add(Lang.Set("PageSettingsGeneral", "priority" + i.ToString(), lang));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbPriority.Items", ex.Message, ex.StackTrace)); }

                try
                {
                    cbLauncher.Items.Clear();
                    for (int i = 0; i < Convert.ToInt16(Lang.Set("PageSettingsGeneral", "minimize")); i++)
                        cbLauncher.Items.Add(Lang.Set("PageSettingsGeneral", "minimize" + i.ToString(), lang));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbLauncher.Items", ex.Message, ex.StackTrace)); }

                /*
                 *  Блок ИНТЕРФЕЙС
                 */
                try
                {
                    cbLang.Items.Clear();
                    foreach (var jp in Lang.Translated())
                        cbLang.Items.Add(jp.Value.ToString().Remove(0,3));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbLangPriority.Items", ex.Message, ex.StackTrace)); }

                // Устанавливаем выбранный системой язык
                try
                {
                    switch (lang)
                    {
                        case "en": cbLang.SelectedIndex = 1; break;
                        case "de": cbLang.SelectedIndex = 2; break;
                        default: cbLang.SelectedIndex = 0; break;
                    }
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbLang.Items", ex.Message, ex.StackTrace)); }

                // Заголовок локали
                try { cbLangLocale.Content = Lang.Set("PageSettingsGeneral", "cbLangLocale", lang); }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbLangLocale.Content", ex.Message, ex.StackTrace)); }

                // Приоритет загрузки локализации
                try
                {
                    cbLangPriority.Items.Clear();
                    for (int i = 0; i < Convert.ToInt16(Lang.Set("PageSettingsGeneral", "LangPriority")); i++)
                        cbLangPriority.Items.Add(Lang.Set("PageSettingsGeneral", "cbLangPriority" + i.ToString(), lang));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbLangPriority.Items", ex.Message, ex.StackTrace)); }


                // Устанавливаем выбранный параметр локализации
                try
                {
                    if (MainWindow.XmlDocument.Root.Element("info") != null)
                        if (MainWindow.XmlDocument.Root.Element("info").Attribute("locale") != null)
                            cbLangPriority.SelectedIndex = Convert.ToInt16(MainWindow.XmlDocument.Root.Element("info").Attribute("locale").Value.Trim());
                        else
                            cbLangPriority.SelectedIndex = 0;
                    else
                        cbLangPriority.SelectedIndex = 0;
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbLang.Items", ex.Message, ex.StackTrace)); }

                
                /*
                 *  Блок ОПТИМИЗАЦИЯ
                 */

                /*
                 * <settings kill="False" force="False" aero="False" video="False" weak="False" winxp="False" launcher="0" />
                 */
                try
                {
                    cbKill.IsChecked = Check("settings", "kill");
                    cbForce.IsChecked = Check("settings", "force");
                    cbVideo.IsChecked = Check("settings", "video");
                    cbWeak.IsChecked = Check("settings", "weak");
                    cbAero.IsChecked = Check("settings", "aero");

                    if (Check("settings", "winxp"))
                    {
                        cbAero.IsChecked = false;
                        cbAero.IsEnabled = false;
                    }
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbKill, cbForce ...", ex.Message, ex.StackTrace)); }

                // Определяем приоритет игры в системе
                try
                {
                    var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\WorldOfTanks.exe\PerfOptions");
                    cbPriority.SelectedIndex = getPriority((int)key.GetValue("CpuPriorityClass"), false);
                }
                catch (Exception ex)
                {
                    cbPriority.SelectedIndex = 2;
                    Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "Reestr Reading", ex.Message, ex.StackTrace));
                }
                finally { cbPriority.SelectionChanged += cbPriority_SelectionChanged; }

                // Устанавливаем значение чекбокса лаунчера
                try
                {
                    cbLauncher.SelectedIndex = 0;   // Устанавливаем базу

                    if (MainWindow.XmlDocument.Root.Element("settings") != null)
                        if (MainWindow.XmlDocument.Root.Element("settings").Attribute("launcher") != null)
                            cbLauncher.SelectedIndex = Convert.ToInt16(MainWindow.XmlDocument.Root.Element("settings").Attribute("launcher").Value);
                }
                catch (Exception ex)
                {
                    cbLauncher.SelectedIndex = 0;
                    Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbLauncher.SelectedIndex = 0;", ex.Message, ex.StackTrace));
                }


                /*
                 *  Блок ДРУГОЕ
                 */

                /*
                 * <info video="True" news="True" multipack="True" notification="0.0.0.0" language="ru">
                 */
                try
                {
                    cbNotifyVideo.IsChecked = Check("info", "video");
                    cbNotifyNews.IsChecked = Check("info", "news");
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "LoadingPage()", "cbNotifyVideo.IsChecked, cbNotifyNews.IsChecked", ex.Message, ex.StackTrace)); }
            }));
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
            try
            {
                if (MainWindow.XmlDocument.Root.Element(el) != null)
                    if (MainWindow.XmlDocument.Root.Element(el).Attribute(attr) != null)
                        if (MainWindow.XmlDocument.Root.Element(el).Attribute(attr).Value == "True")
                            return true;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "Check()", "Element: " + el, "Attribute: " + attr, ex.Message, ex.StackTrace)); }

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
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "Set()", "Element: " + el, "Attribute: " + attr, "Value: " + val, ex.Message, ex.StackTrace)); }
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
            try
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
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "getPriority()", "Priority: " + pr.ToString(), "Save: " + save.ToString(), ex.Message, ex.StackTrace));
                return 2;
            }
        }

        private void PageSettingsGeneral_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cbPriority.SelectionChanged += cbPriority_SelectionChanged;
                cbLauncher.SelectionChanged += cbLauncher_SelectionChanged;
                cbLang.SelectionChanged += cbLang_SelectionChanged;
                cbLangPriority.SelectionChanged += cbLangPriority_SelectionChanged;
            }
            catch (Exception) { }

            try { MainWindow.LoadingPanelShow(); }
            catch (Exception) { }
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

        private void cbLauncher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Set("settings", "launcher", cbLauncher.SelectedIndex.ToString());
        }

        private void cbLangPriority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Set("info", "locale", cbLangPriority.SelectedIndex.ToString());
        }

        private void cbLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (cbLang.SelectedIndex)
                {
                    case 1: Set("info", "language", "en"); break;
                    case 2: Set("info", "language", "de"); break;
                    default: Set("info", "language", "ru"); break;
                }
            }
            catch (Exception) { }
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
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("SettingsGeneral.xaml", "cbPriority_SelectionChanged()", ex.Message, ex.StackTrace)); }
            }
        }
    }
}