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
            // Проверяем существование резервной копии файла настроек
            string backupSettings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";
            if (!File.Exists("settings.xml") && File.Exists(backupSettings)) { File.Copy(backupSettings, "settings.xml", true); }

            if (!File.Exists("Ionic.Zip.dll")) { File.WriteAllBytes("Ionic.Zip.dll", Properties.Resources.Ionic_Zip); }
            if (!File.Exists("restart.exe")) { File.WriteAllBytes("restart.exe", Properties.Resources.restart); }
            if (!File.Exists("Newtonsoft.Json.dll")) { File.WriteAllBytes("Newtonsoft.Json.dll", Properties.Resources.Newtonsoft_Json); }
            if (!File.Exists("ProcessesLibrary.dll")) { File.WriteAllBytes("ProcessesLibrary.dll", Properties.Resources.ProcessesLibrary); }
            if (!File.Exists("settings.xml")) { File.WriteAllText("settings.xml", Properties.Resources.SettingsXML); }
        }
    }
}
