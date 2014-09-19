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

        private double fund;

        public double Fund
        {
            get { return fund; }
            set
            {
                fund = value;
                RaisePropertyChangeEvent("Fund");
            }
        }

        private double total;

        public double Total
        {
            get { return total; }
            set
            {
                total = value;
                RaisePropertyChangeEvent("Total");
            }
        }

        private double benchmark;

        public double Benchmark
        {
            get { return benchmark; }
            set
            {
                benchmark = value;
                RaisePropertyChangeEvent("Benchmark");
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
                            Fund = (double)item.Value["total"],
                            Total = 0,
                            Benchmark = 0
                        });
                    }
                }
                else
                {
                    assetClasses.Add(new AssetClass() { Class = "Null", Fund = 1.56, Total = 1.56, Benchmark = 4.82 });
                    assetClasses.Add(new AssetClass() { Class = "Null", Fund = 2.92, Total = 2.92, Benchmark = 17.91 });
                    assetClasses.Add(new AssetClass() { Class = "Null", Fund = 13.24, Total = 0, Benchmark = 0.04 });
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
