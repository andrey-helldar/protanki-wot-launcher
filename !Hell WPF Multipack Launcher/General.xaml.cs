using System;
using System.Collections.Generic;
using System.Linq;
//using System.Xml.Linq;
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
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class General : Page
    {
        Classes.Debugging Debugging = new Classes.Debugging();
        Classes.YoutubeVideo YoutubeClass = new Classes.YoutubeVideo();
        Classes.Wargaming WargamingClass = new Classes.Wargaming();
        Classes.Language Lang = new Classes.Language();
        Classes.Variables Variables = new Classes.Variables();

        private bool show_notify = true;

        private string lang = (string)MainWindow.JsonSettingsGet("info.language");


        public General()
        {
            InitializeComponent();

            string nickname = String.Empty;

            try
            {
                nickname = (string)MainWindow.JsonSettingsGet("info.user_name");
                if (nickname.Length > 0) nickname = ", " + nickname;
            }
            catch (Exception) { nickname = String.Empty; }

            Task.Factory.StartNew(() => SetInterface());
            Task.Factory.StartNew(() => ShowNotify(Lang.Set("PageGeneral", "lStatus", lang) + nickname + "!", "", false));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Устанавливаем заголовок в зависимости от типа версии
            try
            {
                lType.Content = Lang.Set("PageGeneral", (string)MainWindow.JsonSettingsGet("multipack.type"), lang) + " #" +
                    Version.Parse((string)MainWindow.JsonSettingsGet("multipack.version")).Revision;
            }
            catch (Exception) { lType.Content = Lang.Set("PageGeneral", "base", lang); }

            // Загружаем список видео и новостей
            try
            {
                Task.Factory.StartNew(() => StatusBarSet(false, 1, true, true)).Wait();

                Task.WaitAll(new Task[]{
                    Task.Factory.StartNew(() => YoutubeClass.Start()),
                    Task.Factory.StartNew(() => WargamingClass.Start())               
                });

                Task.WaitAll(new Task[]{
                    Task.Factory.StartNew(() => ViewNews()),
                    Task.Factory.StartNew(() => ViewNews(false))
                });

                Task.Factory.StartNew(() => StatusBarSet(false, 1, true, true, true));
                Task.Factory.StartNew(() => Notify()); // Выводим уведомления видео
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "Page_Loaded()", ex.Message, ex.StackTrace)); }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    if ((bool)MainWindow.JsonSettingsGet("multipack.update") && MainWindow.JsonSettingsGet("multipack.update") != null)
                        Dispatcher.BeginInvoke(new ThreadStart(delegate
                        {
                            lStatus.Text = Lang.Set("PageGeneral", "UpdatesMultipack", (string)MainWindow.JsonSettingsGet("info.language"), (string)MainWindow.JsonSettingsGet("multipack.new_version"));

                            bNotification.Visibility = System.Windows.Visibility.Visible;
                            bUpdate.Visibility = System.Windows.Visibility.Visible;
                        }));
                    else
                        Dispatcher.BeginInvoke(new ThreadStart(delegate
                        {
                            bNotification.Visibility = System.Windows.Visibility.Hidden;
                            bUpdate.Visibility = System.Windows.Visibility.Hidden;
                        }));

                    if ((bool)MainWindow.JsonSettingsGet("game.update") && MainWindow.JsonSettingsGet("game.update") != null)
                        Dispatcher.BeginInvoke(new ThreadStart(delegate { lStatus.Text = Lang.Set("PageGeneral", "UpdatesGame", lang, (string)MainWindow.JsonSettingsGet("game.new_version")); }));

                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "Page_Loaded()", "multipack.update", ex.Message, ex.StackTrace)); }
            });

            try { StatusBarSet(false, 1, true, true, true); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "Page_Loaded()", ex.Message, ex.StackTrace)); }

            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try { MainWindow.LoadPage.Visibility = Visibility.Hidden; }
                catch (Exception) { }
            }));
        }

        private void StatusBarSet(bool inc, int max_inc = 0, bool set_max = false, bool disp = false, bool value_set = false)
        {
            try
            {
                if (disp)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        if (value_set)
                        {
                            pbStatus.Maximum = 1;
                            pbStatus.Value = 0;
                        }
                        else
                        {
                            pbStatus.Maximum = set_max ? max_inc : pbStatus.Maximum + max_inc;
                            if (inc) pbStatus.Value++;
                            else
                                pbStatus.Value = 0;
                        }
                    }));
                }
                else
                {
                    if (value_set)
                    {
                        pbStatus.Maximum = 1;
                        pbStatus.Value = 0;
                    }
                    else
                    {
                        pbStatus.Maximum = set_max ? max_inc : pbStatus.Maximum + max_inc;
                        if (inc) pbStatus.Value++;
                        else
                            pbStatus.Value = 0;
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Размещаем блоки с информацией по видео на форме
        /// </summary>
        private bool ViewNews(bool youtube = true)
        {
            try
            {
                try
                {
                    int count = youtube ? YoutubeClass.Count() : WargamingClass.Count();

                    StatusBarSet(true, count);

                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        if (count == 0)
                        { // Если список пуст, то добавляем одну строку с уведомлением
                            try
                            {
                                Grid gridPanel = new Grid();
                                gridPanel.Width = double.NaN;
                                gridPanel.Margin = new Thickness(0);
                                gridPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                                Label labelNotify = new Label();
                                labelNotify.Height = double.NaN;
                                labelNotify.Margin = new Thickness(0);
                                labelNotify.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                                labelNotify.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                                labelNotify.Content = Lang.Set("PageGeneral", "RecordsNotFound", lang);
                                Grid.SetRow(labelNotify, 0);
                                Grid.SetColumn(labelNotify, 0);
                                gridPanel.Children.Add(labelNotify);

                                ListBoxItem lbi = new ListBoxItem();
                                lbi.SetResourceReference(ListBoxItem.StyleProperty, "ListBoxItemGeneral");
                                lbi.Content = gridPanel;

                                try { if (youtube) lbVideo.Items.Add(lbi); else lbNews.Items.Add(lbi); }
                                catch (Exception ex0) { Debugging.Save("General.xaml", "ViewNews()", "Count " + (youtube ? "VIDEO" : "NEWS") + " is " + count.ToString(), ex0.Message, ex0.StackTrace); }

                                StatusBarSet(true);
                            }
                            catch (Exception ex1) { Debugging.Save("General.xaml", "ViewNews()", "if (count == 0)", "Записи не обнаружены", ex1.Message, ex1.StackTrace); }
                        }
                        else
                        {
                            for (int i = 0; i < (count - 1); i++)
                            {
                                try
                                {
                                    // Добавляем решетку для размещения элементов
                                    Grid gridPanel = new Grid();
                                    gridPanel.Width = double.NaN;
                                    gridPanel.Margin = new Thickness(0);
                                    gridPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                                    // Добавляем дату
                                    Label labelDate = new Label();
                                    labelDate.SetResourceReference(Label.StyleProperty, "ListBoxLabelGeneral");
                                    try { labelDate.Content = youtube ? YoutubeClass.List[i].DateShort : WargamingClass.List[i].DateShort; }
                                    catch (Exception) { labelDate.Content = "1970-1-1"; }
                                    gridPanel.Children.Add(labelDate);

                                    // Кнопка "Воспроизвести"
                                    if (youtube)
                                    {
                                        Button buttonPlay = new Button();
                                        buttonPlay.Content = ">";
                                        buttonPlay.Name = (youtube ? "PlayYoutube_" : "PlayWargaming_") + i.ToString();
                                        buttonPlay.SetResourceReference(Button.StyleProperty, "ListBoxPlayGeneral");
                                        buttonPlay.Click += PlayPreview;
                                        gridPanel.Children.Add(buttonPlay);
                                    }

                                    // Кнопка "Закрыть"
                                    Button buttonClose = new Button();
                                    buttonClose.Content = "X";
                                    buttonClose.Name = (youtube ? "CloseYoutube_" : "CloseWargaming_") + i.ToString();
                                    buttonClose.SetResourceReference(Button.StyleProperty, "ListBoxCloseGeneral");
                                    buttonClose.Click += CloseBlock;
                                    gridPanel.Children.Add(buttonClose);

                                    // Добавляем заголовок в гиперссылку
                                    TextBlock blockTitle = new TextBlock();
                                    blockTitle.SetResourceReference(TextBlock.StyleProperty, "ListBoxTitleGeneral");

                                    // Добавляем идентификатор записи
                                    Hyperlink hyperID = new Hyperlink(new Run(""));
                                    hyperID.NavigateUri = new Uri(youtube ? YoutubeClass.List[i].Link : WargamingClass.List[i].Link);
                                    hyperID.Name = (youtube ? "LinkYoutube_" : "LinkWargaming_") + i.ToString();
                                    this.RegisterName(hyperID.Name, hyperID);


                                    // Гиперссылка для заголовка
                                    Run run = new Run();
                                    run.Text = youtube ? YoutubeClass.List[i].Title : WargamingClass.List[i].Title;
                                    if (youtube)
                                    {
                                        run.Name = "run_" + i.ToString();
                                        this.RegisterName(run.Name, run);
                                    }

                                    Hyperlink hyperlink = new Hyperlink(run);
                                    hyperlink.NavigateUri = new Uri(youtube ? YoutubeClass.List[i].Link : WargamingClass.List[i].Link);
                                    hyperlink.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_RequestNavigate);
                                    hyperlink.Name = (youtube ? "lu_" : "lw_") + i.ToString();
                                    hyperlink.SetResourceReference(Hyperlink.StyleProperty, "ListBoxTitleHyperlinkGeneral");
                                    this.RegisterName(hyperlink.Name, hyperlink);
                                    blockTitle.Inlines.Add(hyperlink);
                                    blockTitle.Inlines.Add(hyperID);

                                    gridPanel.Children.Add(blockTitle);

                                    ListBoxItem lbi = new ListBoxItem();
                                    lbi.SetResourceReference(ListBoxItem.StyleProperty, "ListBoxItemGeneral");
                                    lbi.Content = gridPanel;

                                    try
                                    {
                                        if (youtube)
                                            lbVideo.Items.Add(lbi);
                                        else
                                            lbNews.Items.Add(lbi);

                                        Thread.Sleep(50);
                                    }
                                    catch (Exception ex2) { Debugging.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", ex2.Message, ex2.StackTrace); }
                                }
                                catch (Exception ex3) { Debugging.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", "Record: " + i.ToString(), ex3.Message, ex3.StackTrace); }


                                StatusBarSet(true);
                            }

                            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Sleeping_Show_Item));
                        }
                    }));

                }
                catch (Exception ex4) { Debugging.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", ex4.Message, ex4.StackTrace); }
            }
            catch (Exception e) { Debugging.Save("General.xaml", "ViewNews()", "Youtube = " + youtube.ToString(), e.Message, e.StackTrace); }

            return true;
        }

        private void Notify()
        {
            /*
             *  Видео
             */
            try
            {
                if ((bool)MainWindow.JsonSettingsGet("info.video") && MainWindow.JsonSettingsGet("info.video") != null)
                {
                    if (YoutubeClass.Count() > 0)
                    {
                        DeleteOldVideo(); // Перед выводом уведомлений проверяем даты. Все лишние удаляем

                        foreach (var el in YoutubeClass.List)
                        {
                            if (show_notify)
                            {
                                bool show_this = true;
                                string id_md5 = Variables.Md5(el.ID);

                                try
                                {
                                    string elem = MainWindow.JsonSettingsGet("youtube").ToString();
                                    if (elem.IndexOf(id_md5) > -1) show_this = false;
                                }
                                catch (Exception) { }


                                if (show_this)
                                {
                                    Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Sleeping_Notify));

                                    for (int i = 0; i < 2; i++) // Если цикл прерван случайно, то выжидаем еще 5 секунд перед повторным запуском
                                    {
                                        try
                                        {
                                            // Если запущен клиент игры - ждем 5 секунд до следующей проверки
                                            while (Process.GetProcessesByName("WorldOfTanks").Length > 0 ||
                                                Process.GetProcessesByName("WoTLauncher").Length > 0)
                                                Thread.Sleep(5000);

                                            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Sleeping_Notify));
                                        }
                                        catch (Exception) { }
                                    }

                                    try
                                    {
                                        MainWindow.JsonSettingsSet("info.notify_link", el.Link);

                                        Task.Factory.StartNew(() => ShowNotify(Lang.Set("PageGeneral", "ShowVideo", lang), el.Title));

                                        string elem = MainWindow.JsonSettingsGet("youtube").ToString();
                                        if (elem.IndexOf(id_md5) == -1)
                                            MainWindow.JsonSettingsSet("youtube", id_md5, "array");
                                        else
                                            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Sleeping_Notify));
                                    }
                                    catch (Exception ex0) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "Notify()", "YOUTUBE (Inner)", el.ToString(), ex0.Message, ex0.StackTrace, ex0.Source)); }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "Notify()", "YOUTUBE", ex.Message, ex.StackTrace, ex.Source)); }


            /*
             *  Новости
             */
            try
            {
                if ((bool)MainWindow.JsonSettingsGet("info.news") && MainWindow.JsonSettingsGet("info.news") != null)
                {
                    if (WargamingClass.Count() > 0)
                    {
                        foreach (var el in WargamingClass.List)
                        {
                            if (show_notify)
                            {
                                bool show_this = true;
                                string id_md5 = Variables.Md5(el.Link);

                                try
                                {
                                    string elem = MainWindow.JsonSettingsGet("wargaming").ToString();
                                    if (elem.IndexOf(id_md5) > -1) show_this = false;
                                }
                                catch (Exception) { }

                                if (show_this)
                                {
                                    Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Sleeping_Notify));

                                    for (int i = 0; i < 2; i++) // Если цикл прерван случайно, то выжидаем еще 5 секунд перед повторным запуском
                                    {
                                        try
                                        {
                                            // Если запущен клиент игры - ждем 5 секунд до следующей проверки
                                            while (Process.GetProcessesByName("WorldOfTanks").Length > 0 ||
                                                Process.GetProcessesByName("WoTLauncher").Length > 0)
                                                Thread.Sleep(5000);

                                            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Sleeping_Notify));
                                        }
                                        catch (Exception) { }
                                    }

                                    try
                                    {
                                        MainWindow.JsonSettingsSet("info.notify_link", el.Link);

                                        Task.Factory.StartNew(() => ShowNotify(Lang.Set("PageGeneral", "ShowNews", lang), el.Title));

                                        string elem = MainWindow.JsonSettingsGet("wargaming").ToString();
                                        if (elem.IndexOf(id_md5) == -1)
                                            MainWindow.JsonSettingsSet("wargaming", Variables.Md5(el.Link), "array");
                                        else
                                            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Sleeping_Notify));
                                    }
                                    catch (Exception ex0) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "Notify()", "WARGAMING", el.ToString(), ex0.Message, ex0.StackTrace)); }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "Notify()", "WARGAMING", ex.Message, ex.StackTrace)); }
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
                    try { if (!YoutubeClass.CheckDate((string)MainWindow.JsonSettingsGet("multipack.date"), el.Date)) YoutubeClass.Delete(el.ID); }
                    catch (Exception) { DeleteOldVideo(); }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Вывод уведомления либо в строку статуса, либо всплывающим уведомлением в системном трее
        /// </summary>
        /// <param name="text">Текст отображения</param>
        /// <param name="caption">Заголовок</param>
        /// <param name="isPopup">TRUE - вывод в системном трее, иначе - в строке статуса формы</param>
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
                            caption = caption != null ? caption : (string)MainWindow.JsonSettingsGet("info.ProductName");
                            MainWindow.Notifier.ShowBalloonTip(5000, caption, text, System.Windows.Forms.ToolTipIcon.Info);
                        }
                        catch (Exception ex0) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "ShowNotify()", "Caption: " + caption, text, "IsPopup = " + isPopup.ToString(), ex0.Message, ex0.StackTrace)); }
                    }
                    else
                        lStatus.Text = text;
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "ShowNotify()", "Caption: " + caption, text, "IsPopup = " + isPopup.ToString(), ex.Message, ex.StackTrace)); }
        }

        /// <summary>
        /// Открытие ссылок из нотификатора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyClick(object sender, EventArgs e)
        {
            try { Process.Start((string)MainWindow.JsonSettingsGet("info.notify_link")); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("MainWindow", "NotifyClick()", "Link: " + (string)MainWindow.JsonSettingsGet("info.notify_link"), ex.Message, ex.StackTrace)); }
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
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "Hyperlink_RequestNavigate()", "Link: " + e.Uri.AbsoluteUri, ex.Message, ex.StackTrace)); }
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

                switch (arr[0])
                {
                    case "CloseWargaming":
                        Hyperlink elem = Find(el, "LinkWargaming_" + arr[1]);
                        if (elem != null) Variables.ElementToBan("news", elem.NavigateUri.AbsoluteUri);
                        break;

                    default:
                        Hyperlink elemY = Find(el, "LinkYoutube_" + arr[1]);

                        if (elemY != null)
                        {
                            //string[] item = elemY.NavigateUri.AbsoluteUri.Split('/');
                            Variables.ElementToBan("video", elemY.NavigateUri.AbsoluteUri);
                        }
                        break;
                }

                el.IsSelected = true;
                lb.Items.Remove(lb.SelectedItem);
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "CloseBlock()", ex.Message, ex.StackTrace)); }
        }

        /// <summary>
        /// Функция воспроизведения превью видео
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayPreview(object sender, RoutedEventArgs e)
        {
            (sender as Button).Dispatcher.BeginInvoke(new Action(delegate()
            {
                try
                {
                    ListBoxItem el = (((sender as Button).Parent as Grid).Parent as ListBoxItem);
                    string[] arr = (sender as Button).Name.Split('_');
                    Hyperlink elemY = Find(el, "lu_" + arr[1]);
                    if (elemY != null)
                    {
                        // Достаем линк
                        string y = elemY.NavigateUri.AbsoluteUri;
                        string link = y.Remove(0, y.IndexOf("v=") + 2);
                        //link = link.Remove(link.IndexOf("&"));

                        // Выводим юзеру
                        MainWindow.PreviewVideo(link, FindRun(el, "run_" + arr[1]).Text);
                    }
                    else
                        MainWindow.PreviewVideo("", "", false);
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "PlayPreview()", ex.Message, ex.StackTrace)); }
            }));
        }

        /// <summary>
        /// Ищем элемент по имени
        /// </summary>
        /// <param name="sender">Передаем строку ListBox</param>
        /// <param name="name">Передаем имя искомого элемента</param>
        /// <returns>Возвращаем элемент</returns>
        private Hyperlink Find(ListBoxItem sender, string name)
        {
            try
            {
                object wantedNode = sender.FindName(name);
                if (wantedNode is Hyperlink)
                    return wantedNode as Hyperlink;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("General.xaml", "Find()", "Find name: " + name, ex.Message, ex.StackTrace));
                return null;
            }
        }

        /// <summary>
        /// Ищем элемент по имени
        /// </summary>
        /// <param name="sender">Передаем строку ListBox</param>
        /// <param name="name">Передаем имя искомого элемента</param>
        /// <returns>Возвращаем элемент</returns>
        private Run FindRun(ListBoxItem sender, string name)
        {
            try
            {
                object wantedNode = sender.FindName(name);
                if (wantedNode is Run)
                    return wantedNode as Run;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("General.xaml", "FindRun()", "Find name: " + name, ex.Message, ex.StackTrace));
                return null;
            }
        }

        private void bOptimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show(Lang.Set("Optimize", "Optimize", lang) + "?", (string)MainWindow.JsonSettingsGet("info.ProductName"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    StatusBarSet(true);

                    new Classes.Optimize().Start(
                            (bool)MainWindow.JsonSettingsGet("settings.winxp"),
                            true,
                            true,
                            true,
                            (bool)MainWindow.JsonSettingsGet("settings.video"),
                            (bool)MainWindow.JsonSettingsGet("settings.weak"),
                            true
                        );

                    StatusBarSet(true);
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "bOptimize_Click()", ex.Message, ex.StackTrace)); }
        }

        private void bSettings_Click(object sender, RoutedEventArgs e)
        {
            OpenPage("Settings");
        }

        private void bFeedback_Click(object sender, RoutedEventArgs e)
        {
            OpenPage("Feedback");
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ProcessStart((string)MainWindow.JsonSettingsGet("multipack.link"));
        }

        private void bNotification_Click(object sender, RoutedEventArgs e)
        {
            OpenPage("Update");
        }

        /// <summary>
        /// Если юзер уже подтверждал свое согласие на передачу данных сервису,
        /// то выводить уведомление повторно нет смысла, иначе выводим
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bUserProfile_Click(object sender, RoutedEventArgs e)
        {
            //if ((bool)MainWindow.JsonSettingsGet("info.user_accept"))
                OpenPage("UserProfile");
            /*else
                OpenPage("GiveEmail");*/
        }

        /// <summary>
        /// Сводим смену контента формы к одной функции для более легкого дебага
        /// </summary>
        /// <param name="page">Имя открываемой формы</param>
        private void OpenPage(string page)
        {
            show_notify = false; // Вырубаем вывод уведомлений

            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible; }));
            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(page); }));
        }

        /// <summary>
        /// Костыль в виде установки значений интерфейса
        /// </summary>
        private void SetInterface()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                try
                {
                    lNews.Content = Lang.Set("PageGeneral", "lNews", lang);
                    lVideo.Content = Lang.Set("PageGeneral", "lVideo", lang);

                    bUpdate.ToolTip = Lang.Set("Tooltip", "bUpdate", lang);
                    bNotification.ToolTip = Lang.Set("Tooltip", "bNotification", lang);
                    //bUserProfile.ToolTip = Lang.Set("Tooltip", "bUserProfile", lang);

                    bSettings.ToolTip = Lang.Set("Tooltip", "bSettings", lang);
                    bFeedback.ToolTip = Lang.Set("Tooltip", "bFeedback", lang);

                    bLauncher.ToolTip = Lang.Set("Tooltip", "bLauncher", lang);
                    bOptimize.ToolTip = Lang.Set("Tooltip", "bOptimize", lang);
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("PageGeneral", "SetInterface()", ex.Message, ex.StackTrace)); }
            }));
        }

        private void bLauncher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string game_path = ((string)MainWindow.JsonSettingsGet("game.path")).Replace(Properties.Resources.Default_JSON_Splitter, @"\");

                if (System.IO.File.Exists(game_path + "WoTLauncher.exe"))
                {
                    Task.Factory.StartNew(() => new Classes.Optimize().Start(
                            (bool)MainWindow.JsonSettingsGet("settings.winxp"),
                            (bool)MainWindow.JsonSettingsGet("settings.kill"),
                            (bool)MainWindow.JsonSettingsGet("settings.force"),
                            (bool)MainWindow.JsonSettingsGet("settings.aero"),
                            (bool)MainWindow.JsonSettingsGet("settings.video"),
                            (bool)MainWindow.JsonSettingsGet("settings.weak"),
                            false
                        ));

                    Task.Factory.StartNew(() => MainWindow.ProcessStart(game_path, "WoTLauncher.exe"));

                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        switch ((int)MainWindow.JsonSettingsGet("settings.launcher"))
                        {
                            case 1: MainWindow.State.Hide(); break;
                            case 2: MainWindow.State.WindowState = System.Windows.WindowState.Minimized; break;
                            case 3: MainWindow.State.Close(); break;
                            default: break;
                        }
                    }));
                }
                else
                    MessageBox.Show(Lang.Set("MainProject", "Game_Not_Found", (string)MainWindow.JsonSettingsGet("info.language")));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("General.xaml", "bLauncher_Click()", ex.Message, ex.StackTrace)); }
        }

        private void bDonate_Click(object sender, RoutedEventArgs e)
        {
            OpenPage("Donate");
        }
    }
}
