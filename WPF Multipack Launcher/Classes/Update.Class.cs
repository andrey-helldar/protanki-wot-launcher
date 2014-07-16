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
        public async Task SaveFromResources()
        {
            try
            {
                if (!File.Exists("Ionic.Zip.dll")) { File.WriteAllBytes("Ionic.Zip.dll", Properties.Resources.Ionic_Zip); }
                if (!File.Exists("restart.exe")) { File.WriteAllBytes("restart.exe", Properties.Resources.restart); }
                if (!File.Exists("Newtonsoft.Json.dll")) { File.WriteAllBytes("Newtonsoft.Json.dll", Properties.Resources.Newtonsoft_Json); }
                if (!File.Exists("ProcessesLibrary.dll")) { File.WriteAllBytes("ProcessesLibrary.dll", Properties.Resources.ProcessesLibrary); }

                string Settings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";
                if (!File.Exists(Settings)) { File.WriteAllText(Settings, Properties.Resources.SettingsXML); }
            }
            catch (Exception ex) { new Debug().Save("UpdateLauncher", "SaveFromResources()", ex.Message); }
        }
    }
}
