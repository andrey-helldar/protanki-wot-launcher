using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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
                "WoTLogger3",
                "InputPersonalization",
                "s",
                "s.bat",
                "TOTALCMD",
                "TOTALCMD32",
                "TOTALCMD64",
                "AIMP3",
                "winamp"
            };

            return vipProcess;
        }
    }
}
