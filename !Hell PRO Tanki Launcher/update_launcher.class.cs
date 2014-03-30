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

        private bool summ;
        private bool onlyCheck = false;

        private Version remoteVersion,
            localVersion;

        private bool checksum(string filename, string summ)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(filename))
                    {
                        return md5.ComputeHash(stream).ToString() == summ ? true : false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CheckLocal(bool onlycheck=false)
        {
            try
            {
                onlyCheck = onlycheck;

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);

                if (!worker.IsBusy) { worker.RunWorkerAsync(); }
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

                summ = this.checksum("launcher.update", doc.GetElementsByTagName("version")[0].Attributes["checksumm"].InnerText);

                //if (summ)
                //{
                    if (localVersion < remoteVersion)
                    {
                        if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }

                        WebClient client1 = new WebClient();
                        client1.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                        client1.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                        client1.DownloadFileAsync(new Uri(@"http://ai-rus.com/pro/launcher.exe"), "launcher.update");
                    }
                /*}
                else
                {
                    if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                }*/
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
                    if (!onlyCheck) this.Download();
                }
            }
            catch (Exception)
            {
                if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                if (!onlyCheck) this.Download();
            }
        }
    }
}