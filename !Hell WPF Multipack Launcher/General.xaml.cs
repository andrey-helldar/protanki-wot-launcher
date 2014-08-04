using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _Hell_WPF_Multipack_Launcher
{
    /// <summary>
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class General : Page
    {
        public XDocument XmlGeneral = new XDocument();

        public General()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Устанавливаем заголовок в зависимости от типа версии
            if (XmlGeneral.Root != null)
                if (XmlGeneral.Root.Element("multipack") != null)
                    if (XmlGeneral.Root.Element("multipack").Element("type") != null)
                        lType.Content = XmlGeneral.Root.Element("multipack").Element("type").Value == "base" ? "Базовая версия" : "Расширенная версия";
                    else
                        lType.Content = "Базовая версия";
                else
                    lType.Content = "Базовая версия";
            else
                lType.Content = "Базовая версия";
        }
    }
}
