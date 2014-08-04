using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    public class Loading
    {
        public string Title, Link, Date;
        public Loading(string mTitle, string mLink, string mDate) { Title = mTitle; Link = mLink; Date = mDate; }
        public Loading() { Title = ""; Link = ""; Date = ""; }
    }

    public class Wargaming
    {
        Debug Debug = new Debug();

        public string mTitle, mLink, mDate;
        public List<Loading> List;
        public List<List<Loading>> Range;

        public Task Start(string lang = "en")
        {
            try
            {
                XDocument doc = XDocument.Load(lang == "ru" ? Properties.Resources.RssWotRU : Properties.Resources.RssWotEn);

                foreach (XElement el in doc.Root.Element("channel").Elements("item"))
                    Add(el.Element("title").Value, el.Element("link").Value, el.Element("pubDate").Value);
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Wargaming.News.Class", "Start()", ex.Message)); }
        }

        public void Add(string title, string link, string date)
        {
            try
            {
                try { if (List.Count <= 0) { } }
                catch (Exception)
                {
                    List = new List<Loading>();
                    Range = new List<List<Loading>>();
                }

                List.Add(new Loading(title, link, date));
                Range.Add(List);

                mTitle = Range[0][0].Title;
                mLink = Range[0][0].Link;
                mDate = Range[0][0].Date;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("Wargaming.News.Class", "Add()", ex.Message,
                    "Title: " + title,
                    "Link: " + link,
                    "Date: " + date));
            }
        }

        public void Clear()
        {
            try { if (List.Count > 0) List.Clear(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Wargaming.News.Class", "Clear()", ex.Message)); }
        }

        public int IndexOf(string str)
        {
            try { if (List.Count > -1) { for (int i = 0; i < List.Count; i++) { if (List[i].Link == str) { return i; } } } return -1; }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Wargaming.News.Class", "IndexOf()", ex.Message, "Search: " + str)); return -1; }
        }

        public int Count()
        {
            try { return List.Count; }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Wargaming.News.Class", "Count()", ex.Message)); return 0; }
        }

        /// <summary>
        /// Запускаем процессрекурсии на удаление
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string link)
        {
            try
            {
                if (List.Count > 0)
                    for (int i = 0; i < List.Count; i++)
                        if (List[i].Link == link) { List.RemoveAt(i); }
            }
            catch (Exception /*ex*/) { /*Debug.Save("Wargaming.News.Class", "Count(id = " + id + ")", ex.Message);*/ /*Delete(id);*/ }

            try { List.TrimExcess(); }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Wargaming.News.Class", "Delete()", "Function: TrimExcess()", ex.Message)); }
        }
    }
}
