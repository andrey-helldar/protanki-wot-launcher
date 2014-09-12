using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Processes
{
    public class Range
    {
        public string Name, Description;
        public Range(string mName, string mDescription) { Name = mName; Description = mDescription; }
        public Range() { Name = ""; Description = ""; }
    }

    public class Listing
    {
        public string mName, mDescription;
        public List<Range> List;
        public List<List<Range>> subRange;

        public void Add(string name, string description)
        {
            try { if (List.Count <= 0) { } }
            catch (Exception)
            {
                List = new List<Range>();
                subRange = new List<List<Range>>();
            }

            List.Add(new Range(name, description));
            subRange.Add(List);

            mName = subRange[0][0].Name;
            mDescription = subRange[0][0].Description;
        }

        public List<Range> View()
        {
            return List;
        }

        public void Clear()
        {
            try
            {
                List.Clear();
            }
            catch (Exception) { }
        }

        public bool IndexOf(string str)
        {
            try
            {
                foreach (var pr in List)
                    if (pr.Name == str) return true;
                return false;
            }
            catch (Exception) { return false; }
        }

        public int Count()
        {
            try { return List.Count; }
            catch (Exception) { return 0; }
        }

        public string Send(string address, string json)
        {
            try { return POST(address, "data=" + json); }
            catch (Exception) { return "FAIL"; }
        }

        private static string POST(string Url, string Data)
        {
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                byte[] sentData = Encoding.GetEncoding("Utf-8").GetBytes(Data);
                req.ContentLength = sentData.Length;
                System.IO.Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                string Out = String.Empty;
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    Out += str;
                    count = sr.Read(read, 0, 256);
                }
                return Out;
            }
            catch (Exception) { return "FAIL"; }
        }
    }

    public class Global
    {
        public bool Search(string process)
        {
            try
            {
                foreach(string proc in Processes())
                    if (proc == process) return true;
            }
            catch (Exception) { return false; }

            return false;
        }

        public Array Processes()
        {
            string[] vipProcess = {
                Process.GetCurrentProcess().ProcessName.ToString(),
                "Multipack Launcher.vshost",
                "s",
                "s.bat",
				"restart",
				"WorldOfTanks",
				"WoTLauncher",

				"CCC",
				"atieclxx",
				"MOM",
				"csrss",
				"dwm",
				"MKey",
				"icq",
				"fraps",
				"AsRoutineController",
                "avp",
				"avpui",
				"conhost",
				"devenv",
				"explorer",
				"GitExtensions",
				"IntelliTrace",
				"iusb3mon",
				"ManagerCryptoFS",
				"NetiCtrlTray",
				"NetSvcHelp",
				"NvBackend",
                "NvXDSync",
				"nvstreamsvc",
				"NvTmru",
				"nvtray",
				"nvvsvc",
				"nvxdsync",
				"plugin-nm-server",
				"qip",
				"RtkNGUI64",
				"Skype",
                "Skype ",
				"taskeng",
				"taskhost",
				"U3BoostSvr64",
				"U3BoostSvr32",
				"winlogon",
				"xPopups",
				"xStarter",
				"xStartUI",
                "RaidCall",
                "raidcall",
                "Overwolf",
                "magent",
                "USBGuard",
                "YCMMirage",
                "ccApp",
                "SmcGui",
                "AI Suite II",
                "ICQ",
                "cmd",
                "ts3client_win32",
                "ts3client_win64",
                "FamItrfc",
                "NetMap",
                "Cloud",
                "YandexDisk",
                "Dropbox",
                "RazerImperatorSysTray",
                "Volume2",
                "egui",
                "AvastUI",
                "avastui",
                "WoTLogger3",
                "InputPersonalization",
                "TOTALCMD",
                "TOTALCMD32",
                "TOTALCMD64",
                "AIMP3",
                "winamp",
                "DTShellHlp",
                "Viber",
                "AllShareAgent",
                "HydraDM",
                "ScreenCapture",
                "jusched",
                "QuickGesture",
                "avgnt",
                "avscan",
                "RtHDVCpl",
                "Adguard",
                "CoolSense",
                "AirGCFG",
                "RazerNagaSysTray",
                "ccSvcHst",
                "WWAHost",
                "msseces",
                "HPOSD",
                "avgui",
                "MSIAfterburner",
                "RzSynapse",
                "mbamgui",
                "HPMSGSVC",
                "htcUPCTLoader",
                "main",
                "dllhost",
                "drwagnui",
                "VDeck",
                "RtWLan",
                "nusb3mon",
                "AnVir",
                "Mouse",
                "nis",
                "RaUI",
                "rundll32",
                "wcourier",
                "VolCtrl",
                "BtvStack",
                "DCSHelper",
                "GPUTweak",
                "NetSph",
                "WiFi GO! Server",
                "WiFileTransfer",
                "breakaway",
                "SoundMAX",
                "cis",
                "cistray",
"1cv7l",
"1cv8",
"1cv8c",
"360rp",
"360sd",
"360sdupd",
"360Tray",
"360webshield",
"3G HSDPA Modem",
"3G Internet",
"3G life modem",
"3G Modem Manager",
"3yuar0ib",
"AAWTray",
"ACFanControl",
"ACU",
"ACWLIcon",
"Ad-Aware",
"Ad-AwareAdmin",
"adawarebp",
"AdAwareTray",
"Adblock",
"AdBlock.Agent",
"ADBlockerTray",
"Adguard",
"AdMunch",
"admunch",
"AdMunch64",
"AdMuncherUpdater",
"AdStopperTrayApp",
"ah",
"AIMP2",
"AIMP2Portable",
"AIMP3lib",
"Air Mouse",
"AirDisplay",
"AirGCFG",
"AirNCFG",
"ANTIVIRЬ",
"AnVir",
"anvir64",
"AnVir64",
"ApUI",
"ApVxdWin",
"AsFnControl",
"ashDisp",
"ASUS Console Starter",
"ASUS Docking",
"AsusAudioCenter",
"ASUSManager",
"AsusTPCenter",
"AsusTPHelper",
"AsusTPLauncher",
"AsusTPLoader",
"asusUPCTLoader",
"AsusWSPanel",
"AsusWSService",
"ASUSxGPU-Z",
"Ati2evxx",
"atibtmon",
"atidimsvc",
"atitray",
"avast.setup",
"avastui",
"avcenter",
"avciman",
"avcom",
"avgcfgex",
"AVGIDSMonitor",
"avgmfapx",
"avgnt",
"avgtray",
"avgui",
"avg_tuht_stf_all_2014_423",
"Avira.OE.Systray",
"avira_system_speedup",
"AVKTray",
"AvP",
"Beeline Helper",
"Beeline Home Internet",
"Beeline Internet at Home",
"Black",
"Boingo Wi-Fi",
"Boost",
"boostspeed",
"BoostSpeed",
"BoostSpeedPortable",
"BOOSTS~1",
"cAIMP",
"CCP",
"ccSvcHst",
"ccsvchst",
"cfp",
"cis",
"cistray",
"CisTray",
"clamscan",
"ClamTray",
"CLI",
"Client",
"client3",
"CLIStart",
"cmd64",
"cmmon32",
"ComUpdatus",
"ControlCenter",
"ControlCenter_Side",
"cooling",
"Core Temp",
"CORE TEMP",
"Core Temp_x64",
"CoreTemp64",
"CpuLevelUpHook32",
"CpuLevelUpHook64",
"CpuLevelUpHookLaunch",
"CpuPowerMonitor",
"DeLay",
"dinotify",
"DM2",
"DoScan",
"dragon",
"DriverAP",
"drwagnui",
"DRWAGNUI",
"DWHWizrd",
"DWRCC",
"DWRCST",
"dwscanner",
"eeclnt",
"evaer",
"FamItrf2",
"fancontroller",
"FanHelp",
"frwl_notify",
"FRWL_NOTIFY",
"fshoster32",
"FSM32",
"gbtray",
"GdBgInx64",
"GeForce_Experience_v2.1.0.0",
"GFExperience",
"H3GMS",
"H760",
"H800",
"HDAudioCPL",
"Hid",
"hkcmd",
"Hotkey",
"hotkey",
"HotkeyApp",
"HotKeyb",
"Hotkeycontrol",
"HotKeyOSD",
"HotkeyUtility",
"HPKEYBOARDx",
"hpMonitor21",
"hpMonitor23",
"HPMSGSVC",
"HPWAMain",
"HPWA_Main",
"HSPA USB MODEM",
"HSSCP",
"HUAWEI Modem",
"HUAWEI Modem 2.0",
"HUAWEI Modem Fon",
"HUAWEI Modem Live",
"HUAWEI Modem Micro",
"HUAWEI Modem Mini",
"HUAWEI Modem Plus",
"Huawei3GModemSoftware",
"HuaweiModemSoftware",
"HUD",
"IMF",
"IMhid",
"InstStub",
"KLM",
"KMCONFIG",
"kss",
"kxetray",
"LachesisSysTray",
"LAN Manager",
"MacsFanControl",
"mbae",
"mbam",
"mbamgui",
"mcagent",
"McPvTray",
"McTray",
"McUICnt",
"mcupdate",
"mcupdui",
"McVsShld",
"Mouse",
"MouseCamera",
"MouseDrv",
"mousehid",
"MouseKeyboardCenterx64_1049",
"MouseServer",
"MouseTracer",
"MRT",
"mywifi",
"MzCPUAccelerator",
"MzGameAccelerator",
"MzRAMBooster",
"nanoav",
"NAT",
"nav",
"NAV",
"Net4Switch",
"NetAnimate",
"netsession_win",
"netsetman",
"NetSph",
"NetTraffic",
"netview",
"NetworkClient",
"NetworkGenie",
"NetworkIndicator",
"NetworkIndicator_RUS",
"NetworkManager",
"networx",
"NetWorx",
"nis",
"NIS",
"NOBuClient",
"nod32egui",
"nod32kernel.dat",
"nspmain",
"NST",
"nst",
"nto",
"nTuneCmd",
"nusb3mon",
"NVBACK~1",
"NvCplSetupInt",
"nvcplui",
"nvidiaInspector",
"NVIDIAOCAP",
"NvLedServiceHost",
"NVMonitor",
"nvraidservice",
"nvsmartmaxapp",
"nvspcaps",
"nvspcaps64",
"NVTray",
"nvwmi64",
"NWD2105",
"NWD2205",
"osk",
"OverclockingCenter",
"OverwolfBrowser",
"OverwolfHelper",
"OverwolfHelper64",
"OverwolfLauncher",
"OverwolfTeamSpeakInstaller",
"Panda_URL_Filtering",
"ProtectionUtilSurrogate",
"Rambooster",
"RaUI",
"RAVBg64",
"RAVCpl64",
"Razer Barracuda AC-1 Gaming Audio card",
"RazerAnansiSysTray",
"RazerCore",
"razerhid",
"RazerImperatorTray",
"RazerIngameEngine",
"RazerMambaSysTray",
"RazerNagaSysTray",
"RazerNostromoSysTray",
"razerofa",
"RazerOrochiTray",
"RazerStarCraftIISysTray",
"razertra",
"rbmonitor",
"RBTray",
"RCHotKey",
"RDVCHG",
"RtDCpl64",
"RTFTrack",
"RTHDCPL",
"RtHDVBg",
"RtHDVCpl",
"RtkBtMnt",
"RtkNGUI",
"RtVOsd",
"RTVOSD64",
"RtWLan",
"RtWlan",
"rundll32",
"RunUSBGuard",
"rusb3mon",
"Ryos MK Monitor",
"RzCefRenderProcess",
"RzCommsInGameApplet",
"RzSynapse",
"SecGu",
"SecureAPlus",
"SecureCRT",
"SecureEraseDropAgent",
"Skype.exe.exe",
"SkypePlugin",
"skypePM",
"SkypePortable",
"SOUNDMAN",
"soundman",
"SoundMAX",
"SoundMAX",
"soundrec",
"SoundTray",
"SoundWireServer",
"SpywareTerminatorShield",
"SpywareTerminatorUpdate",
"sqlmangr",
"sqlservr",
"stpass",
"ts3server_win32",
"ts3server_win64",
"TUSBAudioCpl",
"TWCU",
"wcourier",
"WiFi GO! Server",
"WiFi HotSpot Creator",
"WiFiCreator",
"WiFiGuard",
"WiFileTransfer",
"WifiManager",
"WiFiMsg",
"WiMAXCU",
"Wireless",
"Wireless Broadband",
"wirelesscm",
"WirelessManager",
"WirelessModem",
"WiseGameBooster",
"WiseMemoryOptimzer",
"WiseRegCleaner",
"WiseTray",
"WLAN Optimizer",
"WLanGUI",
"WlanMgr",
"WlanUtility",
"WLAN_Service"
            };

            return vipProcess;
        }
    }
}
