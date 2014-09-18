using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace WPFCharting
{
    public class MainWindowModel : ObservableCollection<KeyValuePair<string, int>>
    {
        public MainWindowModel()
        {
            init();
        }
        public void init()
        {
            Add(new KeyValuePair<string, int>("Dog", 30));
            Add(new KeyValuePair<string, int>("Cat", 25));
            Add(new KeyValuePair<string, int>("Rat", 5));
            Add(new KeyValuePair<string, int>("Hampster", 8));
            Add(new KeyValuePair<string, int>("Rabbit", 12));
        }
        public ObservableCollection<KeyValuePair<string, int>> getData()
        {
            return this;
        }
    }

}