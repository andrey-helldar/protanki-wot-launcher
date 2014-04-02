using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Xml;

namespace _Hell_PRO_Tanki_Launcher
{
    class update_launcher
    {
        debug debug = new debug();

        private bool onlyCheck = false,
            nowUpdate = true;

        private string url = @"http://ai-rus.com/pro/";

        private Version remoteVersion;

        BackgroundWorker downloadUpdates = new BackgroundWorker();

        public void CheckUpdates(bool check = false)
        {
            try
            {
                onlyCheck = check;

                downloadUpdates.WorkerReportsProgress = true;
                downloadUpdates.WorkerSupportsCancellation = true;
                downloadUpdates.DoWork += new DoWorkEventHandler(downloadUpdates_DoWork);
                downloadUpdates.RunWorkerCompleted += new RunWorkerCompletedEventHandler(downloadUpdates_RunWorkerCompleted);

                if (!downloadUpdates.IsBusy) { downloadUpdates.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                debug.Save("public void CheckUpdates(bool check = false)", "", ex.Message);
            }
        }

        private bool Checksumm(string filename, string summ)
        {
            try
            {
                using (FileStream fs = File.OpenRead(filename))
                {
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] fileData = new byte[fs.Length];
                    fs.Read(fileData, 0, (int)fs.Length);
                    byte[] checkSumm = md5.ComputeHash(fileData);
                    return BitConverter.ToString(checkSumm).Replace("-", String.Empty) == (summ).ToUpper() ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                //pbDownload.Value = e.ProgressPercentage;
            }
            catch (Exception ex)
            {
                debug.Save("private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)", "pbDownload.Value = e.ProgressPercentage;", ex.Message);
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                //if (summ)
                //{
                if (DialogResult.Yes == MessageBox.Show(fIndex.ActiveForm, "Обнаружена новая версия лаунчера (" + remoteVersion.ToString() + ")" + Environment.NewLine +
                    "Применить обновление сейчас?", Application.ProductName + " v" + Application.ProductVersion, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    Process.Start("updater.exe", "launcher.update \"" + Process.GetCurrentProcess().ProcessName + ".exe\"");
                    Process.GetCurrentProcess().Kill();
                }
                /*}
                else
                {
                    if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                }*/
            }
            catch (Exception) { }
        }

        private void downloadUpdates_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var client = new WebClient())
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(url + "protanks.xml");

                ///
                /// Избавляемся от ненужных файлов
                /// 
                if (File.Exists("processes.exe")) { File.Delete("processes.exe"); }

                /// 
                /// Скачиваем необходимые файлы
                /// 
                DownloadFile("Ionic.Zip.dll", doc.GetElementsByTagName("Ionic.Zip")[0].InnerText, doc.GetElementsByTagName("Ionic.Zip")[0].Attributes["checksumm"].InnerText);
                DownloadFile("Newtonsoft.Json.dll", doc.GetElementsByTagName("Newtonsoft.Json")[0].InnerText, doc.GetElementsByTagName("Newtonsoft.Json")[0].Attributes["checksumm"].InnerText);
                DownloadFile("ProcessesLibrary.dll", doc.GetElementsByTagName("processesLibrary")[0].InnerText, doc.GetElementsByTagName("processesLibrary")[0].Attributes["checksumm"].InnerText);
                DownloadFile("LanguagePack.dll", doc.GetElementsByTagName("languagePack")[0].InnerText, doc.GetElementsByTagName("languagePack")[0].Attributes["checksumm"].InnerText);
                DownloadFile("restart.exe", doc.GetElementsByTagName("restart")[0].InnerText, doc.GetElementsByTagName("restart")[0].Attributes["checksumm"].InnerText);
                DownloadFile("updater.exe", doc.GetElementsByTagName("updater")[0].InnerText, doc.GetElementsByTagName("updater")[0].Attributes["checksumm"].InnerText);

                /// 
                /// А теперь проверяем обновления основного файла программы
                /// и запускаем механизм обновления
                /// 
                DownloadFile("launcher.update", doc.GetElementsByTagName("version")[0].InnerText, doc.GetElementsByTagName("version")[0].Attributes["checksumm"].InnerText, true);
            }
        }

        private void downloadUpdates_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /// 
            /// На всякий случай, после того, как скачали обновление файла основной программы,
            /// Запускаем процесс обновления, конечно, если пользователь разрешил нам немедленную установку
            /// 

            if (File.Exists("launcher.update"))
            {
                Process.Start("updater.exe", "launcher.update \"Multipack Launcher.exe\"");
                Process.GetCurrentProcess().Kill();
            }
        }


        /// 
        /// Так как при скачивании файлов мы делаем много одинаковых операций по скачивании и проверке,
        /// целесообразней завернуть все в 1 функцию и передавать ей параметры
        /// 
        private void DownloadFile(string filename, string xmlVersion, string xmlChecksumm, bool showMessageBeforeDownload = false)
        {
            try
            {
                if (!(File.Exists(filename)) || (File.Exists(filename) && new Version(FileVersionInfo.GetVersionInfo(filename).FileVersion) < new Version(xmlVersion)))
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(new Uri(url + filename), filename);

                        if (!Checksumm(filename, xmlChecksumm))
                        {
                            int errCount = 0;

                            while (errCount < 3)
                            {
                                try
                                {
                                    client.DownloadFile(new Uri(url + filename), filename);
                                    ++errCount;
                                }
                                catch (Exception ex1)
                                {
                                    debug.Save("private void DownloadFile(string filename, string xmlVersion, string xmlChecksumm)", "Filename: " + filename + Environment.NewLine + "Error count: " + errCount, ex1.Message);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.Save("private void DownloadFile(string filename, string xmlVersion, string xmlChecksumm)", "Filename: " + filename, ex.Message);
            }
        }
    }
}