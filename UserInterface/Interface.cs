using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace UserInterface
{
    public class Interface
    {
        public Bitmap Background(string control = null)
        {
            switch (control)
            {
                case "fIndex": return Properties.Resources.Background;
                case "pbFeedback": return Properties.Resources.Feedback;
                case "pbSettings": return Properties.Resources.Settings;

                default: return null;
            }
        }
    }
}
