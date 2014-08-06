using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    public class VideoLoading
    {
        public string ID, Title, Content, Link, Date, DateShort;
        public VideoLoading(string mProcess, string mDescription, string mContent, string mLink, string mDate, string mDateShort) { ID = mProcess; Title = mDescription; Content = mContent; Link = mLink; Date = mDate; DateShort = mDateShort; }
        public VideoLoading() { ID = ""; Title = ""; Content = ""; Link = ""; Date = ""; DateShort = ""; }
    }

    public class YoutubeVideo
    {
        Debug Debug = new Debug();

        public string mID, mTitle, mContent, mLink, mDate, mDateShort;
        public List<VideoLoading> List;
        public List<List<VideoLoading>> Range;

        public void Start()
        {
            try
            {
                XDocument doc = XDocument.Load(String.Format(Properties.Resources.RssYoutube, Properties.Resources.YoutubeChannel));
                XNamespace ns = "http://www.w3.org/2005/Atom";

                foreach (XElement el in doc.Root.Elements(ns + "entry"))
                {
                    if (!new Classes.Variables().ElementBan(el.Element(ns + "id").Value.Remove(0, 42).ToUpper()))
                    {
                        string link = "";
                        foreach (XElement subEl in el.Elements(ns + "link")) { if (subEl.Attribute("rel").Value == "alternate") { link = subEl.Attribute("href").Value; break; } }

                        Add(
                            el.Element(ns + "id").Value.Remove(0, 42),
                            (el.Element(ns + "title").Value.IndexOf(" / PRO") >= 0 ? el.Element(ns + "title").Value.Remove(el.Element(ns + "title").Value.IndexOf(" / PRO")) : el.Element(ns + "title").Value),
                            el.Element(ns + "content").Value.Remove(256) + (el.Element(ns + "content").Value.Length > 256 ? "..." : ""),
                            link,
                            el.Element(ns + "published").Value.Remove(10),
                            DateTime.Parse(el.Element(ns + "published").Value.Remove(10)).ToString("dd.MM"));
                    }
                }
            }
            catch (Exception ex) { Debug.Save("Youtube.Class", "Start()", ex.Message); }
        }

        public void Add(string id, string title, string content, string link, string date, string dateShort)
        {
            try
            {
                try { if (List.Count <= 0) { } }
                catch (Exception)
                {
                    List = new List<VideoLoading>();
                    Range = new List<List<VideoLoading>>();
                }

                List.Add(new VideoLoading(id, title, content, link, date, dateShort));
                Range.Add(List);

                mID = Range[0][0].ID;
                mTitle = Range[0][0].Title;
                mContent = Range[0][0].Content;
                mLink = Range[0][0].Link;
                mDate = Range[0][0].Date;
                mDateShort = Range[0][0].DateShort;
            }
            catch (Exception ex)
            {
                Debug.Save("Youtube.Class", "Add()", ex.Message,
                    "ID: " + id,
                    "Title: " + title,
                    "Content: " + content,
                    "Link: " + link,
                    "Date: " + date,
                    "Short Date: " + dateShort);
            }
        }

        public void Clear()
        {
            try { if (List.Count > 0) List.Clear(); }
            catch (Exception ex) { Debug.Save("Youtube.Class", "Clear()", ex.Message); }
        }

        public int IndexOf(string str)
        {
            try { if (List.Count > -1) { for (int i = 0; i < List.Count; i++) { if (List[i].ID == str) { return i; } } } return -1; }
            catch (Exception ex) { Debug.Save("Youtube.Class", "IndexOf()", ex.Message, "Search: " + str); return -1; }
        }

        public int Count()
        {
            try { return List.Count; }
            catch (Exception ex) { Debug.Save("Youtube.Class", "Count()", ex.Message); return 0; }
        }

        /// <summary>
        /// Запускаем процессрекурсии на удаление
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            try
            {
                if (List.Count > 0)
                {
                    for (int i = 0; i < List.Count; i++)
                        if (List[i].ID == id) { List.RemoveAt(i); }
                }
            }
            catch (Exception /*ex*/) { /*Debug.Save("Youtube.Class", "Count(id = " + id + ")", ex.Message);*/ /*Delete(id);*/ }

            try { List.TrimExcess(); }
            catch (Exception ex) { Debug.Save("Youtube.Class", "Delete()", "Function: TrimExcess()", ex.Message); }
        }


        /// <summary>
        /// Если дата старее даты выпуска модпака,
        /// то выводим в результат "false" как запрет на вывод.
        /// </summary>
        /// <param name="packDate">Дата выпуска мультипака</param>
        /// <param name="videoDate">Дата видео</param>
        /// <returns>Во всех иных случаях выводим "true", то есть дата валидная</returns>
        public bool CheckDate(string packDate = null, string videoDate = null)
        {
            try
            {
                if (packDate != null && videoDate != null)
                    if (DateTime.Parse(videoDate) < DateTime.Parse(packDate)) { return false; }
                return true;
            }
            catch (Exception ex) { Debug.Save("Youtube.Class", "ParseDate()", "Pack Date = " + packDate, "Video date = " + videoDate, ex.Message); return true; }
        }
    }
}
