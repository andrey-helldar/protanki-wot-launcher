using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;
using Ionic.Zip;


namespace _Hell_PRO_Tanki_Launcher
{
    class Debug
    {
        public string Code = "TIjgwJYQyUyC2E3BRBzKKdy54C37dqfYjyInFbfMeYed0CacylTK3RtGaedTHRC6";
        public string Youtube = "PROTankiWoT";

        /// <summary>
        /// Сохраняем информацию обработчика в файл
        /// </summary>
        public void Save(string func, params string[] mess)
        {
            string split =  Environment.NewLine + "-------------------------------" + Environment.NewLine;
            string result = func + split;

            foreach(string str in mess){
                result += str + split;
            }            

            if (!Directory.Exists("temp")) { Directory.CreateDirectory("temp"); }
            string filename = String.Format("{0}_-_{1}_{2}.debug", UserID(), Application.ProductVersion, DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff"));
            File.WriteAllText(@"temp\" + filename, result, Encoding.UTF8);

            //MessageBox.Show(fIndex.ActiveForm, result, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void Archive(string path)
        {
            try
            {
                Delete(path);

                if (Directory.Exists(path + @"\temp"))
                {
                    if (!Directory.Exists(path + @"\debug")) { Directory.CreateDirectory(path + @"\debug"); }

                    using (ZipFile zip = new ZipFile())
                    {
                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        zip.AddDirectory(path + @"\temp");
                        zip.Save(path + @"\debug\" + UserID() + "_-_" + "_" + Application.ProductVersion + "_" + DateTime.Now.ToString("yyyy-MM-dd h-m-s") + ".zip");
                    }

                    Directory.Delete(path + @"\temp", true);
                }
            }
            catch (Exception ex)
            {
                Save("public bool Archive()", ex.Message);
            }
        }

        private void Delete(string path)
        {
            try
            {
                if (Directory.Exists(path + @"\temp"))
                {
                    var info = new DirectoryInfo(path + @"\temp");

                    foreach (FileInfo file in info.GetFiles())
                    {
                        DateTime now = DateTime.Now;
                        DateTime cf = File.GetCreationTime(file.FullName);

                        TimeSpan ts = now - cf;

                        /* Удаляем все файлы, старше 3-х суток
                         * 1 минута = 60 сек
                         * 1 час = 60 минут = 3600 сек
                         * 1 сутки = 24 часа = 86400 сек
                         * 2 суток = 172800 сек
                         * 3 суток = 259200 сек
                         */

                        if (ts.TotalSeconds > 86400) { File.Delete(file.FullName); }
                    }
                }
            }
            catch (Exception ex)
            {
                Save("private void Delete(string path)", "Debug mode", ex.Message);
            }
        }

        public void Send()
        {
            if (Directory.Exists("Debug"))
            {
                var info = new DirectoryInfo("Debug");
                string userID = UserID();

                foreach (FileInfo file in info.GetFiles())
                {
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("code", Code);
                    nvc.Add("uid", userID);
                    Task.Factory.StartNew(() => HttpUploadFile("http://ai-rus.com/wot/debug/", file.FullName, "file", "application/x-zip-compressed", nvc)).Wait();
                    File.Delete(file.FullName);
                }
            }
        }

        public void HttpUploadFile(string url, string file, string paramName, string contentType, System.Collections.Specialized.NameValueCollection nvc)
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
            finally
            {
                wr = null;
            }
        }

        /// <summary>
        /// Функция, определяющая уникальный идентификатор пользователя.
        /// Нужна для удобства сортировки файлов на сайте
        /// </summary>
        /// <returns>
        /// Если функция сработает без ошибок, то вернет кэш-сумму, являющуюся идентификатором,
        /// иначе вернет нулевое значение (null)
        /// </returns>
        public string UserID()
        {
            try
            {
                string name = Environment.MachineName +
                    Environment.UserName +
                    Environment.UserDomainName +
                    Environment.OSVersion.ToString();

                using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
                {
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(name));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++) { sBuilder.Append(data[i].ToString("x2")); }

                    return sBuilder.ToString();
                }
            }catch(Exception ex){
                Save("private string UserID()", ex.Message);
                return null;
            }
        }
    }
}
