using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Processes_Library
{
    public class Range
    {
        public string Name, Description;
        public Range(string mName, string mDescription) { Name = mName; Description = mDescription; }
        public Range() { Name = ""; Description = ""; }
    }

    public class ProcessList
    {
        private static string code = "TIjgwJYQyUyC2E3BRBzKKdy54C37dqfYjyInFbfMeYed0CacylTK3RtGaedTHRC6";

        public string mName, mDescription;
        public List<Range> List;
        public List<List<Range>> subRange;

        public void Add(string name, string description)
        {
            try
            {
                if (List.Count <= 0) { }
            }
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
                {
                    if (pr.Name == str) return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Count()
        {
            try
            {
                return List.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<string> Send(string json)
        {
            try
            {
                return POST("http://ai-rus.com/wot/processes/", "data=" + json);
            }
            catch (Exception)
            {
                return "FAIL";
            }
        }

        private static string POST(string Url, string Data)
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
    }

    public class ProcessesLibrary
    {
        public Array Processes()
        {
            string[] vipProcess = {
                Process.GetCurrentProcess().ProcessName.ToString(),
                "Multipack Launcher.vshost",
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
                "s",
                "s.bat",
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
                "FlashPlayerPlugin_13_0_0_182",
                "NetSph",
                "WiFi GO! Server",
                "WiFileTransfer",
                "breakaway",
                "SoundMAX",
                "cis",
                "cistray"
            };

            return vipProcess;
        }
    }
}
