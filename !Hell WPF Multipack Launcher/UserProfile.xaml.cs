using System;
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
            catch (Exception) { }

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
            catch (Exception) { }


            try
            {
                // Проверяем актуальность токена
                bool active = CheckElement("access_token") && CheckElement("expires_at") && CheckElement("nickname") && CheckElement("account_id");

                Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        try
                        {
                            MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible;
                            Thread.Sleep(Convert.ToInt16(Properties.Resources.Default_Navigator_Sleep));


                            active = CheckElement("access_token") && CheckElement("expires_at") && CheckElement("nickname") && CheckElement("account_id");

                            if (!active)
                            {
                                try { MainWindow.Navigator(); }
                                catch (Exception) { }
                            }
                            else
                            {
                                Dispatcher.BeginInvoke(new ThreadStart(delegate
                                    {
                                        try
                                        {
                                            /*
                                             *      Читаем информацию о пользователе
                                             */
                                            nickname = (string)MainWindow.JsonSettingsGet("token.nickname"); // Переменная, чтобы по 100 раз не читать конфиг
                                            account_id = (string)MainWindow.JsonSettingsGet("token.account_id");
                                            access_token = (string)MainWindow.JsonSettingsGet("token.access_token");

                                            PlayerName.Text = nickname;

                                            JObject obj = JObject.Parse(WarAPI.AccountInfo(account_id, access_token));
                                            JObject
                                                Clan = null,
                                                Battles = null,
                                                Provinces = null;


                                            if (SelectToken(obj, "status", false) == "ok")
                                            {
                                                /* =========================================
                                                 *       Проверяем клан
                                                 *       Если параметр пуст - деактивируем вкладки
                                                 * =========================================*/
                                                try
                                                {
                                                    if (SelectToken(obj, "clan_id") == "")
                                                    {
                                                        tiClanInfo.IsEnabled = false;
                                                        tiClanBattles.IsEnabled = false;
                                                        tiClanProvinces.IsEnabled = false;
                                                    }
                                                    else
                                                    {
                                                        Clan = JObject.Parse(WarAPI.ClanInfo(SelectToken(obj, "clan_id"), access_token));
                                                        Battles = JObject.Parse(WarAPI.ClanBattles(SelectToken(obj, "clan_id"), access_token));
                                                        Provinces = JObject.Parse(WarAPI.ClanProvinces(SelectToken(obj, "clan_id"), access_token, "type,name,arena_i18n,prime_time,revenue,occupancy_time,attacked"));

                                                        //obj["data"][GetElement("account_id")]["clan_id"] = 103556; // Подставной клан                                                
                                                    }
                                                }
                                                catch (Exception) { }



                                                /* =========================================
                                                 *       Общая информация о пользователе
                                                 * =========================================*/
                                                try
                                                {
                                                    // Краткая информация по клану
                                                    if (Clan != null)
                                                    {
                                                        try
                                                        {
                                                            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                                            dt = dt.AddSeconds(Convert.ToDouble(SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "members." + account_id + ".created_at")));
                                                            TimeSpan ts = DateTime.Now - dt;

                                                            PlayerClan.Text = "[" + SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "abbreviation") + "]";

                                                            PlayerClan2.Text = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "name");
                                                            ClanFullname.Text = PlayerClan2.Text;

                                                            PlayerZvanie.Text = Lang.Set("Rank", SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "members." + account_id + ".role"), lang);

                                                            //   emblems.medium
                                                            //   SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "emblems.medium")
                                                            //ClanEmblem.Source = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "emblems.medium");
                                                            //ClanEmblem2.Source = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "emblems.medium");

                                                            var image = new Image();
                                                            var fullFilePath = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "emblems.medium", true);

                                                            BitmapImage bitmap = new BitmapImage();
                                                            bitmap.BeginInit();
                                                            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                                                            bitmap.EndInit();

                                                            ClanEmblem.Source = bitmap;
                                                            ClanEmblem2.Source = bitmap;
                                                        }
                                                        catch (Exception) { }
                                                    }
                                                    else
                                                    {
                                                        PlayerClan2.Text = Lang.Set("PageUser", "NotClan", lang);
                                                        PlayerZvanie.Text = "";
                                                    }


                                                    // Процент побед
                                                    PercWins.Text = Lang.Set("PageUser", "tbPercentWins", lang);
                                                    PercWinsPerc.Text = String.Format("{0}%", (Math.Round(((Convert.ToDouble(SelectToken(obj, "statistics.all.wins")) / Convert.ToDouble(SelectToken(obj, "statistics.all.battles"))) * 100), 2)).ToString());

                                                    // Личный рейтинг
                                                    MyRating.Text = Lang.Set("PageUser", "tbMyRating", lang);
                                                    MyRatingPerc.Text = SelectToken(obj, "global_rating");

                                                    // Средний опыт за бой
                                                    AvgXP.Text = Lang.Set("PageUser", "tbAvgXP", lang);
                                                    AvgXPPerc.Text = SelectToken(obj, "statistics.all.battle_avg_xp");

                                                    // Количество боев
                                                    BattleCount.Text = Lang.Set("PageUser", "tbCountWars", lang);
                                                    BattleCountPerc.Text = SelectToken(obj, "statistics.all.battles");

                                                    // Средний нанесенный урон за бой
                                                    AvgDamage.Text = Lang.Set("PageUser", "tbAvgDamage", lang);
                                                    AvgDamagePerc.Text = SelectToken(obj, "statistics.all.avg_damage_assisted");
                                                }
                                                catch (Exception) { }



                                                /*PlayerGold.Text = SelectToken(obj, "private.gold");
                                                rating.Content = "Личный рейтинг: " + SelectToken(obj, "global_rating");
                                                credit.Content = "Кредитов: " + SelectToken(obj, "private.credits");
                                                opit.Content = "Свободный опыт: " + SelectToken(obj, "private.free_xp");

                                                prem.Content = SelectToken(obj, "private.is_premium") == "True" ? "Премиум аккаунт" : "Базовый аккаунт";*/

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

                                                /*
                                                 *   ВКЛАДКА КЛАН
                                                 *       Обшая информация
                                                 */
                                                if (Clan != null)
                                                {
                                                    try
                                                    {
                                                        ClanDesc.Text = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "motto");
                                                        //ClanFullname.Text += SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "name");
                                                        //ClanAbbr.Text += SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "abbreviation");
                                                        ClanCount.Text = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "members_count");

                                                        dataonTitle.Text = Lang.Set("PageUser", "tbDataOn", lang);
                                                        dataon.Text = DateFormat(SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "updated_at"));

                                                    }
                                                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Clan", ex.Message, ex.StackTrace)); }


                                                    /*
                                                     *       Члены клана
                                                     */
                                                    try
                                                    {
                                                        ClanMembers.Items.Clear();
                                                        int i = 1;

                                                        foreach (var member in (JObject)Clan["data"][SelectToken(obj, "clan_id")]["members"])
                                                        {
                                                            /*
                                                             * <ListBoxItem Style="{DynamicResource lbiProcess}" Width="396">
                                                             *   <Grid>
                                                             *       <Grid.ColumnDefinitions>
                                                             *           <ColumnDefinition Width="30"/>
                                                             *           <ColumnDefinition Width="155"/>
                                                             *           <ColumnDefinition Width="130"/>
                                                             *           <ColumnDefinition Width="80"/>
                                                             *       </Grid.ColumnDefinitions>
                                                             *       <TextBlock Style="{DynamicResource CmID}" Text="100" />
                                                             *       <TextBlock Style="{DynamicResource CmName}" Text="d_voronoff" Grid.Column="1"/>
                                                             *       <TextBlock Style="{DynamicResource CmTitle}" Text="Командующий" Grid.Column="2"/>
                                                             *       <TextBlock Style="{DynamicResource CmDate}" Text="01.01.2014" Grid.Column="3"/>
                                                             *   </Grid>
                                                             * </ListBoxItem>
                                                             */
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
                                                            //CmTitle.Text = (string)member.Value["role"];
                                                            CmTitle.SetResourceReference(TextBlock.StyleProperty, "CmTitle");
                                                            Grid.SetColumn(CmTitle, 2);

                                                            TextBlock CmDate = new TextBlock();
                                                            CmDate.Text = DateFormat((string)member.Value["created_at"]);
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
                                                            /*ClanMembers.Items.Add(String.Format("{0}  ::  {1}  ::  {2}  ::  {3}",
                                                                (++i).ToString(),
                                                                (string)member.Value["account_name"],
                                                                (string)member.Value["role_i18n"],
                                                                (string)member.Value["created_at"]
                                                            ));*/
                                                        }
                                                    }
                                                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Clan members", ex.Message, ex.StackTrace)); }



                                                    /*
                                                     *       Бои клана
                                                     */
                                                    try
                                                    {
                                                        if (SelectTokenClan(Battles, SelectToken(obj, "clan_id"), "status", false) == "ok")
                                                        {
                                                            JArray arr = (JArray)Battles["data"][SelectToken(obj, "clan_id")];
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
                                                                    JObject GlobalProvinces = JObject.Parse(WarAPI.GlobalProvinces((string)battle["provinces"][0]));


                                                                    Grid gr = new Grid();

                                                                    ColumnDefinition cd1 = new ColumnDefinition();
                                                                    ColumnDefinition cd2 = new ColumnDefinition();
                                                                    ColumnDefinition cd3 = new ColumnDefinition();
                                                                    ColumnDefinition cd4 = new ColumnDefinition();

                                                                    cd1.Width = new GridLength(30, GridUnitType.Pixel);
                                                                    cd2.Width = new GridLength(155, GridUnitType.Pixel);
                                                                    cd3.Width = new GridLength(130, GridUnitType.Pixel);
                                                                    cd4.Width = new GridLength(80, GridUnitType.Pixel);

                                                                    gr.ColumnDefinitions.Add(cd1);
                                                                    gr.ColumnDefinitions.Add(cd2);
                                                                    gr.ColumnDefinitions.Add(cd3);
                                                                    gr.ColumnDefinitions.Add(cd4);

                                                                    Image im = new Image();
                                                                    im.SetResourceReference(Image.StyleProperty, "Icon_" + (string)battle["type"]);

                                                                    TextBlock tbID = new TextBlock();
                                                                    tbID.Text = DateFormat((string)battle["time"], "m:s");
                                                                    tbID.SetResourceReference(TextBlock.StyleProperty, "CmTIME");
                                                                    Grid.SetColumn(tbID, 0);

                                                                    TextBlock CmName = new TextBlock();
                                                                    CmName.Text = SelectTokenNoClan(GlobalProvinces, (string)battle["provinces"][0] + ".province_i18n");
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

                                                                    ClanBattles.Items.Add(gr);
                                                                }
                                                            }
                                                            else
                                                                ClanBattlesNoRecords.Text = Lang.Set("PageUser", "ClanBattlesNoRecords", lang);
                                                            /* else
                                                             {
                                                                 TextBlock tbCB = new TextBlock();
                                                                 tbCB.Text = Lang.Set("PageGeneral", "RecordsNotFound", lang);

                                                                 ListBoxItem lbiCB = new ListBoxItem();
                                                                 lbiCB.SetResourceReference(ListBoxItem.StyleProperty, "rec_not_found");
                                                                 lbiCB.Content = tbCB;

                                                                 ClanBattles.Items.Add(lbiCB);
                                                             }*/
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
                                                        if (SelectTokenClan(Provinces, SelectToken(obj, "clan_id"), "status", false) == "ok")
                                                        {
                                                            JObject arr = (JObject)Provinces["data"];
                                                            if (arr.Count > 0)
                                                            {
                                                                ClanProvinces.Items.Clear();

                                                                foreach (var province in arr)
                                                                {
                                                                    Grid gr = new Grid();
                                                                    if ((string)province.Value["attacked"] == "true") gr.SetResourceReference(Grid.StyleProperty, "attacked");

                                                                    ColumnDefinition cd1 = new ColumnDefinition();
                                                                    ColumnDefinition cd2 = new ColumnDefinition();
                                                                    ColumnDefinition cd3 = new ColumnDefinition();
                                                                    ColumnDefinition cd4 = new ColumnDefinition();
                                                                    ColumnDefinition cd5 = new ColumnDefinition();
                                                                    ColumnDefinition cd6 = new ColumnDefinition();
                                                                    ColumnDefinition cd7 = new ColumnDefinition();

                                                                    cd1.Width = new GridLength(30, GridUnitType.Auto);
                                                                    cd2.Width = new GridLength(30, GridUnitType.Auto);
                                                                    cd3.Width = new GridLength(30, GridUnitType.Auto);
                                                                    cd4.Width = new GridLength(30, GridUnitType.Auto);
                                                                    cd5.Width = new GridLength(30, GridUnitType.Auto);
                                                                    cd6.Width = new GridLength(30, GridUnitType.Auto);

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
                                                                    t3.Text = DateFormat((string)province.Value["prime_time"], "m:s");
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

                                                                    ClanProvinces.Items.Add(gr);
                                                                }

                                                            }
                                                            else
                                                                ClanProvincesNoRecords.Text = Lang.Set("PageUser", "ClanProvincesNoRecords", lang);
                                                            /*else
                                                            {
                                                                TextBlock tbCP = new TextBlock();
                                                                tbCP.Text = Lang.Set("PageGeneral", "RecordsNotFound", lang);

                                                                ListBoxItem lbiCP = new ListBoxItem();
                                                                lbiCP.SetResourceReference(ListBoxItem.StyleProperty, "rec_not_found");
                                                                lbiCP.Content = tbCP;

                                                                ClanProvinces.Items.Add(lbiCP);
                                                            }*/
                                                        }
                                                    }
                                                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "Clan provincies", ex.Message, ex.StackTrace)); }
                                                }
                                            }
                                            else
                                            {
                                                MainWindow.JsonSettingsRemove("token");

                                                switch (SelectToken(obj, "error.message", false))
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
        /// Из объекта JObject выбираем нужный токен.
        /// </summary>
        /// <param name="obj">JObject</param>
        /// <param name="key">Ключ выборки</param>
        /// <param name="data">Если TRUE, то выбираем ключ из массива DATA информации о пользователе</param>
        /// <returns>Возвращаем значение элемента в текстовом формате</returns>
        private string SelectToken(JObject obj, string key, bool data = true)
        {
            try { return obj.SelectToken(!data ? key : String.Format("data.{0}.{1}", account_id, key)).ToString(); }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "SelectToken()", "Key: " + key, "Data: " + data, obj.ToString(), ex.Message, ex.StackTrace));
                return null;
            }
        }

        /// <summary>
        /// Из объекта клана JObject выбираем нужный токен.
        /// </summary>
        /// <param name="obj">JObject</param>
        /// <param name="clan_id">Идентификатор клана</param>
        /// <param name="key">Ключ выборки</param>
        /// <param name="data">Если TRUE, то выбираем ключ из массива DATA информации о пользователе</param>
        /// <returns>Возвращаем значение элемента в текстовом формате</returns>
        private string SelectTokenClan(JObject obj, string clan_id, string key, bool data = true)
        {
            try { return obj.SelectToken(!data ? key : String.Format("data.{0}.{1}", clan_id, key)).ToString(); }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "SelectTokenClan()", "Clan" + clan_id, "Key: " + key, "Data: " + data, obj.ToString(), ex.Message, ex.StackTrace));
                return "fail";
            }
        }

        /// <summary>
        /// Получение информации из раздела
        /// </summary>
        /// <param name="obj">JObject</param>
        /// <param name="key">Ключ</param>
        /// <param name="data">Искать в разделе DATA?</param>
        /// <returns>JSON</returns>
        private string SelectTokenNoClan(JObject obj, string key, bool data = true)
        {
            try { return obj.SelectToken(!data ? key : String.Format("data.{0}", key)).ToString(); }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "SelectTokenNoClan()", "Key: " + key, "Data: " + data, obj.ToString(), ex.Message, ex.StackTrace));
                return "fail";
            }
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

        private string DateFormat(string date, string format = "dd.MM.yyyy")
        {
            try
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(double.Parse(date)).ToLocalTime();
                return dtDateTime.ToString(format);

                //return DateTime.FromOADate(double.Parse(date)).ToString(format);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return date;
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
                        new WarApiOpenID().ShowDialog();

                        // Если токен неактивен, загружаем главную страницу
                        if (!(CheckElement("access_token") && CheckElement("expires_at") && CheckElement("nickname") && CheckElement("account_id")))
                        {
                            Task.Factory.StartNew(() =>
                            {
                                try { Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); })); }
                                catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "bClose_Click()", ex.Message, ex.StackTrace)); }
                            });
                        }
                        else
                        {
                            // Если все нормально - грузим информацию о пользователе
                            Task.Factory.StartNew(() => { AccountInfo(); });
                        }
                    }
                    catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "if (!active)", ex.Message, ex.StackTrace)); }
                }
                else
                    Task.Factory.StartNew(() => { AccountInfo(); });
            }
            catch (Exception ex0) { Task.Factory.StartNew(() => Debugging.Save("UserProfile.xaml", "AccountInfo()", "if (active)", ex0.Message, ex0.StackTrace)); }
        }
    }
}
