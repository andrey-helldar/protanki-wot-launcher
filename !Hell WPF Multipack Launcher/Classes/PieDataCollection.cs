using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PieControls;

namespace _Hell_WPF_Multipack_Launcher.Classes
{
    public class PieDataCollection<T> : ObservableCollection<T> where T : PieSegment
    {
        public string CollectionName { get; set; }
    }
}
