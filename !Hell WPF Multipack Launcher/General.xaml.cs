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
using System.Diagnostics;

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
            Task.WaitAll(new Task[]{
                Task.Factory.StartNew(() => YoutubeClass.Start()),
                Task.Factory.StartNew(() => WargamingClass.Start())                
            });

            Task.WaitAll(new Task[]{
                Task.Factory.StartNew(() => ViewNews()),
                Task.Factory.StartNew(() => ViewNews(false))
            });

            Task.Factory.StartNew(() => VideoNotify()); // Выводим уведомления
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => ShowNotify("Добро пожаловать!"));
        }

        /// <summary>
        /// Размещаем блоки с информацией по видео на форме
        /// </summary>
        private void ViewNews(bool youtube = true)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        for (int i = 0; i < (youtube ? YoutubeClass.Count() : WargamingClass.Count()); i++)
                        {
                            // Добавляем решетку для размещения элементов
                            Grid gridPanel = new Grid();
                            /*gridPanel.Width = double.NaN;*/
                            gridPanel.Height = 70;
                            gridPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                            /*gridPanel.VerticalAlignment = System.Windows.VerticalAlignment.Top;*/
                            gridPanel.Name = String.Format("Panel_{0}_{1}", i.ToString(), youtube.ToString()).ToLower();

                            RowDefinition gridRow1 = new RowDefinition();
                            gridRow1.Height = new GridLength(30);
                            gridPanel.RowDefinitions.Add(gridRow1);
                            gridPanel.RowDefinitions.Add(new RowDefinition());

                            ColumnDefinition gridColumn1 = new ColumnDefinition();
                            gridColumn1.Width = new GridLength(100);
                            gridPanel.ColumnDefinitions.Add(gridColumn1);
                            gridPanel.ColumnDefinitions.Add(new ColumnDefinition());

                            // Добавляем дату
                            Label labelDate = new Label();
                            labelDate.Height = double.NaN;
                            labelDate.Margin = new Thickness(0, 0, 0, 0);
                            labelDate.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            labelDate.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                            try { labelDate.Content = youtube ? YoutubeClass.List[i].Date : WargamingClass.List[i].Date; }
                            catch (Exception) { labelDate.Content = "1970-1-1"; }
                            Grid.SetRow(labelDate, 0);
                            Grid.SetColumn(labelDate, 0);
                            gridPanel.Children.Add(labelDate);

                            // Кнопка "Закрыть"
                            TextBlock blockClose = new TextBlock();
                            blockClose.Width = 20;
                            blockClose.Height = double.NaN;
                            blockClose.Margin = new Thickness(0);
                            blockClose.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            blockClose.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

                            // Гиперссылка для кнопки "Закрыть"
                            Hyperlink hyperClose = new Hyperlink(new Run("X"));
                            hyperClose.NavigateUri = new Uri("http://" + gridPanel.Name);
                            hyperClose.RequestNavigate += new RequestNavigateEventHandler(CloseBlock_RequestNavigate);
                            blockClose.Inlines.Clear();
                            blockClose.Inlines.Add(hyperClose);
                            Grid.SetRow(blockClose, 0);
                            Grid.SetColumn(blockClose, 1);
                            gridPanel.Children.Add(blockClose);

                            // Добавляем заголовок в гиперссылку
                            TextBlock blockTitle = new TextBlock();
                            blockTitle.TextWrapping = TextWrapping.Wrap;
                            blockTitle.Width = double.NaN;
                            blockTitle.Height = double.NaN;
                            blockTitle.Margin = new Thickness(0, 0, 0, 0);
                            blockTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                            blockTitle.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

                            TextBlock hyperText = new TextBlock();
                            hyperText.Text = youtube ? YoutubeClass.List[i].Title : WargamingClass.List[i].Title;
                            hyperText.TextWrapping = TextWrapping.WrapWithOverflow;
                            hyperText.Margin = new Thickness(2);
                            hyperText.Width = double.NaN;
                            hyperText.Height = double.NaN;
                            hyperText.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                            hyperText.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            
                            // Гиперссылка для заголовка
                            //Hyperlink hyperlink = new Hyperlink(new Run(hyperText.Text));
                            Hyperlink hyperlink = new Hyperlink();
                            //Hyperlink hyperlink = new Hyperlink(new Run(youtube ? YoutubeClass.List[i].Title : WargamingClass.List[i].Title));
                            hyperlink.NavigateUri = new Uri(youtube ? YoutubeClass.List[i].Link : WargamingClass.List[i].Link);
                            hyperlink.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_RequestNavigate);
                            hyperlink.Inlines.Add(hyperText);
                            blockTitle.Inlines.Add(hyperlink.Inlines.FirstInline);

                            Grid.SetRow(blockTitle, 1);
                            Grid.SetColumn(blockTitle, 0);
                            Grid.SetColumnSpan(blockTitle, 2);
                            gridPanel.Children.Add(blockTitle);

                            /*try { this.Dispatcher.BeginInvoke(new ThreadStart(delegate { svVideo.Content = panel; })); }
                            catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "Page_Loaded()", "Apply video to form", ex0.Message, ex0.StackTrace)); }*/

                            ListBoxItem lbi = new ListBoxItem();
                            lbi.Content = gridPanel;
                            
                            try
                            {
                                if (youtube)
                                    lbVideo.Items.Add(lbi);
                                else
                                    lbNews.Items.Add(lbi);
                            }
                            catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", ex0.Message, ex0.StackTrace)); }
                        }
                    }));

                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", ex.Message, ex.StackTrace)); }
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
                        try
                        {
                            while (System.Diagnostics.Process.GetProcessesByName("WorldOfTanks").Length > 0 ||
                                System.Diagnostics.Process.GetProcessesByName("WoTLauncher").Length > 0)
                                Thread.Sleep(5000);

                            Thread.Sleep(5000);
                        }
                        catch (Exception) { }
                    }

                    try
                    {
                        NotifyLink = el.Link;

                        Task.Factory.StartNew(() => ShowNotify("Смотреть видео"));

                        if (XmlGeneral.Root.Element("youtube") != null)
                            XmlGeneral.Root.Element("youtube").Add(new XElement("video", el.ID));
                        else
                            XmlGeneral.Root.Add(new XElement("youtube", new XElement("video", el.ID)));
                    }
                    catch (Exception) { }
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
                    caption = caption != null ? caption : MainWindow.ProductName;
                    MainWindow.Notifier.ShowBalloonTip(5000, caption, text, System.Windows.Forms.ToolTipIcon.Info);
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ShowNotify()", ex.Message, "Caption: " + caption, text)); }
        }

        /// <summary>
        ////Открываем гиперссылку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Параметры</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "Hyperlink_RequestNavigate()", ex.Message, "Link: " + e.Uri.AbsoluteUri)); }
        }


        private void CloseBlock_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                MessageBox.Show("Инициировано удаление записи с идентификатором: " + e.Uri.AbsoluteUri);
                e.Handled = true;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "CloseBlock_RequestNavigate()", ex.Message, "Action: " + e.Uri.AbsoluteUri)); }
        }
    }
}
