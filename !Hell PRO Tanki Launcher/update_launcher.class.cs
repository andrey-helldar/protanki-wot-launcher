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

        private bool onlyCheck = false;

        private string url = @"http://ai-rus.com/pro/";

        private Version remoteVersion,
            localVersion;

        BackgroundWorker checkLibrary = new BackgroundWorker();
        BackgroundWorker worker = new BackgroundWorker();

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

        public void CheckLocal(bool onlycheck = false)
        {
            try
            {
                onlyCheck = onlycheck;

                checkLibrary.WorkerReportsProgress = true;
                checkLibrary.WorkerSupportsCancellation = true;
                checkLibrary.DoWork += new DoWorkEventHandler(checkLibrary_DoWork);
                checkLibrary.RunWorkerCompleted += new RunWorkerCompletedEventHandler(checkLibrary_RunWorkerCompleted);

                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);

                if (!checkLibrary.IsBusy) { checkLibrary.RunWorkerAsync(); }
            }
            catch (Exception) { }
        }

        private void Download()
        {
            try
            {
                var client = new WebClient();
                XmlDocument doc = new XmlDocument();
                doc.Load(url + "protanks.xml");

                remoteVersion = new Version(doc.GetElementsByTagName("version")[0].InnerText);
                localVersion = new Version(Application.ProductVersion);

                if (localVersion < remoteVersion)
                {
                    if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }

                    WebClient client1 = new WebClient();
                    client1.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    client1.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    client1.DownloadFileAsync(new Uri(url + "launcher.exe"), "launcher.update");
                }
            }
            catch (Exception) { }
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

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                /// Здесь нужно внедрить использование функции Checksumm();

                if (File.Exists("launcher.update") && new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion) > new Version(Application.ProductVersion))
                {
                    Process.Start("updater.exe", "launcher.update \"Multipack Launcher\"");
                    //Process.Start("updater.exe", "launcher.update \"" + Process.GetCurrentProcess().ProcessName + "\"");
                    try
                    {
                        if (File.Exists("!Hell PRO Tanki Launcher.exe")) { File.Delete("!Hell PRO Tanki Launcher.exe"); }
                    }
                    catch (Exception) { }
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                    if (!onlyCheck) Download();
                }
            }
            catch (Exception)
            {
                if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                if (!onlyCheck) Download();
            }
        }

        private void checkLibrary_DoWork(object sender, DoWorkEventArgs e)
        {
            var client = new WebClient();
            XmlDocument doc = new XmlDocument();
            doc.Load(url + "protanks.xml");

            DownloadFile("Ionic.Zip.dll", "Ionic.Zip.dll", doc.GetElementsByTagName("Ionic.Zip")[0].InnerText, doc.GetElementsByTagName("Ionic.Zip")[0].Attributes["checksumm"].InnerText);
            DownloadFile("Newtonsoft.Json.dll", "Newtonsoft.Json.dll", doc.GetElementsByTagName("Newtonsoft.Json")[0].InnerText, doc.GetElementsByTagName("Newtonsoft.Json")[0].Attributes["checksumm"].InnerText);
            DownloadFile("ProcessesLibrary.dll", "ProcessesLibrary.dll", doc.GetElementsByTagName("processesLibrary")[0].InnerText, doc.GetElementsByTagName("processesLibrary")[0].Attributes["checksumm"].InnerText);
            DownloadFile("Ionic.Zip.dll", "Ionic.Zip.dll", doc.GetElementsByTagName("Newtonsoft.Json")[0].InnerText, doc.GetElementsByTagName("Newtonsoft.Json")[0].Attributes["checksumm"].InnerText);
            DownloadFile("Ionic.Zip.dll", "Ionic.Zip.dll", doc.GetElementsByTagName("Newtonsoft.Json")[0].InnerText, doc.GetElementsByTagName("Newtonsoft.Json")[0].Attributes["checksumm"].InnerText);
            DownloadFile("Ionic.Zip.dll", "Ionic.Zip.dll", doc.GetElementsByTagName("Newtonsoft.Json")[0].InnerText, doc.GetElementsByTagName("Newtonsoft.Json")[0].Attributes["checksumm"].InnerText);



            try
            {
                // Processes Library
                if (!File.Exists("ProcessesLibrary.dll") || getFileVersion("ProcessesLibrary.dll") < new Version(doc.GetElementsByTagName("processesLibrary")[0].InnerText))
                {
                    client.DownloadFile(new Uri(url + "ProcessesLibrary.dll"), "ProcessesLibrary.dll");
                }
            }
            catch (Exception ex1)
            {
                debug.Save("private void checkLibrary_DoWork(object sender, DoWorkEventArgs e)", "Processes Library", ex1.Message);
            }

            if (File.Exists("processes.exe")) { File.Delete("processes.exe"); }

            try
            {
                // Updater
                if (!File.Exists("updater.exe") || getFileVersion("updater.exe") < new Version(doc.GetElementsByTagName("updater")[0].InnerText))
                {
                    client.DownloadFile(new Uri(url + "updater.exe"), "updater.exe");
                }
            }
            catch (Exception ex1)
            {
                debug.Save("private void checkLibrary_DoWork(object sender, DoWorkEventArgs e)", "Updater", ex1.Message);
            }

            try
            {
                // Restarter
                if (!File.Exists("restart.exe") || getFileVersion("restart.exe") < new Version(doc.GetElementsByTagName("restart")[0].InnerText))
                {
                    client.DownloadFile(new Uri(url + "restart.exe"), "restart.exe");
                }
            }
            catch (Exception ex1)
            {
                debug.Save("private void checkLibrary_DoWork(object sender, DoWorkEventArgs e)", "Restarter", ex1.Message);
            }
        }

        private void checkLibrary_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!worker.IsBusy) { worker.RunWorkerAsync(); }
        }

        private Version getFileVersion(string filename)
        {
            return new Version(FileVersionInfo.GetVersionInfo(filename).FileVersion);
        }


        /// Так как при скачивании файлов мы делаем много одинаковых операций по скачивании и проверке,
        /// целесообразней завернуть все в 1 функцию и передавать ей параметры
        /// 
        private void DownloadFile(string localFile, string remoteFile, string xmlVersion, string xmlChecksumm)
        {
            try
            {
                // Для работы нам нужна библиотека Ionic.Zip.dll
                if (!File.Exists(localFile) || new Version(FileVersionInfo.GetVersionInfo(localFile).FileVersion) < new Version(xmlVersion))
                {
                    using(var client = new WebClient()){

                        client.DownloadFile(new Uri(url + remoteFile), localFile);
                        
                        if (!Checksumm(localFile, xmlChecksumm))
                        {
                            int errCount = 0;

                            while (errCount < 3)
                            {
                                client.DownloadFile(new Uri(url + remoteFile), localFile);
                                ++errCount;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.Save("private void DownloadFile(string localFile, string remoteFile, string xmlVersion, string xmlChecksumm)", "Local file: " + localFile, ex.Message);
            }
        }
    }
}