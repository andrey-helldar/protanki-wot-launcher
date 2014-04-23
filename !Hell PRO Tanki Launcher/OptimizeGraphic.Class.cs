using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace _Hell_PRO_Tanki_Launcher
{
    class OptimizeGraphic
    {
        public void Optimize(bool commonTest = false, bool autoVideo = false, bool autoWeak = false)
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
                                //case "RENDER_PIPELINE": el.Element("activeOption").SetValue(autoWeak ? "	1	" : "	0	"); break;
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
                                /*case "SHADOWS_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	4	" : "	2	"); break;
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
                                case "MOTION_BLUR_QUALITY": el.Element("activeOption").SetValue(autoWeak ? "	3	" : "	3	"); break;*/
                                default: break;
                            }
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

               /* if (docPref.Root.Element("graphicsPreferences").Element("graphicsSettingsVersion") != null)
                    docPref.Root.Element("graphicsPreferences").Element("graphicsSettingsVersion").SetValue("	0	");
                else
                    docPref.Root.Element("graphicsPreferences").Add(new XElement("graphicsSettingsVersion", "	0	"));

                if (docPref.Root.Element("graphicsPreferences").Element("graphicsSettingsVersionMinor") != null)
                    docPref.Root.Element("graphicsPreferences").Element("graphicsSettingsVersionMinor").SetValue("	2	");
                else
                    docPref.Root.Element("graphicsPreferences").Add(new XElement("graphicsSettingsVersionMinor", "	2	"));*/

                if (docPref.Root.Element("scriptsPreferences").Element("replayPrefs") != null)
                    if (docPref.Root.Element("scriptsPreferences").Element("replayPrefs").Element("fpsPerfomancer") != null)
                        docPref.Root.Element("scriptsPreferences").Element("replayPrefs").Element("fpsPerfomancer").SetValue("	STAKLg=	");

                if (docPref.Root.Element("scriptsPreferences").Element("fov") != null)
                    docPref.Root.Element("scriptsPreferences").Element("fov").SetValue("	85.000000	");

                if (docPref.Root.Element("scriptsPreferences").Element("loginPage") != null)
                    if (docPref.Root.Element("scriptsPreferences").Element("loginPage").Element("showLoginWallpaper") != null)
                        docPref.Root.Element("scriptsPreferences").Element("loginPage").Element("showLoginWallpaper").SetValue(autoWeak ? "	false	" : "	true	");

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

                /********************************************
                 * Распаковываем архив с улучшениями графики
                 * *****************************************/
                UnzipResMods().Wait();
            }
        }

        private async Task UnzipResMods()
        {
            try
            {
                /* ********************************************
                 *  http://goo.gl/xLwuLq
                 *  ******************************************/
                if (!Directory.Exists("temp")) Directory.CreateDirectory("temp");
                else if (File.Exists(@"temp\res_mods.zip")) File.Delete(@"temp\res_mods.zip");

                File.WriteAllBytes(@"temp\res_mods.zip", Properties.Resources.res_mods);

                using (ZipFile zip = ZipFile.Read(@"temp\res_mods.zip"))
                {
                    try
                    {
                        zip.ExtractAll(@"..\", ExtractExistingFileAction.OverwriteSilently);
                        zip.Dispose();
                    }
                    catch (Exception)
                    {
                        // Если возникла ошибка (обычно если файлы *.tmp найдены),
                        // то удаляем их и запускаем заново функцию
                        foreach (string file in Directory.GetFiles(@"..\res_mods\", "*.tmp", SearchOption.AllDirectories))
                            try { if (File.Exists(file)) File.Delete(file); }
                            finally { }

                        foreach (string file in Directory.GetFiles(@"..\res_mods\", "*.PendingOverwrite", SearchOption.AllDirectories))
                            try { if (File.Exists(file)) File.Delete(file); }
                            finally { }

                        // И перезапускаем функцию
                        UnzipResMods().Wait();                        
                    }
                }

                if (File.Exists(@"temp\res_mods.zip")) File.Delete(@"temp\res_mods.zip");
            }
            catch (Exception ex)
            {
                new Debug().Save("OptimizeGraphic", "UnzipResMods()", ex.Message);
            }
        }
    }
}
