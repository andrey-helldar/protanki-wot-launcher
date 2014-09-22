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
        private ObservableCollection<AssetClass> classes;

        /// <summary>
        /// Handle clicks on the listview column heading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            GridViewColumn column = ((GridViewColumnHeader)e.OriginalSource).Column;
            piePlotter.PlottedProperty = column.Header.ToString();
        }

        public UserProfile()
        {
            InitializeComponent();

            Task.Factory.StartNew(() => { AccountInfo(); });
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigator("General", "Settings.xaml");
        }

        /// <summary>
        /// Получаем информацию по аккаунту.
        /// Если токен активен, сразу выводим.
        /// Если нет, вначале запрашиваем авторизацию, после чего получаем информацию.
        /// </summary>
        private void AccountInfo()
        {
            Classes.WargamingAPI WarAPI = new Classes.WargamingAPI();

            bool active = CheckElement("access_token") && CheckElement("expires_at") && CheckElement("nickname") && CheckElement("account_id");

            if (active)
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(MainWindow.XmlDocument.Root.Element("token").Attribute("expires_at").Value)).ToLocalTime();

                active = dtDateTime > DateTime.UtcNow;
            }

            if (!active)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    try
                    {
                        WarApiOpenID WarApiOpenID = new WarApiOpenID();
                        WarApiOpenID.WB.Source = new Uri(WarAPI.OpenID());
                        WarApiOpenID.ShowDialog();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace); }
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

                           //obj["data"][GetElement("account_id")]["clan_id"] = 103556;
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

                                   PlayerClan.Text = SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "name");
                                   PlayerClanDays.Text = ts.Days.ToString();
                               }
                               catch (Exception) { tbUpClan.Text = "Произошла ошибка или ты не состоишь в клане " + SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "name"); }
                               

                               PlayerGold.Text = SelectToken(obj, "private.gold");
                               rating.Content = "Личный рейтинг: "+SelectToken(obj, "global_rating");
                               credit.Content = "Кредитов: " + SelectToken(obj, "private.credits");
                               opit.Content = "Свободный опыт: " + SelectToken(obj, "private.free_xp");

                               prem.Content = SelectToken(obj, "private.is_premium") == "True" ? "Премиум аккаунт" : "Базовый аккаунт";

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


                               /*
                                *   ВКЛАДКА КЛАН
                                *       Обшая информация
                                */
                               ClanFullname.Text += SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "name");
                               ClanAbbr.Text += SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "abbreviation");
                               ClanCount.Text += SelectTokenClan(Clan, SelectToken(obj, "clan_id"), "members_count");


                               /*
                                *       Члены клана
                                */
                               ClanMembers.Items.Clear();
                               int i = 0;

                               foreach (var member in (JObject)Clan["data"][SelectToken(obj, "clan_id")]["members"])
                               {
                                   ClanMembers.Items.Add(String.Format("{0}  ::  {1}  ::  {2}  ::  {3}",
                                       (++i).ToString(),
                                       (string)member.Value["account_name"],
                                       (string)member.Value["role_i18n"],
                                       (string)member.Value["created_at"]
                                   ));
                               }



                               /*
                                *       Бои клана
                                */
                               ClanBattles.Items.Clear();

                               if (SelectTokenClan(Battles, SelectToken(obj, "clan_id"), "status", false) == "ok")
                               {
                                   foreach (var battle in (JArray)Battles["data"][SelectToken(obj, "clan_id")])
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
                                       
                                       ClanBattles.Items.Add(
                                           String.Format("{0}  ::  {1}  ::  {2}  ::  {3}",
                                                (string)battle["type"],
                                                (string)battle["time"],
                                                SelectTokenNoClan(GlobalProvinces, (string)battle["provinces"][0] + ".province_i18n"),
                                                (string)battle["arenas"][0]["name_i18n"]
                                       ));
                                   }
                               }
                               else
                                   MessageBox.Show("STATUS NOT OK");


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

                               if (SelectTokenClan(Provinces, SelectToken(obj, "clan_id"), "status", false) == "ok")
                               {
                                   ClanProvinces.Items.Clear();

                                   foreach (var province in (JObject)Provinces["data"])
                                   {
                                       ClanProvinces.Items.Add(
                                           String.Format(
                                            "{0}\t::\t{1}\t::\t{2}\t::\t{3}\t::\t{4}\t::\t{5}\t::\t{6}",
                                            (string)province.Value["type"],
                                            (string)province.Value["name"],
                                            (string)province.Value["arena_i18n"],
                                            (string)province.Value["prime_time"],
                                            (string)province.Value["revenue"],
                                            (string)province.Value["occupancy_time"],
                                            (string)province.Value["attacked"]
                                           )
                                       );
                                   }
                               }
                               else
                                   MessageBox.Show("STATUS NOT OK");
                           }
                           else
                               MessageBox.Show("STATUS NOT OK _0");
                       }
                       catch (Exception ex) { MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace); }
                   }));
            }
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
            return obj.SelectToken(!data ? key : String.Format("data.{0}.{1}", GetElement("account_id"), key)).ToString();
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
            return obj.SelectToken(!data ? key : String.Format("data.{0}.{1}", clan_id, key)).ToString();
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
            return obj.SelectToken(!data ? key : String.Format("data.{0}", key)).ToString();
        }


        /// <summary>
        /// Проверяем существует ли аттрибут
        /// </summary>
        /// <param name="attr">Аттрибут для проверки</param>
        /// <returns>TRUE - существует, FALSE - не существует</returns>
        private bool CheckElement(string attr)
        {
            if (MainWindow.XmlDocument.Root.Element("token") != null)
                if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr) != null)
                    if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value != "")
                        return true;
            return false;
        }

        /// <summary>
        /// Получаем значение аттрибута
        /// </summary>
        /// <param name="attr">Аттрибут</param>
        /// <returns>Значение</returns>
        private string GetElement(string attr)
        {
            if (MainWindow.XmlDocument.Root.Element("token") != null)
                if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr) != null)
                    if (MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value != "")
                        return MainWindow.XmlDocument.Root.Element("token").Attribute(attr).Value;
            return "NULL";
        }
    }
}
