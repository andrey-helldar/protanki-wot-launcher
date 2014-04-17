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
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
                case "bVideo": sender.Text = lang == "ru" ? "Видео" : "Video"; break;
            }

        }
    }
}