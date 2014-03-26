using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Processes_Library
{
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
				"AI Suite II",
				"AsRoutineController",
				"avpui",
				"chrome",
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
				"taskeng",
				"taskhost",
				"TeamViewer",
				"TOTALCMD64.EXE",
				"TOTALCMD.EXE",
				"TOTALCMD64",
				"TOTALCMD",
				"tv_w32",
				"tv_x64",
				"U3BoostSvr64",
				"U3BoostSvr32",
				"winlogon",
				"xPopups",
				"xStarter",
				"xStartUI",
                "RaidCall"
            };

            return vipProcess;
        }
    }
}
