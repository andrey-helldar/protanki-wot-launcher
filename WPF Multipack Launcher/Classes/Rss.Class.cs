using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Multipack_Launcher.Classes
{
    public class VideoLoading
    {
        public string ID, Title, Content, Link, Date;
        public VideoLoading(string mProcess, string mDescription, string mContent, string mLink, string mDate) { ID = mProcess; Title = mDescription; Content = mContent; Link = mLink; Date = mDate; }
        public VideoLoading() { ID = ""; Title = ""; Content = ""; Link = ""; Date = ""; }
    }

    public class YoutubeVideo
    {
        Debug Debug = new Debug();

        public string mID, mTitle, mContent, mLink, mDate;
        public List<VideoLoading> List;
        public List<List<VideoLoading>> Range;

        public void Add(string id, string title, string content, string link, string date)
        {
            try
            {
                try { if (List.Count <= 0) { } }
                catch (Exception)
                {
                    List = new List<VideoLoading>();
                    Range = new List<List<VideoLoading>>();
                }

                List.Add(new VideoLoading(id, title, content, link, date));
                Range.Add(List);

                mID = Range[0][0].ID;
                mTitle = Range[0][0].Title;
                mContent = Range[0][0].Content;
                mLink = Range[0][0].Link;
                mDate = Range[0][0].Date;
            }
            catch (Exception ex)
            {
                Debug.Save("RSS.Class", "Add()", ex.Message,
                    "ID: " + id,
                    "Title: " + title,
                    "Content: " + content,
                    "Link: " + link,
                    "Date: " + date);
            }
        }

        public void Clear()
        {
            try { if (List.Count > 0) List.Clear(); }
            catch (Exception ex) { Debug.Save("RSS.Class", "Clear()", ex.Message); }
        }

        public int IndexOf(string str)
        {
            try { if (List.Count > -1) { for (int i = 0; i < List.Count; i++) { if (List[i].ID == str) { return i; } } } return -1; }
            catch (Exception ex) { Debug.Save("RSS.Class", "IndexOf()", ex.Message, "Search: " + str); return -1; }
        }

        public int Count()
        {
            try { return List.Count; }
            catch (Exception ex) { Debug.Save("RSS.Class", "Count()", ex.Message); return 0; }
        }

        /// <summary>
        /// Запускаем процессрекурсии на удаление
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            try
            {
                if (List.Count > -1)
                    for (int i = 0; i < List.Count; i++)
                        if (List[i].ID == id) { List.RemoveAt(i); }
            }
            catch (Exception ex) { Debug.Save("RSS.Class", "Count(id = " + id + ")", ex.Message); Delete(id); }
        }
    }
}
