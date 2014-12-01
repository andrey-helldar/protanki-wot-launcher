using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Modpack
{
    public class Creator
    {
        public readonly string multipack_id = "05b877de3562048c5d1bd7cc18d4f286";
        public readonly string multipack_youtube = "PROTankiWoT";

        public readonly string IniSection = "protanki";

        // Ссылки
        public string multipack_link_video_all = "http://goo.gl/LXaU7T";
        public string multipack_link_updates = "http://file.theaces.ru/mods/proupdate/update.json";

        public JObject Config()
        {
            /*
             * [protanki]
             * type=extended
             * version=12
             * path=0.9.4
             * date=30.11.2014
             * language=ru
             */
            try
            {
                if (File.Exists("config.json"))
                {
                    using (StreamReader file = File.OpenText("config.json"))
                    using (JsonTextReader reader = new JsonTextReader(file))
                        return (JObject)JToken.ReadFrom(reader);
                }
                else
                {
                    return new JObject(
                        new JProperty("type", new IniFile("config.ini").IniReadValue(IniSection, "type").ToLower()),
                        new JProperty("version", new IniFile("config.ini").IniReadValue(IniSection, "path") + "." + new IniFile("config.ini").IniReadValue(IniSection, "version")),
                        new JProperty("date", new IniFile("config.ini").IniReadValue(IniSection, "date")),
                        new JProperty("language", new IniFile("config.ini").IniReadValue(IniSection, "language"))
                    );
                }
            }
            catch (Exception) { }

            return null;
        }
    }
}
