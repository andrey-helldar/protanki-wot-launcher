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
                /* =====================================
                 *      Loading page
                 * =====================================*/
                case "PageLoading":
                    switch (control)
                    {
                        case "lLoading":
                            switch (lang)
                            {
                                case "ru": return "Загрузка...";
                                case "en": return "Loading...";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }




                /* =====================================
                 *      MainWindow
                 * =====================================*/
                case "MainProject":
                    switch (control)
                    {
                        case "bPlay":
                            switch (lang)
                            {
                                case "ru": return "Играть";
                                case "en": return "Play";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }




                    /* =====================================
                     *      Settings General page
                     * =====================================*/
                case "PageSettingsGeneral":
                    switch (control)
                    {
                        /* -----------------------------
                         *      Priority
                         * -----------------------------*/
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

                        /* -----------------------------
                         *      Minimize
                         * -----------------------------*/
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


                        /* -----------------------------
                         *      Optimize
                         * -----------------------------*/

                        case "gbOptimize":
                            switch (lang)
                            {
                                case "ru": return "Оптимизация ПК и игры:";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "cbKill":
                            switch (lang)
                            {
                                case "ru": return "Закрывать процессы при запуске игры";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "cbForce":
                            switch (lang)
                            {
                                case "ru": return "Принудительно закрывать процессы";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "cbVideo":
                            switch (lang)
                            {
                                case "ru": return "Уменьшить качество графики в игре";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "cbWeak":
                            switch (lang)
                            {
                                case "ru": return "Очень слабый компьютер";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "cbAero":
                            switch (lang)
                            {
                                case "ru": return "Отключать Windows Aero";
                                case "en": return "000";
                                default: return toReturn;
                            }


                        /* -----------------------------
                         *      Optimize
                         * -----------------------------*/

                        case "gbOther":
                            switch (lang)
                            {
                                case "ru": return "Другое:";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "cbNotifyVideo":
                            switch (lang)
                            {
                                case "ru": return "Уведомлять о новых видео";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "cbNotifyNews":
                            switch (lang)
                            {
                                case "ru": return "Уведомлять о новых новостях";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }




                /* =====================================
                 *      Settings Processes page
                 * =====================================*/
                case "PageSettingsProcesses":
                    switch (control)
                    {
                        case "lProcesses":
                            switch (lang)
                            {
                                case "ru": return "Процессы";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "lDescription":
                            switch (lang)
                            {
                                case "ru": return "Описание";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "statusGlobal":
                            switch (lang)
                            {
                                case "ru": return "Глобальный список процессов";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "statusGlobalDesc":
                            switch (lang)
                            {
                                case "ru": return "Отключение процессов из данного списка невозможно";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "statusUser":
                            switch (lang)
                            {
                                case "ru": return "Процессы, которые НЕЛЬЗЯ закрывать";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "statusUserDesc":
                            switch (lang)
                            {
                                case "ru": return "Процессы, которые будут игнорироваться лаунчером при оптимизации ПК";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "statusNormal":
                            switch (lang)
                            {
                                case "ru": return "Процессы, не имеющие приоритета";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "statusNormalDesc":
                            switch (lang)
                            {
                                case "ru": return "Процессы, которые будут завершаться при оптимизации ПК";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }
                    



                        /* =====================================
                         *      Settings General page
                         * =====================================*/
                case "PageSettings":
                    switch (control)
                    {
                        case "tbSettingsTitle":
                            switch (lang)
                            {
                                case "ru": return "Настройки >";
                                case "en": return "Settings >";
                                default: return toReturn;
                            }

                        case "tbSettingsStProcesses":
                            switch (lang)
                            {
                                case "ru": return "Процессы";
                                case "en": return "Processes";
                                default: return toReturn;
                            }

                        case "tbSettingsStShare":
                            switch (lang)
                            {
                                case "ru": return "Общие";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "bClose":
                            switch (lang)
                            {
                                case "ru": return "Закрыть";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }



                /* =====================================
                 *      Error page
                 * =====================================*/
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



                /* =====================================
                 *      Feedback page
                 * =====================================*/
                case "PageFeedback":
                    switch (control)
                    {
                        case "gbFeedback":
                            switch (lang)
                            {
                                case "ru": return "Обратная связь";
                                case "en": return "Feedback";
                                default: return toReturn;
                            }

                        case "tbComment":
                            switch (lang)
                            {
                                case "ru": return "Если у Вас возникли проблемы в работе лаунчера и/или модов, или же есть какие-либо пожелания, Вы можете заполнить форму ниже и отправить сообщение разработчикам:";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "lSelectCategory":
                            switch (lang)
                            {
                                case "ru": return "Выберите категорию:";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "lSetEmail":
                            switch (lang)
                            {
                                case "ru": return "Введите Ваш email:";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "bAttach":
                            switch (lang)
                            {
                                case "ru": return "Прикрепить файл";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "bSend":
                            switch (lang)
                            {
                                case "ru": return "Отправить";
                                case "en": return "Send";
                                default: return toReturn;
                            }

                        case "bClose":
                            switch (lang)
                            {
                                case "ru": return "Выход";
                                case "en": return "Exit";
                                default: return toReturn;
                            }

                        /* -----------------------------
                         *      Categories
                         * -----------------------------*/

                        case "rbWishMultipack":
                            switch (lang)
                            {
                                case "ru": return "Пожелания к мультипаку";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "rbWishLauncher":
                            switch (lang)
                            {
                                case "ru": return "Пожелания к лаунчеру";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "rbWishInstaller":
                            switch (lang)
                            {
                                case "ru": return "Пожелания к установщику";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "rbErrorMultipack":
                            switch (lang)
                            {
                                case "ru": return "Найдена ошибка в мультипаке";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "rbErrorLauncher":
                            switch (lang)
                            {
                                case "ru": return "Найдена ошибка в лаунчере";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        case "rbErrorInstaller":
                            switch (lang)
                            {
                                case "ru": return "Найдена ошибка в установщике";
                                case "en": return "000";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }



                /* =====================================
                 *      General page
                 * =====================================*/
                case "PageGeneral":
                    switch (control)
                    {
                        case "lStatus":
                            switch (lang)
                            {
                                case "ru": return "Добро пожаловать";
                                case "en": return "Welcome";
                                default: return toReturn;
                            }

                        case "lNews":
                            switch (lang)
                            {
                                case "ru": return "НОВОСТИ";
                                case "en": return "NEWS";
                                default: return toReturn;
                            }

                        case "lVideo":
                            switch (lang)
                            {
                                case "ru": return "ВИДЕО";
                                case "en": return "VIDEO";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }

                default: return toReturn;
            }
        }
    }
}