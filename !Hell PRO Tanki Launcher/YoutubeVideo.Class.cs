using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Hell_PRO_Tanki_Launcher
{
    public class VideoLoading
    {
        public string ID, Title, Content, Link, Date;
        public VideoLoading(string mProcess, string mDescription, string mContent, string mLink, string mDate) { ID = mProcess; Title = mDescription; Content = mContent; Link = mLink; Date = mDate; }
        public VideoLoading() { ID = ""; Title = ""; Content = ""; Link = ""; Date = ""; }
    }

    public class YoutubeVideo
    {
        public string ID, Title, Content, Link,Date;
        public List<VideoLoading> List;
        public List<List<VideoLoading>> Range;

        public int Add(string id, string title, string content, string link, string date)
        {
            try { if (List.Count <= 0) { } }
            catch (Exception)
            {
                List = new List<VideoLoading>();
                Range = new List<List<VideoLoading>>();
            }

            List.Add(new VideoLoading(ID, Title, Content, Link, Date));
            Range.Add(List);

            ID = Range[0][0].ID;
            Title = Range[0][0].Title;
            Content = Range[0][0].Content;
            Link = Range[0][0].Link;
            Date = Range[0][0].Date;

            return IndexOf(ID);
        }

        public void Clear()
        {
            if (List.Count > 0) List.Clear();
        }

        public int IndexOf(string str)
        {
            try { if (List.Count > -1) { for (int i = 0; i < List.Count; i++) { if (List[i].ID == str) { return i; } } } return -1; }
            catch (Exception) { return -1; }
        }

        public int Count()
        {
            return List.Count;
        }

        public void Delete(string id)
        {
            if (List.Count > -1)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    if (List[i].ID == id) { List.RemoveRange(i, 0); break; }
                }
            }
        }
    }
}
