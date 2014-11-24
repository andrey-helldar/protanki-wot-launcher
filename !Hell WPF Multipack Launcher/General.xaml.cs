﻿using System;
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
using Newtonsoft.Json.Linq;

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
        Classes.Language Lang = new Classes.Language();
        Classes.Variables Variables = new Classes.Variables();

        private string NotifyLink = String.Empty;
        private string lang = MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value;


        public General()
        {
            InitializeComponent();

            string nickname = String.Empty;

            try
            {
                if (MainWindow.XmlDocument.Root.Element("token") != null)
                    if (MainWindow.XmlDocument.Root.Element("token").Attribute("nickname") != null)
                        if (MainWindow.XmlDocument.Root.Element("token").Attribute("nickname").Value != "")
                            nickname = ", " + MainWindow.XmlDocument.Root.Element("token").Attribute("nickname").Value;
                        else
                            MainWindow.XmlDocument.Root.Element("token").Remove();
            }
            catch (Exception) { nickname = String.Empty; }

            Task.Factory.StartNew(() => Variables.Start()).Wait();

            Task.Factory.StartNew(() => SetInterface());
            Task.Factory.StartNew(() => ShowNotify(Lang.Set("PageGeneral", "lStatus", lang) + nickname + "!", "", false));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Устанавливаем заголовок в зависимости от типа версии
            try
            {
                if (MainWindow.XmlDocument.Root.Element("multipack").Attribute("type") != null)
                    lType.Content = Lang.Set("PageGeneral", MainWindow.XmlDocument.Root.Element("multipack").Attribute("type").Value, lang);
            }
            catch (Exception) { lType.Content = Lang.Set("PageGeneral", "base", lang); }

            // Загружаем список видео и новостей
            try
            {
                StatusBarSet(false, 1, true, true);

                Task.WaitAll(new Task[]{
                    Task.Factory.StartNew(() => YoutubeClass.Start()),
                    Task.Factory.StartNew(() => WargamingClass.Start())                
                });
                //StatusBarSet(true, 1);


                Task.WaitAll(new Task[]{
                    Task.Factory.StartNew(() => ViewNews()),
                    Task.Factory.StartNew(() => ViewNews(false))
                });

                StatusBarSet(false, 1, true, true, true);

                Task.Factory.StartNew(() => VideoNotify()); // Выводим уведомления
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "Page_Loaded()", ex.Message, ex.StackTrace)); }


            try { StatusBarSet(false, 1, true, true, true); MainWindow.LoadingPanelShow(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "Page_Loaded()", ex.Message, ex.StackTrace)); }

            //Task.Factory.StartNew(() => GetTanksVersion());
            //  Получаем инфу с сайта
            Task.Factory.StartNew(() => GetInfo());
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
                Task.Factory.StartNew(() =>
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
                                    catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Count " + (youtube ? "VIDEO" : "NEWS") + " is " + count.ToString(), ex0.Message, ex0.StackTrace)); }

                                    StatusBarSet(true);
                                }
                                catch (Exception ex1) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "if (count == 0)", "Записи не обнаружены", ex1.Message, ex1.StackTrace)); }
                            }

                            for (int i = 0; i < count; i++)
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
                                    hyperID.NavigateUri = new Uri(youtube ? "http://" + YoutubeClass.List[i].ID : WargamingClass.List[i].Link);
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
                                    catch (Exception ex2) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", ex2.Message, ex2.StackTrace)); }
                                }
                                catch (Exception ex3) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", "FOR: " + i.ToString(), ex3.Message, ex3.StackTrace)); }


                                StatusBarSet(true);
                            }

                            Thread.Sleep(Convert.ToInt16(Properties.Resources.Sleeping_News));
                        }));

                    }
                    catch (Exception ex4) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Apply " + (youtube ? "VIDEO" : "NEWS") + " to form", ex4.Message, ex4.StackTrace)); }
                });
            }
            catch (Exception e) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ViewNews()", "Youtube = " + youtube.ToString(), e.Message, e.StackTrace)); }

            return true;
        }

        private void VideoNotify()
        {
            try
            {
                if (MainWindow.XmlDocument.Root.Element("info") != null)
                    if (MainWindow.XmlDocument.Root.Element("info").Attribute("video") != null)
                        if (MainWindow.XmlDocument.Root.Element("info").Attribute("video").Value == "True")
                        {
                            if (YoutubeClass.Count() > 0)
                            {

                                if (MainWindow.XmlDocument.Root.Element("youtube") != null)
                                    foreach (var el in MainWindow.XmlDocument.Root.Element("youtube").Elements("video")) { YoutubeClass.Delete(el.Value); }
                                else MainWindow.XmlDocument.Root.Add(new XElement("youtube", null));

                                Task.Factory.StartNew(() => DeleteOldVideo()).Wait(); // Перед выводом уведомлений проверяем даты. Все лишние удаляем

                                foreach (var el in YoutubeClass.List)
                                {
                                    Thread.Sleep(5000);

                                    for (int i = 0; i < 2; i++) // Если цикл прерван случайно, то выжидаем еще 5 секунд перед повторным запуском
                                    {
                                        try
                                        {
                                            // Если запущен клиент игры - ждем 5 секунд до следующей проверки
                                            while (Process.GetProcessesByName("WorldOfTanks").Length > 0 ||
                                                Process.GetProcessesByName("WoTLauncher").Length > 0)
                                                Thread.Sleep(5000);

                                            Thread.Sleep(5000);
                                        }
                                        catch (Exception) { }
                                    }

                                    try
                                    {
                                        NotifyLink = el.Link;

                                        Task.Factory.StartNew(() => ShowNotify(Lang.Set("PageGeneral", "ShowVideo", lang), el.Title));

                                        if (MainWindow.XmlDocument.Root.Element("youtube") != null)
                                            MainWindow.XmlDocument.Root.Element("youtube").Add(new XElement("video", el.ID));
                                        else
                                            MainWindow.XmlDocument.Root.Add(new XElement("youtube", new XElement("video", el.ID)));
                                    }
                                    catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "VideoNotify()", el.ToString(), ex0.Message, ex0.StackTrace)); }
                                }
                            }
                        }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "VideoNotify()", ex.Message, ex.StackTrace)); }
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
                            caption = caption != null ? caption : MainWindow.ProductName;
                            MainWindow.Notifier.ShowBalloonTip(5000, caption, text, System.Windows.Forms.ToolTipIcon.Info);
                        }
                        catch (Exception ex0) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ShowNotify()", "Caption: " + caption, text, "IsPopup = " + isPopup.ToString(), ex0.Message, ex0.StackTrace)); }
                    }
                    else
                        lStatus.Text = text;
                }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "ShowNotify()", "Caption: " + caption, text, "IsPopup = " + isPopup.ToString(), ex.Message, ex.StackTrace)); }
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
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "Hyperlink_RequestNavigate()", "Link: " + e.Uri.AbsoluteUri, ex.Message, ex.StackTrace)); }
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
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "CloseBlock()", ex.Message, ex.StackTrace)); }
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
                        link = link.Remove(link.IndexOf("&"));

                        // Выводим юзеру
                        MainWindow.PreviewVideo(link, FindRun(el, "run_" + arr[1]).Text);
                    }
                    else
                        MainWindow.PreviewVideo("", "", false);
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "PlayPreview()", ex.Message, ex.StackTrace)); }
            }));
        }

        public bool ElementToBan(string block, string item)
        {
            try
            {
                XElement el = MainWindow.XmlDocument.Root.Element("do_not_display");
                if (el != null)
                    if (el.Element(block) != null)
                        if (el.Element(block).Element("item") != null)
                            if (el.Element(block).Elements("item").Count() > 0)
                            {
                                foreach (string str in el.Element(block).Elements("item"))
                                    if (str == item) return true;

                                el.Element(block).Add(new XElement("item", item));
                            }
                            else
                                el.Element(block).Element("item").SetValue(item);
                        else
                            el.Element(block).Add(new XElement("item", item));
                    else
                        el.Add(new XElement(block, new XElement("item", item)));
                else
                    MainWindow.XmlDocument.Root.Add(new XElement("do_not_display", new XElement(block, new XElement("item", item))));

                return true;
            }
            catch (Exception ex0)
            {
                Task.Factory.StartNew(() => Debug.Save("General.xaml", "ElementToBan()", "Block: " + block, "Item: " + item, ex0.Message, ex0.StackTrace));
                return true;
            }
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
                Task.Factory.StartNew(() => Debug.Save("General.xaml", "Find()", "Find name: " + name, ex.Message, ex.StackTrace));
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
                Task.Factory.StartNew(() => Debug.Save("General.xaml", "FindRun()", "Find name: " + name, ex.Message, ex.StackTrace));
                return null;
            }
        }

        private void bOptimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show(Lang.Set("Optimize", "Optimize", lang) + "?", MainWindow.ProductName, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    StatusBarSet(true);

                    new Classes.Optimize().Start(
                            Variables.GetElement("settings", "winxp"),
                            Variables.GetElement("settings", "kill"),
                            Variables.GetElement("settings", "force"),
                            Variables.GetElement("settings", "aero"),
                            Variables.GetElement("settings", "video"),
                            Variables.GetElement("settings", "weak"),
                            true
                        );

                    StatusBarSet(true);
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "bOptimize_Click()", ex.Message, ex.StackTrace)); }
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
            OpenPage("Update");
        }

        private void bNotification_Click(object sender, RoutedEventArgs e)
        {
            OpenPage("Notification");
        }

        private void bUserProfile_Click(object sender, RoutedEventArgs e)
        {
            OpenPage("UserProfile");
        }

        /// <summary>
        /// Сводим смену контента формы к одной функции для более легкого дебага
        /// </summary>
        /// <param name="page">Имя открываемой формы</param>
        private void OpenPage(string page)
        {
            Task.Factory.StartNew(() => MainWindow.LoadingPanelShow(1)).Wait();

            Task.Factory.StartNew(() =>
            {
                try
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(page); }));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("General.xaml", "OpenPage()", "Page: " + page, ex.Message, ex.StackTrace)); }
            });
        }



        /// <summary>
        /// Костыль в виде установки значений интерфейса
        /// </summary>
        private void SetInterface()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                // Text
                try
                {
                    lNews.Content = Lang.Set("PageGeneral", "lNews", lang);
                    lVideo.Content = Lang.Set("PageGeneral", "lVideo", lang);
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("PageGeneral", "SetInterface()", ex.Message, ex.StackTrace)); }
            }));
        }


        private void GetInfo()
        {
            string ans = String.Empty;

            try
            {
                Thread.Sleep(3000);

                Classes.POST POST = new Classes.POST();

                Dispatcher.BeginInvoke(new ThreadStart(delegate
                        {
                            pbStatus.Maximum = 1;
                            pbStatus.Value = 0;
                        }));

                JObject json = new JObject(
                                new JProperty("code", Properties.Resources.API),
                                new JProperty("user_id", Variables.GetUserID()),
                                new JProperty("user_name", GetTokenRec("nickname")),
                                new JProperty("user_email", GetTokenRec("email")),
                                new JProperty("modpack_type", Variables.MultipackType),
                                new JProperty("modpack_ver", Variables.MultipackVersion.ToString()),
                                new JProperty("launcher", Application.Current.GetType().Assembly.GetName().Version.ToString()),
                                new JProperty("game", Variables.TanksVersion.ToString()),
                                new JProperty("game_test", Variables.CommonTest),
                                new JProperty("youtube", Properties.Resources.YoutubeChannel),
                                new JProperty("lang", lang),
                                new JProperty("os", "disabled")
                            );

                ans = POST.Send(Properties.Resources.API_DEV_Address + Properties.Resources.API_DEV_Info, json);
                JObject answer = JObject.Parse(ans);

                /*
                 * code
                 * status
                 * version
                 */

                if (answer["status"].ToString() != "FAIL" && answer["code"].ToString() == Properties.Resources.API)
                {
                    if (answer["status"].ToString() == "OK")
                    {
                        if (Variables.TanksVersion < new Version(answer["version"].ToString()))
                        {
                            Dispatcher.BeginInvoke(new ThreadStart(delegate
                            {
                                lStatus.Text = Lang.Set("PageUpdate", "gbCaption", lang, answer["version"].ToString());
                            }));
                        }
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(new ThreadStart(delegate
                        {
                            lStatus.Text = Lang.Set("API", answer["content"].ToString(), lang);
                        }));
                    }
                }
                else
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        lStatus.Text = Lang.Set("API", "gbCaption", lang, answer["content"].ToString());
                    }));
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("PageGeneral", "GetInfo()", ans, ex.Message, ex.StackTrace)); }
        }

        /// <summary>
        ////Получаем ключ токена, если существует
        /// </summary>
        /// <param name="rec">Передаем имя аттрибута</param>
        /// <returns>Получаем значение, если ключ существует</returns>
        private string GetTokenRec(string rec)
        {
            try
            {
                if (rec.Length > 0)
                {
                    if (MainWindow.XmlDocument.Root.Element("token") != null)
                        if (MainWindow.XmlDocument.Root.Element("token").Attribute(rec) != null)
                            return MainWindow.XmlDocument.Root.Element("token").Attribute(rec).Value;
                        else return "";
                    else return "";
                }
                else
                    return "";
            }
            catch (Exception) { return ""; }
        }

        private void ShowErr(System.IO.IOException ioEx)
        {
            MessageBox.Show(ioEx.Message + Environment.NewLine + Environment.NewLine + ioEx.StackTrace);
        }
    }
}
