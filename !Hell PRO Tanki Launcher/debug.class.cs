using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;


namespace _Hell_PRO_Tanki_Launcher
{
    class Debug
    {
        private BackgroundWorker workerSend = new BackgroundWorker();

        private string code = "TIjgwJYQyUyC2E3BRBzKKdy54C37dqfYjyInFbfMeYed0CacylTK3RtGaedTHRC6";

        /// <summary>
        /// Сохраняем информацию обработчика в файл
        /// </summary>
        public void Save(string func, string place, string mess)
        {
            if (!Directory.Exists("temp")) { Directory.CreateDirectory("temp"); }
            File.WriteAllText(@"temp\" + UserID() + "_-_" + DateTime.Now.ToString("yyyy-MM-dd h-m-s.ffffff") + ".Debug",
                func + Environment.NewLine + "-------------------------------" + Environment.NewLine +
                place + Environment.NewLine + "-------------------------------" + Environment.NewLine +
                mess, Encoding.UTF8);

            //MessageBox.Show(fIndex.ActiveForm, mess, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool Archive(string path)
        {
            bool exp = false;

            try
            {
                this.Delete(path);

                if (!Directory.Exists(path + @"\temp")) { return false; }
                if (!Directory.Exists(path + @"\Debug")) { Directory.CreateDirectory(path + @"\Debug"); }

                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.AddDirectory(path + @"\temp");
                    zip.Save(path + @"\Debug\" + UserID() + "_-_" + "Debug-" + DateTime.Now.ToString("yyyy-MM-dd h-m-s") + ".zip");
                }

                Directory.Delete(path + @"\temp", true);
            }
            catch (Exception ex)
            {
                this.Save("public bool Archive()", "Debug mode", ex.Message);
            }

            return exp;
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

                        if (ts.TotalSeconds > 259200) { File.Delete(file.FullName); }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Save("private void Delete(string path)", "Debug mode", ex.Message);
            }
        }

        public void Send()
        {
            workerSend.WorkerReportsProgress = true;
            workerSend.WorkerSupportsCancellation = true;
            workerSend.DoWork += new DoWorkEventHandler(workerSend_DoWork);

            if (!workerSend.IsBusy) { workerSend.RunWorkerAsync(); }
        }

        private void workerSend_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Directory.Exists("Debug"))
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    var info = new DirectoryInfo("Debug");
                    string userID = UserID();

                    foreach (FileInfo file in info.GetFiles())
                    {
                        try
                        {
                            client.UploadFile("http://ai-rus.com/wot/Debug/" + code + "/" + userID, file.FullName);
                            File.Delete(file.FullName);
                        }
                        catch (Exception ex)
                        {
                            Save("private void workerSend_DoWork(object sender, DoWorkEventArgs e)", "Send Debug files: " + file.FullName, ex.Message);
                        }
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
        private string UserID()
        {
            try
            {
                string name = Environment.MachineName +
                    Environment.UserName +
                    Environment.UserDomainName +
                    Environment.Version.ToString() +
                    Environment.OSVersion.ToString();

                using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
                {
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(name));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++) { sBuilder.Append(data[i].ToString("x2")); }

                    return sBuilder.ToString();
                }
            }catch(Exception ex){
                Save("private string UserID()", "", ex.Message);
                return null;
            }
        }
    }
}
