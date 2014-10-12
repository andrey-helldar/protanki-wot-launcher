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
                Task.WaitAll(new Task[]{
                    Task.Factory.StartNew(() => SavingFile("restart.exe", Properties.Resources.restart)),
                    Task.Factory.StartNew(() => SavingFile("Processes.Library.dll", Properties.Resources.Processes_Library)),
                    
                    Task.Factory.StartNew(() => SavingFile("Ionic.Zip.dll", Properties.Resources.Ionic_Zip)),
                    Task.Factory.StartNew(() => SavingFile("Newtonsoft.Json.dll", Properties.Resources.Newtonsoft_Json)),
                    Task.Factory.StartNew(() => SavingFile("Ookii.Dialogs.Wpf.dll", Properties.Resources.Ookii_Dialogs_Wpf)),

                    Task.Factory.StartNew(() => {
                        string Settings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.xml";
                        if (!File.Exists(Settings)) { File.WriteAllText(Settings, Properties.Resources.SettingsXML); }
                    })
                });

                return true;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => new Debug().Save("Update.Class", "SaveFromResources()", ex.Message, ex.StackTrace));
                return false;
            }
        }

        private void SavingFile(string filename, byte[] resource)
        {
            try { if (!File.Exists(filename)) File.WriteAllBytes(filename, resource);}
            catch (Exception) { }
        }
    }
}
