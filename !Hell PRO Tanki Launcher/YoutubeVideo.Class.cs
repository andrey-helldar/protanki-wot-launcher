using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Hell_PRO_Tanki_Launcher
{
    public class VideoLoading
    {
        public string ID, Title, Content, Link;
        public VideoLoading(string mProcess, string mDescription, string mContent, string mLink) { ID = mProcess; Title = mDescription; Content = mContent; Link = mLink; }
        public VideoLoading() { ID = ""; Title = ""; Content = ""; Link = ""; }
    }

    public class YoutubeVideo
    {
        public string ID, Title, Content, Link;
        public List<VideoLoading> List;
        private List<List<VideoLoading>> subList;

        public int Add(string id, string title, string content, string link)
        {
            try { if (List.Count <= 0) { } }
            catch (Exception)
            {
                List = new List<VideoLoading>();
                subList = new List<List<VideoLoading>>();
            }

            List.Add(new VideoLoading(ID, Title, Content, Link));
            subList.Add(List);

            ID = subList[0][0].ID;
            Title = subList[0][0].Title;
            Content = subList[0][0].Content;
            Link = subList[0][0].Link;

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
