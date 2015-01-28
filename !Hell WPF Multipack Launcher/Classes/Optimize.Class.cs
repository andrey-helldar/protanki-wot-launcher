using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;
using System.IO;
using Ionic.Zip;
using Processes;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Optimize
    {
        Debugging Debugging = new Debugging();

        public void Start(
            bool WinXP = true,
            bool Kill = false,
            bool ForceKill = false,
            bool Aero = false,
            bool Video = false,
            bool Weak = false,
            bool Manual = false)
        {
            Task.Factory.StartNew(() => Progress(0, 2, true, true)).Wait();

            MainWindow.LoadPage.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                MainWindow.LoadPage.Visibility = System.Windows.Visibility.Visible;
                MainWindow.LoadPage.Content = new Classes.Language().Set("PageSettingsGeneral", "Optimize", (string)MainWindow.JsonSettingsGet("info.language"));
            }));
            Thread.Sleep(100);
            Task.Factory.StartNew(() => Progress(1)).Wait();

            /***************************
             * Disable Windows Aero
             * *************************/
            try
            {
                if (!WinXP)
                    if ((Manual || Aero) && (bool)MainWindow.JsonSettingsGet("settings.admin"))
                    {
                        Process.Start(new ProcessStartInfo("cmd", @"/c net stop uxsms"));
                        MainWindow.JsonSettingsSet("settings.aero_disable", true, "bool");
                        Task.Factory.StartNew(() => Progress(1)).Wait();
                    }
            }
            catch (Exception ex) { Debugging.Save("Optimize.Class", "Start()", "Disable Windows Aero", ex.Message, ex.StackTrace); }


            /***************************
             * Kill & Force Kill
             * *************************/
            try
            {
                if (Manual || Kill)
                {
                    Global ProcessesGlobal = new Processes.Global();
                    Listing ProcessesList = new Listing();

                    int count = (Manual || ForceKill) ? 2 : 1;

                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            Task.Factory.StartNew(() => Progress(0, Process.GetProcesses().Length)).Wait();
                            var processes = Process.GetProcesses();

                            foreach (var process in processes)
                            {
                                if (process.SessionId == Process.GetCurrentProcess().SessionId &&
                                    Array.IndexOf(ProcessesGlobal.Processes(), process.ProcessName) == -1 && // Global processes list
                                    MainWindow.JsonSettingsGet("processes").ToString().IndexOf(process.ProcessName) == -1) // User processes list
                                    if (i == 0) process.CloseMainWindow(); else process.Kill();

                                Task.Factory.StartNew(() => Progress(1)).Wait();
                            }

                            // If ForceKill is True, then...
                            if (count > 0)
                            {
                                Task.Factory.StartNew(() => Progress(0, 5)).Wait();

                                // Отображаем прогресс, пока ждем завершения процессов
                                for (int s = 0; s < 5; s++)
                                {
                                    Task.Factory.StartNew(() => Progress(1)).Wait();
                                    Thread.Sleep(1000);
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception ex) { Debugging.Save("Optimize.Class", "Start()", "Kill & Force Kill", ex.Message, ex.StackTrace); }


            /***************************
             * Graphic optimize
             * *************************/
            try
            {
                if (Manual || Video || Weak)
                {
                    Task.Factory.StartNew(() => Progress(0, 1)).Wait();

                    try
                    {
                        string pathPreferences = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                            @"\Wargaming.net\WorldOfTanks\preferences" + ((bool)MainWindow.JsonSettingsGet("game.test") ? "_ct" : "") + ".xml";

                        if (File.Exists(pathPreferences))
                        {
                            XDocument docPref = XDocument.Load(pathPreferences);

                            if (docPref.Root.Element("graphicsPreferences") != null)
                            {
                                Task.Factory.StartNew(() => Progress(0, (from s in docPref.Root.Element("graphicsPreferences").Descendants("entry") select s).Count())).Wait();

                                foreach (XElement el in docPref.Root.Element("graphicsPreferences").Elements("entry"))
                                {
                                    if (el.Element("label") != null)
                                        switch (el.Element("label").Value.Trim())
                                        {
                                            case "SHADOWS_QUALITY": el.Element("activeOption").SetValue("	4	"); break;
                                            case "DECALS_QUALITY": el.Element("activeOption").SetValue("	3	"); break;
                                            case "LIGHTING_QUALITY": el.Element("activeOption").SetValue("	4	"); break;
                                            case "TEXTURE_QUALITY": el.Element("activeOption").SetValue("	1	"); break;
                                            case "TERRAIN_QUALITY": el.Element("activeOption").SetValue("	0	"); break;
                                            case "SPEEDTREE_QUALITY": el.Element("activeOption").SetValue("	1	"); break;
                                            case "WATER_QUALITY": el.Element("activeOption").SetValue("	3	"); break;
                                            case "FAR_PLANE": el.Element("activeOption").SetValue("	0	"); break;
                                            case "FLORA_QUALITY": el.Element("activeOption").SetValue("	4	"); break;
                                            case "OBJECT_LOD": el.Element("activeOption").SetValue("	1	"); break;
                                            case "VEHICLE_DUST_ENABLED": el.Element("activeOption").SetValue("	0	"); break;
                                            case "VEHICLE_TRACES_ENABLED": el.Element("activeOption").SetValue("	0	"); break;
                                            case "SMOKE_ENABLED": el.Element("activeOption").SetValue("	0	"); break;
                                            case "SNIPER_MODE_EFFECTS_QUALITY": el.Element("activeOption").SetValue("	3	"); break;
                                            case "PS_USE_PERFORMANCER": el.Element("activeOption").SetValue("	0	"); break;
                                            case "EFFECTS_QUALITY": el.Element("activeOption").SetValue("	3	"); break;
                                            case "SNIPER_MODE_GRASS_ENABLED": el.Element("activeOption").SetValue("	0	"); break;
                                            case "POST_PROCESSING_QUALITY": el.Element("activeOption").SetValue("	4	"); break;
                                            case "MOTION_BLUR_QUALITY": el.Element("activeOption").SetValue("	3	"); break;
                                            default: break;
                                        }

                                    Task.Factory.StartNew(() => Progress(1)).Wait();
                                }
                            }

                            if (docPref.Root.Element("devicePreferences").Element("waitVSync") != null)
                                if (docPref.Root.Element("devicePreferences").Element("waitVSync").Value == "	false	")
                                    if (docPref.Root.Element("devicePreferences").Element("tripleBuffering") != null)
                                        docPref.Root.Element("devicePreferences").Element("tripleBuffering").SetValue("	false	");
                                    else
                                        docPref.Root.Element("devicePreferences").Add(new XElement("tripleBuffering", "	false	"));

                            if (docPref.Root.Element("devicePreferences").Element("customAAMode") != null)
                                docPref.Root.Element("devicePreferences").Element("customAAMode").SetValue("	0	");
                            else
                                docPref.Root.Element("devicePreferences").Add(new XElement("customAAMode", "	0	"));

                            if (docPref.Root.Element("devicePreferences").Element("drrScale") != null)
                                docPref.Root.Element("devicePreferences").Element("drrScale").SetValue("	0.850000	");
                            else
                                docPref.Root.Element("devicePreferences").Add(new XElement("drrScale", "	0.850000	"));

                            /*if (docPref.Root.Element("devicePreferences").Element("windowed") != null)
                                docPref.Root.Element("devicePreferences").Element("windowed").SetValue("	false	");
                            else
                                docPref.Root.Element("devicePreferences").Add(new XElement("windowed", "	false	"));*/

                            if (docPref.Root.Element("devicePreferences").Element("waitVSync") != null)
                                docPref.Root.Element("devicePreferences").Element("waitVSync").SetValue("	false	");
                            else
                                docPref.Root.Element("devicePreferences").Add(new XElement("waitVSync", "	false	"));

                            if (docPref.Root.Element("scriptsPreferences").Element("replayPrefs") != null)
                                if (docPref.Root.Element("scriptsPreferences").Element("replayPrefs").Element("fpsPerfomancer") != null)
                                    docPref.Root.Element("scriptsPreferences").Element("replayPrefs").Element("fpsPerfomancer").SetValue("	STAKLg=	");

                            if (docPref.Root.Element("scriptsPreferences").Element("fov") != null)
                                docPref.Root.Element("scriptsPreferences").Element("fov").SetValue("	85.000000	");

                            if (docPref.Root.Element("scriptsPreferences").Element("loginPage") != null)
                                if (docPref.Root.Element("scriptsPreferences").Element("loginPage").Element("showLoginWallpaper") != null)
                                    docPref.Root.Element("scriptsPreferences").Element("loginPage").Element("showLoginWallpaper").SetValue((bool)MainWindow.JsonSettingsGet("settings.weak") ? "	false	" : "	true	");

                            if ((bool)MainWindow.JsonSettingsGet("settings.weak") && docPref.Root.Element("devicePreferences") != null)
                                if (docPref.Root.Element("devicePreferences").Element("aspectRatio") != null)
                                    switch (docPref.Root.Element("devicePreferences").Element("aspectRatio").Value.Trim())
                                    {
                                        case "1.777778":
                                            docPref.Root.Element("devicePreferences").Element("fullscreenWidth").SetValue("1280");
                                            docPref.Root.Element("devicePreferences").Element("fullscreenHeight").SetValue("768");
                                            break;

                                        case "1.600000":
                                            docPref.Root.Element("devicePreferences").Element("fullscreenWidth").SetValue("1280");
                                            docPref.Root.Element("devicePreferences").Element("fullscreenHeight").SetValue("960");
                                            break;

                                        case "1.900000":
                                            docPref.Root.Element("devicePreferences").Element("fullscreenWidth").SetValue("1280");
                                            docPref.Root.Element("devicePreferences").Element("fullscreenHeight").SetValue("768");
                                            break;

                                        default:
                                            docPref.Root.Element("devicePreferences").Element("fullscreenWidth").SetValue("1024");
                                            docPref.Root.Element("devicePreferences").Element("fullscreenHeight").SetValue("768");
                                            break;
                                    }

                            docPref.Save(pathPreferences);

                            StreamReader sr = new StreamReader(pathPreferences);
                            string content = sr.ReadToEnd();
                            sr.Close();

                            content = content.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine, "");

                            StreamWriter sw = new StreamWriter(pathPreferences);
                            sw.Write(content);
                            sw.Close();
                        }
                    }
                    catch (Exception ex0) { Debugging.Save("Optimize.Class", "Graphic()", ex0.Message, ex0.StackTrace); }

                    Task.Factory.StartNew(() => Progress(1)).Wait();
                }
            }
            catch (Exception ex) { Debugging.Save("Optimize.Class", "Start()", "Graphic optimize", ex.Message, ex.StackTrace); }
            finally
            {
                MainWindow.LoadPage.Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    MainWindow.LoadPage.Content = new Classes.Language().Set("PageLoading", "lLoading", (string)MainWindow.JsonSettingsGet("info.language"));
                    MainWindow.LoadPage.Visibility = System.Windows.Visibility.Hidden;
                }));
                Task.Factory.StartNew(() => Progress(1));
            }
        }

        private void Progress(int value, int max = 0, bool resetValue = false, bool resetMax = false)
        {
            try
            {
                MainWindow.OptimizeProgress.Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    if (max > 0)
                        if (resetMax)
                            MainWindow.OptimizeProgress.Maximum = max;
                        else
                            MainWindow.OptimizeProgress.Maximum = MainWindow.OptimizeProgress.Maximum + max;

                    if (resetValue)
                        MainWindow.OptimizeProgress.Value = value;
                    else
                        if (MainWindow.OptimizeProgress.Value > MainWindow.OptimizeProgress.Maximum)
                            MainWindow.OptimizeProgress.Value = MainWindow.OptimizeProgress.Maximum;
                        else
                            MainWindow.OptimizeProgress.Value = MainWindow.OptimizeProgress.Value + value;
                }));
            }
            catch (Exception ex) { Debugging.Save("Optimize.Class", "Progress()", ex.Message, ex.StackTrace); }
        }
    }
}
