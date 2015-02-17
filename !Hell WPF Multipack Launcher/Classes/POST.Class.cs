using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class POST
    {
        Debugging Debugging = new Debugging();
        Crypt Crypt = new Crypt();


        public string Send(string Url, JObject json, string encoded_string = null, bool manual_enc = true)
        {
            string Data = String.Empty;
            string Out = String.Empty;

            try
            {
                string UserID = new Classes.Variables().GetUserID();

                if (encoded_string == null)
                {
                    if (Properties.Resources.API_DEV_CRYPT == "1" && manual_enc)
                        Data = "data=" + Crypt.Encrypt(json.ToString(), UserID) + "&u=" + UserID + "&e=" + Properties.Resources.API_DEV_CRYPT;
                    else
                        Data = "data=" + json.ToString();
                }
                else
                {
                    Data = "data=" + encoded_string + "&u=" + UserID + "&e=" + Properties.Resources.API_DEV_CRYPT;
                }

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
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    Out += str;
                    count = sr.Read(read, 0, 256);
                }

                return Out;
            }
            catch (WebException we) { Debugging.Save("POST.Class", "Send()", "URL: " + Url, "Data: " + Data, "Out: " + Out, we.Message, we.StackTrace); return "FAIL"; }
            catch (Exception ex) { Debugging.Save("POST.Class", "Send()", "URL: " + Url, "Data: " + Data, "Out: " + Out, ex.Message, ex.StackTrace); return "FAIL"; }
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
                catch (WebException we) { Debugging.Save("POST.Class", "HttpUploadFile()", "URL: " + url, "File: " + file, "Parameter: " + paramName, "Content type: " + contentType, we.Message, we.StackTrace); rs = wr.GetRequestStream(); }

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
            catch (WebException we) { Debugging.Save("POST.Class", "HttpUploadFile()", "URL: " + url, "File: " + file, "Parameter: " + paramName, "Content type: " + contentType, we.Message, we.StackTrace); }
            catch (Exception ex) { Debugging.Save("POST.Class", "HttpUploadFile()", "URL: " + url, "File: " + file, "Parameter: " + paramName, "Content type: " + contentType, ex.Message, ex.StackTrace); }
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
                    if (count != 0) sb.Append(Encoding.UTF8.GetString(buf, 0, count));
                }
                while (count > 0);

                return JObject.Parse(sb.ToString());
            }
            catch (WebException we) { Debugging.Save("POST.Class", "JsonResponse()", "URL: " + uri, we.Message, we.StackTrace); return null; }
            catch (Exception ex) { Debugging.Save("POST.Class", "JsonResponse()", "URL: " + uri, ex.Message, ex.StackTrace); return null; }
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
            catch (Exception ex) { Debugging.Save("POST.Class", "DataRegex()", "Input string: " + str, ex.Message, ex.StackTrace); return str; }
        }

        public string Shield(string text)
        {
            try { return text.Replace("\"", "\\\"").Trim(); }
            catch (Exception ex) { new Debugging().Save("POST Class", "Shield()", text, ex.Message, ex.StackTrace); }

            return text;
        }

        /// <summary>
        /// Функция автоматической отправке неотправленных тикетов
        /// </summary>
        public void AutosendTicket()
        {
            // Переносим тикеты в новое место
            try
            {
                if (Directory.Exists(Environment.CurrentDirectory + @"\tickets"))
                    Copy(Environment.CurrentDirectory + @"\tickets", MainWindow.SettingsDir + "tickets");
            }
            catch (Exception) { }

            // Готовим тикеты
            try
            {
                string status = String.Empty;
                int count = 0;

                if (Directory.Exists(MainWindow.SettingsDir + "tickets"))
                {
                    Language Lang = new Language();
                    string lang = (string)MainWindow.JsonSettingsGet("info.language");

                    string log = String.Empty;

                    foreach (FileInfo file in new DirectoryInfo(MainWindow.SettingsDir + "tickets").GetFiles("*.ticket"))
                    {
                        try
                        {
                            JObject answer = JObject.Parse(Send(
                                Properties.Resources.API_DEV_Address + Properties.Resources.API_DEV_Ticket,
                                null,
                                File.ReadAllText(file.FullName, Encoding.UTF8)));

                            if ((string)answer["status"] != "FAIL" && (string)answer["code"] == Properties.Resources.API)
                            {
                                switch ((string)answer["status"])
                                {
                                    case "OK":
                                        status += (string)answer["id"] + "; ";
                                        count++;
                                        if (File.Exists(file.FullName)) File.Delete(file.FullName);
                                        break;
                                    case "BANNED":
                                        status += answer["content"] + "; ";
                                        count++;
                                        if (File.Exists(file.FullName)) File.Delete(file.FullName);
                                        break;
                                    default:
                                        log += (string)answer["status"] + " : " + file.Name + Environment.NewLine;
                                        break;
                                }
                            }
                            else
                            {
                                if (answer["status"].ToString() == "FAIL" && answer["content"].ToString() != "SOFTWARE_NOT_AUTORIZED")
                                    if (File.Exists(file.FullName)) File.Delete(file.FullName);
                            }

                            // Пишем лог
                            log += String.Format("{0} : {1} : {2}" + Environment.NewLine, (string)answer["status"], (string)answer["content"], file.Name);
                        }
                        catch (Exception e) { Task.Factory.StartNew(() => Debugging.Save("POST.Class", "AutosendTicket()", file.FullName, e.Message, e.StackTrace)); }
                    }

                    if (status.Length > 0)
                    {

                        MainWindow.Notifier.ShowBalloonTip(5000,
                            Lang.Set("PostClass", "AutoTicket", lang),
                            Lang.Set("PostClass", "AutoTicketCount", lang, count.ToString()) + Environment.NewLine + Environment.NewLine +
                                Lang.Set("PostClass", "AutoTicketStatus", lang, status),
                            System.Windows.Forms.ToolTipIcon.Info);
                    }

                    // Записываем лог
                    string logDir = MainWindow.SettingsDir + @"log\";
                    if (!Directory.Exists(logDir))
                        Directory.CreateDirectory(logDir);

                    File.WriteAllText(logDir + "tickets-" + DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff") + ".log", log);
                }
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debugging.Save("POST.Class", "AutosendTicket()", ex.Message, ex.StackTrace)); }
        }


        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);

            Directory.Delete(sourceDirectory);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (Directory.Exists(target.FullName) == false)
                Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
                fi.MoveTo(Path.Combine(target.ToString(), fi.Name));            

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}