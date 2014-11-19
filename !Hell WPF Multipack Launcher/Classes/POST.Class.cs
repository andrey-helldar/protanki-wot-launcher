using System;
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

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class POST
    {
        Debug Debug = new Debug();


        public string Send(string Url, string Data)
        {
            try
            {
                Data = "data=" + Data;

                WebRequest req = WebRequest.Create(Url);
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                byte[] sentData = Encoding.GetEncoding("Utf-8").GetBytes(Data);
                req.ContentLength = sentData.Length;
                Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                WebResponse res = req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);
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
            catch (WebException we) { Debug.Save("POST.Class", "Send()", "URL: " + Url, "Data: " + Data, we.Message, we.StackTrace); return "FAIL"; }
            catch (Exception ex) { Debug.Save("POST.Class", "Send()", "URL: " + Url, "Data: " + Data, ex.Message, ex.StackTrace); return "FAIL"; }
        }

        public void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            try
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = CredentialCache.DefaultCredentials;

                Stream rs;
                try { rs = wr.GetRequestStream(); }
                catch (WebException we) { Debug.Save("POST.Class", "HttpUploadFile()", "URL: " + url, "File: " + file, "Parameter: " + paramName, "Content type: " + contentType, we.Message, we.StackTrace); rs = wr.GetRequestStream(); }

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
                byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    rs.Write(buffer, 0, bytesRead);
                fileStream.Close();

                byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
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
            catch (WebException we) { Debug.Save("POST.Class", "HttpUploadFile()", "URL: " + url, "File: " + file, "Parameter: " + paramName, "Content type: " + contentType, we.Message, we.StackTrace); }
            catch (Exception ex) { Debug.Save("POST.Class", "HttpUploadFile()", "URL: " + url, "File: " + file, "Parameter: " + paramName, "Content type: " + contentType, ex.Message, ex.StackTrace); }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/7003740/how-to-convert-namevaluecollection-to-json-string
        /// </summary>
        /// <param name="json"></param>
        /// <returns>Строка</returns>
        public string Json(Dictionary<string, string> dic)
        {
            try { return JsonConvert.SerializeObject(dic); }
            catch (Exception ex) { Debug.Save("POST.Class", "Json()", ex.Message, ex.StackTrace); }
            return null;
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
            catch (Exception ex) { Debug.Save("POST.Class", "FromJson()", json, ex.Message, ex.StackTrace); return null; }
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
                    if (count != 0)  sb.Append(Encoding.UTF8.GetString(buf, 0, count));
                }
                while (count > 0);

                return JsonConvert.DeserializeObject<JObject>(sb.ToString());
            }
            catch (WebException we) { Debug.Save("POST.Class", "JsonResponse()", "URL: " + uri, we.Message, we.StackTrace); return null; }
            catch (Exception ex) { Debug.Save("POST.Class", "JsonResponse()", "URL: " + uri, ex.Message, ex.StackTrace); return null; }
        }

        public string DataRegex(string str)
        {
            try
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
            catch (Exception ex) { Debug.Save("POST.Class", "DataRegex()", "Input string: " + str, ex.Message, ex.StackTrace); return str; }
        }

        /*public string RequestInfo(string request)
        {
            string result = String.Empty;

            try
            {
                Dictionary<string, string> jData = new Dictionary<string, string>();
                jData.Add("code", Properties.Resources.API);
                jData.Add("request", request);

                result = Send(Properties.Resources.Developer + Properties.Resources.API_Dev_Info, "data=" + Json(jData));

                Dictionary<string, string> status = FromJson(result);
                return status["info"];
            }
            catch (Exception ex)
            {
                new Debug().Save("POST Class", "RequestInfo()", "Request: " + request, result, ex.Message, ex.StackTrace);
                return "FAIL";
            }
        }*/

        public string Shield(string text)
        {
            try { return text.Replace("\"", "\\\"").Trim(); }
            catch (Exception ex) { new Debug().Save("POST Class", "Shield()", text, ex.Message, ex.StackTrace); }

            return text;
        }
    }
}
