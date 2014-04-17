﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _Hell_Language_Pack
{
    public class fLanguage
    {
        public void toolTip(Control sender, string lang = null)
        {
            ToolTip toolTip = new ToolTip();

            toolTip.AutoPopDelay = 2000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            switch (sender.Name)
            {
                case "bOptimizePC":
                    toolTip.SetToolTip(sender, lang == "ru" ? "Оптимизировать Ваш ПК для поднятия FPS" : "");
                    break;

                case "llUserProcesses":
                    toolTip.SetToolTip(sender, lang == "ru" ? "Процессы, выбранные Вами, могут быть убраны из списка приоритета" : "");
                    break;

                case "llGlobalProcesses":
                    toolTip.SetToolTip(sender, lang == "ru" ? "Процессы из глобального списка приоритетов не могут быть отключены." + Environment.NewLine + "Даже при снятии галки они автоматически будут включены." : "");
                    break;

                default: break;

            }
        }

        public string InterfaceLanguage(string formName, Control sender, string lang)
        {
            switch (formName)
            {
                // Главная форма
                case "fIndex":
                    switch (sender.Name)
                    {
                        case "bPlay": return lang == "ru" ? "Играть" : "Play";
                        case "bLauncher": return lang == "ru" ? "Лаунчер" : "Launcher";
                        case "bUpdate": return lang == "ru" ? "Обновить" : "Update";
                        case "bVideo": return lang == "ru" ? "Видео" : "Video"; 
                        case "bOptimizePC": return lang == "ru" ? "Оптимизировать" : "Optimize";
                        case "bExit": return lang == "ru" ? "Выход" : "Exit"; 
                        case "bShowVideo": return lang == "ru" ? "Видео" : "Video";
                        case "bShowNews": return lang == "ru" ? "Новости" : "News"; 
                        case "bSettings": return lang == "ru" ? "Настройки" : "Settings"; 
                        case "llBlockCaption": return lang == "ru" ? "Видео" : "Video";
                        case "llLoadingVideoData": return lang == "ru" ? "Подождите, идет загрузка..." : "Please, wait...";
                        case "llActually": return lang == "ru" ? "Вы используете самые свежие моды!" : "Video";
                        default: return sender.Text;
                    }

                case "fSettings":
                    // Форма настроек
                    switch (sender.Name)
                    {
                        case "llTitle": return lang == "ru" ? "Настройки..." : "Settings..."; 
                        case "gbOptimization": return lang == "ru" ? "Оптимизация:" : "Optimize:"; 
                        case "cbKillProcesses": return lang == "ru" ? "Закрывать приложения при запуске игры" : "Close processes with start game"; 
                        case "cbForceClose": return lang == "ru" ? "Принудительно завершать приложения" : "Force closing processes"; 
                        case "cbAero": return lang == "ru" ? "Отключать Windows Aero при запуске игры" : "Disable Windows Aero with start game"; 
                        case "cbVideoQuality": return lang == "ru" ? "Уменьшить качество графики в игре" : "Reduce the quality of the graphics in the game"; 
                        case "cbVideoQualityWeak": return lang == "ru" ? "Очень слабый компьютер" : "Very weak computer"; 

                        case "gbOther": return lang == "ru" ? "Уведомлять о новых видео:" : "Notify new video"; 
                        case "gbPriority": return lang == "ru" ? "Приоритет игры в системе:" : "Priority game in system";
                        case "cbBalanceCPU": return lang == "ru" ? "Распределить нагрузку ЦП" : "CPU load distribution";
                        case "gbProcesses": return lang == "ru" ? "Какие процессы НЕЛЬЗЯ закрывать при запуске игры:" : "What processes must NOT CLOSE when the game starts"; 
                        case "lvProcessesUser":
                            ListView listView = new ListView();
                            listView.Columns[0].Text = lang == "ru" ? "Процесс" : "Process";
                            listView.Columns[1].Text = lang == "ru" ? "Описание" : "Description";
                            return sender.Text;
                            
                        case "llUserProcesses": return lang == "ru" ? "Процессы, выбранные пользователем" : "Processes selected by the user"; 
                        case "llGlobalProcesses": return lang == "ru" ? "Процессы из глобального списка" : "Processes from the global list"; 
                        case "lDescProcesses": return lang == "ru" ? "ВНИМАНИЕ!!! В список исключений рекомендуется добавлять действительно важные программы!" : "WARNING! In the list of exceptions is recommended to add the really important programs!"; 
                        case "bSave": return lang == "ru" ? "Сохранить" : "Save & Exit"; 
                        case "bCancel": return lang == "ru" ? "Отмена" : "Cancel"; 
                        case "llRecoverySettings": return lang == "ru" ? "Восстановить настройки..." : "Recovery game settings...";
                        default: return sender.Text;
                    }
                    

                // Форма уведомления о новых версиях
                case "fNewVersion":
                    switch (sender.Name)
                    {
                        case "bDownload": return lang == "ru" ? "Скачать" : "Download"; 
                        case "bCancel": return lang == "ru" ? "Не надо" : "Cancel"; 
                        case "cbNotification": return lang == "ru" ? "Не уведомлять меня об этой версии" : "Do not notify me about this version";
                        default: return sender.Text;
                    }

                // Форма отправки тикетов
                case "fWarning":
                    switch (sender.Name)
                    {
                        case "lDesc": return lang == "ru" ? "Если у Вас возникли проблемы в работе лаунчера или есть какие-либо пожелания, Вы можете заполнить форму ниже и отправить сообщение разработчику:" : "If you have any problems in the launcher or have any comments, you can fill out the form below and send a message to the developer"; 
                        case "rbWish": return lang == "ru" ? "Пожелание к лаунчеру" : "Wishing to launcher"; 
                        case "rbBug": return lang == "ru" ? "Найдена ошибка" : "Found error"; 
                        case "bSend": return lang == "ru" ? "Отправить" : "Send"; 
                        case "bCancel": return lang == "ru" ? "Выход" : "Cancel";
                        default: return sender.Text;
                    }

                default: return sender.Text;
            }
        }
    }
}