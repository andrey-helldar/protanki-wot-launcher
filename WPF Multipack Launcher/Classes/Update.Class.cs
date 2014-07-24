using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WPF_Multipack_Launcher.Classes
{
    class Update
    {
        public void SaveFromResources()
        {
            try
            {
                if (!File.Exists("restart.exe")) { File.WriteAllBytes("restart.exe", Properties.Resources.restart); }
                if (!File.Exists("ProcessesLibrary.dll")) { File.WriteAllBytes("ProcessesLibrary.dll", Properties.Resources.ProcessesLibrary); }

                string Settings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";
                if (!File.Exists(Settings)) { File.WriteAllText(Settings, Properties.Resources.SettingsXML); }
            }
            catch (Exception ex) { new Debug().Save("Update.Class", "SaveFromResources()", ex.Message); }
        }
    }
}
