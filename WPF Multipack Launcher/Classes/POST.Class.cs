﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WPF_Multipack_Launcher.Classes
{
    class POST
    {
        //public static string Send(string Url, string Data)
        public string Send(string Url, string Data)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
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
            return Out;
        }

        public async Task CountUsers(string productName = null,
            string productVersion = "0.0.0.0", string packVersion = "0.0.0.0",
            string packType = "null", string youtube = "null", string lang = "en")
        {
            try
            {
                await Task.Delay(10000);

                NameValueCollection myJsonData = new NameValueCollection();
                myJsonData.Add("code", Properties.Resources.Code);
                myJsonData.Add("user_id", new Variables.Variables().GetUserID().Result);
                myJsonData.Add("youtube", youtube);
                myJsonData.Add("packtype", packType);
                myJsonData.Add("packversion", packVersion);
                myJsonData.Add("name", productName != null ? productName : System.Windows.Application.Current.MainWindow.GetType().Assembly.GetName().Name);
                myJsonData.Add("version", productVersion);
                myJsonData.Add("lang", lang);

                System.Windows.MessageBox.Show(Send(Properties.Resources.DeveloperUsers, "data=" + JsonConvert.SerializeObject(myJsonData)));
            }
            catch (WebException ex)
            {
                new Debug().Save("POST Class: CountUsers()",
                    "productName: " + productName,
                    "productVersion: " + productVersion,
                    "packVersion: " + packVersion,
                    "packType: " + packType,
                    "youtube: " + youtube,
                    ex.Message);
            }
        }

        public void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs;
            try
            {
                rs = wr.GetRequestStream();
            }
            catch (WebException)
            {
                rs = wr.GetRequestStream();
            }

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
            }
            catch (Exception)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally { wr = null; }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/7003740/how-to-convert-namevaluecollection-to-json-string
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string Json(Dictionary<string, string> dic)
        {
            try { return JsonConvert.SerializeObject(dic); }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/4749639/deserializing-json-to-net-object-using-newtonsoft-or-linq-to-json-maybe
        /// 
        /// http://james.newtonking.com/json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public Dictionary<string, string> FromJson(string json)
        {
            try { return JsonConvert.DeserializeObject<Dictionary<string, string>>(json); }
            catch (Exception) { return null; }
        }

        public JObject JsonResponse(string uri)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                int count = 0;
                do
                {
                    count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                        sb.Append(Encoding.UTF8.GetString(buf, 0, count));
                }
                while (count > 0);
                //return sb.ToString();

                return JsonConvert.DeserializeObject<JObject>(sb.ToString());
            }
            catch (Exception) { return null; }
        }

        public string DataRegex(string str)
        {
            // Убираем лишние теги
            string[] arrRegex = { @"\<font(.*?)\>", @"\<\/font\>", @"\<ul\>", @"\<\/ul\>", @"\<\/li\>" };
            foreach (string reg in arrRegex)
            {
                Regex myRegex = new Regex(reg, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                str = myRegex.Replace(str, @"");
            }

            // Заменяем список
            string[] arrRegex1 = { @"\<li\>" };
            foreach (string reg in arrRegex1)
            {
                Regex myRegex = new Regex(reg, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                str = myRegex.Replace(str, Environment.NewLine + " * ");
            }

            // Заменяем список
            string[] arrRegex2 = { @"\<br\>" };
            foreach (string reg in arrRegex2)
            {
                Regex myRegex = new Regex(reg, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                str = myRegex.Replace(str, Environment.NewLine);
            }

            str = str.Replace("__________________________________________", Environment.NewLine + "__________________________________________");

            return str;
        }
    }
}
