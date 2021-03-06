﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScottLogic.Shapes;


namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Page
    {
        Classes.Debugging Debugging = new Classes.Debugging();
        Classes.Language Lang = new Classes.Language();
        Classes.WargamingAPI WarAPI = new Classes.WargamingAPI();

        string lang = (string)MainWindow.JsonSettingsGet("info.language");

        string nickname = String.Empty;
        string account_id = String.Empty;
        string access_token = String.Empty;
        string clan_id = String.Empty;

        //private ObservableCollection<AssetClass> classes;

        /// <summary>
        /// Handle clicks on the listview column heading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            /*try { piePlotter.PlottedProperty = ((GridViewColumnHeader)e.OriginalSource).Column.Header.ToString(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "OnColumnHeaderClick()", ex.Message, ex.StackTrace)); }*/
        }

        public UserProfile()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible; }));
            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

            Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
        }

        /// <summary>
        /// Получаем информацию по аккаунту.
        /// Если токен активен, сразу выводим.
        /// Если нет, вначале запрашиваем авторизацию, после чего получаем информацию.
        /// </summary>
        private void AccountInfo()
        {
            // Копируем ник юзера в блок Info XML
            try
            {
                string user_name = (string)MainWindow.JsonSettingsGet("token.nickname");
                if (user_name != null && user_name != "") MainWindow.JsonSettingsSet("info.user_name", user_name);
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Set username", ex.Message, ex.StackTrace)); }

            // Apply language
            try
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        bClose.Content = Lang.Set("PageSettings", "bClose", lang);

                        gbSostav.Header = Lang.Set("PageUser", "tbPersonnel", lang);
                        dataonTitle.Text = Lang.Set("PageUser", "tbDataOn", lang);
                    }));
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Apply language", ex.Message, ex.StackTrace)); }


            try
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        try
                        {
                            MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible;
                            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

                            if (!(CheckElement("access_token") && CheckElement("expires_at") && CheckElement("nickname") && CheckElement("account_id")))
                            {
                                try
                                {
                                    MainWindow.JsonSettingsRemove("token");
                                    MainWindow.Navigator();
                                }
                                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Token is FAIL", ex.Message, ex.StackTrace)); }
                            }
                            else
                            {
                                Dispatcher.BeginInvoke(new ThreadStart(delegate
                                    {
                                        try
                                        {
                                            nickname = (string)MainWindow.JsonSettingsGet("token.nickname");
                                            account_id = (string)MainWindow.JsonSettingsGet("token.account_id");
                                            access_token = (string)MainWindow.JsonSettingsGet("token.access_token");

                                            PlayerName.Text = nickname;

                                            JObject JAccountInfo = WarAPI.AccountInfo(account_id, access_token);
                                            JObject
                                                Clan = null,
                                                Battles = null,
                                                Provinces = null;


                                            if ((string)JAccountInfo.SelectToken("status") == "ok")
                                            {
                                                /* =========================================
                                                 *       Проверяем клан
                                                 *       Если параметр пуст - деактивируем вкладки
                                                 * =========================================*/
                                                try
                                                {
                                                    if (JAccountInfo.SelectToken(String.Format("data.{0}.clan_id", account_id)) == null)
                                                    {
                                                        tiClanInfo.IsEnabled = false;
                                                        tiClanBattles.IsEnabled = false;
                                                        tiClanProvinces.IsEnabled = false;
                                                    }
                                                    else
                                                    {
                                                        //obj["data"]["2732865"]["clan_id"] = 60118; // Подставной клан  
                                                        clan_id = (string)JAccountInfo.SelectToken(String.Format("data.{0}.clan_id", account_id));

                                                        Clan = WarAPI.ClanInfo(clan_id, access_token);
                                                        Battles = WarAPI.ClanBattles(clan_id, access_token);
                                                        Provinces = WarAPI.ClanProvinces(clan_id, access_token, "type,name,arena_i18n,prime_time,revenue,occupancy_time,attacked");

                                                    }
                                                }
                                                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Check Clan", ex.Message, ex.StackTrace)); }


                                                /* =========================================
                                                 *       Общая информация о пользователе
                                                 * =========================================*/
                                                try
                                                {
                                                    if ((string)Clan.SelectToken("status") == "ok")
                                                    {
                                                        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                                        dt = dt.AddSeconds((double)Clan.SelectToken(String.Format("data.{0}.members.{1}.created_at", clan_id, account_id)));
                                                        TimeSpan ts = DateTime.Now - dt;

                                                        PlayerClan.Text = "[" + (string)Clan.SelectToken(String.Format("data.{0}.abbreviation", clan_id)) + "]";

                                                        PlayerClan2.Text = (string)Clan.SelectToken(String.Format("data.{0}.name", clan_id));
                                                        ClanFullname.Text = PlayerClan2.Text;

                                                        PlayerZvanie.Text = Lang.Set("Rank", (string)Clan.SelectToken(String.Format("data.{0}.members.{1}.role", clan_id, account_id)), lang);

                                                        BitmapImage bitmap = new BitmapImage();
                                                        bitmap.BeginInit();
                                                        bitmap.UriSource = new Uri((string)Clan.SelectToken(String.Format("data.{0}.emblems.medium", clan_id)), UriKind.Absolute);
                                                        bitmap.EndInit();

                                                        ClanEmblem.Source = bitmap;
                                                        ClanEmblem2.Source = bitmap;
                                                    }
                                                    else
                                                    {
                                                        PlayerClan2.Text = Lang.Set("PageUser", "NotClan", lang);
                                                        PlayerZvanie.Text = String.Empty;
                                                    }

                                                    // Процент побед
                                                    PercWins.Text = Lang.Set("PageUser", "tbPercentWins", lang);
                                                    PercWinsPerc.Text = (Math.Round(((double)JAccountInfo.SelectToken(String.Format("data.{0}.statistics.all.wins", account_id)) / (double)JAccountInfo.SelectToken(String.Format("data.{0}.statistics.all.battles", account_id))) * 100, 2)).ToString();

                                                    // Личный рейтинг
                                                    MyRating.Text = Lang.Set("PageUser", "tbMyRating", lang);
                                                    MyRatingPerc.Text = SetSumm((string)JAccountInfo.SelectToken(String.Format("data.{0}.global_rating", account_id)));

                                                    // Средний опыт за бой
                                                    AvgXP.Text = Lang.Set("PageUser", "tbAvgXP", lang);
                                                    AvgXPPerc.Text = SetSumm((string)JAccountInfo.SelectToken(String.Format("data.{0}.statistics.all.battle_avg_xp", account_id)));

                                                    // Количество боев
                                                    BattleCount.Text = Lang.Set("PageUser", "tbCountWars", lang);
                                                    BattleCountPerc.Text = SetSumm((string)JAccountInfo.SelectToken(String.Format("data.{0}.statistics.all.battles", account_id)));

                                                    // Средний нанесенный урон за бой
                                                    AvgDamage.Text = Lang.Set("PageUser", "tbAvgDamage", lang);
                                                    AvgDamagePerc.Text = SetSumm((string)JAccountInfo.SelectToken(String.Format("data.{0}.statistics.all.avg_damage_assisted", account_id)));
                                                }
                                                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "User Info", ex.Message, ex.StackTrace)); }

                                                try
                                                {
                                                    PlayerGold.Text = SetSumm((string)JAccountInfo.SelectToken(String.Format("data.{0}.private.gold", account_id)));
                                                    PlayerCredit.Text = SetSumm((string)JAccountInfo.SelectToken(String.Format("data.{0}.private.credits", account_id)));
                                                    PlayerXP.Text = SetSumm((string)JAccountInfo.SelectToken(String.Format("data.{0}.private.free_xp", account_id)));
                                                    iAccountType.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/{0};component/Resources/{1}", (string)MainWindow.JsonSettingsGet("info.ProductName"), (bool)JAccountInfo.SelectToken(String.Format("data.{0}.private.is_premium", account_id)) ? "ico-account-premium.png" : "ico-account-base.png")));

                                                    iAccountType.ToolTip = Lang.Set("Global", (bool)JAccountInfo.SelectToken(String.Format("data.{0}.private.is_premium", account_id)) ? "IsPremium" : "IsBase", lang);
                                                }
                                                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "User Credits", ex.Message, ex.StackTrace)); }

                                                /*Batles.Text = SelectToken(obj, "statistics.all.battles");                               
                                                Wins.Text = SelectToken(obj, "statistics.all.wins");
                                                Loss.Text = SelectToken(obj, "statistics.all.losses");

                                                WinsPercent.Text = (Math.Round(((Convert.ToDouble(SelectToken(obj, "statistics.all.wins")) / Convert.ToDouble(SelectToken(obj, "statistics.all.battles"))) * 100), 2)).ToString();
                                                LossPercent.Text = (Math.Round(((Convert.ToDouble(SelectToken(obj, "statistics.all.losses")) / Convert.ToDouble(SelectToken(obj, "statistics.all.battles"))) * 100), 2)).ToString();
                                                WhoPercent.Text = (Math.Round(((Convert.ToDouble(SelectToken(obj, "statistics.all.draws")) / Convert.ToDouble(SelectToken(obj, "statistics.all.battles"))) * 100), 2)).ToString();

                                                AvgXP.Text = SelectToken(obj, "statistics.all.battle_avg_xp");*/


                                                /*
                                                 *   ГРАФИК
                                                 */
                                                /*try
                                                {
                                                    this.DataContext = new ObservableCollection<AssetClass>(AssetClass.ConstructTestData(new JObject(
                                                        new JProperty("wins",
                                                            new JObject(
                                                                new JProperty("name", "Победы"),
                                                                new JProperty("total", Convert.ToInt16(SelectToken(obj, "statistics.all.wins")))
                                                            )
                                                        ),
                                                        new JProperty("losses",
                                                            new JObject(
                                                                new JProperty("name", "Поражения"),
                                                                new JProperty("total", Convert.ToInt16(SelectToken(obj, "statistics.all.losses")))
                                                            )
                                                        ),
                                                        new JProperty("draws",
                                                            new JObject(
                                                                new JProperty("name", "Ничьи"),
                                                                new JProperty("total", Convert.ToInt16(SelectToken(obj, "statistics.all.draws")))
                                                            )
                                                        )
                                                    )));
                                                }
                                                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Graphic", ex.Message, ex.StackTrace)); }
                                                */



                                                /* =========================================
                                                 *       Общая информация о клане
                                                 * =========================================*/
                                                if (Clan != null)
                                                {
                                                    try
                                                    {
                                                        ClanDesc.Text = (string)Clan.SelectToken(String.Format("data.{0}.motto", clan_id));
                                                        ClanCount.Text = (string)Clan.SelectToken(String.Format("data.{0}.members_count", clan_id));

                                                        dataonTitle.Text = Lang.Set("PageUser", "tbDataOn", lang);
                                                        dataon.Text = DateFormat((double)Clan.SelectToken(String.Format("data.{0}.updated_at", clan_id)));

                                                    }
                                                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Clan", ex.Message, ex.StackTrace)); }


                                                    /* =========================================
                                                     *       Члены клана
                                                     * =========================================*/
                                                    try
                                                    {
                                                        ClanMembers.Items.Clear();
                                                        int i = 1;

                                                        foreach (var member in (JObject)Clan.SelectToken(String.Format("data.{0}.members", clan_id)))
                                                        {
                                                            Grid gr = new Grid();
                                                            gr.SetResourceReference(Grid.StyleProperty, "GridW470");

                                                            ColumnDefinition cd1 = new ColumnDefinition();
                                                            ColumnDefinition cd2 = new ColumnDefinition();
                                                            ColumnDefinition cd3 = new ColumnDefinition();
                                                            ColumnDefinition cd4 = new ColumnDefinition();

                                                            cd1.Width = new GridLength(30, GridUnitType.Auto);
                                                            cd2.Width = new GridLength(1, GridUnitType.Star);
                                                            cd3.Width = new GridLength(130, GridUnitType.Auto);
                                                            cd4.Width = new GridLength(80, GridUnitType.Auto);

                                                            gr.ColumnDefinitions.Add(cd1);
                                                            gr.ColumnDefinitions.Add(cd2);
                                                            gr.ColumnDefinitions.Add(cd3);
                                                            gr.ColumnDefinitions.Add(cd4);

                                                            TextBlock tbID = new TextBlock();
                                                            tbID.Text = (i++).ToString();
                                                            tbID.SetResourceReference(TextBlock.StyleProperty, "CmID");
                                                            Grid.SetColumn(tbID, 0);

                                                            TextBlock CmName = new TextBlock();
                                                            CmName.Text = (string)member.Value["account_name"];
                                                            CmName.SetResourceReference(TextBlock.StyleProperty, "CmName");
                                                            Grid.SetColumn(CmName, 1);

                                                            TextBlock CmTitle = new TextBlock();
                                                            CmTitle.Text = Lang.Set("Rank", (string)member.Value["role"], lang);
                                                            CmTitle.SetResourceReference(TextBlock.StyleProperty, "CmTitle");
                                                            Grid.SetColumn(CmTitle, 2);

                                                            TextBlock CmDate = new TextBlock();
                                                            CmDate.Text = DateFormat((double)member.Value["created_at"]);
                                                            CmDate.SetResourceReference(TextBlock.StyleProperty, "CmDate");
                                                            Grid.SetColumn(CmDate, 3);

                                                            gr.Children.Add(tbID);
                                                            gr.Children.Add(CmName);
                                                            gr.Children.Add(CmTitle);
                                                            gr.Children.Add(CmDate);


                                                            ListBoxItem lbi = new ListBoxItem();
                                                            lbi.SetResourceReference(ListBoxItem.StyleProperty, "lbiProcess");
                                                            lbi.Content = gr;

                                                            ClanMembers.Items.Add(lbi);
                                                        }
                                                    }
                                                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Clan members", ex.Message, ex.StackTrace)); }



                                                    /* =========================================
                                                     *       Бои клана
                                                     * =========================================*/
                                                    try
                                                    {
                                                        tbBattles.Text = Lang.Set("PageUser", "tbBattles", lang);

                                                        if ((string)Battles.SelectToken("status") == "ok")
                                                        {
                                                            JArray arr = (JArray)Battles.SelectToken("data." + clan_id);

                                                            if (arr.Count > 0)
                                                            {
                                                                ClanBattles.Items.Clear();

                                                                foreach (var battle in arr)
                                                                {
                                                                    /*
                                                                     * Тип
                                                                     * Время
                                                                     * Провинция
                                                                     * Игровая карта
                                                                     * 
                                                                     * Тип боя:
                                                                     *       for_province — бой за провинцию;
                                                                     *       meeting_engagement — встречный бой;
                                                                     *       landing — бой за высадку.
                                                                     */
                                                                    //JObject GlobalProvinces = WarAPI.GlobalProvinces((string)battle["provinces"][0]);


                                                                    Grid gr = new Grid();
                                                                    gr.SetResourceReference(Grid.StyleProperty, "ClanBattlesGrid");

                                                                    ColumnDefinition cd1 = new ColumnDefinition();
                                                                    ColumnDefinition cd2 = new ColumnDefinition();
                                                                    ColumnDefinition cd3 = new ColumnDefinition();
                                                                    ColumnDefinition cd4 = new ColumnDefinition();

                                                                    cd1.Width = new GridLength(1, GridUnitType.Auto);
                                                                    //cd2.Width = new GridLength(250, GridUnitType.Pixel);
                                                                    cd3.Width = new GridLength(1, GridUnitType.Auto);
                                                                    cd4.Width = new GridLength(1, GridUnitType.Auto);

                                                                    gr.ColumnDefinitions.Add(cd1);
                                                                    gr.ColumnDefinitions.Add(cd2);
                                                                    gr.ColumnDefinitions.Add(cd3);
                                                                    gr.ColumnDefinitions.Add(cd4);

                                                                    Image im = new Image();
                                                                    im.SetResourceReference(Image.StyleProperty, "Icon_" + (string)battle["type"]);

                                                                    TextBlock tbID = new TextBlock();
                                                                    tbID.Text = DateFormat((double)battle["time"], "HH:mm");
                                                                    tbID.SetResourceReference(TextBlock.StyleProperty, "CmTIME");
                                                                    Grid.SetColumn(tbID, 3);

                                                                    TextBlock CmName = new TextBlock();
                                                                    //CmName.Text = SelectTokenNoClan(GlobalProvinces, (string)battle["provinces"][0] + ".province_i18n");
                                                                    CmName.Text = (string)battle["provinces_i18n"][0]["name_i18n"];
                                                                    CmName.SetResourceReference(TextBlock.StyleProperty, "CmName");
                                                                    Grid.SetColumn(CmName, 1);

                                                                    TextBlock CmTitle = new TextBlock();
                                                                    CmTitle.Text = (string)battle["arenas"][0]["name_i18n"];
                                                                    CmTitle.SetResourceReference(TextBlock.StyleProperty, "CmTitle");
                                                                    Grid.SetColumn(CmTitle, 2);

                                                                    gr.Children.Add(im);
                                                                    gr.Children.Add(tbID);
                                                                    gr.Children.Add(CmName);
                                                                    gr.Children.Add(CmTitle);

                                                                    ListBoxItem lbi = new ListBoxItem();
                                                                    lbi.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                                                                    lbi.SetResourceReference(Grid.StyleProperty, "ClanBattles");
                                                                    lbi.Content = gr;

                                                                    ClanBattles.Items.Add(lbi);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //ClanBattlesNoRecords.Text = Lang.Set("PageUser", "ClanBattlesNoRecords", lang);

                                                                ListBoxItem lbi = new ListBoxItem();
                                                                lbi.SetResourceReference(ListBoxItem.StyleProperty, "rec_not_found");

                                                                TextBlock tb = new TextBlock();
                                                                tb.Text = Lang.Set("PageUser", "ClanBattlesNoRecords", lang);

                                                                lbi.Content = tb;

                                                                ClanBattles.Items.Clear();
                                                                ClanBattles.Items.Add(lbi);
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Clan battles", ex.Message, ex.StackTrace)); }


                                                    /*
                                                     *       Провинции клана
                                                     *       
                                                     * Тип:
                                                     *   Стартовая
                                                     *   Обычная
                                                     *   Ключевая
                                                     * Название
                                                     * Игровая карта
                                                     * Прайм-тайм
                                                     * Доход
                                                     * Время владения
                                                     * Провинция атакована
                                                     * 
                                                     * type
                                                     * name
                                                     * arena_i18n
                                                     * prime_time
                                                     * revenue
                                                     * occupancy_time
                                                     * attacked
                                                     */
                                                    try
                                                    {
                                                        tbProvinces.Text = Lang.Set("PageUser", "tbProvinces", lang);

                                                        if ((string)Provinces.SelectToken("status") == "ok")
                                                        {
                                                            JObject arr = (JObject)Provinces["data"];
                                                            if (arr.Count > 0)
                                                            {
                                                                ClanProvinces.Items.Clear();

                                                                foreach (var province in arr)
                                                                {
                                                                    Grid gr = new Grid();
                                                                    gr.SetResourceReference(Grid.StyleProperty, "ClanBattlesGrid");

                                                                    ColumnDefinition cd1 = new ColumnDefinition();
                                                                    ColumnDefinition cd2 = new ColumnDefinition();
                                                                    ColumnDefinition cd3 = new ColumnDefinition();
                                                                    ColumnDefinition cd4 = new ColumnDefinition();
                                                                    ColumnDefinition cd5 = new ColumnDefinition();
                                                                    ColumnDefinition cd6 = new ColumnDefinition();
                                                                    ColumnDefinition cd7 = new ColumnDefinition();

                                                                    cd1.Width = new GridLength(1, GridUnitType.Auto);
                                                                    //cd2.Width = new GridLength(1, GridUnitType.Auto);
                                                                    cd3.Width = new GridLength(1, GridUnitType.Auto);
                                                                    cd4.Width = new GridLength(50, GridUnitType.Pixel);
                                                                    cd5.Width = new GridLength(1, GridUnitType.Auto);
                                                                    cd6.Width = new GridLength(50, GridUnitType.Pixel);

                                                                    gr.ColumnDefinitions.Add(cd1);
                                                                    gr.ColumnDefinitions.Add(cd2);
                                                                    gr.ColumnDefinitions.Add(cd3);
                                                                    gr.ColumnDefinitions.Add(cd4);
                                                                    gr.ColumnDefinitions.Add(cd5);
                                                                    gr.ColumnDefinitions.Add(cd6);

                                                                    Image im = new Image();
                                                                    im.SetResourceReference(Image.StyleProperty, "province_types_" + (string)province.Value["type"]);

                                                                    TextBlock t1 = new TextBlock();
                                                                    t1.Text = (string)province.Value["name"];
                                                                    t1.SetResourceReference(TextBlock.StyleProperty, "t1");
                                                                    Grid.SetColumn(t1, 1);

                                                                    TextBlock t2 = new TextBlock();
                                                                    t2.Text = (string)province.Value["arena_i18n"];
                                                                    t2.SetResourceReference(TextBlock.StyleProperty, "t2");
                                                                    Grid.SetColumn(t2, 2);

                                                                    TextBlock t3 = new TextBlock();
                                                                    t3.Text = DateFormat((double)province.Value["prime_time"], "HH:mm");
                                                                    t3.SetResourceReference(TextBlock.StyleProperty, "t3");
                                                                    Grid.SetColumn(t3, 3);

                                                                    TextBlock t4 = new TextBlock();
                                                                    t4.Text = (string)province.Value["revenue"];
                                                                    t4.SetResourceReference(TextBlock.StyleProperty, "t4");
                                                                    Grid.SetColumn(t4, 4);

                                                                    TextBlock t5 = new TextBlock();
                                                                    t5.Text = (string)province.Value["occupancy_time"];
                                                                    t5.SetResourceReference(TextBlock.StyleProperty, "t5");
                                                                    Grid.SetColumn(t5, 5);

                                                                    gr.Children.Add(im);
                                                                    gr.Children.Add(t1);
                                                                    gr.Children.Add(t2);
                                                                    gr.Children.Add(t3);
                                                                    gr.Children.Add(t4);
                                                                    gr.Children.Add(t5);

                                                                    ListBoxItem lbi = new ListBoxItem();
                                                                    lbi.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                                                                    lbi.SetResourceReference(ListBoxItem.StyleProperty, (bool)province.Value["attacked"] ? "ClanProvincesAttacked" : "ClanProvincesNonAttacked");
                                                                    lbi.Content = gr;

                                                                    ClanProvinces.Items.Add(lbi);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //ClanProvincesNoRecords.Text = Lang.Set("PageUser", "ClanProvincesNoRecords", lang);

                                                                ListBoxItem lbi = new ListBoxItem();
                                                                lbi.SetResourceReference(ListBoxItem.StyleProperty, "rec_not_found");

                                                                TextBlock tb = new TextBlock();
                                                                tb.Text = Lang.Set("PageUser", "ClanProvincesNoRecords", lang);

                                                                lbi.Content = tb;

                                                                ClanProvinces.Items.Clear();
                                                                ClanProvinces.Items.Add(lbi);
                                                            }

                                                        }
                                                    }
                                                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Clan provincies", ex.Message, ex.StackTrace)); }
                                                }
                                            }
                                            else
                                            {
                                                MainWindow.JsonSettingsRemove("token");

                                                switch ((string)JAccountInfo.SelectToken("error.message"))
                                                {
                                                    case "INVALID_ACCESS_TOKEN": MessageBox.Show(Lang.Set("PageUser", "ActivateWarID", lang)); break;
                                                    default: MessageBox.Show(Lang.Set("PageUser", "ErrorDataJson", lang)); break;
                                                }

                                                try { MainWindow.Navigator(); }
                                                catch (Exception) { }
                                            }
                                        }
                                        catch (Exception e) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "if (!active)", e.Message, e.StackTrace)); }
                                    }));
                            }
                        }
                        catch (Exception e) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", e.Message, e.StackTrace)); }
                        finally
                        {
                            MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden;
                            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));
                        }
                    }));
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Проверяем существует ли аттрибут
        /// </summary>
        /// <param name="attr">Аттрибут для проверки</param>
        /// <returns>TRUE - существует, FALSE - не существует</returns>
        private bool CheckElement(string attr)
        {
            try
            {
                string elem = (string)MainWindow.JsonSettingsGet("token." + attr);
                if (elem != null && elem != "") return true;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "CheckElement()", "Attribute: " + attr, ex.Message, ex.StackTrace)); }
            return false;
        }

        private string DateFormat(double date, string format = "dd.MM.yyyy")
        {
            try
            {
                if (date > 0)
                {
                    DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds(date).ToLocalTime();
                    return dtDateTime.ToString(format);
                }
                else
                    return "--:--";

                //return DateTime.FromOADate(double.Parse(date)).ToString(format);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return date.ToString();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            bool active = false;
            try { active = CheckElement("access_token") && CheckElement("expires_at") && CheckElement("nickname") && CheckElement("account_id"); }
            catch (Exception) { active = false; }

            try
            {
                Task.Factory.StartNew(() =>
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible; }));
                    Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));
                });

                // Проверяем актуальность даты
                if (active)
                {
                    DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds((double)MainWindow.JsonSettingsGet("token.expires_at")).ToLocalTime();
                    active = dtDateTime > DateTime.UtcNow;
                }

                // Если токен неверный, либо устарел - выводим авторизацию
                if (!active)
                {
                    try { MainWindow.JsonSettingsRemove("token"); }
                    catch (Exception) { }

                    try
                    {
                        // Live hack
                        Window1 Win1 = new Window1();
                        Win1.Show();
                        Win1.Close();

                        // Открываем окно
                        //new WarApiOpenID().ShowDialog();

                        Dispatcher.BeginInvoke(new ThreadStart(delegate
                        {
                            MainWindow.LoadPage.Content = Lang.Set("PageLoading", "lLoading", (string)MainWindow.JsonSettingsGet("info.language"));
                            MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible;
                        }));
                        Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));

                        Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator("WgOpenIdAIRUS"); }));
                    }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "if (!active)", ex.Message, ex.StackTrace)); }
                }
                else
                    Task.Factory.StartNew(() => { AccountInfo(); });
            }
            catch (Exception ex0) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "if (active)", ex0.Message, ex0.StackTrace)); }
        }

        private string SetSumm(string summ)
        {
            try
            {
                object[] args = { Convert.ToDouble(summ.Replace(".", ",")) };
                return String.Format("{0,1:N0}", args);
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "SetSumm()", "Summ = " + summ, ex.Message, ex.StackTrace)); }
            return "---";
        }
    }
}