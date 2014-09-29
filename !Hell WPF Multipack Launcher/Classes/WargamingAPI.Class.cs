using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class WargamingAPI
    {
        Debug Debug = new Debug();


        /// <summary>
        /// Формирование ссылки для авторизации приложения в системе Wargaming OpenID
        /// </summary>
        /// <returns>Ссылка на авторизацию методом GET</returns>
        public string OpenID()
        {
            try
            {
                string Data = "?application_id=" + Properties.Resources.API;
                Data += "&redirect_uri="+Properties.Resources.Developer+"/tk.html";
                Data += "&display=page";

                return Properties.Resources.API_Protocol+ Properties.Resources.API_OpenID + Data;
            }
            catch (Exception) { return "FAIL"; }
        }


        /// <summary>
        /// Запрашиваем список пользователей для вычисления ACCOUNT_ID
        /// </summary>
        /// <param name="name">Имя пользователя для начала поиска (поисковый запрос)</param>
        /// <returns>Выводим список пользователей с идентификаторами</returns>
        public Dictionary<string, string> AccountList(string name)
        {
            string Data = String.Empty;

            try
            {
                Data = "application_id=" + Properties.Resources.API;
                Data += "&fields=" + "nickname,account_id";
                Data += "&search=" + name;

                JObject obj = JObject.Parse(POST(Properties.Resources.API_Protocol + Properties.Resources.API_Account_List, Data));
                Dictionary<string, string> users = new Dictionary<string, string>();

                users.Add("status", obj.SelectToken("status").ToString());

                try
                {
                    if (obj.SelectToken("status").ToString() == "ok")
                        for (int i = 0; i < Convert.ToInt16(obj.SelectToken("count").ToString()); i++)
                            users.Add(obj.SelectToken("data[" + i.ToString() + "].nickname").ToString(), obj.SelectToken("data[" + i.ToString() + "].account_id").ToString());

                    return users;
                }
                catch (Exception) { return users; }                
            }
            catch (WebException we) { Debug.Save("WargamingAPI.Class", "AccountList()", "Username: " + name, "JSON: " + Data, we.Message, we.StackTrace); return null; }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "AccountList()", "Username: " + name, "JSON: " + Data, ex.Message, ex.StackTrace); return null; }
        }

        /// <summary>
        /// Получение информации о пользователе
        /// </summary>
        /// <param name="account_id">Идентификатор пользователя</param>
        /// <param name="access_token">Если ключ введен, отображается личная информация, иначе только общая</param>
        /// <returns>Возврат данных в формате JSON</returns>
        public string AccountInfo(string account_id, string access_token="")
        {
            /*
             * https://ru.wargaming.net/developers/api_reference/wot/account/info/
             */

            try
            {
                string Data = "application_id=" + Properties.Resources.API;
                //Data += "&fields=" + "clan_id,global_rating,private.credits,private.free_xp,private.gold,private.is_premium,private.premium_expires_at,statistics.all.battle_avg_xp,statistics.all.battles";
                Data += "&account_id=" + account_id;
                if (access_token.Length > 0) Data += "&access_token=" + access_token;

                return POST(Properties.Resources.API_Protocol + Properties.Resources.API_Account_Info, Data);
            }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "AccountInfo()", "Account ID: " + account_id, ex.Message, ex.StackTrace); return null; }
        }

        /// <summary>
        /// Информация о клане
        /// </summary>
        /// <param name="clan_id">ID клана</param>
        /// <param name="access_token">Токен</param>
        /// <param name="fields">Запрос определенных полей</param>
        /// <returns>JSON ответ</returns>
        public string ClanInfo(string clan_id, string access_token="", string fields="")
        {
            /*
             * https://api.worldoftanks.ru/wot/clan/info/
             */

            string Data = String.Empty;

            try
            {
                Data = "application_id=" + Properties.Resources.API;
                Data += "&clan_id=" + clan_id;
                if (access_token.Trim().Length > 0) Data += "&access_token=" + access_token;
                if (fields.Trim().Length > 0) Data += "&fields=" + fields;

                return POST(Properties.Resources.API_Protocol + Properties.Resources.API_Clan_Info, Data);
            }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "ClanInfo()", "Clan ID: " + clan_id, ex.Message, ex.StackTrace); }

            return null;
        }
        
        /// <summary>
        /// Список боев клана
        /// </summary>
        /// <param name="clan_id">ID клана</param>
        /// <param name="access_token">Токен</param>
        /// <param name="fields">Запрос определенных полей</param>
        /// <returns>JSON ответ</returns>
        public string ClanBattles(string clan_id, string access_token="", string fields = "")
        {
            /*
             * api.worldoftanks.ru/wot/clan/battles/
             */
            string Data = String.Empty;

            try
            {
                Data = "application_id=" + Properties.Resources.API;
                Data += "&clan_id=" + clan_id;
                if (access_token.Trim().Length > 0) Data += "&access_token=" + access_token;
                if (fields.Trim().Length > 0) Data += "&fields=" + fields;

                return POST(Properties.Resources.API_Protocol + Properties.Resources.API_Clan_Battles, Data);
            }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "ClanBattles()", "Clan ID: " + clan_id, ex.Message, ex.StackTrace); return null; }
        }

        /// <summary>
        /// Провинции клана
        /// </summary>
        /// <param name="clan_id">ID клана</param>
        /// <param name="access_token">Токен</param>
        /// <param name="fields">Запрос определенных полей</param>
        /// <returns>JSON ответ</returns>
        public string ClanProvinces(string clan_id, string access_token = "", string fields = "")
        {
            /*
             * api.worldoftanks.ru/wot/clan/provinces/
             */
            string Data = String.Empty;

            try
            {
                Data = "application_id=" + Properties.Resources.API;
                Data += "&clan_id=" + clan_id;
                if (access_token.Trim().Length > 0) Data += "&access_token=" + access_token;
                if (fields.Trim().Length > 0) Data += "&fields=" + fields;

                return POST(Properties.Resources.API_Protocol + Properties.Resources.API_Clan_Provinces, Data);
            }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "ClanProvinces()", "Clan ID: " + clan_id, ex.Message, ex.StackTrace); return null; }
        }


        /// <summary>
        /// Список провинций Глобальной карты
        /// При передаче ID конкретной провинции, можно получить данные только по ней
        /// </summary>
        /// <param name="province_id">Идентификатор провинции</param>
        /// <param name="map_id">Идентификатор Глобальной карты:  globalwars</param>
        /// <returns>JSON ответ</returns>
        public string GlobalProvinces(string province_id = "", string map_id = "globalmap")
        {
            /*
             * api.worldoftanks.ru/wot/globalwar/provinces/
             */
            string Data = String.Empty;

            try
            {
                Data = "application_id=" + Properties.Resources.API;
                Data += "&map_id=" + map_id;
                if (province_id.Trim().Length > 0) Data += "&province_id=" + province_id;

                return POST(Properties.Resources.API_Protocol + Properties.Resources.API_Global_Provinces, Data);
            }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "ClanProvinces()", "Province ID: " + province_id, "Map ID: " + map_id, ex.Message, ex.StackTrace); return null; }
        }


        /*public string ClanMember(string account_id)
        {
            /*
             * https://api.worldoftanks.ru/wot/clan/membersinfo/
             *
            return POST(Properties.Resources.API_Protocol + Properties.Resources.API_Account_Info, Data);
        }*/


        /// <summary>
        /// Отправка запросов на сервер методом POST
        /// </summary>
        /// <param name="Url">Передача URL формы обработчика</param>
        /// <param name="Data">Передаваемые данные</param>
        /// <returns>Возврат JSON строки с ответом</returns>
        private string POST(string Url, string Data)
        {
            try
            {
                WebRequest req = WebRequest.Create(Url);
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "text/json; charset=UTF-8";
                byte[] sentData = Encoding.GetEncoding("Utf-8").GetBytes(Data);
                req.ContentLength = sentData.Length;
                Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                System.Net.WebResponse res = req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                string Out = String.Empty;
                while (count > 0)
                {
                    Out += new String(read, 0, count);
                    count = sr.Read(read, 0, 256);
                }

                return Out;
            }
            catch (WebException we) { Debug.Save("WargamingAPI.Class", "POST()", "Url: " + Url, "Data: " + Data, we.Message, we.StackTrace); return null; }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "POST()", "Url: " + Url, "Data: " + Data, ex.Message, ex.StackTrace); return null; }
        }


        public JObject Token(string Uri)
        {
            Uri = Uri.Remove(0, Uri.IndexOf("?") + 1);
            Uri = Uri.Replace("&", "\",\"").Replace("=", "\":\"");
            return JObject.Parse("{" + Uri.Remove(0, 2) + "\"}");
        }

        public string TokenString(string Uri)
        {
            Uri = Uri.Remove(0, Uri.IndexOf("?") + 1);
            Uri = Uri.Replace("&", "\",\"").Replace("=", "\":\"");
            Uri = "{" + Uri.Remove(0, 2) + "\"}";

            return Uri;
        }


        private Dictionary<string, string> FromJSON(string json)
        {
            try { return JsonConvert.DeserializeObject<Dictionary<string, string>>(json); }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "FromJSON()", json, ex.Message, ex.StackTrace); return null; }
        }
    }
}
