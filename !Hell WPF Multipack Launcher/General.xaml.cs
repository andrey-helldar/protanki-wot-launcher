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

        string NotifyLink = String.Empty;


        public General()
        {
            InitializeComponent();

            string player = "";
            if (MainWindow.XmlDocument.Root.Element("info") != null)
                if (MainWindow.XmlDocument.Root.Element("info").Attribute("player") != null)
                    if (MainWindow.XmlDocument.Root.Element("info").Attribute("player").Value != "")
                    player = ", " + MainWindow.XmlDocument.Root.Element("info").Attribute("player").Value;

            Task.Factory.StartNew(() => ShowNotify("Добро пожаловать" + player + "!", "", false));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Устанавливаем заголовок в зависимости от типа версии
            //lType.Content = "Базовая версия";

            /*if (MainWindow.XmlDocument.Root != null)
                if (MainWindow.XmlDocument.Root.Element("multipack") != null)
                    if (MainWindow.XmlDocument.Root.Element("multipack").Element("type") != null)
                        lType.Content = MainWindow.XmlDocument.Root.Element("multipack").Element("type").Value == "base" ? "Базовая версия" : "Расширенная версия";*/


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

        /// <summary>
        /// Размещаем блоки с информацией по видео на форме
        /// </summary>
        private void ViewNews(bool youtube = true)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    int count = youtube ? YoutubeClass.Count() : WargamingClass.Count();

                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {

                        if (count == 0)
                        { // Если список пуст, то добавляем одну строку с уведомлением
                            Grid gridPanel = new Grid();
                            gridPanel.Width = double.NaN;
                            gridPanel.Margin = new Thickness(0);
                            gridPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                            Label labelNotify = new Label();
                            labelNotify.Height = double.NaN;
                            labelNotify.Margin = new Thickness(0);
                            labelNotify.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                            labelNotify.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                            labelNotify.Content = "Записи не обнаружены";
                            Grid.SetRow(labelNotify, 0);
                            Grid.SetColumn(labelNotify, 0);
                            gridPanel.Children.Add(labelNotify);

                            ListBoxItem lbi = new ListBoxItem();
                            lbi.SetResourceReference(TextBlock.StyleProperty, "ListBoxItemGeneral");
                            lbi.Content = gridPanel;

                            try { if (youtube) lbVideo.Items.Add(lbi); else lbNews.Items.Add(lbi); }
                            catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Count " + (youtube ? "VIDEO" : "NEWS") + " is "+count.ToString(), ex0.Message, ex0.StackTrace)); }

                        }
                        else
                            for (int i = 0; i < count; i++)
                            {
                                try
                                {
                                    // Добавляем решетку для размещения элементов
                                    Grid gridPanel = new Grid();
                                    gridPanel.Width = double.NaN;
                                    /*gridPanel.Height = 70;*/
                                    gridPanel.Margin = new Thickness(0);
                                    gridPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                                    /*gridPanel.VerticalAlignment = System.Windows.VerticalAlignment.Top;*/

                                    RowDefinition gridRow1 = new RowDefinition();
                                    gridRow1.Height = new GridLength(30);
                                    gridPanel.RowDefinitions.Add(gridRow1);
                                    gridPanel.RowDefinitions.Add(new RowDefinition());

                                    ColumnDefinition gridColumn1 = new ColumnDefinition();
                                    gridColumn1.Width = GridLength.Auto;
                                    gridPanel.ColumnDefinitions.Add(gridColumn1);
                                    gridPanel.ColumnDefinitions.Add(new ColumnDefinition());

                                    // Добавляем дату
                                    Label labelDate = new Label();
                                    labelDate.Height = double.NaN;
                                    labelDate.Margin = new Thickness(0, 0, 0, 0);
                                    labelDate.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                    labelDate.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                                    try { labelDate.Content = youtube ? YoutubeClass.List[i].DateShort : WargamingClass.List[i].DateShort; }
                                    catch (Exception) { labelDate.Content = "1970-1-1"; }
                                    Grid.SetRow(labelDate, 0);
                                    Grid.SetColumn(labelDate, 0);
                                    gridPanel.Children.Add(labelDate);

                                    // Кнопка "Воспроизвести"
                                    Button buttonPlay = new Button();
                                    buttonPlay.Content = ">";
                                    buttonPlay.Name = (youtube ? "PlayYoutube_" : "PlayWargaming_") + i.ToString();
                                    buttonPlay.Width = 20;
                                    buttonPlay.Height = double.NaN;
                                    buttonPlay.Margin = new Thickness(20, 0, 0, 0);
                                    buttonPlay.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                    buttonPlay.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                                    buttonPlay.Click += PlayPreview;
                                    Grid.SetRow(buttonPlay, 0);
                                    Grid.SetColumn(buttonPlay, 1);
                                    gridPanel.Children.Add(buttonPlay);

                                    // Кнопка "Закрыть"
                                    Button buttonClose = new Button();
                                    buttonClose.Content = "X";
                                    buttonClose.Name = (youtube ? "CloseYoutube_" : "CloseWargaming_") + i.ToString();
                                    buttonClose.Width = 20;
                                    buttonClose.Height = double.NaN;
                                    buttonClose.Margin = new Thickness(50, 0, 0, 0);
                                    buttonClose.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                    buttonClose.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                                    buttonClose.Click += CloseBlock;
                                    Grid.SetRow(buttonClose, 0);
                                    Grid.SetColumn(buttonClose, 1);
                                    gridPanel.Children.Add(buttonClose);

                                    // Добавляем заголовок в гиперссылку
                                    TextBlock blockTitle = new TextBlock();
                                    blockTitle.TextWrapping = TextWrapping.Wrap;
                                    blockTitle.Width = double.NaN;
                                    blockTitle.Height = double.NaN;
                                    blockTitle.Padding = new Thickness(5);
                                    blockTitle.Margin = new Thickness(0);
                                    blockTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                                    blockTitle.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

                                    // Добавляем идентификатор записи
                                    Hyperlink hyperID = new Hyperlink(new Run(""));
                                    hyperID.NavigateUri = new Uri(youtube ? "http://" + YoutubeClass.List[i].ID : WargamingClass.List[i].Link);
                                    hyperID.Name = (youtube ? "LinkYoutube_" : "LinkWargaming_") + i.ToString();
                                    this.RegisterName(hyperID.Name, hyperID);

                                    // Гиперссылка для заголовка
                                    Hyperlink hyperlink = new Hyperlink(new Run(youtube ? YoutubeClass.List[i].Title : WargamingClass.List[i].Title));
                                    hyperlink.NavigateUri = new Uri(youtube ? YoutubeClass.List[i].Link : WargamingClass.List[i].Link);
                                    hyperlink.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_RequestNavigate);
                                    blockTitle.Inlines.Add(hyperlink);
                                    blockTitle.Inlines.Add(hyperID);

                                    Grid.SetRow(blockTitle, 1);
                                    Grid.SetColumn(blockTitle, 0);
                                    Grid.SetColumnSpan(blockTitle, 2);
                                    gridPanel.Children.Add(blockTitle);

                                    /*try { this.Dispatcher.BeginInvoke(new ThreadStart(delegate { svVideo.Content = panel; })); }
                                    catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "Page_Loaded()", "Apply video to form", ex0.Message, ex0.StackTrace)); }*/

                                    ListBoxItem lbi = new ListBoxItem();
                                    lbi.SetResourceReference(TextBlock.StyleProperty, "ListBoxItemGeneral");
                                    lbi.Content = gridPanel;

                                    try
                                    {
                                        if (youtube)
                                            lbVideo.Items.Add(lbi);
                                        else
                                            lbNews.Items.Add(lbi);

                                        Thread.Sleep(50);
                                    }
                                    catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", ex0.Message, ex0.StackTrace)); }
                                }
                                catch (Exception ex1) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", "FOR: " + i.ToString(), ex1.Message, ex1.StackTrace)); }
                            }

                        Thread.Sleep(Convert.ToInt16(Properties.Resources.Sleeping_News));
                    }));

                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", ex.Message, ex.StackTrace)); }
            });
        }

        private void VideoNotify()
        {
            try
            {
                if (MainWindow.XmlDocument.Root.Element("youtube") != null)
                    foreach (var el in MainWindow.XmlDocument.Root.Element("youtube").Elements("video")) { YoutubeClass.Delete(el.Value); }
                else MainWindow.XmlDocument.Root.Add(new XElement("youtube", null));

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

                        Task.Factory.StartNew(() => ShowNotify("Смотреть видео", el.Title));

                        if (MainWindow.XmlDocument.Root.Element("youtube") != null)
                            MainWindow.XmlDocument.Root.Element("youtube").Add(new XElement("video", el.ID));
                        else
                            MainWindow.XmlDocument.Root.Add(new XElement("youtube", new XElement("video", el.ID)));
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
            catch (Exception) { }
        }

        private void ShowNotify(string text, string caption = null, bool isPopup = true)
        {
            try
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    if (isPopup)
                    {
                        try
                        {
                            caption = caption != null ? caption : MainWindow.ProductName;
                            MainWindow.Notifier.ShowBalloonTip(5000, caption, text, System.Windows.Forms.ToolTipIcon.Info);
                        }
                        catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ShowNotify()", ex0.Message, "Caption: " + caption, text)); }
                    }
                    else
                    {
                        lStatus.Text = text;
                    }
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


        /// <summary>
        /// Функция скрытия блока с записью новости/видео
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseBlock(object sender, RoutedEventArgs e)
        {
            try
            {
                ListBoxItem el = (((sender as Button).Parent as Grid).Parent as ListBoxItem);
                ListBox lb = ((((sender as Button).Parent as Grid).Parent as ListBoxItem).Parent as ListBox);

                string[] arr = (sender as Button).Name.Split('_');
                
                switch(arr[0]){
                    case "CloseWargaming":
                        Hyperlink elem = Find(el, "LinkWargaming_" + arr[1]);
                        if (elem != null) ElementToBan("news", elem.NavigateUri.AbsoluteUri);
                        break;

                    default:
                        Hyperlink elemY = Find(el, "LinkYoutube_" + arr[1]);
                        
                        if (elemY != null)
                        {
                            string[] item = elemY.NavigateUri.AbsoluteUri.Split('/');
                            ElementToBan("video", item[2].ToUpper());
                        }
                        break;
                }
                     
                el.IsSelected = true;
                lb.Items.Remove(lb.SelectedItem);
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "CloseBlock()", ex.Message)); }
        }

        /// <summary>
        /// Функция воспроизведения превью видео
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayPreview(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OK");
        }

        public bool ElementToBan(string block, string item)
        {
            if (MainWindow.XmlDocument.Root.Element("do_not_display") != null)
                if (MainWindow.XmlDocument.Root.Element("do_not_display").Element(block) != null)
                    if (MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Element("item") != null)
                        if (MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Elements("item").Count() > 0)
                        {
                            foreach (string str in MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Elements("item"))
                                if (str == item) return true;

                            MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Add(new XElement("item", item));
                        }
                        else
                            MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Element("item").SetValue(item);
                    else
                        MainWindow.XmlDocument.Root.Element("do_not_display").Element(block).Add(new XElement("item", item));
                else
                    MainWindow.XmlDocument.Root.Element("do_not_display").Add(new XElement(block, new XElement("item", item)));
            else
                MainWindow.XmlDocument.Root.Add(new XElement("do_not_display", new XElement(block, new XElement("item", item))));

            return true;
        }

        /// <summary>
        /// Ищем элемент по имени
        /// </summary>
        /// <param name="sender">Передаем строку ListBox</param>
        /// <param name="name">Передаем имя искомого элемента</param>
        /// <returns>Возвращаем элемент</returns>
        private Hyperlink Find(ListBoxItem sender, string name)
        {
            object wantedNode = sender.FindName(name);
            if (wantedNode is Hyperlink)
                return wantedNode as Hyperlink;
            else
                return null;
        }

        private void bOptimize_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите оптимизировать ПК вручную?", "Launcher", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Classes.Variables Vars = new Classes.Variables();

                new Classes.Optimize().Start(
                        Vars.GetElement("settings", "winxp"),
                        Vars.GetElement("settings", "kill"),
                        Vars.GetElement("settings", "force"),
                        Vars.GetElement("settings", "aero"),
                        Vars.GetElement("settings", "video"),
                        Vars.GetElement("settings", "weak"),
                        true
                    );
            }
        }

        private void bSettings_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("Settings", "General.xaml");
        }

        private void bFeedback_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("Feedback", "General.xaml");
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("Update", "General.xaml");
        }

        private void bNotification_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("Notification", "General.xaml");
        }

        private void bUserProfile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("UserProfile", "General.xaml");
        }
    }
}
