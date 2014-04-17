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

                default: break;
                    
            }
        }

        public void InterfaceLanguage(string formName, Control sender, string lang)
        {
            switch (formName)
            {
                // Главная форма
                case "fIndex":
                    switch (sender.Name)
                    {
                        case "bPlay": sender.Text = lang == "ru" ? "Играть" : "Play";
                        case "bLauncher": sender.Text = lang == "ru" ? "Лаунчер" : "Launcher";
                        case "bUpdate": sender.Text = lang == "ru" ? "Обновить" : "Update";
                        case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; 
                        case "bOptimizePC": sender.Text = lang == "ru" ? "Оптимизировать" : "Optimize";
                        case "bExit": sender.Text = lang == "ru" ? "Выход" : "Exit"; 
                        case "bShowVideo": sender.Text = lang == "ru" ? "Видео" : "Video";
                        case "bShowNews": sender.Text = lang == "ru" ? "Новости" : "News"; 
                        case "bSettings": sender.Text = lang == "ru" ? "Настройки" : "Settings"; 
                        case "llBlockCaption": sender.Text = lang == "ru" ? "Видео" : "Video";
                        case "llLoadingVideoData": sender.Text = lang == "ru" ? "Подождите, идет загрузка..." : "Please, wait...";
                        case "llActually": sender.Text = lang == "ru" ? "Вы используете самые свежие моды!" : "Video";
                        default: return null;
                    }

                case "fSettings":
                    // Форма настроек
                    switch (sender.Name)
                    {
                        case "llTitle": sender.Text = lang == "ru" ? "Настройки..." : "Settings..."; 
                        case "gbOptimization": sender.Text = lang == "ru" ? "Оптимизация:" : "Optimize:"; 
                        case "cbKillProcesses": sender.Text = lang == "ru" ? "Закрывать приложения при запуске игры" : "Close processes with start game"; 
                        case "cbForceClose": sender.Text = lang == "ru" ? "Принудительно завершать приложения" : "Force closing processes"; 
                        case "cbAero": sender.Text = lang == "ru" ? "Отключать Windows Aero при запуске игры" : "Disable Windows Aero with start game"; 
                        case "cbVideoQuality": sender.Text = lang == "ru" ? "Уменьшить качество графики в игре" : "Reduce the quality of the graphics in the game"; 
                        case "cbVideoQualityWeak": sender.Text = lang == "ru" ? "Очень слабый компьютер" : "Very weak computer"; 

                        case "gbOther": sender.Text = lang == "ru" ? "Уведомлять о новых видео:" : "Notify new video"; 
                        case "gbPriority": sender.Text = lang == "ru" ? "Приоритет игры в системе:" : "Priority game in system";
                        case "cbBalanceCPU": sender.Text = lang == "ru" ? "Распределить нагрузку ЦП" : "CPU load distribution";
                        case "gbProcesses": sender.Text = lang == "ru" ? "Какие процессы НЕЛЬЗЯ закрывать при запуске игры:" : "What processes must NOT CLOSE when the game starts"; 
                        case "lvProcessesUser":
                            ListView listView = new ListView();
                            listView.Columns[0].Text = lang == "ru" ? "Процесс" : "Process";
                            listView.Columns[1].Text = lang == "ru" ? "Описание" : "Description";
                            
                        case "llUserProcesses": sender.Text = lang == "ru" ? "Процессы, выбранные пользователем" : "Processes selected by the user"; 
                        case "llGlobalProcesses": sender.Text = lang == "ru" ? "Процессы из глобального списка" : "Processes from the global list"; 
                        case "lDescProcesses": sender.Text = lang == "ru" ? "ВНИМАНИЕ!!! В список исключений рекомендуется добавлять действительно важные программы!" : "WARNING! In the list of exceptions is recommended to add the really important programs!"; 
                        case "bSave": sender.Text = lang == "ru" ? "Сохранить" : "Save & Exit"; 
                        case "bCancel": sender.Text = lang == "ru" ? "Отмена" : "Cancel"; 
                        case "llRecoverySettings": sender.Text = lang == "ru" ? "Восстановить настройки..." : "Recovery game settings..."; 
                        default: return null;
                    }
                    

                // Форма уведомления о новых версиях
                case "fNewVersion":
                    switch (sender.Name)
                    {
                        case "bDownload": sender.Text = lang == "ru" ? "Скачать" : "Download"; 
                        case "bCancel": sender.Text = lang == "ru" ? "Не надо" : "Cancel"; 
                        case "cbNotification": sender.Text = lang == "ru" ? "Не уведомлять меня об этой версии" : "Do not notify me about this version"; 
                        default: return null;
                    }

                // Форма отправки тикетов
                case "fWarning":
                    switch (sender.Name)
                    {
                        case "lDesc": sender.Text = lang == "ru" ? "Если у Вас возникли проблемы в работе лаунчера или есть какие-либо пожелания, Вы можете заполнить форму ниже и отправить сообщение разработчику:" : "If you have any problems in the launcher or have any comments, you can fill out the form below and send a message to the developer"; 
                        case "rbWish": sender.Text = lang == "ru" ? "Пожелание к лаунчеру" : "Wishing to launcher"; 
                        case "rbBug": sender.Text = lang == "ru" ? "Найдена ошибка" : "Found error"; 
                        case "bSend": sender.Text = lang == "ru" ? "Отправить" : "Send"; 
                        case "bCancel": sender.Text = lang == "ru" ? "Выход" : "Cancel"; 
                        default: return null;
                    }

                default: return null;
            }
        }
    }
}