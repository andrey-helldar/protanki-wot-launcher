﻿using System;
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
        POST POST = new POST();
        Debug Debug = new Debug();

        public string AccountID(string name = "null")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                String.Format("%s/account/list/?application_id=%s&search=%s",
                Properties.Resources.APIlink,
                Properties.Resources.API,
                name)
                );

            request.Method = "GET";
            request.ContentType = "text/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            JObject obj = JObject.Parse(result);
            string id;
            try
            {
                //id = obj.SelectToken("data[0].id").ToString();
                return obj.SelectToken("data[0].id").ToString();
            }
            catch
            {
                //do something
                return "null";
            }
        }

        public string AccountList(string name)
        {
            string Data = String.Empty;

            try
            {
                Data = "application_id=" + Properties.Resources.API;
                Data += "&fields=" + "nickname,account_id";
                Data += "&search=" + name;

                //return Data;

                WebRequest req = WebRequest.Create(Properties.Resources.API_Account_List);
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                byte[] sentData = Encoding.GetEncoding("Utf-8").GetBytes(Data);
                req.ContentLength = sentData.Length;
                System.IO.Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                string Out = String.Empty;
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    Out += str;
                    count = sr.Read(read, 0, 256);
                }
                //return Out;

                JObject obj = JObject.Parse(Out);
                JObject sobj;
                try
                {
                    string tre = obj.SelectToken("data[]").ToString();
                    sobj = JObject.Parse(tre);
                    string resu="";

                    return sobj.ToString();

                    /*resu += String.Format("User: %s\t\t\tID: %s", sobj[0].SelectToken["account_id"], sobj[0].SelectToken["nickname"]) + Environment.NewLine;
                    resu += String.Format("User: %s\t\t\tID: %s", sobj[1].SelectToken["account_id"], sobj[1].SelectToken["nickname"]) + Environment.NewLine;
                    resu += String.Format("User: %s\t\t\tID: %s", sobj[2].SelectToken["account_id"], sobj[2].SelectToken["nickname"]) + Environment.NewLine;
                    resu += String.Format("User: %s\t\t\tID: %s", sobj[3].SelectToken["account_id"], sobj[3].SelectToken["nickname"]) + Environment.NewLine;
                    return resu;*/
                }
                catch (Exception) { }

                return null;
            }
            catch (WebException we) { Debug.Save("WargamingAPI.Class", "AccountList()", "Username: " + name, "JSON: " + Data, we.Message); return null; }
            catch (Exception ex) { Debug.Save("WargamingAPI.Class", "AccountList()", "Username: " + name, "JSON: " + Data, ex.Message); return null; }
        }
    }
}