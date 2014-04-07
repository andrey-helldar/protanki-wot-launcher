using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace _Hell_PRO_Tanki_Launcher
{
    class UpdateLauncher
    {
        Debug Debug = new Debug();

        private string url = @"http://ai-rus.com/pro/";

        public void Check(bool launcher = false)
        {
            try
            {
                XDocument doc = XDocument.Load(url + "protanks.xml");

                DownloadSettings().Wait(); // Загружаем файл настроек

                DeleteFile("processes.exe \"!Hell PRO Tanki Launcher.exe\""); // Удаляем ненужные файлы

                /// Если файлы имеют нулевой размер, то удаляем их
                DeleteNullFile("settings.xml, Ionic.Zip.dll, restart.exe, updater.exe, Newtonsoft.Json.dll, ProcessesLibrary.dll, LanguagePack.dll, launcher.update");

                // Проверяем целостность файлов
                CheckFile("Ionic.Zip.dll, restart.exe, updater.exe, Newtonsoft.Json.dll, ProcessesLibrary.dll, LanguagePack.dll");

                if (!launcher) // Определяем будем запускать скачивание до обновления или после
                {
                    var task1 = DownloadFile("Ionic.Zip.dll", doc.Root.Element("Ionic.Zip").Value, doc.Root.Element("Ionic.Zip").Attribute("checksum").Value);
                    var task2 = DownloadFile("restart.exe", doc.Root.Element("restart").Value, doc.Root.Element("restart").Attribute("checksum").Value);
                    var task3 = DownloadFile("LanguagePack.dll", doc.Root.Element("languagePack").Value, doc.Root.Element("languagePack").Attribute("checksum").Value);

                    Task.WhenAll(task1, task2, task3);
                }
                else
                {
                    try
                    {
                        // Скачиваем необходимые файлы
                        var task4 = DownloadFile("updater.exe", doc.Root.Element("updater").Value, doc.Root.Element("updater").Attribute("checksum").Value);
                        var task5 = DownloadFile("Newtonsoft.Json.dll", doc.Root.Element("Newtonsoft.Json").Value, doc.Root.Element("Newtonsoft.Json").Attribute("checksum").Value);
                        var task6 = DownloadFile("ProcessesLibrary.dll", doc.Root.Element("processesLibrary").Value, doc.Root.Element("processesLibrary").Attribute("checksum").Value);

                        Task.WhenAll(task4, task5, task6);

                        if (File.Exists("launcher.update") && new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion) > new Version(Application.ProductVersion))
                        {
                            Process.Start("updater.exe", "launcher.update \"" + Application.ProductName + ".exe\"");
                            Process.GetCurrentProcess().Kill();
                        }
                        else if (new Version(Application.ProductVersion) < new Version(doc.Root.Element("version").Value))
                        {
                            DownloadFile("launcher.exe", doc.Root.Element("version").Value, doc.Root.Element("version").Attribute("checksum").Value, "launcher.update").Wait();
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

        private async Task DownloadFile(string filename, string xmlVersion, string xmlchecksum, string localFile = null)
        {
            localFile = localFile != null ? localFile : filename;

            DeleteNullFile(localFile);

            try
            {
                if ((File.Exists(localFile) && new Version(FileVersionInfo.GetVersionInfo(localFile).FileVersion) < new Version(xmlVersion)) || !File.Exists(localFile))
                {
                    using (var client = new WebClient())
                    {
                        try
                        {
                            int errorCount = 0;

                            // Скачиваем файл
                            await client.DownloadFileTaskAsync(new Uri(url + filename), localFile);

                            // Проверяем контрольную сумму. Если она нарушена, применяем 3 попытки к скачиванию
                            if (!Checksum(localFile, xmlchecksum) && File.Exists(localFile))
                            {
                                while (errorCount < 3)
                                {
                                    File.Delete(localFile);
                                    await client.DownloadFileTaskAsync(new Uri(url + filename), localFile);
                                    errorCount++;
                                }
                            }
                            client.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Debug.Save("private async Task DownloadFile(string filename, string xmlVersion, string xmlchecksum, string localFile = null)",
                                "Error download: EX" + Environment.NewLine +
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
                Debug.Save("private async Task DownloadFile(string filename, string xmlVersion, string xmlchecksum, string localFile = null)",
                    "Error download: EX1" + Environment.NewLine +
                    "Filename: " + filename + Environment.NewLine +
                    "Localname: " + (localFile != null ? localFile : "null") + Environment.NewLine +
                    "URL: " + url,
                    ex1.Message);
            }
        }

        private async Task DownloadSettings()
        {
            if (!File.Exists("settings.xml"))
            {
                using (var client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(new Uri(url + "settings.xml"), "settings.xml");
                    client.Dispose();
                }
            }
        }

        private void CheckFile(params string[] fileArr)
        {
            foreach (string filename in fileArr)
                if (File.Exists(filename))
                {
                    try
                    {
                        string s = FileVersionInfo.GetVersionInfo(filename).FileVersion;
                    }
                    catch (Exception)
                    {
                        File.Delete(filename);
                    }
                }
        }

        private void DeleteNullFile(params string[] fileParams)
        {
            foreach (string filename in fileParams)
                if (File.Exists(filename) && new FileInfo(filename).Length == 0) { File.Delete(filename); }
        }

        private void DeleteFile(params string[] fileArr)
        {
            foreach (string filename in fileArr)
                if (File.Exists(filename)) { File.Delete(filename); }
        }
    }
}