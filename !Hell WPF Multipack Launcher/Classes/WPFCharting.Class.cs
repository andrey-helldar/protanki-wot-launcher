using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace WPFCharting
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private MainWindowModel _mwm;
        public MainWindowVM()
        {
            _mwm = new MainWindowModel();
            _data = _mwm;
            propChanged("data");

        }
        private ObservableCollection<KeyValuePair<string, int>> _data;
        public ObservableCollection<KeyValuePair<string, int>> data
        {
            get { return _data; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void propChanged(String propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }
    }
}