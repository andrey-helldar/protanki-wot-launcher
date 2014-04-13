using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace _Hell_PRO_Tanki_Launcher
{
    class OptimizeGraphic
    {
        public void Optimize(bool commonTest = false, bool autoVideo=false, bool autoWeak = false)
        {
            if (autoVideo)
            {
                string pathPref = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\preferences" + (commonTest ? "_ct" : "") + ".xml";
                XDocument docPref = XDocument.Load(pathPref);

                if (docPref.Root.Element("graphicsPreferences") != null)
                    foreach (XElement el in docPref.Root.Element("graphicsPreferences").Elements("entry"))
                    {
                        if (el.Element("label") != null)
                            switch (el.Element("label").Value.Trim())
                            {
                                case "RENDER_PIPELINE": el.Element("activeOption").SetValue(autoWeak ? "	1	" : "	0	"); break;
                                case "SHADOWS_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	4	" : "	2	"); break;
                                case "DECALS_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	4	" : "	2	"); break;
                                case "LIGHTING_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	4	" : "	2	"); break;
                                case "TEXTURE_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	3	" : "	2	"); break;
                                case "TERRAIN_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	4	" : "	2	"); break;
                                case "SPEEDTREE_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	3	" : "	2	"); break;
                                case "WATER_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	3	" : "	2	"); break;
                                case "FAR_PLANE": el.Element("activeOption").SetValue(autoWeak ? "	3	" : "	2	"); break;
                                case "FLORA_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	4	" : "	2	"); break;
                                case "OBJECT_LOD": el.Element("activeOption").SetValue(autoWeak ? "	3	" : "	2	"); break;
                                case "VEHICLE_DUST_ENABLED": el.Element("activeOption").SetValue(autoWeak ? "	0	" : "	0	"); break;
                                case "VEHICLE_TRACES_ENABLED": el.Element("activeOption").SetValue(autoWeak ? "	0	" : "	0	"); break;
                                case "SMOKE_ENABLED": el.Element("activeOption").SetValue(autoWeak ? "	0	" : "	0	"); break;
                                case "SNIPER_MODE_EFFECTS_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	3	" : "	2	"); break;
                                case "PS_USE_PERFORMANCER": el.Element("activeOption").SetValue(autoWeak ? "	0	" : "	0	"); break;
                                case "EFFECTS_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	4	" : "	2	"); break;
                                case "SNIPER_MODE_GRASS_ENABLED": el.Element("activeOption").SetValue(autoWeak ? "	0	" : "	0	"); break;
                                case "POST_PROCESSING_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	1	" : "	2	"); break;
                                case "MOTION_BLUR_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	3	" : "	3	"); break;
                                default: break;
                            }
                    }

                if (docPref.Root.Element("devicePreferences").Element("windowed") != null)
                    docPref.Root.Element("devicePreferences").Element("windowed").SetValue("	false	");

                if (docPref.Root.Element("devicePreferences").Element("waitVSync") != null)
                    docPref.Root.Element("devicePreferences").Element("waitVSync").SetValue("	false	");

                if (docPref.Root.Element("graphicsPreferences").Element("graphicsSettingsVersion") != null)
                    docPref.Root.Element("graphicsPreferences").Element("graphicsSettingsVersion").SetValue(autoWeak ? "	4	" : "	0	");

                if (docPref.Root.Element("graphicsPreferences").Element("graphicsSettingsVersionMinor") != null)
                    docPref.Root.Element("graphicsPreferences").Element("graphicsSettingsVersionMinor").SetValue(autoWeak ? "	2	" : "	0	");

                if (docPref.Root.Element("devicePreferences").Element("customAAMode") != null)
                    docPref.Root.Element("devicePreferences").Element("customAAMode").SetValue(autoWeak ? "	1	" : "	0	");

                if (docPref.Root.Element("devicePreferences").Element("drrScale") != null)
                    docPref.Root.Element("devicePreferences").Element("drrScale").SetValue(autoWeak ? "	0.500000	" : "	0.900000	");

                if (docPref.Root.Element("scriptsPreferences").Element("replayPrefs") != null)
                    if (docPref.Root.Element("scriptsPreferences").Element("replayPrefs").Element("fpsPerfomancer") != null)
                        docPref.Root.Element("scriptsPreferences").Element("replayPrefs").Element("fpsPerfomancer").SetValue(autoWeak ? "	STAwCi4=	" : "	STAKLg==	");

                if (docPref.Root.Element("scriptsPreferences").Element("fov") != null)
                    docPref.Root.Element("scriptsPreferences").Element("fov").SetValue(autoWeak ? "	70.000000	" : "	90.000000	");

                if (docPref.Root.Element("scriptsPreferences").Element("loginPage") != null)
                    if (docPref.Root.Element("scriptsPreferences").Element("loginPage").Element("showLoginWallpaper") != null)
                        docPref.Root.Element("scriptsPreferences").Element("loginPage").Element("showLoginWallpaper").SetValue(autoWeak ? "	false	" : "	true	");

                if (docPref.Root.Element("scriptsPreferences").Element("replayPrefs") != null)
                    if (docPref.Root.Element("scriptsPreferences").Element("replayPrefs").Element("fpsPerfomancer") != null)
                        docPref.Root.Element("scriptsPreferences").Element("replayPrefs").Element("fpsPerfomancer").SetValue(autoWeak ? "	STAKLg==	" : "	STAKLg==	");

                if (autoWeak && docPref.Root.Element("devicePreferences") != null)
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

                docPref.Save(pathPref);

                StreamReader sr = new StreamReader(pathPref);
                string content = sr.ReadToEnd();
                sr.Close();

                content = content.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine, "");

                StreamWriter sw = new StreamWriter(pathPref);
                sw.Write(content);
                sw.Close();
            }
        }
    }
}
