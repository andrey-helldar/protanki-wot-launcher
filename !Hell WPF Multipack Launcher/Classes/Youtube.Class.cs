using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    public class VideoLoading
    {
        public string ID, Title, Link, Date, DateShort;
        public VideoLoading(string mProcess, string mTitle, string mLink, string mDate, string mDateShort) { ID = mProcess; Title = mTitle; Link = mLink; Date = mDate; DateShort = mDateShort; }
        public VideoLoading() { ID = ""; Title = ""; Link = ""; Date = ""; DateShort = ""; }
    }

    public class YoutubeVideo
    {
        Debugging Debugging = new Debugging();

        public string mID, mTitle, mLink, mDate, mDateShort;
        public List<VideoLoading> List;
        public List<List<VideoLoading>> Range;

        public void Start()
        {
            try
            {
                // Получаем список видео через Youtube API v3
                Classes.POST POST = new Classes.POST();
                string channelID = (string)(POST.JsonResponse(String.Format(Properties.Resources.Youtube_Channels_List, Properties.Resources.Youtube_Channel, Properties.Resources.Youtube_API))).SelectToken("items[0].contentDetails.relatedPlaylists.uploads");
                JObject videosObj = POST.JsonResponse(String.Format(Properties.Resources.Youtube_PlaylistItems_List, channelID, Properties.Resources.Youtube_API));
                JArray videos = (JArray)videosObj["items"];

                Variables Variables = new Variables();

                foreach(var video in videos)
                {
                    if (!Variables.ElementIsBan("video", (string)video.SelectToken("snippet.resourceId.videoId")))
                    {
                        Add(
                            (string)video.SelectToken("snippet.resourceId.videoId"),
                            (((string)video.SelectToken("snippet.title")).IndexOf(" / PRO") > 0 ? ((string)video.SelectToken("snippet.title")).Remove(((string)video.SelectToken("snippet.title")).IndexOf(" / PRO")) : (string)video.SelectToken("snippet.title")),
                            String.Format(Properties.Resources.Youtube_Video, (string)video.SelectToken("snippet.resourceId.videoId")),
                            ((string)video.SelectToken("snippet.publishedAt")).Remove(10),
                            DateTime.Parse("2015-05-14T18:48:34.000Z").ToString("dd.MM"));
                    }
                }
            }
            catch (Exception ex) { Debugging.Save("Youtube.Class", "Start()", ex.Message, ex.StackTrace); }
        }

        public void Add(string id, string title, string link, string date, string dateShort)
        {
            try
            {
                try { if (List.Count <= 0) { } }
                catch (Exception)
                {
                    List = new List<VideoLoading>();
                    Range = new List<List<VideoLoading>>();
                }

                List.Add(new VideoLoading(id, title, link, date, dateShort));
                Range.Add(List);

                mID = Range[0][0].ID;
                mTitle = Range[0][0].Title;
                mLink = Range[0][0].Link;
                mDate = Range[0][0].Date;
                mDateShort = Range[0][0].DateShort;
            }
            catch (Exception ex)
            {
                Debugging.Save("Youtube.Class", "Add()", ex.Message, ex.StackTrace,
                    "ID: " + id,
                    "Title: " + title,
                    "Link: " + link,
                    "Date: " + date,
                    "Short Date: " + dateShort);
            }
        }

        public void Clear()
        {
            try { if (List.Count > 0) List.Clear(); }
            catch (Exception ex) { Debugging.Save("Youtube.Class", "Clear()", ex.Message, ex.StackTrace); }
        }

        public int IndexOf(string str)
        {
            try { if (List.Count > -1) { for (int i = 0; i < List.Count; i++) { if (List[i].ID == str) { return i; } } } return -1; }
            catch (Exception ex) { Debugging.Save("Youtube.Class", "IndexOf()", ex.Message, ex.StackTrace, "Search: " + str); return -1; }
        }

        public int Count()
        {
            try { return List.Count > 0 ? List.Count : 0; }
            catch (Exception) { return 0; }
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
                    for (int i = 0; i < List.Count; i++)
                        if (List[i].ID == id) { List.RemoveAt(i); }
            }
            catch (Exception /*ex*/) { /*Debugging.Save("Youtube.Class", "Count(id = " + id + ")", ex.Message);*/ /*Delete(id);*/ }

            try { List.TrimExcess(); }
            catch (Exception ex) { Debugging.Save("Youtube.Class", "Delete()", "Function: TrimExcess()", ex.Message, ex.StackTrace); }
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
            catch (Exception ex) { Debugging.Save("Youtube.Class", "ParseDate()", "Pack Date = " + packDate, "Video date = " + videoDate, ex.Message, ex.StackTrace); return true; }
        }
    }
}