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
        private DialogResult enableUpdate = DialogResult.Yes;

        BackgroundWorker downloadUpdates = new BackgroundWorker();

        public void CheckUpdates(bool check = false)
        {
            try
            {
                /// Если параметр onlyCheck включен, то проверяем обновления только
                /// для файлов библиотек, минуя проверку обновлений основного приложения
                onlyCheck = check;

                if (!check)
                {
                    /// Считываем версию основного файла приложения
                    /// Нужно для вывода окна с запросом обновления
                    XmlDocument doc = new XmlDocument();
                    doc.Load(url + "protanks.xml");
                    string xmlVer = doc.GetElementsByTagName("version")[0].InnerText;

                    if ((File.Exists("launcher.update") && new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion) < new Version(xmlVer)) ||
                        (!File.Exists("launcher.update") && new Version(Application.ProductVersion) < new Version(xmlVer)))
                    {
                        enableUpdate = MessageBox.Show(fIndex.ActiveForm,
                            "Обнаружена новая версия: " + xmlVer.ToString() + Environment.NewLine +
                            "Текущая версия: " + Application.ProductVersion + Environment.NewLine + Environment.NewLine +
                            "Применить обновление сейчас?",
                            Application.ProductName + " v" + Application.ProductVersion, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    }
                    else
                    {
                        /// Так как файл "launcher.update" не соответствует условиям,
                        /// то есть не требует обновления, то удаляем его, если он существует
                        if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.Save("public void CheckUpdates(bool check = false)", "onlyCheck", ex.Message);
            }


            try
            {
                /// Создаем backgroundWorker и запускаем процедуру обновления файлов
                downloadUpdates.WorkerReportsProgress = true;
                downloadUpdates.WorkerSupportsCancellation = true;
                downloadUpdates.DoWork += new DoWorkEventHandler(downloadUpdates_DoWork);
                downloadUpdates.RunWorkerCompleted += new RunWorkerCompletedEventHandler(downloadUpdates_RunWorkerCompleted);

                if (!downloadUpdates.IsBusy) { downloadUpdates.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                debug.Save("public void CheckUpdates(bool check = false)", "if (!downloadUpdates.IsBusy) { downloadUpdates.RunWorkerAsync(); }", ex.Message);
            }
        }

        private bool Checksumm(string filename, string summ)
        {
            try
            {
                if (File.Exists(filename))
                    using (FileStream fs = File.OpenRead(filename))
                    {
                        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        byte[] fileData = new byte[fs.Length];
                        fs.Read(fileData, 0, (int)fs.Length);
                        byte[] checkSumm = md5.ComputeHash(fileData);
                        return BitConverter.ToString(checkSumm) == summ.ToUpper() ? true : false;
                    }
                else
                    return false;
            }
            catch (Exception ex)
            {
                debug.Save("private bool Checksumm(string filename, string summ)", "Filename: " + filename, ex.Message);
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
                if (enableUpdate == DialogResult.Yes && !onlyCheck)
                    DownloadFile("launcher.update", doc.GetElementsByTagName("version")[0].InnerText, doc.GetElementsByTagName("version")[0].Attributes["checksumm"].InnerText, Application.ProductName);
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
                Process.Start("updater.exe", "launcher.update \"Multipack Launcher.exe\" \"!Hell Multipack Launcher.exe\"");
                Process.GetCurrentProcess().Kill();
            }
        }


        /// 
        /// Так как при скачивании файлов мы делаем много одинаковых операций по скачивании и проверке,
        /// целесообразней завернуть все в 1 функцию и передавать ей параметры
        /// 
        private void DownloadFile(string filename, string xmlVersion, string xmlChecksumm, string localFilename = null)
        {
            try
            {
                localFilename = localFilename != null ? localFilename : filename;

                if (!(File.Exists(localFilename)) || new Version(FileVersionInfo.GetVersionInfo(localFilename).FileVersion) < new Version(xmlVersion))
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(new Uri(url + filename), localFilename);

                        if (!Checksumm(localFilename, xmlChecksumm))
                        {
                            int errCount = 0;

                            while (errCount < 3)
                            {
                                try
                                {
                                    client.DownloadFile(new Uri(url + filename), localFilename);
                                    ++errCount;
                                }
                                catch (Exception ex1)
                                {
                                    debug.Save("private void DownloadFile(string filename, string xmlVersion, string xmlChecksumm)", "Filename: " + filename + Environment.NewLine + "Error cicle: " + errCount.ToString(), ex1.Message);
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