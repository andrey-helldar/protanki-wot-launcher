using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using Ionic.Zip;

namespace WPF_Multipack_Launcher.Classes
{
    class Optimize
    {
        Debug Debug = new Debug();

        public void Start(
            bool WinXP = true,
            bool Kill = false,
            bool ForceKill = false,
            bool Aero = false,
            bool Video = false,
            bool Weak = false,
            bool Manual = false)
        {
            int progress = 0,
                maxProgress = 0;


            /***************************
             * Disable Windows Aero
             * *************************/
            try
            {
                if (!WinXP)
                    if (Manual || Aero)
                    {
                        ++maxProgress;
                        Process.Start(new ProcessStartInfo("cmd", @"/c net stop uxsms"));
                        ++progress;
                    }
            }
            catch (Exception ex) { Debug.Save("Optimize.Class", "Start()", "Disable Windows Aero", ex.Message); }


            /***************************
             * Kill & Force Kill
             * *************************/
            try
            {
                if (Manual || Kill)
                {
                    Processes.Global processesLibrary = new Processes.Global();
                    Processes.Listing processList = new Processes.Listing();

                    int session = Process.GetCurrentProcess().SessionId;
                    bool kill = false;

                    for (int i = 0; i < 2; i++)
                    {
                        int processCount = Process.GetProcesses().Length;
                        maxProgress += processCount;

                        foreach (var process in Process.GetProcesses())
                        {
                            try
                            {
                                if (process.SessionId == session &&
                                    Array.IndexOf(processesLibrary.Processes(), process.ProcessName) == -1 && // Global processes list
                                    !processList.IndexOf(process.ProcessName)) // User processes list
                                    if (!kill) process.CloseMainWindow(); else process.Kill();
                            }
                            finally { ++progress; }
                        }

                        // If ForceKill is True, then...
                        if (Manual || ForceKill)
                        {
                            kill = true;
                            Thread.Sleep(5);
                        }
                        else
                            break;
                    }
                }
            }
            catch (Exception ex) { Debug.Save("Optimize.Class", "Start()", "Kill & Force Kill", ex.Message); }


            /***************************
             * Graphic optimize
             * *************************/
            try
            {
                if (Manual)
                {
                    ++maxProgress;
                    Graphic();
                    ++progress;
                }
            }
            catch (Exception ex) { Debug.Save("Optimize.Class", "Start()", "Graphic optimize", ex.Message); }
        }

        private void Sleep(int sec = 5)
        {
            try { for (int i = 0; i < sec; i++) Thread.Sleep(5000); }
            catch (Exception ex) { Debug.Save("Optimize.Class", "Sleep()", "Timeout is " + sec.ToString() + " seconds", ex.Message); }
        }

        private void Graphic(bool commonTest=false, bool weak = false)
        {
            try
            {
                string pathPreferences = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+
                    @"\Wargaming.net\WorldOfTanks\preferences" + (commonTest ? "_ct" : "") + ".xml";

                if (File.Exists(pathPreferences))
                {
                    XDocument docPref = XDocument.Load(pathPreferences);

                    if (docPref.Root.Element("graphicsPreferences") != null)
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
                        }

                    if (docPref.Root.Element("devicePreferences").Element("waitVSync") != null)
                        if (docPref.Root.Element("devicePreferences").Element("waitVSync").Value == "	false	")
                        {
                            if (docPref.Root.Element("devicePreferences").Element("tripleBuffering") != null)
                                docPref.Root.Element("devicePreferences").Element("tripleBuffering").SetValue("	false	");
                            else
                                docPref.Root.Element("devicePreferences").Add(new XElement("tripleBuffering", "	false	"));
                        }

                    if (docPref.Root.Element("devicePreferences").Element("customAAMode") != null)
                        docPref.Root.Element("devicePreferences").Element("customAAMode").SetValue("	0	");
                    else
                        docPref.Root.Element("devicePreferences").Add(new XElement("customAAMode", "	0	"));

                    if (docPref.Root.Element("devicePreferences").Element("drrScale") != null)
                        docPref.Root.Element("devicePreferences").Element("drrScale").SetValue("	0.850000	");
                    else
                        docPref.Root.Element("devicePreferences").Add(new XElement("drrScale", "	0.850000	"));

                    if (docPref.Root.Element("devicePreferences").Element("windowed") != null)
                        docPref.Root.Element("devicePreferences").Element("windowed").SetValue("	false	");
                    else
                        docPref.Root.Element("devicePreferences").Add(new XElement("windowed", "	false	"));

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
                            docPref.Root.Element("scriptsPreferences").Element("loginPage").Element("showLoginWallpaper").SetValue(weak ? "	false	" : "	true	");

                    if (weak && docPref.Root.Element("devicePreferences") != null)
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
            catch (Exception ex) { Debug.Save("Optimize.Class", "Graphic()", ex.Message); }
        }
    }
}
