using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Diagnostics;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Page
    {
        Classes.Debugging Debugging = new Classes.Debugging();

        public Update()
        {
            InitializeComponent();

            Task.Factory.StartNew(() => MultipackUpdate());
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (cbNotify.IsChecked == true)
                        MainWindow.JsonSettingsSet("info.notification", MainWindow.JsonSettingsGet("multipack.new_version"));

                    string link = (string)MainWindow.JsonSettingsGet("multipack.link");
                    if (link != String.Empty) Process.Start(link);

                    MainWindow.JsonSettingsSet("info.session", System.Diagnostics.Process.GetCurrentProcess().Id, "int");
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Update.xaml", "bUpdate_Click()", ex.Message, ex.StackTrace)); }
            });
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    MainWindow.JsonSettingsSet("info.session", System.Diagnostics.Process.GetCurrentProcess().Id, "int");
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Update.xaml", "bCancel_Click()", ex.Message, ex.StackTrace)); }
            });
        }

        private void MultipackUpdate()
        {
            try
            {
                Classes.Language Lang = new Classes.Language();
                string lang = (string)MainWindow.JsonSettingsGet("info.language");

                /*
                 *      Применяем локализацию интерфейса
                 */
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    gbCaption.Content = Lang.Set("PageUpdate", "gbCaption", lang);
                    lDownloadFromLink.Content = Lang.Set("PageUpdate", "lDownloadFromLink", lang);
                    cbNotify.Content = Lang.Set("PageUpdate", "cbNotify", lang);
                    bUpdate.Content = Lang.Set("PageUpdate", "bUpdate", lang);
                    bCancel.Content = Lang.Set("PageUpdate", "bCancel", lang);

                    /*
                     *      Подгружаем другие данные
                     */
                    newVersion.Content = new Classes.Variables().VersionSharp((string)MainWindow.JsonSettingsGet("multipack.new_version"), false);
                    tbContent.Text = ParseChangelog((string)MainWindow.JsonSettingsGet("multipack.changelog"));
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Update.xaml", "MultipackUpdate()", ex.Message, ex.StackTrace)); }
        }

        private string ParseChangelog(string log)
        {
            try
            {
                if (log.Length > 0)
                {
                    /*
                     * В этой версии произошли следующие изменения:
                     * <font size='12' color='808080'>
                     *  <ul>
                     *      <li>Обновлен мод Received Damage Announcer от PROТанки, добавлена функция автоинформирования команды о том, что артиллерия отстрелялась... по вам :( ;</li>
                     *      <li>Обновлен мод анонсов о стримах с розыгрышами игрового золота, мод стал лучше и теперь можно выбрать режим \"Не беспокоить\", в котором мод не будет напоминать о стриме;</li>
                     *      <li>Обновлена конфигурация послебоевых сообщение от Armagomen;</li>
                     *      <li>Обновлена конфигурация сессионной статистики от Armagomen;</li>
                     *      <li>Обновлена конфигурация послебоевых сообщение от Meddio;</li>
                     *      <li>Обновлена конфигурация сессионной статистики от Meddio.</li>
                     *      <li>__________________</li>
                     *      <li>Буду благодарен, если Вы еще раз посмотрите рекламный ролик в начале видео.</li>
                     *  </ul>
                     * </font>__________________________________________<br>
                     * <font color='#E00000'>Новая версия доступна по ссылке в описании к видео.</font>
                     */

                    /*
                     * Удаляем все после данного тега, включая его сам
                     * Тем самым сокращаем время работы регулярных выражений
                     */
                    log = log.Remove(log.IndexOf("</font>"));

                    // Доп обработки строк
                    log = log.Replace("<ul>", Environment.NewLine + Environment.NewLine);
                    log = log.Replace("</ul>", "");

                    // Удаляем все теги <font>
                    Regex regex = new Regex(@"<font(.*)>");
                    Match match = regex.Match(log);
                    while (match.Success)
                    {
                        try { log = log.Replace(match.Value, ""); }
                        catch (Exception) { }
                        match = match.NextMatch();
                    }

                    /*
                     *  Обрабатываем элементы списка
                     */
                    Regex regexLI = new Regex(@"<li>(.*)</li>");
                    Match matchLI = regexLI.Match(log);
                    while (matchLI.Success)
                    {
                        try { log = " * " + matchLI.Value.Replace("<li>", "").Replace("</li>", "") + Environment.NewLine; }
                        catch (Exception) { }
                        matchLI = matchLI.NextMatch();
                    }

                    MessageBox.Show(log);
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("Update.xaml", "ParseChangelog()", ex.Message, ex.StackTrace)); }
            return log;
        }

        private void PageUpdate_Loaded(object sender, RoutedEventArgs e)
        {
            try { MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden; }
            catch (Exception) { }
        }
    }
}
