﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    public class Loading
    {
        public string Title, Link, Date, DateShort;
        public Loading(string mTitle, string mLink, string mDate, string mDateShort) { Title = mTitle; Link = mLink; Date = mDate; DateShort = mDateShort; }
        public Loading() { Title = ""; Link = ""; Date = ""; DateShort = ""; }
    }

    public class Wargaming
    {
        Debug Debug = new Debug();

        private XDocument tmpDoc = new XDocument();

        public string mTitle, mLink, mDate, mDateShort;
        public List<Loading> List;
        public List<List<Loading>> Range;

        public void Start(XDocument XmlGeneral)
        {
            try
            {
                string lang = Properties.Resources.Default_Lang;

                if (XmlGeneral.Root.Element("info") != null)
                    if (XmlGeneral.Root.Element("info").Attribute("language") != null)
                        lang = XmlGeneral.Root.Element("info").Attribute("language").Value;

                XDocument doc = XDocument.Load(lang == "ru" ? Properties.Resources.RssWotRU : Properties.Resources.RssWotEn);

                tmpDoc = XmlGeneral;

                foreach (XElement el in doc.Root.Element("channel").Elements("item"))
                    if (!ElementBan(el.Element("link").Value))
                    {
                        Add(
                            el.Element("title").Value,
                            el.Element("link").Value,
                            el.Element("pubDate").Value,
                            DateTime.Parse(el.Element("pubDate").Value).ToString("dd.MM"));
                    }

                tmpDoc.Remove();
            }
            catch (Exception ex) { Task.Factory.StartNew(() => Debug.Save("Wargaming.News.Class", "Start()", ex.Message, XmlGeneral.ToString())); }
        }

        /// <summary>
        /// Проверка внесен ли элемент новости/видео в так называемый "черный список"
        /// </summary>
        /// <param name="item">Входящий идентификатор записи для проверки</param>
        /// <returns>
        ///     TRUE - запись находится в черном списке;
        ///     FALSE - запись "чистая"
        /// </returns>
        private bool ElementBan(string item)
        {
            if (tmpDoc.Root.Element("do_not_display") != null)
                if (tmpDoc.Root.Element("do_not_display").Element("news") != null)
                    if (tmpDoc.Root.Element("do_not_display").Element("news").Element("item") != null)
                        foreach (string str in tmpDoc.Root.Element("do_not_display").Element("news").Elements("item"))
                            if (str == item) return true;

            return false;
        }


        public void Add(string title, string link, string date, string dateShort)
        {
            try
            {
                try { if (List.Count <= 0) { } }
                catch (Exception)
                {
                    List = new List<Loading>();
                    Range = new List<List<Loading>>();
                }

                List.Add(new Loading(title, link, date, dateShort));
                Range.Add(List);

                mTitle = Range[0][0].Title;
                mLink = Range[0][0].Link;
                mDate = Range[0][0].Date;
                mDateShort = Range[0][0].DateShort;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => Debug.Save("Wargaming.News.Class", "Add()", ex.Message,
                    "Title: " + title,
                    "Link: " + link,
                    "Date: " + date,
                    "Date Short: " + dateShort));
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
