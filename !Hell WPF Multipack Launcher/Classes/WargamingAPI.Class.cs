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
        public string Request()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                String.Format("%s/account/list/?application_id=%s&search=%s",
                Properties.Resources.APIlink,
                Properties.Resources.API,
                "First_Helldar")
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
            }
        }
    }
}
