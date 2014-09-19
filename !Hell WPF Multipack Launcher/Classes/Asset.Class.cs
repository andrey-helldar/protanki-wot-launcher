using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace _Hell_WPF_Multipack_Launcher
{
    public class AssetClass : INotifyPropertyChanged
    {
        private String myClass;

        public String Class
        {
            get { return myClass; }
            set
            {
                myClass = value;
                RaisePropertyChangeEvent("Class");
            }
        }

        private double statistic;

        public double Statistic
        {
            get { return statistic; }
            set
            {
                statistic = value;
                RaisePropertyChangeEvent("Statistic");
            }
        }



        public static List<AssetClass> ConstructTestData(JObject obj = null)
        {
            try
            {
                List<AssetClass> assetClasses = new List<AssetClass>();

                if (obj != null)
                {
                    foreach (var item in (JObject)obj)
                    {
                        assetClasses.Add(new AssetClass() {
                            Class = (string)item.Value["name"],
                            Statistic = (int)item.Value["total"],
                        });
                    }
                }
                else
                {
                    assetClasses.Add(new AssetClass() { Class = "Null", Statistic = 1 });
                    assetClasses.Add(new AssetClass() { Class = "Null", Statistic = 1 });
                    assetClasses.Add(new AssetClass() { Class = "Null", Statistic = 1 });
                }

                return assetClasses;
            }
            catch (Exception) { return null; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangeEvent(String propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

        #endregion
    }
}
