using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _Hell_Language_Pack
{
    public class fLanguage
    {
        public void toolTip(Control sender)
        {
            ToolTip toolTip = new ToolTip();

            toolTip.AutoPopDelay = 2000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            switch (sender.Name)
            {
                case "bOptimizePC":
                    toolTip.SetToolTip(sender, "Оптимизировать Ваш ПК для поднятия FPS");
                    break;

                case "llUserProcesses":
                    toolTip.SetToolTip(sender, "Процессы, выбранные Вами, могут быть убраны из списка приоритета");
                    break;

                case "llGlobalProcesses":
                    toolTip.SetToolTip(sender, "Процессы из глобального списка приоритетов не могут быть отключены." + Environment.NewLine + "Даже при снятии галки они автоматически будут включены.");
                    break;

                default:
                    break;
            }
        }

        public string InterfaceLanguage(Control sender, string lang)
        {
            switch (sender.Name)
            {
                    // Главная форма
                case "bPlay": sender.Text = lang == "ru" ? "Играть" : "Play"; break;
                case "bLauncher": sender.Text = lang == "ru" ? "Лаунчер" : "Launcher"; break;
                case "bUpdate": sender.Text = lang == "ru" ? "Обновить" : "Update"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bOptimizePC": sender.Text = lang == "ru" ? "Оптимизировать" : "Optimize"; break;
                case "bExit": sender.Text = lang == "ru" ? "Выход" : "Exit"; break;
                case "bShowVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bShowNews": sender.Text = lang == "ru" ? "Новости" : "News"; break;
                case "bSettings": sender.Text = lang == "ru" ? "Настройки" : "Settings"; break;
                case "llBlockCaption": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "llLoadingVideoData": sender.Text = lang == "ru" ? "Подождите, идет загрузка..." : "Please, wait..."; break;
                case "llActually": sender.Text = lang == "ru" ? "Вы используете самые свежие моды!" : "Video"; break;


                    // Форма настроек
                case "llTitle": sender.Text = lang == "ru" ? "Настройки..." : "Settings..."; break;
                case "gbOptimization": sender.Text = lang == "ru" ? "Оптимизация:" : "Optimize:"; break;
                case "cbKillProcesses": sender.Text = lang == "ru" ? "Закрывать приложения при запуске игры" : "Close processes with start game"; break;
                case "cbForceClose": sender.Text = lang == "ru" ? "Принудительно завершать приложения" : "Force closing processes"; break;
                case "cbAero": sender.Text = lang == "ru" ? "Отключать Windows Aero при запуске игры" : "Disable Windows Aero with start game"; break;
                case "cbVideoQuality": sender.Text = lang == "ru" ? "Уменьшить качество графики в игре" : ; break;
                case "cbVideoQualityWeak": sender.Text = lang == "ru" ? "Очень слабый компьютер" : ; break;

                case "gbOther": sender.Text = lang == "ru" ? "Уведомлять о новых видео:" : ; break;
                case "gbPriority": sender.Text = lang == "ru" ? "Приоритет игры в системе:" : ; break;
                case "gbProcesses": sender.Text = lang == "ru" ? "Какие процессы НЕЛЬЗЯ закрывать при запуске игры:" : ; break;
                case "lvProcessesUser": sender = lang == "ru" ?  :; break;
                case "llUserProcesses": sender.Text = lang == "ru" ? "Процессы, выбранные пользователем" : ; break;
                case "llGlobalProcesses": sender.Text = lang == "ru" ? "Процессы из глобального списка" :; break;
                case "lDescProcesses": sender.Text = lang == "ru" ? "ВНИМАНИЕ!!! В список исключений рекомендуется добавлять действительно важные программы!" : ; break;
                case "bSave": sender.Text = lang == "ru" ? "Сохранить" : "Save & Exit"; break;
                case "bCancel": sender.Text = lang == "ru" ? "Отмена" : "Cancel"; break;
                case "llRecoverySettings": sender.Text = lang == "ru" ? "Восстановить настройки..." : "Recovery game settings..."; break;


                    // Форма уведомления о новых версиях
                case "bDownload": sender.Text = lang == "ru" ? "Скачать" : "Download"; break;
                case "bCancel": sender.Text = lang == "ru" ? "Не надо" : "Cancel"; break;
                case "cbNotification": sender.Text = lang == "ru" ? "Не уведомлять меня об этой версии" : ; break;


                    // Форма отправки тикетов
                case "lDesc": sender.Text = lang == "ru" ? "Если у Вас возникли проблемы в работе лаунчера или есть какие-либо пожелания, Вы можете заполнить форму ниже и отправить сообщение разработчику:" : ; break;
                case "rbWish": sender.Text = lang == "ru" ? "Пожелание к лаунчеру" : ; break;
                case "rbBug": sender.Text = lang == "ru" ? "Найдена ошибка" : ; break;
                case "bSend": sender.Text = lang == "ru" ? "Отправить" : "Send"; break;
                case "bCancel": sender.Text = lang == "ru" ? "Выход" : "Cancel"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
            }

        }
    }
}