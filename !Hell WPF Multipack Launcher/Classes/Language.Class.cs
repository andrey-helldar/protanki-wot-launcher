using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Language
    {
        public string Set(string page, string control, string lang = "", string text = "")
        {
            try { if (lang == "") lang = Properties.Resources.Default_Lang; }
            catch (Exception) { }

            string toReturn = "null";

            switch (page)
            {
                case "PageSettings":     // Settings Page
                    switch (control)
                    {
                        /*
                         *  PRIORITY
                         */
                        case "priority": return "5";

                        case "priority0":
                            switch (lang)
                            {
                                case "ru": return "Высокий";
                                case "en": return "High priority";
                                default: return toReturn;
                            }
                        case "priority1":
                            switch (lang)
                            {
                                case "ru": return "Выше среднего";
                                case "en": return "Above average";
                                default: return toReturn;
                            }
                        case "priority2":
                            switch (lang)
                            {
                                case "ru": return "Средний";
                                case "en": return "Average priority";
                                default: return toReturn;
                            }
                        case "priority3":
                            switch (lang)
                            {
                                case "ru": return "Ниже среднего";
                                case "en": return "Below average";
                                default: return toReturn;
                            }
                        case "priority4":
                            switch (lang)
                            {
                                case "ru": return "Низкий";
                                case "en": return "Low priority";
                                default: return toReturn;
                            }

                        /*
                         *      MINIMIZE
                         */
                        case "minimize": return "4";


                        case "minimize0":
                            switch (lang)
                            {
                                case "ru": return "Не закрывать лаунчер";
                                case "en": return "Do not close the Launcher";
                                default: return toReturn;
                            }
                        case "minimize1":
                            switch (lang)
                            {
                                case "ru": return "Сворачивать в трей при запуске игры";
                                case "en": return "Minimize to tray when the game starts";
                                default: return toReturn;
                            }
                        case "minimize2":
                            switch (lang)
                            {
                                case "ru": return "Минимизировать лаунчер на панель задач";
                                case "en": return "Minimize Launcher to the taskbar";
                                default: return toReturn;
                            }
                        case "minimize3":
                            switch (lang)
                            {
                                case "ru": return "Закрывать при запуске игры";
                                case "en": return "Close when the game starts";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }

                    /*
                     *      Page Error
                     */
                case "PageError":
                    switch (control)
                    {
                        case "Error1":
                            switch (lang)
                            {
                                case "ru": return "Ошибка";
                                case "en": return "Error";
                                default: return toReturn;
                            }

                        case "ErrorStatus":
                            switch (lang)
                            {
                                case "ru": return "Произошла ошибка";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }

                default: return toReturn;
            }
        }
    }
}