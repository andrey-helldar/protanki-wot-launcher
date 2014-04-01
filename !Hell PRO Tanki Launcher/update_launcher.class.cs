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
                doc.Load(@"http://ai-rus.com/pro/protanks.xml");

                remoteVersion = new Version(doc.GetElementsByTagName("version")[0].InnerText);
                localVersion = new Version(Application.ProductVersion);

                if (localVersion < remoteVersion)
                {
                    if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }

                    WebClient client1 = new WebClient();
                    client1.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    client1.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    client1.DownloadFileAsync(new Uri(@"http://ai-rus.com/pro/launcher.exe"), "launcher.update");
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
                if (File.Exists("launcher.update") && new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion) > new Version(Application.ProductVersion))
                {
                    Process.Start("updater.exe", "launcher.update \"" + Process.GetCurrentProcess().ProcessName + "\"");
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
            doc.Load(@"http://ai-rus.com/pro/protanks.xml");

            try
            {
                // Для работы нам нужна библиотека Ionic.Zip.dll
                if (!File.Exists("Ionic.Zip.dll"))
                {
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/Ionic.Zip.dll"), "Ionic.Zip.dll");
                }
            }
            catch (Exception ex1)
            {
                debug.Save("private void checkLibrary_DoWork(object sender, DoWorkEventArgs e)", "if (!File.Exists(\"Ionic.Zip.dll\"))", ex1.Message);
            }


            try
            {
                // Newtonsoft.Json.dll
                if (!File.Exists("Newtonsoft.Json.dll") || getFileVersion("Newtonsoft.Json.dll") < new Version(doc.GetElementsByTagName("Newtonsoft.Json")[0].InnerText))
                {
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/Newtonsoft.Json.dll"), "Newtonsoft.Json.dll");
                }
            }
            catch (Exception ex1)
            {
                debug.Save("private void checkLibrary_DoWork(object sender, DoWorkEventArgs e)", "Newtonsoft.Json.dll", ex1.Message);
            }

            try
            {
                // Processes Library
                if (!File.Exists("ProcessesLibrary.dll") || getFileVersion("ProcessesLibrary.dll") < new Version(doc.GetElementsByTagName("processesLibrary")[0].InnerText))
                {
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/ProcessesLibrary.dll"), "ProcessesLibrary.dll");
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
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/updater.exe"), "updater.exe");
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
                    client.DownloadFile(new Uri(@"http://ai-rus.com/pro/restart.exe"), "restart.exe");
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
    }
}