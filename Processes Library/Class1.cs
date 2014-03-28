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

        public void Clear()
        {
            this.Range.Clear();
        }
    }

    public class ProcessesLibrary
    {
        public Array Processes()
        {
            string[] vipProcess = {
                Process.GetCurrentProcess().ProcessName.ToString(),
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
				"TeamViewer",
				"tv_w32",
				"tv_x64",
				"U3BoostSvr64",
				"U3BoostSvr32",
				"winlogon",
				"xPopups",
				"xStarter",
				"xStartUI",
                "RaidCall",
                "Overwolf",
                "Cloud",
                "magent",
                "USBGuard",
                "YCMMirage"
            };

            return vipProcess;
        }
    }
}
