using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Language
    {
        public string Set(string form, string control, string lang = "", string additional = "")
        {
            // Set default language
            try { if (lang == "") lang = Properties.Resources.Default_Lang; }
            catch (Exception) { }


            try
            {
                switch (lang)
                {
                    /*
                     * RUSSIAN
                     */
                    case "ru":
                        switch (form)
                        {
                            // SETTINGS
                            case "Settings":
                                switch (control)
                                {
                                    case "priority": return "5";
                                    case "priority0": return "Высокий";
                                    case "priority1": return "Выше среднего";
                                    case "priority2": return "Средний";
                                    case "priority3": return "Ниже среднего";
                                    case "priority4": return "Низкий";


                                    case "minimize": return "4";
                                    case "minimize0": return "Не закрывать лаунчер";
                                    case "minimize1": return "Сворачивать в трей при запуске игры";
                                    case "minimize2": return "Минимизировать лаунчер на панель задач";
                                    case "minimize3": return "Закрывать при запуске игры";

                                    default: return "null";
                                }
                            default: return "null";
                        }




                    /*
                     * ENGLISH
                     */
                    case "en":
                        switch (form)
                        {
                            // SETTINGS
                            case "Settings":
                                switch (control)
                                {
                                    case "priority": return "5";
                                    case "priority0": return "High priority";
                                    case "priority1": return "Above average";
                                    case "priority2": return "Average priority";
                                    case "priority3": return "Below average";
                                    case "priority4": return "Low priority";


                                    case "minimize": return "4";
                                    case "minimize0": return "Do not close the Launcher";
                                    case "minimize1": return "Minimize to tray when the game starts";
                                    case "minimize2": return "Minimize Launcher to the taskbar";
                                    case "minimize3": return "Close when the game starts";

                                    default: return "null";
                                }
                            default: return "null";
                        }

                    default: return "null";
                }
            }
            catch (Exception ex) { return "null"; }
        }
    }
}