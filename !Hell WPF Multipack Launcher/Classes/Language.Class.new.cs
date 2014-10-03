using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Language
    {
        /// <summary>
        /// Функция определения языка интерфейса
        /// </summary>
        /// <param name="page">Страница или класс расположения элемента</param>
        /// <param name="control">Компонент для перевода</param>
        /// <param name="lang">Язык перевода</param>
        /// <param name="text">Дополнительный текст</param>
        /// <returns>Результат перевода</returns>
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
                                case "de": return "Loading...";
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
                                case "de": return "Spieln";
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
                                case "de": return "Höchste Priorität";
                                default: return toReturn;
                            }

                        case "priority1":
                            switch (lang)
                            {
                                case "ru": return "Выше среднего";
                                case "en": return "Above average";
                                case "de": return "Hohe Priorität";
                                default: return toReturn;
                            }

                        case "priority2":
                            switch (lang)
                            {
                                case "ru": return "Средний";
                                case "en": return "Average priority";
                                case "de": return "Mittlere Priorität";
                                default: return toReturn;
                            }

                        case "priority3":
                            switch (lang)
                            {
                                case "ru": return "Ниже среднего";
                                case "en": return "Below average";
                                case "de": return "Durchschnittliche Priorität";
                                default: return toReturn;
                            }

                        case "priority4":
                            switch (lang)
                            {
                                case "ru": return "Низкий";
                                case "en": return "Low priority";
                                case "de": return "Unwichtig";
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
                                case "de": return "Schließen Sie nicht das Launcher";
                                default: return toReturn;
                            }

                        case "minimize1":
                            switch (lang)
                            {
                                case "ru": return "Сворачивать в трей при запуске игры";
                                case "en": return "Minimize to tray when the game starts";
                                case "de": return "Minimieren wenn das Spiel beginnt";
                                default: return toReturn;
                            }

                        case "minimize2":
                            switch (lang)
                            {
                                case "ru": return "Минимизировать лаунчер на панель задач";
                                case "en": return "Minimize Launcher to the taskbar";
                                case "de": return "Launcher in die Taskleiste";
                                default: return toReturn;
                            }

                        case "minimize3":
                            switch (lang)
                            {
                                case "ru": return "Закрывать при запуске игры";
                                case "en": return "Close when the game starts";
                                case "de": return "Schließen, wenn das Spiel beginnt";
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
                                case "de": return "PC und Spiel Optimierung:";
                                default: return toReturn;
                            }

                        case "cbKill":
                            switch (lang)
                            {
                                case "ru": return "Закрывать процессы при запуске игры";
                                case "en": return "000";
                                case "de": return "Prozesse schließen wenn das Spiel beginnt";
                                default: return toReturn;
                            }

                        case "cbForce":
                            switch (lang)
                            {
                                case "ru": return "Принудительно закрывать процессы";
                                case "en": return "000";
                                case "de": return "Prozesse Schließung erzwingen";
                                default: return toReturn;
                            }

                        case "cbVideo":
                            switch (lang)
                            {
                                case "ru": return "Уменьшить качество графики в игре";
                                case "en": return "000";
                                case "de": return "Grafikeinstellungen runterstellen";
                                default: return toReturn;
                            }

                        case "cbWeak":
                            switch (lang)
                            {
                                case "ru": return "Очень слабый компьютер";
                                case "en": return "000";
                                case "de": return "Leistungsschwache Rechner";
                                default: return toReturn;
                            }

                        case "cbAero":
                            switch (lang)
                            {
                                case "ru": return "Отключать Windows Aero";
                                case "en": return "000";
                                case "de": return "Windows Aero-Oberfläche deaktivieren";
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
                                case "de": return "Verschiedenes";
                                default: return toReturn;
                            }

                        case "cbNotifyVideo":
                            switch (lang)
                            {
                                case "ru": return "Уведомлять о новых видео";
                                case "en": return "000";
                                case "de": return "Benachrichtigungen für neue Videos";
                                default: return toReturn;
                            }

                        case "cbNotifyNews":
                            switch (lang)
                            {
                                case "ru": return "Уведомлять о новых новостях";
                                case "en": return "000";
                                case "de": return "Neuste Nachrichten bekommen";
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
                                case "de": return "Prozesse";
                                default: return toReturn;
                            }

                        case "lDescription":
                            switch (lang)
                            {
                                case "ru": return "Описание";
                                case "en": return "000";
                                case "de": return "Beschreibung";
                                default: return toReturn;
                            }

                        case "statusGlobal":
                            switch (lang)
                            {
                                case "ru": return "Глобальный список процессов";
                                case "en": return "000";
                                case "de": return "Wichtige Prozesse";
                                default: return toReturn;
                            }

                        case "statusGlobalDesc":
                            switch (lang)
                            {
                                case "ru": return "Отключение процессов из данного списка невозможно";
                                case "en": return "000";
                                case "de": return "Prozess Beendigung nicht möglich";
                                default: return toReturn;
                            }

                        case "statusUser":
                            switch (lang)
                            {
                                case "ru": return "Процессы, которые НЕЛЬЗЯ закрывать";
                                case "en": return "000";
                                case "de": return "Prozesse die NICHT geschlossen werden dürfen";
                                default: return toReturn;
                            }

                        case "statusUserDesc":
                            switch (lang)
                            {
                                case "ru": return "Процессы, которые будут игнорироваться лаунчером при оптимизации ПК";
                                case "en": return "000";
                                case "de": return "Prozesse die während der Optimierung ignoriert werden";
                                default: return toReturn;
                            }

                        case "statusNormal":
                            switch (lang)
                            {
                                case "ru": return "Процессы, не имеющие приоритета";
                                case "en": return "000";
                                case "de": return "Prozesse ohne Priorität";
                                default: return toReturn;
                            }

                        case "statusNormalDesc":
                            switch (lang)
                            {
                                case "ru": return "Процессы, которые будут завершаться при оптимизации ПК";
                                case "en": return "000";
                                case "de": return "Prozesse die während der Optimierung geschlossen werden";
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
                                case "de": return "Einstellungen";
                                default: return toReturn;
                            }

                        case "tbSettingsStProcesses":
                            switch (lang)
                            {
                                case "ru": return "Процессы";
                                case "en": return "Processes";
                                case "de": return "Prozesse";
                                default: return toReturn;
                            }

                        case "tbSettingsStShare":
                            switch (lang)
                            {
                                case "ru": return "Общие";
                                case "en": return "000";
                                case "de": return "Allgemeine";
                                default: return toReturn;
                            }

                        case "bClose":
                            switch (lang)
                            {
                                case "ru": return "Закрыть";
                                case "en": return "000";
                                case "de": return "Schließen";
                                default: return toReturn;
                            }


                        /* -----------------------------
                         *      Optimize
                         * -----------------------------*/
                        case "tbVipProcesses":
                            switch (lang)
                            {
                                case "ru": return "ВНИМАНИЕ!!! В список исключений рекомендуется добавлять действительно важные программы.";
                                case "en": return "WARNING! We are strongly recommend you to include only a really necessary programs in to exception list.";
                                case "de": return "ACHTUNG!!! in die Ausnahmeliste sollen nur die wichtigsten Programme aufgenommen werden";
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
                                case "de": return "Fehler";
                                default: return toReturn;
                            }

                        case "ErrorStatus":
                            switch (lang)
                            {
                                case "ru": return "Произошла ошибка";
                                case "en": return "000";
                                case "de": return "Fehler";
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
                                case "de": return "Feedback";
                                default: return toReturn;
                            }

                        case "tbComment":
                            switch (lang)
                            {
                                case "ru": return "Если у Вас возникли проблемы в работе лаунчера и/или модов, или же есть какие-либо пожелания, Вы можете заполнить форму ниже и отправить сообщение разработчикам:";
                                case "en": return "000";
                                case "de": return "Bei Problemen mit dem Launcher, der Installation oder Vorschlägen füllen Sie bitte das weiter unten stehender Formular aus und schicken Sie es an die Entwickler";
                                default: return toReturn;
                            }

                        case "lSelectCategory":
                            switch (lang)
                            {
                                case "ru": return "Выберите категорию:";
                                case "en": return "000";
                                case "de": return "Bitte die Kategorie auswählen";
                                default: return toReturn;
                            }

                        case "lSetEmail":
                            switch (lang)
                            {
                                case "ru": return "Введите Ваш email:";
                                case "en": return "000";
                                case "de": return "Tragen Sie ihre email Adresse ein";
                                default: return toReturn;
                            }

                        case "bAttach":
                            switch (lang)
                            {
                                case "ru": return "Прикрепить файл";
                                case "en": return "000";
                                case "de": return "Anhang hinzufügen";
                                default: return toReturn;
                            }

                        case "bSend":
                            switch (lang)
                            {
                                case "ru": return "Отправить";
                                case "en": return "Send";
                                case "de": return "Senden";
                                default: return toReturn;
                            }

                        case "bClose":
                            switch (lang)
                            {
                                case "ru": return "Выход";
                                case "en": return "Exit";
                                case "de": return "Beenden";
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
                                case "de": return "Wünsche zum MultiPack";
                                default: return toReturn;
                            }

                        case "rbWishLauncher":
                            switch (lang)
                            {
                                case "ru": return "Пожелания к лаунчеру";
                                case "en": return "000";
                                case "de": return "Wünsche zum Launcher";
                                default: return toReturn;
                            }

                        case "rbWishInstaller":
                            switch (lang)
                            {
                                case "ru": return "Пожелания к установщику";
                                case "en": return "000";
                                case "de": return "Wünsche zum Installer";
                                default: return toReturn;
                            }

                        case "rbErrorMultipack":
                            switch (lang)
                            {
                                case "ru": return "Найдена ошибка в мультипаке";
                                case "en": return "000";
                                case "de": return "Fehler im MultiPack";
                                default: return toReturn;
                            }

                        case "rbErrorLauncher":
                            switch (lang)
                            {
                                case "ru": return "Найдена ошибка в лаунчере";
                                case "en": return "000";
                                case "de": return "Fehler im Launcher";
                                default: return toReturn;
                            }

                        case "rbErrorInstaller":
                            switch (lang)
                            {
                                case "ru": return "Найдена ошибка в установщике";
                                case "en": return "000";
                                case "de": return "Fehler bei der Installation";
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
                                case "de": return "Willkommen";
                                default: return toReturn;
                            }

                        case "lNews":
                            switch (lang)
                            {
                                case "ru": return "НОВОСТИ";
                                case "en": return "NEWS";
                                case "de": return "NEUIGKEITEN";
                                default: return toReturn;
                            }

                        case "lVideo":
                            switch (lang)
                            {
                                case "ru": return "ВИДЕО";
                                case "en": return "VIDEO";
                                case "de": return "VIDEO";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }



                /* =====================================
                 *      Update page
                 * =====================================*/
                case "PageUpdate":
                    switch (control)
                    {
                        case "gbCaption":
                            switch (lang)
                            {
                                case "ru": return "Обнаружена новая версия:";
                                case "en": return "A new version is available";
                                case "de": return "Es gibt eine neue Version";
                                default: return toReturn;
                            }

                        case "lDownloadFromLink":
                            switch (lang)
                            {
                                case "ru": return "Новая версия доступна по ссылке в описании к видео";
                                case "en": return "New version is available on the link in the description of the video.";
                                case "de": return "Download für die neue version befindet sich in der Videobeschreibung";
                                default: return toReturn;
                            }

                        case "cbNotify":
                            switch (lang)
                            {
                                case "ru": return "Не уведомлять меня об этой версии";
                                case "en": return "Do not notify me about this version";
                                case "de": return "Ich möchte keine Benachrichtigungen über Updates";
                                default: return toReturn;
                            }

                        case "bUpdate":
                            switch (lang)
                            {
                                case "ru": return "Скачать";
                                case "en": return "Download";
                                case "de": return "Download";
                                default: return toReturn;
                            }

                        case "bCancel":
                            switch (lang)
                            {
                                case "ru": return "Не надо";
                                case "en": return "Cancel";
                                case "de": return "Abbrechen";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }



                /* =====================================
                 *      User page
                 * =====================================*/
                case "PageUser":
                    switch (control)
                    {
                        /*
                         *  First subpage
                         */
                        case "tbUserRank":
                            switch (lang)
                            {
                                case "ru": return "Офицер";
                                case "en": return "000";
                                case "de": return "Offizier";
                                default: return toReturn;
                            }

                        case "tbPercentWins":
                            switch (lang)
                            {
                                case "ru": return "Процент побед";
                                case "en": return "000";
                                case "de": return "Gewinnrate in Prozent";
                                default: return toReturn;
                            }

                        case "tbMyRating":
                            switch (lang)
                            {
                                case "ru": return "Личный рейтинг";
                                case "en": return "000";
                                case "de": return "Mein Ranking";
                                default: return toReturn;
                            }

                        case "tbAvgXP":
                            switch (lang)
                            {
                                case "ru": return "Средний опыт за бой";
                                case "en": return "000";
                                case "de": return "Durchschnittliche Erfahrung pro Gefecht";
                                default: return toReturn;
                            }

                        case "tbCountWars":
                            switch (lang)
                            {
                                case "ru": return "Количество боев";
                                case "en": return "000";
                                case "de": return "Gespielte Gefechte";
                                default: return toReturn;
                            }

                        case "tbAvgDamage":
                            switch (lang)
                            {
                                case "ru": return "Средний нанесенный урон за бой";
                                case "en": return "000";
                                case "de": return "Im durchschnitt erteilter Schaden pro Gefecht";
                                default: return toReturn;
                            }

                        /*
                         *  Clan subpage
                         */
                        case "tbPersonnel":
                            switch (lang)
                            {
                                case "ru": return "Личный состав";
                                case "en": return "000";
                                case "de": return "Personal";
                                default: return toReturn;
                            }

                        case "tbDataOn":
                            switch (lang)
                            {
                                //  Например,
                                //      Данные на 29.09.2014 1:03
                                case "ru": return "Данные на";
                                case "en": return "000";
                                case "de": return "Daten für";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }



                /* =====================================
                 *      Ranks
                 * =====================================*/
                case "Rank":
                    switch (control)
                    {
                        case "leader":
                            switch (lang)
                            {
                                case "ru": return "Командир";
                                case "en": return "000";
                                case "de": return "Kommandant";
                                default: return toReturn;
                            }

                        case "vice_leader":
                            switch (lang)
                            {
                                case "ru": return "Зам. командира";
                                case "en": return "000";
                                case "de": return "Stellvertreter des Kommandanten";
                                default: return toReturn;
                            }

                        case "commander":
                            switch (lang)
                            {
                                case "ru": return "Ротный командир";
                                case "en": return "000";
                                case "de": return "Kompaniechef";
                                default: return toReturn;
                            }

                        case "private":
                            switch (lang)
                            {
                                case "ru": return "Солдат";
                                case "en": return "000";
                                case "de": return "Soldat";
                                default: return toReturn;
                            }

                        case "recruit":
                            switch (lang)
                            {
                                case "ru": return "Новобранец";
                                case "en": return "000";
                                case "de": return "Rekrut";
                                default: return toReturn;
                            }

                        case "recruiter":
                            switch (lang)
                            {
                                case "ru": return "Вербовщик";
                                case "en": return "000";
                                case "de": return "Anwerber";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }




                /* =====================================
                 *      Other info
                 * =====================================*/
                case "Optimize":
                    switch (control)
                    {
                        case "Optimize":
                            switch (lang)
                            {
                                case "ru": return "Оптимизировать Ваш ПК для поднятия FPS";
                                case "en": return "Optimize your PC for maximum FPS";
                                case "de": return "Optimieren Sie Ihren PC für maximale FPS";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }




                /* =====================================
                 *      Variable Class
                 * =====================================*/
                case "variables_class":
                    switch (control)
                    {
                        case "do_not_settings":
                            switch (lang)
                            {
                                case "ru": return "Файл настроек не обнаружен!";
                                case "en": return "Configuration file is unavailable!";
                                case "de": return "Konfigurationsdatei ist nicht verfügbar!";
                                default: return toReturn;
                            }

                        default: return toReturn;
                    }

                default: return toReturn;
            }
        }
    }
}