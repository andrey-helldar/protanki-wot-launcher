using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModpackCreatorPROTanki
{
    public class ModpackCreator
    {
        public string multipack_youtube = "PROTankiWoT";

        // Конфиг
        public bool IsJSON = false;

        private string configJSON = "config.json";
        private string configINI = "config.ini";

        public string IniSection = "protanki";

        // Тип мультипака
        public string multipack_type = "base";
        public string multipack_version = "0";
        public string multipack_path = "0.0.0";
        public string multipack_date = "1.1.1970";
        public string multipack_language = "ru";

        // Ссылки
        public string multipack_link_video_all = "http://goo.gl/LXaU7T";
        public string multipack_link_updates = "http://file.theaces.ru/mods/proupdate/update.json";

        public JObject Config()
        {
            try
            {
                if (IsJSON)
                {
                    if (File.Exists("config.json"))
                    {
                        using (StreamReader file = File.OpenText("config.json"))
                        using (JsonTextReader reader = new JsonTextReader(file))
                            return (JObject)JToken.ReadFrom(reader);
                    }
                }
                else
                {
                    return new JObject(
                        new JProperty("type", new IniFile(configINI).IniReadValue(IniSection, "type").ToLower()),
                        new JProperty("version", new IniFile(configINI).IniReadValue(IniSection, "path") + "." + new IniFile(configINI).IniReadValue(IniSection, "version")),
                        new JProperty("date", new IniFile(configINI).IniReadValue(IniSection, "date")),
                        new JProperty("language", new IniFile(configINI).IniReadValue(IniSection, "language"))
                    );
                }
            }
            catch (Exception) { }

            return null;
        }
    }
}
