using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Processes_Library
{
    public class pRange
    {
        public string Process, Description;
        public pRange(string mProcess, string mDescription) { Process = mProcess; Description = mDescription; }
        public pRange() { Process = ""; Description = ""; }
    }

    public class ProcessList
    {
        public string Process, Description;
        public List<pRange> Range;
        public List<List<pRange>> subRange;

        public void Add(string process, string description)
        {
            try
            {
                if (Range.Count <= 0) { }
            }
            catch (Exception)
            {
                Range = new List<pRange>();
                subRange = new List<List<pRange>>();
            }

            Range.Add(new pRange(process, description));
            subRange.Add(Range);

            Process = subRange[0][0].Process;
            Description = subRange[0][0].Description;
        }

        public List<pRange> View()
        {
            return Range;
        }

        public void Clear()
        {
            try
            {
                Range.Clear();
            }
            catch (Exception) { }
        }

        public int IndexOf(string str)
        {
            try
            {
                if (Range.Count > -1)
                {
                    for (int i = 0; i < Range.Count; i++)
                    {
                        if (Range[i].Process == str)
                        {
                            return i;
                        }
                    }
                    return -1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Count()
        {
            try
            {
                return Range.Count;
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
				"klwtblfs",
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
                "egui"
            };

            return vipProcess;
        }
    }
}
