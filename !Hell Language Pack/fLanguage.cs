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
                    toolTip.SetToolTip(sender, "Процессы из глобального списка приоритетов не могут быть отключены. Даже при снятии галки они автоматически будут включены.");
                    break;

                default:
                    break;
            }
        }
    }
}
