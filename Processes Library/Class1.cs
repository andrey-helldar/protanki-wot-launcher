﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Processes_Library
{
    public class Range
    {
        public string Process, Description;
        public Range(string mProcess, string mDescription) { Process = mProcess; Description = mDescription; }
        public Range() { Process = ""; Description = ""; }
    }

    public class ProcessList
    {
        public string mProcess, mDescription;
        public List<Range> Range;
        public List<List<Range>> subRange;

        public void Add(string process, string description)
        {
            try
            {
                if (Range.Count <= 0) { }
            }
            catch (Exception)
            {
                Range = new List<Range>();
                subRange = new List<List<Range>>();
            }

            Range.Add(new Range(process, description));
            subRange.Add(Range);

            Process = subRange[0][0].Process;
            Description = subRange[0][0].Description;
        }

        public List<Range> View()
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

        public bool IndexOf(string str)
        {
            try
            {
                foreach(var pr in Range){
                    if (pr.Process == str) return true;
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
                "TOTALCMD64"
            };

            return vipProcess;
        }
    }
}
