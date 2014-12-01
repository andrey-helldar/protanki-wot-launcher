using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Modpack
{
    public class Creator
    {
        public readonly string multipack_id = "05b877de3562048c5d1bd7cc18d4f286";

        private readonly string IniSection = "protanki";

        // Ссылки
        public readonly string multipack_link_video_all = "http://goo.gl/LXaU7T";
        public readonly string multipack_link_updates = "http://file.theaces.ru/mods/proupdate/update.json";

        public JObject Config(string developer_uri)
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
                // Получаем данные с сервера
                //Response(developer_uri);

                // Обрабатываем файл конфига
                if (File.Exists("config.json"))
                {
                    using (StreamReader file = File.OpenText("config.json"))
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject obj = (JObject)JToken.ReadFrom(reader);
                        obj["version"] = (string)obj["path"] + "." + (string)obj["version"];
                        return obj;
                    }
                }
                else if (File.Exists("config.ini"))
                {
                    string path = Directory.GetCurrentDirectory() + @"\config.ini";

                    return new JObject(
                        new JProperty("type", new IniFile(path).IniReadValue(IniSection, "type").ToLower()),
                        new JProperty("version", new IniFile(path).IniReadValue(IniSection, "path") + "." + new IniFile(path).IniReadValue(IniSection, "version")),
                        new JProperty("path", new IniFile(path).IniReadValue(IniSection, "path")),
                        new JProperty("date", new IniFile(path).IniReadValue(IniSection, "date")),
                        new JProperty("language", new IniFile(path).IniReadValue(IniSection, "language"))
                    );
                }
                else return null;
            }
            catch (Exception) { }

            return null;
        }

        /*private JObject Response(string uri)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                int count = 0;
                do
                {
                    count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0) sb.Append(Encoding.UTF8.GetString(buf, 0, count));
                }
                while (count > 0);

                return JObject.Parse(sb.ToString());
            }
            catch (WebException) { }
            catch (Exception) { }

            return null;
        }*/
    }
}
