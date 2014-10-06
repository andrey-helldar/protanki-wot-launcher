using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Update
    {
        /// <summary>
        /// ИЗвлечение ресурсов, если оригинальные файлы не найдены
        /// </summary>
        /// <returns>TRUE при успешном извлечении, иначе - FALSE</returns>
        public bool SaveFromResources()
        {
            try
            {
                if (!File.Exists("restart.exe")) { File.WriteAllBytes("restart.exe", Properties.Resources.restart); }
                if (!File.Exists("ProcessesLibrary.dll")) { File.WriteAllBytes("Processes.Library.dll", Properties.Resources.Processes_Library); }

                if (!File.Exists("Ionic.Zip.dll")) { File.WriteAllBytes("Ionic.Zip.dll", Properties.Resources.Ionic_Zip); }
                if (!File.Exists("Newtonsoft.Json.dll")) { File.WriteAllBytes("Newtonsoft.Json.dll", Properties.Resources.Newtonsoft_Json); }
                if (!File.Exists("Ookii.Dialogs.Wpf.dll")) { File.WriteAllBytes("Ookii.Dialogs.Wpf.dll", Properties.Resources.Newtonsoft_Json); }

                string Settings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";
                if (!File.Exists(Settings)) { File.WriteAllText(Settings, Properties.Resources.SettingsXML); }

                return true;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => new Debug().Save("Update.Class", "SaveFromResources()", ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}
