﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _Hell_PRO_Tanki_Launcher
{
    class UpdateLauncher
    {
        /// <summary>
        /// Работаем с LINQ to XML
        /// http://habrahabr.ru/post/24673/
        /// </summary>
        /// 
        Debug Debug = new Debug();

        public async Task CheckProcessFile()
        {
            try
            {
                if (Process.GetCurrentProcess().ProcessName != Application.ProductName && Process.GetCurrentProcess().ProcessName != Application.ProductName + ".vshost")
                {
                    Task.Factory.StartNew(() => DeleteFile("Multipack Launcher.exe")).Wait();

                    Process.Start("restart.exe", "\"" + Process.GetCurrentProcess().ProcessName + ".exe\" \"" + Application.ProductName + ".exe\"");
                    Process.GetCurrentProcess().Kill();
                }

            }
            catch (Exception ex) { Debug.Save("UpdateLauncher", "CheckProcessFile()", ex.Message); }
            finally { }
        }

        public void Check()
        {
            try
            {
                SaveFromResources().Wait(); // Проверяем существуют ли файлы. Если нет - сохраняем из ресурсов

                CheckProcessFile().Wait();

                XDocument doc = XDocument.Load(Properties.Resources.LibraryVersions + "version.xml");

                DownloadSettings().Wait(); // Загружаем файл настроек

                /// Если файлы имеют нулевой размер, то удаляем их
                Task.Factory.StartNew(() => DeleteNullFile("settings.xml", "Ionic.Zip.dll", "restart.exe", "Newtonsoft.Json.dll", "ProcessesLibrary.dll", "launcher.update")).Wait();

                // Проверяем целостность файлов
                Task.Factory.StartNew(() => CheckFile(false, "Ionic.Zip.dll", "restart.exe", "Newtonsoft.Json.dll", "ProcessesLibrary.dll")).Wait();

                int i = -1,
                    all = 0;

                foreach (XElement el in doc.Root.Elements())
                    if (el.Attribute("user").Value == "true")
                        ++all;

                Task[] tasks = new Task[all];

                foreach (XElement el in doc.Root.Elements())
                    if (el.Attribute("user").Value == "true")
                    {
                        try
                        {
                            tasks[++i] = DownloadFile(el.Name + "." + el.Attribute("ext").Value, el.Value, el.Attribute("checksum").Value);
                        }
                        finally { }
                    }

                /* try
                 {
                     if (File.Exists("launcher.update") && new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion) > new Version(Application.ProductVersion))
                     {
                         Process.Start("restart.exe", "launcher.update \"" + Application.ProductName + ".exe\"");
                         Process.GetCurrentProcess().Kill();
                     }
                     else if (new Version(Application.ProductVersion) < new Version(doc.Root.Element("version").Value))
                     {
                         DownloadFile("launcher.exe", doc.Root.Element("version").Value, doc.Root.Element("version").Attribute("checksum").Value, "launcher.update").Wait();
                     }
                     else DeleteFile("launcher.update");
                 }
                 catch (Exception ex1) { Debug.Save("public void Check()", "launcher.update", ex1.Message); }*/

                /// Тестируем функцию обновлений лаунчера
                /*bool updLauncher = false;
                if (File.Exists("launcher.update")) File.Delete("launcher.update");
                if (new Version(doc.Root.Element("launcher").Value) > new Version(Application.ProductVersion))
                {
                    updLauncher = true;
                    tasks[++i] = DownloadFile("launcher.exe", doc.Root.Element("launcher").Value, doc.Root.Element("launcher").Attribute("checksum").Value, "launcher.update");
                }*/

                Task.WhenAll(tasks);
            }
            catch (Exception ex) { Debug.Save("public void Check()", ex.Message); }
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
                Debug.Save("private bool checksum()", "Filename: " + filename, ex.Message);
                return false;
            }
        }

        private async Task DownloadFile(string filename, string xmlVersion, string xmlChecksum, string localFile = null)
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
                            await client.DownloadFileTaskAsync(new Uri(Properties.Resources.LibraryVersions + filename), localFile);

                            // Проверяем контрольную сумму. Если она нарушена, применяем 3 попытки к скачиванию
                            if (!Checksum(localFile, xmlChecksum) && File.Exists(localFile))
                                while (errorCount < 3)
                                {
                                    if (!Checksum(localFile, xmlChecksum) && File.Exists(localFile))
                                    {
                                        DeleteFile(localFile);
                                        await Task.Delay(1000);
                                        await client.DownloadFileTaskAsync(new Uri(Properties.Resources.LibraryVersions + filename), localFile);
                                        errorCount++;
                                    }
                                    else { break; }
                                }
                            client.Dispose();
                        }
                        catch (WebException ex)
                        {
                            Debug.Save("private async Task DownloadFile()",
                                "Error download: EX" + Environment.NewLine +
                                "Filename: " + filename + Environment.NewLine +
                                "Localname: " + (localFile != null ? localFile : "null") + Environment.NewLine +
                                "URL: " + Properties.Resources.LibraryVersions+filename,
                                ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                Debug.Save("private async Task DownloadFile()",
                    "Error download: EX1" + Environment.NewLine +
                    "Filename: " + filename + Environment.NewLine +
                    "Localname: " + (localFile != null ? localFile : "null") + Environment.NewLine +
                    "URL: " + Properties.Resources.LibraryVersions+filename,
                    ex1.Message);
            }
        }

        private async Task DownloadSettings()
        {
            try
            {
                string settings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";
                if (!File.Exists(settings)) { using (var client = new WebClient()) { await client.DownloadFileTaskAsync(new Uri(Properties.Resources.LibraryVersions + "settings.xml"), settings); client.Dispose(); } }
            }
            catch (Exception ex) { Debug.Save("UpdateLauncher", "DownloadSettings()", ex.Message); }
        }

        private void CheckFile(bool wait = false, params string[] fileArr)
        {
            try
            {
                foreach (string filename in fileArr)
                    if (File.Exists(filename))
                        try { Task.Factory.StartNew(() => CheckOneFile(filename, wait)).Wait(); }
                        catch (Exception ex) { Debug.Save("CheckFile()", "Filename: " + filename + Environment.NewLine + ex.Message); }
            }
            catch (Exception ex) { Debug.Save("UpdateLauncher", "CheckFile()", ex.Message); }
        }

        private void CheckOneFile(string filename, bool wait = false)
        {
            if (File.Exists(filename))
            {
                try { string s = FileVersionInfo.GetVersionInfo(filename).FileVersion; }
                catch (Exception)
                {
                    if (!wait)
                        File.Delete(filename);
                    else
                    {
                        Thread.Sleep(3000);
                        CheckOneFile(filename, true);
                    }
                }
            }
        }

        private void DeleteNullFile(params string[] fileParams)
        {
            try
            {
                foreach (string filename in fileParams)
                    try
                    {
                        if (File.Exists(filename) && new FileInfo(filename).Length == 0) { File.Delete(filename); }
                    }
                    catch (Exception ex) { Debug.Save("UpdateLauncher", "DeleteNullFile()", filename, ex.Message); }
            }
            catch (Exception ex) { Debug.Save("UpdateLauncher", "DeleteNullFile()", ex.Message); }
        }

        private void DeleteFile(params string[] fileArr)
        {
            try
            {
                foreach (string filename in fileArr)
                    if (File.Exists(filename)) { File.Delete(filename); }
            }
            catch (Exception ex) { Debug.Save("UpdateLauncher", "DeleteFile()", ex.Message); }
            finally { }
        }

        public async Task SaveFromResources()
        {
            try
            {
                //if (!File.Exists("Ionic.Zip.dll")) { File.WriteAllBytes("Ionic.Zip.dll", Properties.Resources.IonicZip); }
                if (!File.Exists("restart.exe")) { File.WriteAllBytes("restart.exe", Properties.Resources.restart); }
                //if (!File.Exists("Newtonsoft.Json.dll")) { File.WriteAllBytes("Newtonsoft.Json.dll", Properties.Resources.Newtonsoft_Json); }
                if (!File.Exists("ProcessesLibrary.dll")) { File.WriteAllBytes("ProcessesLibrary.dll", Properties.Resources.ProcessesLibrary); }

                string Settings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";
                if (!File.Exists(Settings)) { File.WriteAllText(Settings, Properties.Resources.Settings); }
            }
            catch (Exception ex) { Debug.Save("UpdateLauncher", "SaveFromResources()", ex.Message); }
        }
    }
}