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
using System.Threading.Tasks;

namespace _Hell_PRO_Tanki_Launcher
{
    class update_launcher
    {
        debug debug = new debug();

        private string url = @"http://ai-rus.com/pro/";
        private DialogResult enableUpdate = DialogResult.No;

        public void Check()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(url + "protanks.xml");

                if (!File.Exists("settings.xml"))
                {
                    var client = new WebClient();
                    var task0 = Task.Factory.StartNew(() => client.DownloadFile(new Uri(url + "settings.xml"), "settings.xml"));
                    task0.Wait();
                }
                else if (new FileInfo("settings.xml").Length == 0) { File.Delete("settings.xml"); }

                var task1 = Task.Factory.StartNew(() => DownloadFile("Ionic.Zip.dll", doc.GetElementsByTagName("Ionic.Zip")[0].InnerText, doc.GetElementsByTagName("Ionic.Zip")[0].Attributes["checksumm"].InnerText));
                var task2 = Task.Factory.StartNew(() => DownloadFile("restart.exe", doc.GetElementsByTagName("restart")[0].InnerText, doc.GetElementsByTagName("restart")[0].Attributes["checksumm"].InnerText));
                var task3 = Task.Factory.StartNew(() => DownloadFile("updater.exe", doc.GetElementsByTagName("updater")[0].InnerText, doc.GetElementsByTagName("updater")[0].Attributes["checksumm"].InnerText));
                var task4 = Task.Factory.StartNew(() => DownloadFile("Newtonsoft.Json.dll", doc.GetElementsByTagName("Newtonsoft.Json")[0].InnerText, doc.GetElementsByTagName("Newtonsoft.Json")[0].Attributes["checksumm"].InnerText));
                var task5 = Task.Factory.StartNew(() => DownloadFile("ProcessesLibrary.dll", doc.GetElementsByTagName("processesLibrary")[0].InnerText, doc.GetElementsByTagName("processesLibrary")[0].Attributes["checksumm"].InnerText));
                var task6 = Task.Factory.StartNew(() => DownloadFile("LanguagePack.dll", doc.GetElementsByTagName("languagePack")[0].InnerText, doc.GetElementsByTagName("languagePack")[0].Attributes["checksumm"].InnerText));

                Task.WaitAll(task1, task2, task3, task4, task5, task6);
                
                string xmlVer = doc.GetElementsByTagName("version")[0].InnerText;

                if ((File.Exists("launcher.update") && new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion) < new Version(xmlVer)) ||
                    (new Version(Application.ProductVersion) < new Version(xmlVer)))
                {
                    enableUpdate = MessageBox.Show(fIndex.ActiveForm,
                        "Обнаружена новая версия: " + xmlVer.ToString() + Environment.NewLine +
                        "Текущая версия: " + Application.ProductVersion + Environment.NewLine + Environment.NewLine +
                        "Применить обновление сейчас?",
                        Application.ProductName + " v" + Application.ProductVersion, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                }
                else
                {
                    if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                }

                if (File.Exists("processes.exe")) { File.Delete("processes.exe"); } // Удаляем ненужный файл
            }
            catch (Exception ex)
            {
                if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }

                enableUpdate = DialogResult.No;

                debug.Save("public void CheckUpdates()", "", ex.Message);
            }
        }

        private bool Checksumm(string filename, string summ)
        {
            try
            {
                if (File.Exists(filename) && summ != null && new FileInfo(filename).Length > 0)
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

        private void DownloadFile(string filename, string xmlVersion, string xmlChecksumm, string localFile = null)
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
                            //var client = new WebClient();
                            client.DownloadFile(new Uri(url + filename), localFile);

                            if (!Checksumm(localFile, xmlChecksumm) && File.Exists(localFile))
                            {
                                File.Delete(localFile);
                                client.DownloadFile(new Uri(url + filename), localFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            debug.Save("private void DownloadFile(string filename, string xmlVersion, string xmlChecksumm)",
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
                debug.Save("private void DownloadFile(string filename, string xmlVersion, string xmlChecksumm)",
                    "Error download" + Environment.NewLine +
                    "Filename: " + filename + Environment.NewLine +
                    "Localname: " + (localFile != null ? localFile : "null") + Environment.NewLine +
                    "URL: " + url,
                    ex1.Message);
            }
        }
    }
}