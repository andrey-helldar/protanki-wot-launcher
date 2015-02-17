using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    class Language
    {
        /// <summary>
        /// Список поддерживаемых языков.
        /// Данный массив JObject упрощает определение допустимых переводов
        /// </summary>
        /// <returns>JObject массив языков перевода в формате "ru":"Русский", где
        /// ru - key
        /// Русский - value</returns>
        public JObject Translated()
        {
            try
            {
                JObject obj = new JObject();

                obj.Add(new JProperty("ru", "ru:Русский"));	// Andrey Helldar
                obj.Add(new JProperty("en", "en:English"));	// D Voronoff
                obj.Add(new JProperty("de", "de:Deutsch"));	// Alexander Storz
                obj.Add(new JProperty("ua", "ua:Український")); // PROTanki Team

                return obj;
            }
            catch (Exception ex)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => new Debugging().Save("Language Class", "Translated", ex.Message, ex.StackTrace));
                return null;
            }
        }

        /// <summary>
        /// Функция определения языка интерфейса
        /// </summary>
        /// <param name="page">Страница или класс расположения элемента</param>
        /// <param name="control">Компонент для перевода</param>
        /// <param name="lang">Язык перевода:
        /// ru - русский
        /// en - английский
        /// de - немецкий
        /// ua - украинский
        /// </param>
        /// <param name="text">Дополнительный текст</param>
        /// <returns>Результат перевода</returns>
        public string Set(string page, string control, string lang = "", string text = "")
        {
            try { if (lang == "") lang = Properties.Resources.Default_Lang; }
            catch (Exception) { lang = "ru"; }

            string toReturn = "null";

            try
            {
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
                                    case "ua": return "Завантаження...";
                                    default: return toReturn;
                                }

                            default: return toReturn;
                        }



                    /* =====================================
                     *      Buttons
                     * =====================================*/
                    case "Button":
                        switch (control)
                        {
                            case "Iagree":
                                switch (lang)
                                {
                                    case "ru": return "Принять";
                                    case "en": return "Accept";
                                    case "de": return "übernehmen";
                                    case "ua": return "Прийняти";
                                    default: return toReturn;
                                }

                            case "Idisagree":
                                switch (lang)
                                {
                                    case "ru": return "Отказаться";
                                    case "en": return "Cancel";
                                    case "de": return "Abbrechen";
                                    case "ua": return "Скасування";
                                    default: return toReturn;
                                }

                            case "Close":
                                switch (lang)
                                {
                                    case "ru": return "Закрыть";
                                    case "en": return "Close";
                                    case "de": return "Schließen";
                                    case "ua": return "Закрити";
                                    default: return toReturn;
                                }

                            default: return toReturn;
                        }



                    /* =====================================
                     *      Global words
                     * =====================================*/
                    case "Global":
                        switch (control)
                        {
                            case "IsPremium":
                                switch (lang)
                                {
                                    case "ru": return "Премиум аккаунт";
                                    case "en": return "Premium account";
                                    case "de": return "Premium account";
                                    case "ua": return "Преміум аккаунт";
                                    default: return toReturn;
                                }

                            case "IsBase":
                                switch (lang)
                                {
                                    case "ru": return "Базовый аккаунт";
                                    case "en": return "Basic account";
                                    case "de": return "Basic account";
                                    case "ua": return "Базовий аккаунт";
                                    default: return toReturn;
                                }

                            default: return toReturn;
                        }




                    /* =====================================
                     *      STATUSES API
                     * =====================================*/
                    case "API":
                        switch (control)
                        {
                            case "ANSWER":
                                switch (lang)
                                {
                                    case "ru": return "Сервер ответил: " + Environment.NewLine + text;
                                    case "en": return "The server responded: " + Environment.NewLine + text;
                                    case "de": return "Der Server reagiert: " + Environment.NewLine + text;
                                    case "ua": return "Сервер відповів: " + Environment.NewLine + text;
                                    default: return toReturn;
                                }

                            case "SOFTWARE_NOT_AUTORIZED":
                                switch (lang)
                                {
                                    case "ru": return "Приложение не авторизовано на сервере";
                                    case "en": return "The application is not authorized on the server";
                                    case "de": return "Die Anwendung ist nicht auf dem Server autorisiert";
                                    case "ua": return "Додаток не авторизовано на сервері";
                                    default: return toReturn;
                                }

                            case "SERVER_IS_UNAVAILABLE":
                                switch (lang)
                                {
                                    case "ru": return "Сервер статистики временно недоступен";
                                    case "en": return "Server is temporarily unavailable";
                                    case "de": return "Server vorübergehend nicht verfügbar";
                                    case "ua": return "Сервер статистики тимчасово недоступний";
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
                                    case "ua": return "Грати";
                                    default: return toReturn;
                                }

                            case "Preview_NoData":
                                switch (lang)
                                {
                                    case "ru": return "Получены не все данные. Воспроизведение видео невозможно.";
                                    case "en": return "Not enough data. Video playback is not possible.";
                                    case "de": return "Ungültige oder unvollständige Eingabe, Videowiedergabe ist nicht möglich";
                                    case "ua": return "Отримані не всі дані. Відтворення відео неможливо.";
                                    default: return toReturn;
                                }

                            case "Preview_Error":
                                switch (lang)
                                {
                                    case "ru": return "Возникла ошибка при загрузке видео. Попробуйте еще раз.";
                                    case "en": return "Error occurred. Please, try again.";
                                    case "de": return "Es ist ein Fehler aufgetreten, bitte versuchen sie es erneut...";
                                    case "ua": return "Виникла помилка при завантаженні відео. Спробуйте ще раз.";
                                    default: return toReturn;
                                }

                            case "Game_Not_Found":
                                switch (lang)
                                {
                                    case "ru": return "Клиент игры не обнаружен!";
                                    case "en": return "\"World of Tanks\" is not found!";
                                    case "de": return "Die Client-Datei des Spiels \"World of Tanks\" wurde nicht gefunden!";
                                    case "ua": return "Клієнт гри не виявлений!";
                                    default: return toReturn;
                                }

                            case "Pack_Not_Found":
                                switch (lang)
                                {
                                    case "ru": return "Мультипак не обнаружен!";
                                    case "en": return "Multipack is not found!";
                                    case "de": return "Multipack wurde nicht gefunden!";
                                    case "ua": return "Мультипак не знайдений!";
                                    default: return toReturn;
                                }

                            case "Settings_Not_Found":
                                switch (lang)
                                {
                                    case "ru": return "Настройки лаунчера не обнаружены!";
                                    case "en": return "Settings of launcher not found!";
                                    case "de": return "Einstellungen Launcher wurde nicht gefunden!";
                                    case "ua": return "Налаштування лаунчера не виявлені!";
                                    default: return toReturn;
                                }

                            case "Ticket_2_Developer":
                                switch (lang)
                                {
                                    case "ru": return "Для получения инструкций по устранению данной ошибки, Вы можете связаться с разработчиком";
                                    case "en": return "Please, contact the developer, to resolve this error.";
                                    case "de": return "Wenn Sie einen Bug melden möchten setzen Sie bitte die Entwickler darüber in Kenntnis";
                                    case "ua": return "Для отримання інструкцій щодо усунення даної помилки, Ви можете зв'язатися з розробником";
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
                                    case "ua": return "Высокий";
                                    default: return toReturn;
                                }

                            case "priority1":
                                switch (lang)
                                {
                                    case "ru": return "Выше среднего";
                                    case "en": return "Above average";
                                    case "de": return "Hohe Priorität";
                                    case "ua": return "Вище середнього";
                                    default: return toReturn;
                                }

                            case "priority2":
                                switch (lang)
                                {
                                    case "ru": return "Средний";
                                    case "en": return "Average priority";
                                    case "de": return "Mittlere Priorität";
                                    case "ua": return "Середній";
                                    default: return toReturn;
                                }

                            case "priority3":
                                switch (lang)
                                {
                                    case "ru": return "Ниже среднего";
                                    case "en": return "Below average";
                                    case "de": return "Durchschnittliche Priorität";
                                    case "ua": return "Нижче середнього";
                                    default: return toReturn;
                                }

                            case "priority4":
                                switch (lang)
                                {
                                    case "ru": return "Низкий";
                                    case "en": return "Low priority";
                                    case "de": return "Unwichtig";
                                    case "ua": return "Низький";
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
                                    case "de": return "Bitte den Launcher nicht schließen";
                                    case "ua": return "Не закривати лаунчер";
                                    default: return toReturn;
                                }

                            case "minimize1":
                                switch (lang)
                                {
                                    case "ru": return "Сворачивать в трей при запуске игры";
                                    case "en": return "Minimize to tray when the game starts";
                                    case "de": return "Minimieren wenn das Spiel beginnt";
                                    case "ua": return "Згортати в трей при запуску гри";
                                    default: return toReturn;
                                }

                            case "minimize2":
                                switch (lang)
                                {
                                    case "ru": return "Минимизировать лаунчер на панель задач";
                                    case "en": return "Minimize Launcher to the taskbar";
                                    case "de": return "Launcher in die Taskleiste";
                                    case "ua": return "Мінімізувати лаунчер на панель задач";
                                    default: return toReturn;
                                }

                            case "minimize3":
                                switch (lang)
                                {
                                    case "ru": return "Закрывать при запуске игры";
                                    case "en": return "Close when the game starts";
                                    case "de": return "Schließen, wenn das Spiel beginnt";
                                    case "ua": return "Закривати при запуску гри";
                                    default: return toReturn;
                                }


                            /* -----------------------------
                             *      Optimize
                             * -----------------------------*/

                            case "Optimize":
                                switch (lang)
                                {
                                    case "ru": return "Оптимизация...";
                                    case "en": return "Optimization...";
                                    case "de": return "Optimierung...";
                                    case "ua": return "Оптимізація...";
                                    default: return toReturn;
                                }

                            case "gbOptimize":
                                switch (lang)
                                {
                                    case "ru": return "Оптимизация ПК и игры:";
                                    case "en": return "Game and PC optimization:";
                                    case "de": return "PC und Spiel Optimierung:";
                                    case "ua": return "Оптимізація ПК та гри:";
                                    default: return toReturn;
                                }

                            case "cbKill":
                                switch (lang)
                                {
                                    case "ru": return "Закрывать процессы при запуске игры";
                                    case "en": return "Close processes when the game starts";
                                    case "de": return "Prozesse schließen wenn das Spiel beginnt";
                                    case "ua": return "Закривати процеси при запуску гри";
                                    default: return toReturn;
                                }

                            case "cbForce":
                                switch (lang)
                                {
                                    case "ru": return "Принудительно закрывать процессы";
                                    case "en": return "Kill processes when the game starts";
                                    case "de": return "Beenden der Prozesse und Programme erzwingen";
                                    case "ua": return "Примусово закривати процеси";
                                    default: return toReturn;
                                }

                            case "cbVideo":
                                switch (lang)
                                {
                                    case "ru": return "Уменьшить качество графики в игре";
                                    case "en": return "Reduce graphic details in the game";
                                    case "de": return "Grafikeinstellungen des Spiels runterstellen";
                                    case "ua": return "Зменшити якість графіки в грі";
                                    default: return toReturn;
                                }

                            case "cbWeak":
                                switch (lang)
                                {
                                    case "ru": return "Очень слабый компьютер";
                                    case "en": return "Weak PC";
                                    case "de": return "Leistungsschwache Rechner";
                                    case "ua": return "Дуже слабкий комп'ютер";
                                    default: return toReturn;
                                }

                            case "cbAero":
                                switch (lang)
                                {
                                    case "ru": return "Отключать Windows Aero";
                                    case "en": return "Disable Windows Aero";
                                    case "de": return "Windows Aero-Oberfläche deaktivieren";
                                    case "ua": return "Відключати Windows Aero";
                                    default: return toReturn;
                                }

                            case "NeedAdmin":
                                switch (lang)
                                {
                                    case "ru": return "нужны права администратора";
                                    case "en": return "administrator rights are required";
                                    case "de": return "administratorrechte erforderlich";
                                    case "ua": return "потрібні права адміністратора";
                                    default: return toReturn;
                                }


                            /* -----------------------------
                             *      Optimize
                             * -----------------------------*/

                            case "gbOther":
                                switch (lang)
                                {
                                    case "ru": return "Другое:";
                                    case "en": return "Other:";
                                    case "de": return "Verschiedenes:";
                                    case "ua": return "Інше:";
                                    default: return toReturn;
                                }

                            case "cbNotifyVideo":
                                switch (lang)
                                {
                                    case "ru": return "Уведомлять о новых видео";
                                    case "en": return "Notify about new videos";
                                    case "de": return "Benachrichtigungen über neue Videos";
                                    case "ua": return "Повідомляти про нові відео";
                                    default: return toReturn;
                                }

                            case "cbNotifyNews":
                                switch (lang)
                                {
                                    case "ru": return "Уведомлять о новых новостях";
                                    case "en": return "Notify about news";
                                    case "de": return "Neuste Nachrichten bekommen";
                                    case "ua": return "Повідомляти про нові новини";
                                    default: return toReturn;
                                }


                            /* -----------------------------
                             *      Interface
                             * -----------------------------*/
                            case "gbInterface":
                                switch (lang)
                                {
                                    case "ru": return "Интерфейс";
                                    case "en": return "Interface";
                                    case "de": return "Interface";
                                    case "ua": return "Інтерфейс";
                                    default: return toReturn;
                                }

                            // Приоритет чтения языка
                            case "LangPriority": return "3";

                            case "cbLangLocale":
                                switch (lang)
                                {
                                    case "ru": return "Приоритет определения локализации:";
                                    case "en": return "Priority localization:";
                                    case "de": return "Priorität Lokalisierung:";
                                    case "ua": return "Пріоритет визначення локалізації:";
                                    default: return toReturn;
                                }

                            case "cbLangPriority0":
                                switch (lang)
                                {
                                    case "ru": return "Мультипак";
                                    case "en": return "Multipack";
                                    case "de": return "Multipack";
                                    case "ua": return "Мультипак";
                                    default: return toReturn;
                                }

                            case "cbLangPriority1":
                                switch (lang)
                                {
                                    case "ru": return "Клиент игры";
                                    case "en": return "Game client";
                                    case "de": return "Spielclients";
                                    case "ua": return "Клієнт гри";
                                    default: return toReturn;
                                }

                            case "cbLangPriority2":
                                switch (lang)
                                {
                                    case "ru": return "Вручную";
                                    case "en": return "Manual";
                                    case "de": return "Manuell";
                                    case "ua": return "Вручну";
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
                                    case "en": return "Processes";
                                    case "de": return "Prozesse";
                                    case "ua": return "Процеси";
                                    default: return toReturn;
                                }

                            case "lDescription":
                                switch (lang)
                                {
                                    case "ru": return "Описание";
                                    case "en": return "Description";
                                    case "de": return "Beschreibung";
                                    case "ua": return "Опис";
                                    default: return toReturn;
                                }

                            case "statusGlobal":
                                switch (lang)
                                {
                                    case "ru": return "Глобальный список процессов";
                                    case "en": return "Global list of processes";
                                    case "de": return "Wichtige Systemprozesse";
                                    case "ua": return "Глобальний список процесів";
                                    default: return toReturn;
                                }

                            case "statusGlobalDesc":
                                switch (lang)
                                {
                                    case "ru": return "Отключение процессов из данного списка невозможно";
                                    case "en": return "Processes from this list cannot be disabled";
                                    case "de": return "Prozess Beendigung nicht möglich";
                                    case "ua": return "Відключення процесів з даного списку неможливо";
                                    default: return toReturn;
                                }

                            case "statusUser":
                                switch (lang)
                                {
                                    case "ru": return "Процессы, которые НЕЛЬЗЯ закрывать";
                                    case "en": return "DO NOT close this processes ";
                                    case "de": return "Prozesse die NICHT geschlossen werden dürfen";
                                    case "ua": return "Процеси, які НЕ МОЖНА закривати";
                                    default: return toReturn;
                                }

                            case "statusUserDesc":
                                switch (lang)
                                {
                                    case "ru": return "Процессы, которые будут игнорироваться лаунчером при оптимизации ПК";
                                    case "en": return "Processes that would be ignored by Launcher";
                                    case "de": return "Prozesse die während der Optimierung ignoriert werden";
                                    case "ua": return "Процеси, які будуть ігноруватися лаунчером при оптимізації ПК";
                                    default: return toReturn;
                                }

                            case "statusNormal":
                                switch (lang)
                                {
                                    case "ru": return "Процессы, не имеющие приоритета";
                                    case "en": return "Processes without priority";
                                    case "de": return "Prozesse ohne Priorität";
                                    case "ua": return "Процеси, що не мають пріоритету";
                                    default: return toReturn;
                                }

                            case "statusNormalDesc":
                                switch (lang)
                                {
                                    case "ru": return "Процессы, которые будут завершаться при оптимизации ПК";
                                    case "en": return "Processes, that would be closed when optimizing PC.";
                                    case "de": return "Prozesse die während der Optimierung geschlossen werden";
                                    case "ua": return "Процеси, які будуть завершуватися при оптимізації ПК";
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
                                    case "de": return "Einstellungen >";
                                    case "ua": return "Налаштування >";
                                    default: return toReturn;
                                }

                            case "tbSettingsStProcesses":
                                switch (lang)
                                {
                                    case "ru": return "Процессы";
                                    case "en": return "Processes";
                                    case "de": return "Prozesse";
                                    case "ua": return "Процеси";
                                    default: return toReturn;
                                }

                            case "tbSettingsStShare":
                                switch (lang)
                                {
                                    case "ru": return "Общие";
                                    case "en": return "General";
                                    case "de": return "Allgemeine";
                                    case "ua": return "Загальні";
                                    default: return toReturn;
                                }

                            case "bClose":
                                switch (lang)
                                {
                                    case "ru": return "Закрыть";
                                    case "en": return "Close";
                                    case "de": return "Schließen";
                                    case "ua": return "Закрити";
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
                                    case "de": return "ACHTUNG!!! In die Ausnahmeliste sollen nur die wichtigsten Programme aufgenommen werden.";
                                    case "ua": return "УВАГА!!! У список виключень рекомендується додавати дійсно важливі програми.";
                                    default: return toReturn;
                                }


                            /* -----------------------------
                             *      Очистка кэша
                             * -----------------------------*/
                            case "bClearAutorization":
                                switch (lang)
                                {
                                    case "ru": return "Отменить авторизацию WG";
                                    case "en": return "Deauthorize WG";
                                    case "de": return "WG zu deaktivieren";
                                    case "ua": return "Скасувати авторизацію WG";
                                    default: return toReturn;
                                }

                            case "ClearCacheSure":
                                switch (lang)
                                {
                                    case "ru": return "Вы точно хотите отменить авторизацию Wargaming OpenID?";
                                    case "en": return "Are you sure you want to cancel the authorization Wargaming OpenID?";
                                    case "de": return "Sind Sie sicher, dass Sie die Berechtigung Wargaming OpenID abbrechen?";
                                    case "ua": return "Ви точно хочете скасувати авторизацію Wargaming OpenID?";
                                    default: return toReturn;
                                }

                            case "ClearCacheSuccess":
                                switch (lang)
                                {
                                    case "ru": return "Авторизация успешно отменена!";
                                    case "en": return "Authorization successfully canceled!";
                                    case "de": return "Autorisierung erfolgreich abgesagt!";
                                    case "ua": return "Авторизація успішно скасована!";
                                    default: return toReturn;
                                }

                            case "ClearCacheError":
                                switch (lang)
                                {
                                    case "ru": return "Ошибка отмены авторизации. Попробуйте позднее.";
                                    case "en": return "Error cancellation of authorization. Try again later.";
                                    case "de": return "Fehler Widerruf der Zulassung. Versuchen Sie es später noch einmal.";
                                    case "ua": return "Помилка скасування авторизації. Спробуйте пізніше.";
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
                                    case "ua": return "Помилка";
                                    default: return toReturn;
                                }

                            case "ErrorStatus":
                                switch (lang)
                                {
                                    case "ru": return "Возникла ошибка";
                                    case "en": return "An error has occurred";
                                    case "de": return "Es ist ein Fehler aufgetreten";
                                    case "ua": return "Виникла помилка";
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
                                    case "ua": return "Зворотній зв'язок";
                                    default: return toReturn;
                                }

                            case "tbComment":
                                switch (lang)
                                {
                                    case "ru": return "Если у Вас возникли проблемы в работе лаунчера и/или модов, или же есть какие-либо пожелания, Вы можете заполнить форму ниже и отправить сообщение разработчикам:";
                                    case "en": return "In case of problems with Launcher and/or mods, or if you have any ideads, fill this form and send the message to developer.";
                                    case "de": return "Bei Problemen mit dem Launcher, der Installation oder Vorschlägen füllen Sie bitte das weiter unten stehender Formular aus und schicken Sie es an die Entwickler";
                                    case "ua": return "Якщо у Вас виникли проблеми в роботі лаунчера та / або модів, або ж є якісь побажання, Ви можете заповнити форму нижче і відправити повідомлення розробникам";
                                    default: return toReturn;
                                }

                            case "lSelectCategory":
                                switch (lang)
                                {
                                    case "ru": return "Выберите категорию:";
                                    case "en": return "Choose category";
                                    case "de": return "Bitte wählen Sie eine Kategorie aus!";
                                    case "ua": return "Виберіть категорію";
                                    default: return toReturn;
                                }

                            case "lSetEmail":
                                switch (lang)
                                {
                                    case "ru": return "Введите Ваш email:";
                                    case "en": return "Please, enter you email:";
                                    case "de": return "Tragen Sie ihre email Adresse ein";
                                    case "ua": return "Введіть Ваш email:";
                                    default: return toReturn;
                                }

                            case "UncorrectEmail":
                                switch (lang)
                                {
                                    case "ru": return "Вы указали некорректный email-адрес!";
                                    case "en": return "Incorrect email!";
                                    case "de": return "Die angegebene E-Mail-Adresse ist nicht korrekt!";
                                    case "ua": return "Ви вказали некоректний email-адресу!";
                                    default: return toReturn;
                                }

                            case "SendNow":
                                switch (lang)
                                {
                                    case "ru": return "Отправить сейчас?";
                                    case "en": return "Send now?";
                                    case "de": return "Jetzt Senden";
                                    case "ua": return "Надіслати зараз?";
                                    default: return toReturn;
                                }

                            case "bAttach":
                                switch (lang)
                                {
                                    case "ru": return "Прикрепить файл";
                                    case "en": return "Attach file";
                                    case "de": return "Anhang hinzufügen";
                                    case "ua": return "Прикріпити файл";
                                    default: return toReturn;
                                }

                            case "SymbolsTitle":
                                switch (lang)
                                {
                                    case "ru": return "Набрано символов:";
                                    case "en": return "Total symbols:";
                                    case "de": return "Anzahl gewählte Zeichen:";
                                    case "ua": return "Набрано символів";
                                    default: return toReturn;
                                }

                            case "MinimumSymbols":
                                switch (lang)
                                {
                                    case "ru": return "Минимальная длина сообщения: " + text + " символов";
                                    case "en": return "Maximum message lenght is" + text + "symbols";
                                    case "de": return "Minimale Zeichenanzahl eines Beitrags beträgt: " + text + " Zeichen";
                                    case "ua": return "Мінімальна довжина повідомлення: " + text + " символів";
                                    default: return toReturn;
                                }

                            case "bSend":
                                switch (lang)
                                {
                                    case "ru": return "Отправить";
                                    case "en": return "Send";
                                    case "de": return "Senden";
                                    case "ua": return "Відправити";
                                    default: return toReturn;
                                }

                            case "bClose":
                                switch (lang)
                                {
                                    case "ru": return "Выход";
                                    case "en": return "Exit";
                                    case "de": return "Beenden";
                                    case "ua": return "Вихід";
                                    default: return toReturn;
                                }

                            case "TicketSaved":
                                switch (lang)
                                {
                                    case "ru": return "Тикет успешно сохранен и будет автоматически отправлен на сервер познее.";
                                    case "en": return "Ticket successfully saved and will be automatically sent to the server within.";
                                    case "de": return "Ticket erfolgreich gespeichert und wird automatisch an den Server innerhalb gesendet werden.";
                                    case "ua": return "Тікет успішно збережений і буде автоматично відправлений на сервер пізніше";
                                    default: return toReturn;
                                }

                            /* -----------------------------
                             *      Categories
                             * -----------------------------*/

                            case "rbWishMultipack":
                                switch (lang)
                                {
                                    case "ru": return "Пожелания к мультипаку";
                                    case "en": return "Wishes to Multipack";
                                    case "de": return "Wünsche zum Multipack";
                                    case "ua": return "Побажання до Мультипака";
                                    default: return toReturn;
                                }

                            case "rbWishLauncher":
                                switch (lang)
                                {
                                    case "ru": return "Пожелания к лаунчеру";
                                    case "en": return "Wishes to Launcher";
                                    case "de": return "Wünsche zum Launcher";
                                    case "ua": return "Побажання до лаунчера";
                                    default: return toReturn;
                                }

                            case "rbWishInstaller":
                                switch (lang)
                                {
                                    case "ru": return "Пожелания к установщику";
                                    case "en": return "Wishes to Installer";
                                    case "de": return "Wünsche zum Installer";
                                    case "ua": return "Побажання до установника";
                                    default: return toReturn;
                                }

                            case "rbErrorMultipack":
                                switch (lang)
                                {
                                    case "ru": return "Найдена ошибка в мультипаке";
                                    case "en": return "Error in Multipack is found";
                                    case "de": return "Fehler im Multipack";
                                    case "ua": return "Знайдена помилка в мультипаку";
                                    default: return toReturn;
                                }

                            case "rbErrorLauncher":
                                switch (lang)
                                {
                                    case "ru": return "Найдена ошибка в лаунчере";
                                    case "en": return "Error in Louncher is found";
                                    case "de": return "Fehler im Launcher";
                                    case "ua": return "Знайдена помилка в лаунчері";
                                    default: return toReturn;
                                }

                            case "rbErrorInstaller":
                                switch (lang)
                                {
                                    case "ru": return "Найдена ошибка в установщике";
                                    case "en": return "Error in Installer is found";
                                    case "de": return "Fehler bei der Installation";
                                    case "ua": return "Знайдена помилка в установнику";
                                    default: return toReturn;
                                }


                            /* -----------------------------
                             *      Statuses
                             * -----------------------------*/
                            case "OK":
                                switch (lang)
                                {
                                    case "ru": return "Тикет успешно зарегистрирован под номером " + text;
                                    case "en": return "Ticket successfully registered. #" + text;
                                    case "de": return "Fehlermeldung erfolgreich übermittelt unter der Nummer " + text;
                                    case "ua": return "Тікет успішно зареєстровано за номером " + text;
                                    default: return toReturn;
                                }

                            case "BANNED":
                                switch (lang)
                                {
                                    case "ru": return "Ваше сообщение принято к обработке.";
                                    case "en": return "Your message is accepted.";
                                    case "de": return "Ihre Anfrage wird bearbeitet.";
                                    case "ua": return "Ваше повідомлення прийнято до обробки.";
                                    default: return toReturn;
                                }

                            case "FAIL":
                                switch (lang)
                                {
                                    case "ru": return "Ошибка отправки сообщения. Попробуйте еще раз.";
                                    case "en": return "Error sending message. Please, try again later.";
                                    case "de": return "Beim Senden ist ein Fehler aufgetreten. Bitte versuchen Sie es später erneut.";
                                    case "ua": return "Помилка відправки повідомлення. Спробуйте ще раз.";
                                    default: return toReturn;
                                }

                            case "OFFLINE":
                                switch (lang)
                                {
                                    case "ru": return "Сервер временно недоступен.";
                                    case "en": return "The server is temporarily unavailable.";
                                    case "de": return "Der Server ist momentan nicht verfügbar.";
                                    case "ua": return "Сервер тимчасово недоступний.";
                                    default: return toReturn;
                                }


                            //  Статусные сообщения
                            case "DATA_UNVAILABLE":
                                switch (lang)
                                {
                                    case "ru": return "Сервер не получил данные. Попробуйте еще раз.";
                                    case "en": return "The server did not receive the data. Try again.";
                                    case "de": return "Der Server hat die Daten zu empfangen. Versuchen Sie es erneut.";
                                    case "ua": return "Сервер не отримав дані. Спробуйте ще раз.";
                                    default: return toReturn;
                                }

                            case "ERROR_DATA":
                                switch (lang)
                                {
                                    case "ru": return "Ошибка получения данных. Попробуйте еще раз.";
                                    case "en": return "Failed to get data. Please try again later.";
                                    case "de": return "Daten konnten nicht bekommen. Versuchen Sie es erneut.";
                                    case "ua": return "Помилка отримання даних. Спробуйте ще раз.";
                                    default: return toReturn;
                                }

                            case "TICKET_ADD_ERROR":
                                switch (lang)
                                {
                                    case "ru": return "Ошибка обработки тикета. Попробуйте позже.";
                                    case "en": return "Error processing the ticket. Please try again later.";
                                    case "de": return "Fehler bei der Verarbeitung des Tickets. Bitte versuchen Sie es später noch einmal.";
                                    case "ua": return "Помилка обробки тікета. Спробуйте пізніше.";
                                    default: return toReturn;
                                }

                            case "SOFTWARE_NOT_AUTORIZED":
                                switch (lang)
                                {
                                    case "ru": return "Приложение не авторизовано на сервере. Попробуйте позже или обратитесь к разработчику.";
                                    case "en": return "The application is not authorized on the server. Please try again later or contact the manufacturer.";
                                    case "de": return "Die Anwendung ist nicht auf dem Server autorisiert. Bitte versuchen Sie es später noch einmal oder kontaktieren Sie den Hersteller.";
                                    case "ua": return "Додаток не авторизовано на сервері. Спробуйте пізніше або зверніться до розробника.";
                                    default: return toReturn;
                                }

                            case "SERVER_IS_UNAVAILABLE":
                                switch (lang)
                                {
                                    case "ru": return text != "null" && text != "" ? Environment.NewLine + Environment.NewLine + "Сервер ответил: " + Environment.NewLine + text : "";
                                    case "en": return text != "null" && text != "" ? Environment.NewLine + Environment.NewLine + "The server responded: " + Environment.NewLine + text : "";
                                    case "de": return text != "null" && text != "" ? Environment.NewLine + Environment.NewLine + "Der Server reagiert: " + Environment.NewLine + text : "";
                                    case "ua": return text != "null" && text != "" ? Environment.NewLine + Environment.NewLine + "Сервер відповів: " + Environment.NewLine + text : "";
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
                                    case "ua": return "Ласкаво просимо";
                                    default: return toReturn;
                                }

                            case "lNews":
                                switch (lang)
                                {
                                    case "ru": return "НОВОСТИ WARGAMING";
                                    case "en": return "NEWS WARGAMING";
                                    case "de": return "NEUIGKEITEN WARGAMING";
                                    case "ua": return "НОВИНИ WARGAMING";
                                    default: return toReturn;
                                }

                            case "lVideo":
                                switch (lang)
                                {
                                    case "ru": return "ВИДЕО PROТанки";
                                    case "en": return "VIDEO PROTanki";
                                    case "de": return "VIDEO PROTanki";
                                    case "ua": return "ВІДЕО PROТанки";
                                    default: return toReturn;
                                }

                            case "RecordsNotFound":
                                switch (lang)
                                {
                                    case "ru": return "Записи не обнаружены";
                                    case "en": return "No records found";
                                    case "de": return "Aufzeichnungen nicht gefunden";
                                    case "ua": return "Записи не виявлені";
                                    default: return toReturn;
                                }

                            case "ShowVideo":
                                switch (lang)
                                {
                                    case "ru": return "Смотреть видео";
                                    case "en": return "Watch the video";
                                    case "de": return "Video anzeigen";
                                    case "ua": return "Дивитися відео";
                                    default: return toReturn;
                                }

                            case "ShowNews":
                                switch (lang)
                                {
                                    case "ru": return "Читать новость";
                                    case "en": return "Read news";
                                    case "de": return "News lesen";
                                    case "ua": return "Читати новину";
                                    default: return toReturn;
                                }

                            case "base":
                                switch (lang)
                                {
                                    case "ru": return "Базовая версия";
                                    case "en": return "Base version";
                                    case "de": return "Basisversion";
                                    case "ua": return "Базова версія";
                                    default: return toReturn;
                                }

                            case "extended":
                                switch (lang)
                                {
                                    case "ru": return "Расширенная версия";
                                    case "en": return "Extended version";
                                    case "de": return "Erweiterte Version";
                                    case "ua": return "Розширена версія";
                                    default: return toReturn;
                                }

                            case "UpdatesMultipack":
                                switch (lang)
                                {
                                    case "ru": return "Обнаружена новая версия мультипака: " + text;
                                    case "en": return "A new version of Multipack is available: " + text;
                                    case "de": return "Es gibt eine neue Version von Multipack: " + text;
                                    case "ua": return "Виявлена ​​нова версія мультипака: " + text;
                                    default: return toReturn;
                                }

                            case "UpdatesGame":
                                switch (lang)
                                {
                                    case "ru": return "Обнаружена новая версия клиента игры: " + text;
                                    case "en": return "A new version of \"World of Tanks\" is available: " + text;
                                    case "de": return "Es gibt eine neue Version von \"World of Tanks\" : " + text;
                                    case "ua": return "Виявлена ​​нова версія \"World of Tanks\" : " + text;
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
                                    case "ru": return "Обнаружена новая версия: " + text;
                                    case "en": return "A new version is available: " + text;
                                    case "de": return "Es gibt eine neue Version: " + text;
                                    case "ua": return "Виявлена ​​нова версія:" + text;
                                    default: return toReturn;
                                }

                            case "lDownloadFromLink":
                                switch (lang)
                                {
                                    case "ru": return "Новая версия доступна по ссылке в описании к видео";
                                    case "en": return "New version is available on the link in the description of the video.";
                                    case "de": return "Download für die neue version befindet sich in der Videobeschreibung";
                                    case "ua": return "Нова версія доступна за посиланням в опису до відео";
                                    default: return toReturn;
                                }

                            case "cbNotify":
                                switch (lang)
                                {
                                    case "ru": return "Не уведомлять меня об этой версии";
                                    case "en": return "Do not notify me about this version";
                                    case "de": return "Ich möchte keine Benachrichtigungen mehr erhalten";
                                    case "ua": return "Не повідомляти мене про цю версію";
                                    default: return toReturn;
                                }

                            case "bUpdate":
                                switch (lang)
                                {
                                    case "ru": return "Скачать";
                                    case "en": return "Download";
                                    case "de": return "Download";
                                    case "ua": return "Завантажити";
                                    default: return toReturn;
                                }

                            case "bCancel":
                                switch (lang)
                                {
                                    case "ru": return "Не надо";
                                    case "en": return "Cancel";
                                    case "de": return "Abbrechen";
                                    case "ua": return "Не треба";
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
                                    case "en": return "Officer";
                                    case "de": return "Offizier";
                                    case "ua": return "Офіцер";
                                    default: return toReturn;
                                }

                            case "tbPercentWins":
                                switch (lang)
                                {
                                    case "ru": return "Процент побед";
                                    case "en": return "Winrate";
                                    case "de": return "Winrate in Prozent";
                                    case "ua": return "Відсоток перемог";
                                    default: return toReturn;
                                }

                            case "tbMyRating":
                                switch (lang)
                                {
                                    case "ru": return "Личный рейтинг";
                                    case "en": return "Personal rating";
                                    case "de": return "Mein Ranking";
                                    case "ua": return "Особистий рейтинг";
                                    default: return toReturn;
                                }

                            case "tbAvgXP":
                                switch (lang)
                                {
                                    case "ru": return "Средний опыт за бой";
                                    case "en": return "Average experience per battle";
                                    case "de": return "Durchschnittliche Erfahrung pro Gefecht";
                                    case "ua": return "Середній досвід за бій";
                                    default: return toReturn;
                                }

                            case "tbCountWars":
                                switch (lang)
                                {
                                    case "ru": return "Количество боев";
                                    case "en": return "Qty of battles";
                                    case "de": return "Gespielte Gefechte";
                                    case "ua": return "Кількість боїв";
                                    default: return toReturn;
                                }

                            case "tbAvgDamage":
                                switch (lang)
                                {
                                    case "ru": return "Средний нанесенный урон за бой";
                                    case "en": return "Average damage per battle";
                                    case "de": return "Im durchschnitt erteilter Schaden pro Gefecht";
                                    case "ua": return "Середній завданий урон за бій";
                                    default: return toReturn;
                                }

                            /*
                             *  Clan subpage
                             */
                            case "tbPersonnel":
                                switch (lang)
                                {
                                    case "ru": return "Личный состав";
                                    case "en": return "Personnel";
                                    case "de": return "Personal";
                                    case "ua": return "Особовий склад";
                                    default: return toReturn;
                                }

                            case "tbDataOn":
                                switch (lang)
                                {
                                    //  Например,
                                    //      Данные на 29.09.2014 1:03
                                    case "ru": return "Данные на";
                                    case "en": return "The data on";
                                    case "de": return "Daten für";
                                    case "ua": return "Дані на";
                                    default: return toReturn;
                                }

                            case "ErrorDataJson":
                                switch (lang)
                                {
                                    case "ru": return "Ошибка получения данных!";
                                    case "en": return "Error recieving data!";
                                    case "de": return "Fehler: Keine Daten erhalten!";
                                    case "ua": return "Помилка отримання даних!";
                                    default: return toReturn;
                                }

                            case "ActivateWarID":
                                switch (lang)
                                {
                                    case "ru": return "Требуется авторизация для получения информации по Вашему аккаунту!";
                                    case "en": return "You need to login in order to recieve account information!";
                                    case "de": return "Sie müssen sich anmelden, um auf Ihre Daten zugreifen zu können!";
                                    case "ua": return "Необхідно ввійти для отримання інформації за Вашим аккаунтом!";
                                    default: return toReturn;
                                }

                            case "RepeatActivation":
                                switch (lang)
                                {
                                    case "ru": return "Повторить активацию?";
                                    case "en": return "Repeat the activation?";
                                    case "de": return "Wiederholen Sie den Aktivierungs?";
                                    case "ua": return "Повторити активацію?";
                                    default: return toReturn;
                                }

                            case "NotClan":
                                switch (lang)
                                {
                                    case "ru": return "Вы не состоите в клане";
                                    case "en": return "You are not in a clan";
                                    case "de": return "Sie sind in keinem Clan";
                                    case "ua": return "Ви не перебуваєте в клані";
                                    default: return toReturn;
                                }

                            case "ClanBattlesNoRecords":
                                switch (lang)
                                {
                                    case "ru": return "У клана отсутствуют назначенные бои";
                                    case "en": return "There are no designated clan battles";
                                    case "de": return "Es sind keine benannten Clan Schlachten";
                                    case "ua": return "У клану відсутні призначені бої";
                                    default: return toReturn;
                                }

                            case "ClanProvincesNoRecords":
                                switch (lang)
                                {
                                    case "ru": return "У клана нет провинций на Глобальной карте";
                                    case "en": return "The clan has no provinces on the Global Map";
                                    case "de": return "Der Clan hat keine Provinzen auf der Weltkarte";
                                    case "ua": return "У клану немає провінцій на Глобальній карті";
                                    default: return toReturn;
                                }

                            case "tbBattles":
                                switch (lang)
                                {
                                    case "ru": return "Назначенные бои клана:";
                                    case "en": return "Assigned fights Clan:";
                                    case "de": return "Zugewiesen Kämpfe Clan:";
                                    case "ua": return "Призначені бої клану:";
                                    default: return toReturn;
                                }

                            case "tbProvinces":
                                switch (lang)
                                {
                                    case "ru": return "Провинции клана:";
                                    case "en": return "Province of Clan:";
                                    case "de": return "Provinz im Clan:";
                                    case "ua": return "Провінції клану:";
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
                                    case "en": return "Commander";
                                    case "de": return "Kommandant";
                                    case "ua": return "Командир";
                                    default: return toReturn;
                                }

                            case "vice_leader":
                                switch (lang)
                                {
                                    case "ru": return "Зам. командира";
                                    case "en": return "Deputy commander";
                                    case "de": return "Stellvertreter des Kommandanten";
                                    case "ua": return "Заст. командира";
                                    default: return toReturn;
                                }

                            case "commander":
                                switch (lang)
                                {
                                    case "ru": return "Ротный командир";
                                    case "en": return "Company commander";
                                    case "de": return "Kompaniechef";
                                    case "ua": return "Ротний командир";
                                    default: return toReturn;
                                }

                            case "private":
                                switch (lang)
                                {
                                    case "ru": return "Солдат";
                                    case "en": return "Soldier";
                                    case "de": return "Soldat";
                                    case "ua": return "Солдат";
                                    default: return toReturn;
                                }

                            case "recruit":
                                switch (lang)
                                {
                                    case "ru": return "Новобранец";
                                    case "en": return "Recruit";
                                    case "de": return "Rekrut";
                                    case "ua": return "Новобранець";
                                    default: return toReturn;
                                }

                            case "recruiter":
                                switch (lang)
                                {
                                    case "ru": return "Вербовщик";
                                    case "en": return "Recruiter";
                                    case "de": return "Anwerber";
                                    case "ua": return "Вербувальник";
                                    default: return toReturn;
                                }

                            case "junior_officer":
                                switch (lang)
                                {
                                    case "ru": return "Младший офицер";
                                    case "en": return "Junior officer";
                                    case "de": return "Unteroffizier";
                                    case "ua": return "Молодший офіцер";
                                    default: return toReturn;
                                }

                            case "reservist":
                                switch (lang)
                                {
                                    case "ru": return "Резервист";
                                    case "en": return "Reservist";
                                    case "de": return "Reservist";
                                    case "ua": return "Резервіст";
                                    default: return toReturn;
                                }

                            //default: return toReturn;
                            default: return control;
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
                                    case "ua": return "Оптимізувати Ваш ПК для підняття FPS";
                                    default: return toReturn;
                                }

                            case "GlobalProcesses":
                                switch (lang)
                                {
                                    case "ru": return "Процессы из глобального списка приоритетов не могут быть отключены.";
                                    case "en": return "Processes from global list cannot be deactivated.";
                                    case "de": return "Die wichtigen Systemprozesse können nicht beendet werden";
                                    case "ua": return "Процеси з глобального списку пріоритетів не можуть бути відключені.";
                                    default: return toReturn;
                                }

                            default: return toReturn;
                        }




                    /* =====================================
                     *      POST.Class
                     * =====================================*/
                    case "PostClass":
                        switch (control)
                        {
                            case "AutoTicket":
                                switch (lang)
                                {
                                    case "ru": return "Тикеты успешно отправлены";
                                    case "en": return "Tickets successfully sent";
                                    case "de": return "Tickets erfolgreich gesendet";
                                    case "ua": return "Тікети успішно відправлено";
                                    default: return toReturn;
                                }

                            case "AutoTicketWait":
                                switch (lang)
                                {
                                    case "ru": return "Ожидание отправки тикетов";
                                    case "en": return "Waiting for sending tickets";
                                    case "de": return "Warten auf Senden Tickets";
                                    case "ua": return "Очікування відправки тікетів";
                                    default: return toReturn;
                                }

                            case "AutoTicketCount":
                                switch (lang)
                                {
                                    case "ru": return "Успешно отправлено: " + text;
                                    case "en": return "Successfully sent: " + text;
                                    case "de": return "Erfolgreich gesendet: " + text;
                                    case "ua": return "Успішно відправлено";
                                    default: return toReturn;
                                }

                            case "AutoTicketStatus":
                                switch (lang)
                                {
                                    case "ru": return "Присвоенные номера: " + Environment.NewLine + text;
                                    case "en": return "Assigned numbers: " + Environment.NewLine + text;
                                    case "de": return "Zugeordneten Nummern: " + Environment.NewLine + text;
                                    case "ua": return "Присвоєні номери: " + Environment.NewLine + text;
                                    default: return toReturn;
                                }

                            case "EmailAnswer":
                                switch (lang)
                                {
                                    case "ru": return "Ответ будет отправлен на email";
                                    case "en": return "The answer will be sent by email";
                                    case "de": return "Die Antwort wird per E-Mail gesendet werden";
                                    case "ua": return "Відповідь буде відправлено на email";
                                    default: return toReturn;
                                }

                            default: return toReturn;
                        }



                    /* =====================================
                     *      Give Email
                     * =====================================*/
                    case "GiveEmail":
                        switch (control)
                        {
                            case "Caption":
                                switch (lang)
                                {
                                    case "ru": return "Подтверждение запроса";
                                    case "en": return "Confirmation request";
                                    case "de": return "Sicherheitsabfrage";
                                    case "ua": return "Підтвердження запиту";
                                    default: return toReturn;
                                }

                            case "Message":
                                switch (lang)
                                {
                                    case "ru": return "Сервису будет предоставлен доступ к Вашим личным данным:" + Environment.NewLine +
                                                      "> имя пользователя;" + Environment.NewLine +
                                                      "> email-адрес." + Environment.NewLine + Environment.NewLine +
                                                      "Администрация \"AI RUS - Professional IT support\" гарантирует использование Ваших данных только для общения в тикет-системе и исключает передачу оных данных третьим лицам.";
                                    case "en": return "Service will have access to your personal data:" + Environment.NewLine +
                                                      "> username;" + Environment.NewLine +
                                                      "> email-адрес." + Environment.NewLine + Environment.NewLine +
                                                      "Administration \"AI RUS - Professional IT support\" guarantees the use of your data only for communication in the ticket system and prevents the transmission of the add data to third parties.";
                                    case "de": return "Dienst haben Zugang zu Ihren persönlichen Daten:" + Environment.NewLine +
                                                      "> benutzername;" + Environment.NewLine +
                                                      "> e-mail-addresse." + Environment.NewLine + Environment.NewLine +
                                                      "Administration \"AI RUS - Professionelle IT-Support\" gewährleistet die Nutzung Ihrer Daten nur für die Kommunikation im Ticketsystem und verhindert die Übertragung der Zusatzdaten an Dritte weiter.";
                                    case "ua": return "Сервісу буде надано доступ до Ваших особистих даних:" + Environment.NewLine +
                                                      "> ім'я користувача;" + Environment.NewLine +
                                                      "> email-адрес." + Environment.NewLine + Environment.NewLine +
                                                      "Адміністрація \"AI RUS - Professional IT support\" гарантує використання Ваших даних тільки для спілкування в тікет-системі і виключає передачу оних даних третім особам.";
                                    default: return toReturn;
                                }

                            default: return toReturn;
                        }

						
                    /* =====================================
                     *      Tooltip
                     * =====================================*/
                    case "Tooltip":
                        switch (control)
                        {
                            case "rectLang":
                                switch (lang)
                                {
                                    case "ru": return "Изменить язык интерфейса";
                                    case "en": return "Change language";
                                    case "de": return "Sprache Ändern";
                                    case "ua": return "Змінити мову інтерфейсу";
                                    default: return toReturn;
                                }
								
                            case "bUpdate":
                                switch (lang)
                                {
                                    case "ru": return "Обновить мультипак";
                                    case "en": return "Update Multipack";
                                    case "de": return "Update-Multipack";
                                    case "ua": return "Оновити Мультипак";
                                    default: return toReturn;
                                }
								
                            case "bNotification":
                                switch (lang)
                                {
                                    case "ru": return "Изменить язык интерфейса";
                                    case "en": return "Modified List";
                                    case "de": return "Geändert Liste";
                                    case "ua": return "Список змін";
                                    default: return toReturn;
                                }
								
                            case "bUserProfile":
                                switch (lang)
                                {
                                    case "ru": return "Личный кабинет";
                                    case "en": return "Personal cabinet";
                                    case "de": return "Persönlichen Cabinet";
                                    case "ua": return "Особистий кабінет";
                                    default: return toReturn;
                                }
								
								
								
                            case "bSettings":
                                switch (lang)
                                {
                                    case "ru": return "Настройки";
                                    case "en": return "Settings";
                                    case "de": return "Einstellungen";
                                    case "ua": return "Налаштування11";
                                    default: return toReturn;
                                }
								
                            case "bFeedback":
                                switch (lang)
                                {
                                    case "ru": return "Обратная связь";
                                    case "en": return "Feedback";
                                    case "de": return "Rückkopplung";
                                    case "ua": return "Зворотній зв'язок";
                                    default: return toReturn;
                                }
								
                            case "bLauncher":
                                switch (lang)
                                {
                                    case "ru": return "Лаунчер 'World of tanks'";
                                    case "en": return "'World of tanks' Launcher";
                                    case "de": return "'World of tanks' Launcher";
                                    case "ua": return "Лаунчер 'World of tanks'";
                                    default: return toReturn;
                                }
								
                            case "bOptimize":
                                switch (lang)
                                {									
                                    case "ru": return "Оптимизировать Ваш ПК для поднятия FPS";
                                    case "en": return "Optimize your PC for maximum FPS";
                                    case "de": return "Optimieren Sie Ihren PC für maximale FPS";
                                    case "ua": return "Оптимізувати Ваш ПК для підняття FPS";
                                    default: return toReturn;
                                }

                            default: return toReturn;
                        }

                    default: return toReturn;
                }
            }
            catch (Exception ex) { System.Threading.Tasks.Task.Factory.StartNew(() => new Classes.Debugging().Save("Language.Class", "Set()", "Page: " + page, "Control: " + control, "Lang: " + lang, "Text: " + (text.Length > 0 ? text : "null"), ex.Message, ex.StackTrace)); }

            return toReturn;
        }
    }
}