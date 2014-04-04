﻿using System;
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
using System.Threading.Tasks;

namespace _Hell_PRO_Tanki_Launcher
{
    class UpdateLauncher
    {
        Debug Debug = new Debug();

        private string url = @"http://ai-rus.com/pro/";
        private ProgressBar downloadPercent = null;

        public void Check(bool launcher = false, ProgressBar report = null)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(url + "protanks.xml");

                if (!File.Exists("settings.xml"))
                {
                    using (var client = new WebClient())
                        Task.Factory.StartNew(() => client.DownloadFile(new Uri(url + "settings.xml"), "settings.xml")).Wait();
                }

                /// Удаляем ненужные файлы
                if (File.Exists("processes.exe")) { File.Delete("processes.exe"); }
                if (File.Exists("!Hell PRO Tanki Launcher.exe")) { File.Delete("!Hell PRO Tanki Launcher.exe"); }

                /// Если файлы имеют нулевой размер, то удаляем их
                if (File.Exists("settings.xml") && new FileInfo("settings.xml").Length == 0) { File.Delete("settings.xml"); }
                if (File.Exists("Ionic.Zip.dll") && new FileInfo("Ionic.Zip.dll").Length == 0) { File.Delete("Ionic.Zip.dll"); }
                if (File.Exists("restart.exe") && new FileInfo("restart.exe").Length == 0) { File.Delete("restart.exe"); }
                if (File.Exists("updater.exe") && new FileInfo("updater.exe").Length == 0) { File.Delete("updater.exe"); }
                if (File.Exists("Newtonsoft.Json.dll") && new FileInfo("Newtonsoft.Json.dll").Length == 0) { File.Delete("Newtonsoft.Json.dll"); }
                if (File.Exists("ProcessesLibrary.dll") && new FileInfo("ProcessesLibrary.dll").Length == 0) { File.Delete("ProcessesLibrary.dll"); }
                if (File.Exists("LanguagePack.dll") && new FileInfo("LanguagePack.dll").Length == 0) { File.Delete("LanguagePack.dll"); }
                if (File.Exists("launcher.update") && new FileInfo("launcher.update").Length == 0) { File.Delete("launcher.update"); }

                if (!launcher) // Определяем будем запускать скачивание до обновления или после
                {


                    var task1 = Task.Factory.StartNew(() => DownloadFile("Ionic.Zip.dll", doc.GetElementsByTagName("Ionic.Zip")[0].InnerText, doc.GetElementsByTagName("Ionic.Zip")[0].Attributes["checksum"].InnerText));
                    var task2 = Task.Factory.StartNew(() => DownloadFile("restart.exe", doc.GetElementsByTagName("restart")[0].InnerText, doc.GetElementsByTagName("restart")[0].Attributes["checksum"].InnerText));
                    var task6 = Task.Factory.StartNew(() => DownloadFile("LanguagePack.dll", doc.GetElementsByTagName("languagePack")[0].InnerText, doc.GetElementsByTagName("languagePack")[0].Attributes["checksum"].InnerText));

                    Task.WaitAll(task1, task2, task6);
                }
                else
                {
                    try
                    {
                        // Скачиваем необходимые файлы
                        var task3 = Task.Factory.StartNew(() => DownloadFile("updater.exe", doc.GetElementsByTagName("updater")[0].InnerText, doc.GetElementsByTagName("updater")[0].Attributes["checksum"].InnerText));
                        var task4 = Task.Factory.StartNew(() => DownloadFile("Newtonsoft.Json.dll", doc.GetElementsByTagName("Newtonsoft.Json")[0].InnerText, doc.GetElementsByTagName("Newtonsoft.Json")[0].Attributes["checksum"].InnerText));
                        var task5 = Task.Factory.StartNew(() => DownloadFile("ProcessesLibrary.dll", doc.GetElementsByTagName("processesLibrary")[0].InnerText, doc.GetElementsByTagName("processesLibrary")[0].Attributes["checksum"].InnerText));

                        Task.WaitAll(task3, task4, task5);
                        
                        if (File.Exists("launcher.update") && new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion) > new Version(Application.ProductVersion))
                        {
                            Process.Start("updater.exe", "launcher.update \"" + Application.ProductName + ".exe\"");
                            Process.GetCurrentProcess().Kill();
                        }
                        else if (new Version(Application.ProductVersion) < new Version(doc.GetElementsByTagName("version")[0].InnerText))
                        {
                            if (report != null)
                            {
                                downloadPercent = report;
                                downloadPercent.Value = 0;
                            }

                            Task.Factory.StartNew(() => DownloadFile("launcher.exe", doc.GetElementsByTagName("version")[0].InnerText, doc.GetElementsByTagName("version")[0].Attributes["checksum"].InnerText, "launcher.update", true)).Wait();
                        }
                        else if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                    }
                    catch (Exception ex1)
                    {
                        Debug.Save("public void Check(bool launcher = false)", "launcher.update", ex1.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Save("public void Check(bool launcher = false)", "", ex.Message);
            }
        }

        private bool Checksum(string filename, string summ)
        {
            try
            {
                if (File.Exists(filename) && summ != null && new FileInfo(filename).Length > 0)
                    using (FileStream fs = File.OpenRead(filename))
                    {
                        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        byte[] fileData = new byte[fs.Length];
                        fs.Read(fileData, 0, (int)fs.Length);
                        byte[] checksum = md5.ComputeHash(fileData);
                        return BitConverter.ToString(checksum) == summ.ToUpper();
                    }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.Save("private bool checksum(string filename, string summ)", "Filename: " + filename, ex.Message);
                return false;
            }
        }

        private void DownloadFile(string filename, string xmlVersion, string xmlchecksum, string localFile = null, bool showStatus = false)
        {
            localFile = localFile != null ? localFile : filename;

            if (File.Exists(localFile) && new FileInfo(localFile).Length == 0) { File.Delete(localFile); }

            try
            {
                if ((File.Exists(localFile) && new Version(FileVersionInfo.GetVersionInfo(localFile).FileVersion) < new Version(xmlVersion)) || !File.Exists(localFile))
                {
                    using (var client = new WebClient())
                    {
                        try
                        {
                            if (showStatus && downloadPercent != null) { client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged); }
                            client.DownloadFile(new Uri(url + filename), localFile);

                            if (!Checksum(localFile, xmlchecksum) && File.Exists(localFile))
                            {
                                File.Delete(localFile);
                                client.DownloadFile(new Uri(url + filename), localFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Save("private void DownloadFile(string filename, string xmlVersion, string xmlchecksum)",
                                "Filename: " + filename + Environment.NewLine +
                                "Localname: " + (localFile != null ? localFile : "null") + Environment.NewLine +
                                "URL: " + url,
                                ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                Debug.Save("private void DownloadFile(string filename, string xmlVersion, string xmlchecksum)",
                    "Error download" + Environment.NewLine +
                    "Filename: " + filename + Environment.NewLine +
                    "Localname: " + (localFile != null ? localFile : "null") + Environment.NewLine +
                    "URL: " + url,
                    ex1.Message);
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadPercent.Value = e.ProgressPercentage;
        }
    }
}