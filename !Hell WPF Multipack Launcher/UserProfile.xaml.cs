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
        Classes.Debug Debug = new Classes.Debug();
        Classes.Language Lang = new Classes.Language();

        string lang = Properties.Resources.Default_Lang;
        //private ObservableCollection<AssetClass> classes;

        /// <summary>
        /// Handle clicks on the listview column heading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            /*try { piePlotter.PlottedProperty = ((GridViewColumnHeader)e.OriginalSource).Column.Header.ToString(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "OnColumnHeaderClick()", ex.Message, ex.StackTrace)); }*/
        }

        public UserProfile()
        {
            InitializeComponent();

            lang = MainWindow.XmlDocument.Root.Element("info").Attribute("language").Value.Trim();
            Task.Factory.StartNew(() => { AccountInfo(); }).Wait();

            Style s = new Style();
            s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            tc.ItemContainerStyle = s;
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoadingPanelShow(1);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { MainWindow.Navigator(); }));
                }
                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "bClose_Click()", ex.Message, ex.StackTrace)); }
            });
        }

        /// <summary>
        /// Получаем информацию по аккаунту.
        /// Если токен активен, сразу выводим.
        /// Если нет, вначале запрашиваем авторизацию, после чего получаем информацию.
        /// </summary>
        private void AccountInfo()
        {
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
                Classes.WargamingAPI WarAPI = new Classes.WargamingAPI();

                bool active = CheckElement("access_token") && CheckElement("expires_at") && CheckElement("nickname") && CheckElement("account_id");

                try
                {
                    if (active)
                    {
                        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(MainWindow.XmlDocument.Root.Element("token").Attribute("expires_at").Value)).ToLocalTime();

                        active = dtDateTime > DateTime.UtcNow;
                    }

                    if (!active)
                    {
                        if (MainWindow.XmlDocument.Root.Element("token") != null)
                            MainWindow.XmlDocument.Root.Element("token").Remove();
                    }
                }
                catch (Exception e) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "AccountInfo()", "if (active)", e.Message, e.StackTrace)); }

                if (!active)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        try
                        {
                            WarApiOpenID WarApiOpenID = new WarApiOpenID();
                            WarApiOpenID.WB.Source = new Uri(WarAPI.OpenID());
                            WarApiOpenID.ShowDialog();

                            active = CheckElement("access_token") && CheckElement("expires_at") && CheckElement("nickname") && CheckElement("account_id");
                            if (!active)
                            {
                                MessageBox.Show(Lang.Set("PageUser", "ActivateWarID", lang));

                                MainWindow.LoadingPanelShow(1);

                                try { MainWindow.Navigator(); }
                                catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "bClose_Click()", ex.Message, ex.StackTrace)); }
                            }
                        }
                        catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "AccountInfo()", "if (!active)", ex.Message, ex.StackTrace)); }
                    }));
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

                               PlayerName.Text = GetElement("nickname");

                               JObject obj = JObject.Parse(WarAPI.AccountInfo(GetElement("account_id"), GetElement("access_token")));

                               //obj["data"][GetElement("account_id")]["clan_id"] = 103556; // Подставной клан

                               JObject Clan = JObject.Parse(WarAPI.ClanInfo(SelectToken(obj, "clan_id"), GetElement("access_token")));
                               JObject Battles = JObject.Parse(WarAPI.ClanBattles(SelectToken(obj, "clan_id"), GetElement("access_token")));
                               JObject Provinces = JObject.Parse(WarAPI.ClanProvinces(SelectToken(obj, "clan_id"), GetElement("access_token"), "type,name,arena_i18n,prime_time,revenue,occupancy_time,attacked"));


                               if (SelectToken(obj, "status", false) == "ok")
                               {
                                   /* =========================================
                                    *       Общая информация о пользователе
                                    * =========================================*/
                                   try
                                   {
                                       DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                       dt = dt.AddSeconds(Convert.ToDouble(SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "members." + GetElement("account_id") + ".created_at")));
                                       TimeSpan ts = DateTime.Now - dt;


                                       /*DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                       dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "members." + GetElement("account_id") + ".created_at")));
                                       DateTime dtNow = new DateTime();
                                       TimeSpan dtDays = dtNow - dtDateTime;*/

                                       PlayerClan.Text = "[" + SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "abbreviation") + "]";
                                       //PlayerClanDays.Text = ts.Days.ToString();

                                       PlayerClan2.Text = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "name") +
                                           " [" + SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "abbreviation") + "]";
                                       ClanFullname.Text = PlayerClan2.Text;

                                       string uid = MainWindow.XmlDocument.Root.Element("token").Attribute("account_id").Value.Trim();
                                       PlayerZvanie.Text = Lang.Set("Rank", SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "members." + uid + ".role"), lang);

                                       //   emblems.medium
                                       //   SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "emblems.medium")
                                       //ClanEmblem.Source = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "emblems.medium");
                                       //ClanEmblem2.Source = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "emblems.medium");

                                       try
                                       {
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
                                   catch (Exception) { PlayerClan.Text = "[---]"; PlayerClan2.Text = "---"; /*tbUpClan.Text = "Произошла ошибка или ты не состоишь в клане " + SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "name"); */}



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
                                   catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "AccountInfo()", "Graphic", ex.Message, ex.StackTrace)); }
                                   */

                                   /*
                                    *   ВКЛАДКА КЛАН
                                    *       Обшая информация
                                    */
                                   try
                                   {
                                       ClanDesc.Text = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "motto");
                                       //ClanFullname.Text += SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "name");
                                       //ClanAbbr.Text += SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "abbreviation");
                                       ClanCount.Text = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "members_count");

                                       dataonTitle.Text = Lang.Set("PageUser", "tbDataOn", lang);
                                       dataon.Text = DateFormat(SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "updated_at"));

                                   }
                                   catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "AccountInfo()", "Clan", ex.Message, ex.StackTrace)); }


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
                                   catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "AccountInfo()", "Clan members", ex.Message, ex.StackTrace)); }



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
                                   catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "AccountInfo()", "Clan battles", ex.Message, ex.StackTrace)); }


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
                                   catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "AccountInfo()", "Clan provincies", ex.Message, ex.StackTrace)); }
                               }
                               else
                                   MessageBox.Show(Lang.Set("PageUser", "ErrorDataJson", lang));
                           }
                           catch (Exception e) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "AccountInfo()", "if (!active)", e.Message, e.StackTrace)); }
                       }));
                }
            }
            catch (Exception e) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "AccountInfo()", e.Message, e.StackTrace)); }
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
            try { return obj.SelectToken(!data ? key : String.Format("data.{0}.{1}", GetElement("account_id"), key)).ToString(); }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "SelectToken()", "Key: " + key, "Data: " + data, obj.ToString(), ex.Message, ex.StackTrace));
                return "fail";
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
                Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "SelectTokenClan()", "Clan" + clan_id, "Key: " + key, "Data: " + data, obj.ToString(), ex.Message, ex.StackTrace));
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
                Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "SelectTokenNoClan()", "Key: " + key, "Data: " + data, obj.ToString(), ex.Message, ex.StackTrace));
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
                if (MainWindow.XmlDocument.Root.Element("token") != null)
                    if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr) != null)
                        if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value != "")
                            return true;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "CheckElement()", "Attribute: " + attr, ex.Message, ex.StackTrace)); }
            return false;
        }

        /// <summary>
        /// Получаем значение аттрибута
        /// </summary>
        /// <param name="attr">Аттрибут</param>
        /// <returns>Значение</returns>
        private string GetElement(string attr)
        {
            try
            {
                if (MainWindow.XmlDocument.Root.Element("token") != null)
                    if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr) != null)
                        if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value != "")
                            return MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value;
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("UserProfile.xaml", "GetElement()", "Attribute: " + attr, ex.Message, ex.StackTrace)); }

            return "fail";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try { MainWindow.LoadingPanelShow(); }
            catch (Exception) { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 0;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 1;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 2;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 3;
        }

        private string DateFormat(string date, string format = "dd.MM.yyyy")
        {
            try {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(double.Parse(date)).ToLocalTime();
                return dtDateTime.ToString(format);

                //return DateTime.FromOADate(double.Parse(date)).ToString(format);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return date; }
        }
    }
}
