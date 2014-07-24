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
using Newtonsoft.Json;


namespace _Hell_PRO_Tanki_Launcher
{
    class Debug
    {
        public string Code = Properties.Resources.Code;
        public string Youtube = Properties.Resources.Youtube;
        public int Accept = 300;

        /// <summary>
        /// Сохраняем информацию обработчика в файл
        /// </summary>
        public void Save(string func, params string[] mess)
        {
            try
            {
                string uid = UserID();

                Dictionary<string, string> jData = new Dictionary<string, string>();
                jData.Add("uid", uid);
                jData.Add("version", Application.ProductVersion);
                jData.Add("date", DateTime.Now.ToString("yyyy-MM-dd h-m-s"));
                jData.Add("module", func);
                jData.Add("function", mess[0]);

                for (int i = 1; i < mess.Length; i++)
                    jData.Add("param" + i.ToString(), mess[i]);

                if (!Directory.Exists("temp")) { Directory.CreateDirectory("temp"); }
                string filename = String.Format("{0}_{1}.debug", Application.ProductVersion, DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff"));
                File.WriteAllText(@"temp\" + filename, JsonConvert.SerializeObject(jData), Encoding.UTF8);
            }
            finally { }
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

        public void Delete(string path=null)
        {
            try
            {
                if (path == null) path = Application.StartupPath;

                if (Directory.Exists(path + @"\debug"))
                {
                    var info = new DirectoryInfo(path + @"\debug");

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

                        if (ts.TotalSeconds > 172800) { File.Delete(file.FullName); }
                    }
                }
            }
            catch (Exception ex)
            {
                Save("Debug Class", "Delete()", path, ex.Message);
            }
        }

        public void Send()
        {
            if (Directory.Exists("Debug"))
            {
                string userID = UserID();

                foreach (string file in Directory.GetFiles(@"Debug", "*.zip", SearchOption.AllDirectories))
                {
                    try
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("code", Code);
                        nvc.Add("uid", userID);
                        nvc.Add("version", Application.ProductVersion);
                        new SendPOST().HttpUploadFile("http://ai-rus.com/pro/debug.php", file, "file", "application/x-zip-compressed", nvc);
                        File.Delete(file);
                    }
                    finally {  }
                }
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
                Save("Debug Class", "UserID()", ex.Message);
                return null;
            }
        }
    }
}
