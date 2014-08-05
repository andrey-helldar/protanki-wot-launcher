using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class General : Page
    {
        Classes.Debug Debug = new Classes.Debug();
        Classes.YoutubeVideo YoutubeClass = new Classes.YoutubeVideo();
        Classes.Wargaming WargamingClass = new Classes.Wargaming();

        public XDocument XmlGeneral = new XDocument();

        string NotifyLink = String.Empty;


        public General()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => ShowNotify("Добро пожаловать!"));

            // Устанавливаем заголовок в зависимости от типа версии
            if (XmlGeneral.Root != null)
                if (XmlGeneral.Root.Element("multipack") != null)
                    if (XmlGeneral.Root.Element("multipack").Element("type") != null)
                        lType.Content = XmlGeneral.Root.Element("multipack").Element("type").Value == "base" ? "Базовая версия" : "Расширенная версия";
                    else
                        lType.Content = "Базовая версия";
                else
                    lType.Content = "Базовая версия";
            else
                lType.Content = "Базовая версия";


            // Загружаем список видео и новостей
            Task[] task = new Task[3];
            task[1] = YoutubeClass.Start();
            task[2] = WargamingClass.Start();
            Task.WaitAll(task);

            Task.Factory.StartNew(() => VideoNotify()); // Выводим уведомления

            // Размещаем список видео на форме
            Task.Factory.StartNew(() =>
            {
                try
                {
                    for (int i = 0; i < YoutubeClass.Count(); i++)
                    {
                        StackPanel panel = new StackPanel();
                        panel.Width = 100;
                        panel.Height = 50;

                        Grid gridItem = new Grid();

                        Label labelDate = new Label();
                        labelDate.Content = YoutubeClass.List[i].Date;
                        gridItem.DataContext = labelDate;

                        Button buttonClose = new Button();
                        buttonClose.Content = "X";
                        gridItem.DataContext = buttonClose;

                        Label labelTitle = new Label();
                        labelTitle.Content = YoutubeClass.List[i].Title;
                        gridItem.DataContext = labelTitle;

                        panel.DataContext = gridItem;
                    }
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "Page_Loaded()", "Apply video to form", ex.Message)); }
            });
        }

        private void VideoNotify()
        {
            try
            {
                if (XmlGeneral.Root.Element("youtube") != null)
                    foreach (var el in XmlGeneral.Root.Element("youtube").Elements("video")) { YoutubeClass.Delete(el.Value); }
                else XmlGeneral.Root.Add(new XElement("youtube", null));

                Task.Factory.StartNew(() => DeleteOldVideo()); // Перед выводом уведомлений проверяем даты. Все лишние удаляем

                foreach (var el in YoutubeClass.List)
                {
                    Thread.Sleep(5000);

                    for (int i = 0; i < 2; i++) // Если цикл прерван случайно, то выжидаем еще 5 секунд перед повторным запуском
                    {
                        while (System.Diagnostics.Process.GetProcessesByName("WorldOfTanks").Length > 0 ||
                            System.Diagnostics.Process.GetProcessesByName("WoTLauncher").Length > 0)
                            Thread.Sleep(5000);

                        Thread.Sleep(5000);
                    }

                    NotifyLink = el.Link;

                    Task.Factory.StartNew(() => ShowNotify("Смотреть видео"));

                    if (XmlGeneral.Root.Element("youtube") != null)
                        XmlGeneral.Root.Element("youtube").Add(new XElement("video", el.ID));
                    else
                        XmlGeneral.Root.Add(new XElement("youtube", new XElement("video", el.ID)));
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "VideoNotify()", ex.Message)); }
        }

        /// <summary>
        /// Если мы удалили 1 пункт из списка, то дальнейший перебор невозможен.
        /// Но используя рекурсию мы повторяем перебор до тех пор, пока все ненужные
        /// элементы не будут удалены из списка. Profit!
        /// </summary>
        /// <returns>Функция как таковая ничего не возвращает</returns>
        private void DeleteOldVideo()
        {
            try
            {
                foreach (var el in YoutubeClass.List)
                    try { if (!YoutubeClass.CheckDate(MainWindow.MultipackDate, el.Date)) YoutubeClass.Delete(el.ID); }
                    catch (Exception) { DeleteOldVideo(); }
            }
            finally { }
        }

        private void ShowNotify(string text, string caption = null)
        {
            try
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    caption = caption != null ? caption : "AAA";
                    MainWindow.Notifier.ShowBalloonTip(5000, caption, text, System.Windows.Forms.ToolTipIcon.Info);
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ShowNotify()", ex.Message, "Caption: " + caption, text)); }
        }
    }
}
