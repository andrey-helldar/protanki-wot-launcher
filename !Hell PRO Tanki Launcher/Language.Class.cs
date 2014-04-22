using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _Hell_PRO_Tanki_Launcher
{
    class Language
    {
        public void toolTip(Control sender, string lang = "ru")
        {
            ToolTip toolTip = new ToolTip();

            toolTip.AutoPopDelay = 2000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            switch (sender.Name)
            {
                case "bOptimizePC":
                    toolTip.SetToolTip(sender, lang == "ru" ? "Оптимизировать Ваш ПК для поднятия FPS" : "Optimize your PC for maximum FPS");
                    break;

                case "llUserProcesses":
                    toolTip.SetToolTip(sender, lang == "ru" ?
                        "Процессы, выбранные Вами, могут быть убраны из списка приоритета" :
                        "The processes that you have selected can be deleted from priority list");
                    break;

                case "llGlobalProcesses":
                    toolTip.SetToolTip(sender, lang == "ru" ?
                        "Процессы из глобального списка приоритетов не могут быть отключены." + Environment.NewLine + "Даже при снятии галки они автоматически будут включены." :
                        "Processes from global list cannot be deactivated." + Environment.NewLine + "Even if you you uncheck, they will be activated.");
                    break;

                default: break;

            }
        }

        public string InterfaceLanguage(string formName, Control sender, string lang = "en", string type = "base")
        {
            switch (formName)
            {
                /* ************************
                 *         fIndex
                 * ***********************/
                case "fIndex":
                    switch (sender.Name)
                    {
                        case "llTitle":
                            if (lang == "ru")
                                return Application.ProductName + " (" + (type == "full" ? "Расширенная версия" : "Базовая версия") + ")";
                            else
                                return Application.ProductName + " (" + (type == "full" ? "Full" : "Base") + ")";
                        case "bPlay": return lang == "ru" ? "Играть" : "Play";
                        case "bLauncher": return lang == "ru" ? "Лаунчер" : "Launcher";
                        case "bUpdate": return lang == "ru" ? "Обновить" : "Update";
                        case "bVideo": return lang == "ru" ? "Видео" : "Video";
                        case "bOptimizePC": return lang == "ru" ? "Оптимизировать" : "Optimize";
                        case "bExit": return lang == "ru" ? "Выход" : "Exit";
                        case "bShowVideo": return lang == "ru" ? "Видео" : "Video";
                        case "bShowNews": return lang == "ru" ? "Новости" : "News";
                        case "bSettings": return lang == "ru" ? "Настройки" : "Settings";
                        case "llBlockCaption": return lang == "ru" ? "Видео:" : "Video:";
                        case "llLoadingVideoData": return lang == "ru" ? "Подождите, идет загрузка..." : "Downloading, please, wait...";

                        default: return sender.Text;
                    }

                /* ************************
                 *         fSettings
                 * ***********************/
                case "fSettings":
                    switch (sender.Name)
                    {
                        case "llTitle": return lang == "ru" ? "Настройки..." : "Settings...";
                        case "gbOptimization": return lang == "ru" ? "Оптимизация:" : "Optimize:";
                        case "cbKillProcesses": return lang == "ru" ? "Закрывать приложения при запуске игры" : "Close the processes when game starts";
                        case "cbForceClose": return lang == "ru" ? "Принудительно завершать приложения" : "Forcibly terminate the application";
                        case "cbAero": return lang == "ru" ? "Отключать Windows Aero при запуске игры" : "Disable Windows Aero when game starts";
                        case "cbVideoQuality": return lang == "ru" ? "Уменьшить качество графики в игре" : "Reduce the quality of the graphics in the game";
                        case "cbVideoQualityWeak": return lang == "ru" ? "Очень слабый компьютер" : "Very weak computer";

                        case "gbOther": return lang == "ru" ? "Другие:" : "Other settings:";
                        case "cbVideo": return lang == "ru" ? "Уведомлять о новых видео:" : "Notify about new videos";
                        case "gbPriority": return lang == "ru" ? "Приоритет игры в системе:" : "Priority of the game in system";
                        case "cbBalanceCPU": return lang == "ru" ? "Распределить нагрузку ЦП" : "CPU load balancing";
                        case "gbProcesses": return lang == "ru" ? "Какие процессы НЕЛЬЗЯ закрывать при запуске игры:" : "This processes must BE ACTIVE when the game starts";
                        case "llUserProcesses": return lang == "ru" ? "Процессы, выбранные пользователем" : "Processes selected by the user";
                        case "llGlobalProcesses": return lang == "ru" ? "Процессы из глобального списка" : "Processes from the global list";
                        case "lDescProcesses": return lang == "ru" ?
                            "ВНИМАНИЕ!!! В список исключений рекомендуется добавлять действительно важные программы!" :
                            "WARNING! We are strongly recommend you to include only a really necessary programs in to exception list.";
                        case "bSave": return lang == "ru" ? "Сохранить" : "Save";
                        case "bCancel": return lang == "ru" ? "Отмена" : "Cancel";
                        case "llRecoverySettings": return lang == "ru" ? "Восстановить настройки..." : "Recovery game settings...";
                        default: return sender.Text;
                    }


                /* ************************
                 *       fNewVersion
                 * ***********************/
                case "fNewVersion":
                    switch (sender.Name)
                    {
                        case "bDownload": return lang == "ru" ? "Скачать" : "Download";
                        case "bCancel": return lang == "ru" ? "Не надо" : "Cancel";
                        case "cbNotification": return lang == "ru" ? "Не уведомлять меня об этой версии" : "Do not notify me about this version";
                        default: return sender.Text;
                    }

                /* ************************
                 *         fWarning
                 * ***********************/
                case "fWarning":
                    switch (sender.Name)
                    {
                        case "lDesc": return lang == "ru" ?
                            "Если у Вас возникли проблемы в работе лаунчера или есть какие-либо пожелания, Вы можете заполнить форму ниже и отправить сообщение разработчику:" :
                            "If you have any problems with this launcher or have any comments, you can fill out the form below and send a message to the developer";
                        case "rbWish": return lang == "ru" ? "Пожелание к лаунчеру" : "I wish to";
                        case "rbBug": return lang == "ru" ? "Найдена ошибка" : "Error found";
                        case "bSend": return lang == "ru" ? "Отправить" : "Send";
                        case "bCancel": return lang == "ru" ? "Выход" : "Cancel";
                        case "cbAttachDebug": return lang == "ru" ? "Прикрепить логи" : "Attach log-files";
                        default: return sender.Text;
                    }

                default: return sender.Text;
            }
        }

        public string DynamicLanguage(string controlName, string lang = "en", string additionalText = "")
        {
            switch (controlName)
            {
                /* ************************
                 *         fIndex
                 * ***********************/
                case "llVideoAll": return lang == "ru" ? "Все видео" : "All videos";
                case "llActuallyActually": return lang == "ru" ? "Вы используете самые свежие моды" : "You are using the latest mods";
                case "llActuallyNewMods": return lang == "ru" ? "Обнаружена новая версия Мультипака" : "Available new Multipack version";
                case "llActuallyNewGame": return lang == "ru" ? "Обнаружена новая версия игры" : "Available new game version";
                case "llActuallyThisVerMods": return lang == "ru" ? "Текущая версия Мультипака: " : "This Multipack version: ";
                case "llActuallyThisVerGame": return lang == "ru" ? "Текущая версия клиента игры: " : "This WOT version: ";
                case "noMods": return lang == "ru" ? "Мультипак не обнаружен!" : "Multipack not found!";
                case "noTanks": return lang == "ru" ? "Клиент игры не обнаружен!" : "\"World of Tanks\" not found!";
                case "noUpdates": return lang == "ru" ? "Обновления отсутствуют" : "No updates available";

                case "checkUpdates": return lang == "ru" ? "Подождите, предыдущая проверка обновлений не завершена" : "Подождите, предыдущая проверка обновлений не завершена";
                case "reEnterPass": return lang == "ru" ? "Также, после применения настроек графики в игре требуется заново ввести логин/пароль!" : "Также, после применения настроек графики в игре требуется заново ввести логин/пароль!";

                case "noSettings": return lang == "ru" ?
                    "Файл настроек не обнаружен!" + Environment.NewLine + "Лаунчер будет автоматически перезапущен. Во время перезапуска будет применена стандартная конфигурация" :
                    "Launcher has been restart. When restart has been apply default configuration";

                case "video": return lang == "ru" ? "Видео:" : "Video:";
                case "news": return lang == "ru" ? "Новости:" : "News:";

                case "tsShow": return lang == "ru" ? "Главное окно" : "Main window";
                case "tsVideo": return lang == "ru" ? "Видео" : "Video";
                case "tsCheckUpdates": return lang == "ru" ? "Проверить обновления" : "Check updates";
                case "tsSettings": return lang == "ru" ? "Настройки" : "Settings";
                case "tsExit": return lang == "ru" ? "Выход" : "Exit";

                /* ************************
                 *         fWarning
                 * ***********************/
                case "thanks": return lang == "ru" ?
                    "Спасибо за обращение!" + Environment.NewLine + "Разработчик рассмотрит Вашу заявку в ближайшее время" :
                    "Спасибо за обращение!" + Environment.NewLine + "Разработчик рассмотрит Вашу заявку в ближайшее время";
                case "hacking": return lang == "ru" ?
                    "Ведутся работы на сервере. Попробуйте отправить запрос чуть позже." :
                    "Ведутся работы на сервере. Попробуйте отправить запрос чуть позже.";
                case "error": return lang == "ru" ?
                    "Ошибка отправки сообщения. Попробуйте еще раз." :
                    "Ошибка отправки сообщения. Попробуйте еще раз.";
                case "symbolLength": return lang == "ru" ?
                    "Текст не может быть меньше " + additionalText + " символов!" :
                    "Текст не может быть меньше " + additionalText + " символов!";
                case "messAreSended": return lang == "ru" ?
                    "Вы уже отправляли данное сообщение." :
                    "Вы уже отправляли данное сообщение.";
                case "sending": return lang == "ru" ? "Отправка..." : "Sending...";
                case "send": return lang == "ru" ? "Отправить" : "Send";
                case "save": return lang == "ru" ? "Сохранить" : "Save";

                /* ************************
                 *         fSettings
                 * ***********************/
                case "optimizeTitle": return lang == "ru" ? "Оптимизация" : "Optimize";
                case "updatingTitle": return lang == "ru" ? "Обновление" : "Updating";


                case "lvProcessesUser0": return lang == "ru" ? "Процесс" : "Process";
                case "lvProcessesUser1": return lang == "ru" ? "Описание" : "Description";

                case "priority0": return lang == "ru" ? "Высокий" : "High";
                case "priority1": return lang == "ru" ? "Выше среднего" : "Above average";
                case "priority2": return lang == "ru" ? "Средний" : "Average";
                case "priority3": return lang == "ru" ? "Ниже среднего" : "Below the average";
                case "priority4": return lang == "ru" ? "Низкий" : "Low";

                case "reEnterLoginPass": return lang == "ru" ?
                "ВНИМАНИЕ!!!" + Environment.NewLine + "После применения настроек графики в игре требуется заново ввести логин/пароль!" + Environment.NewLine + Environment.NewLine +
                "Настройки графики применяются только при сохранении информации в окне настоек, либо при нажатии на кнопку \"Оптимизировать\" на главном окне программы." + Environment.NewLine +
                "При автоматической оптимизации настройки графики остаются без изменений." :
                "ВНИМАНИЕ!!!" + Environment.NewLine + "После применения настроек графики в игре требуется заново ввести логин/пароль!" + Environment.NewLine + Environment.NewLine +
                "Настройки графики применяются только при сохранении информации в окне настоек, либо при нажатии на кнопку \"Оптимизировать\" на главном окне программы." + Environment.NewLine +
                "При автоматической оптимизации настройки графики остаются без изменений.";
                case "optimize": return lang == "ru" ?
                "ВНИМАНИЕ!!!" + Environment.NewLine + "При оптимизации ПК на время игры будут завершены некоторые пользовательские приложения." + additionalText +
                Environment.NewLine + Environment.NewLine + "Вы хотите продолжить?" :
                "ВНИМАНИЕ!!!" + Environment.NewLine + "При оптимизации ПК на время игры будут завершены некоторые пользовательские приложения." + additionalText +
                Environment.NewLine + Environment.NewLine + "Вы хотите продолжить?";
                case "wait": return lang == "ru" ? "Подождите завершения предыдущей операции" : "Подождите завершения предыдущей операции";

                case "admin": return lang == "ru" ?
                "ВНИМАНИЕ!!!" + Environment.NewLine + "Для выполнения данной операции требуются права администратора!" :
                "ВНИМАНИЕ!!!" + Environment.NewLine + "Для выполнения данной операции требуются права администратора!";

                case "viewVideo": return lang == "ru" ? "Посмотреть видео" : "Show video";

                case "bBalanceCPU1": return lang == "ru" ? "Нагрузка ЦП распределена" : "CPU load is distributed";
                case "bBalanceCPU0": return lang == "ru" ? "Нагрузка ЦП не распределена" : "CPU load is not distributed";

                default: return "null";
            }
        }
    }
}
