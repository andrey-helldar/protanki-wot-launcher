using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class WargamingAPI
    {
        Debug Debug = new Debug();

        public string OpenID()
        {
            try
            {
                string Data = "application_id=" + Properties.Resources.API;
                Data += "&display=page";
                /*
                //return POST(Properties.Resources.API_OpenID, Data);
                File.AppendAllText("access.html", POST(Properties.Resources.API_OpenID, Data));
                File.Open("access.html", FileMode.Open);*/

                //System.Diagnostics.Process.Start(Properties.Resources.API_OpenID+"?"+Data);
                
                return "OK";
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

                JObject obj = JObject.Parse(POST(Properties.Resources.API_Account_List, Data));
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
            catch (WebException we) { Debug.Save("WargamingAPI.Class", "AccountList()", "Username: " + name, "JSON: " + Data, we.Message); return null; }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "AccountList()", "Username: " + name, "JSON: " + Data, ex.Message); return null; }
        }


        //public Dictionary<string, string> AccountInfo(string account_id)
        public string AccountInfo(string account_id)
        {
            // https://ru.wargaming.net/developers/api_reference/wot/account/info/

            string Data = "application_id=" + Properties.Resources.API;
            Data += "&fields=" + "clan_id,global_rating,private.credits,private.free_xp,private.gold,private.is_premium,private.premium_expires_at,statistics.all.battle_avg_xp,statistics.all.battles";
            Data += "&account_id=" + account_id;

            return POST(Properties.Resources.API_Account_Info, Data);
            /*JObject obj = JObject.Parse(POST(Properties.Resources.API_Account_Info, Data));
            Dictionary<string, string> Account = new Dictionary<string, string>();

            Account.Add("status", obj.SelectToken("status").ToString());

            try
            {
                if (obj.SelectToken("status").ToString() == "ok")
                    for (int i = 0; i < Convert.ToInt16(obj.SelectToken("count").ToString()); i++)
                        Account.Add(obj.SelectToken("data[" + i.ToString() + "].nickname").ToString(), obj.SelectToken("data[" + i.ToString() + "].account_id").ToString());

                return Account;
            }
            catch (Exception) { return Account; }   */    
        }


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
            catch (WebException we) { Debug.Save("WargamingAPI.Class", "POST()", "Url: " + Url, "Data: " + Data, we.Message); return null; }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "POST()", "Url: " + Url, "Data: " + Data, ex.Message); return null; }
        }
    }
}
