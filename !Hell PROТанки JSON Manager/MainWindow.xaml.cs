using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yusha_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool TimerBool = true;
        private bool BackupWork = true;

        public MainWindow()
        {
            InitializeComponent();
            this.Title += " v" + Application.Current.GetType().Assembly.GetName().Version.ToString();

            if (!File.Exists("Newtonsoft.Json.dll")) File.WriteAllBytes("Newtonsoft.Json.dll", Properties.Resources.Newtonsoft_Json);

            bLoadJSON.IsEnabled = File.Exists("update.json");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            /*
             * {ChangeBaseEN}
             * {ChangeBaseRU}
             * {LinkBase}
             * 
             * {ChangeExtendedEN}
             * {ChangeExtendedRU}
             * {LinkExtended}
             * 
             * {TanksVersion}
             * {PackVersion}
             */

            //string text = System.IO.File.ReadAllText(@"C:\Users\Public\TestFolder\WriteText.txt");

            try
            {
                if (!File.Exists("template.txt")) File.WriteAllText("template.txt", Properties.Resources.template);

                // Задаем переменные
                string changelogRU = String.Empty,
                    changelogEN = String.Empty;

                // Читаем шаблон
                string text = File.ReadAllText("template.txt");

                // Общие
                text = text.Replace("{TanksVersion}", TanksVersion.Text).Replace("{PackVersion}", PackVersion.Text);

                TextBaseEN.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible; TextBaseEN.TextWrapping = TextWrapping.NoWrap;
                TextBaseRU.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible; TextBaseRU.TextWrapping = TextWrapping.NoWrap;
                TextExtendedEN.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible; TextExtendedEN.TextWrapping = TextWrapping.NoWrap;
                TextExtendedRU.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible; TextExtendedRU.TextWrapping = TextWrapping.NoWrap;
                //Thread.Sleep(100);

                // Базовая
                changelogRU = String.Empty; changelogEN = String.Empty;
                tiBase.IsSelected = true;
                for (int i = 0; i < TextBaseEN.LineCount; i++) if (TextBaseEN.GetLineText(i).Trim().Length > 0) changelogEN += "<li>" + TextBaseEN.GetLineText(i).Trim() + "</li>";
                for (int i = 0; i < TextBaseRU.LineCount; i++) if (TextBaseRU.GetLineText(i).Trim().Length > 0) changelogRU += "<li>" + TextBaseRU.GetLineText(i).Trim() + "</li>";
                text = text.Replace("{ChangeBaseEN}", changelogEN).Replace("{ChangeBaseRU}", changelogRU).Replace("{LinkBase}", LinkBase.Text);
                
                //Thread.Sleep(100);

                // Расширенная
                changelogRU = String.Empty; changelogEN = String.Empty;
                tiExtended.IsSelected = true;
                for (int i = 0; i < TextExtendedEN.LineCount; i++) if (TextExtendedEN.GetLineText(i).Trim().Length > 0) changelogEN += "<li>" + TextExtendedEN.GetLineText(i).Trim() + "</li>";
                for (int i = 0; i < TextExtendedRU.LineCount; i++) if (TextExtendedRU.GetLineText(i).Trim().Length > 0) changelogRU += "<li>" + TextExtendedRU.GetLineText(i).Trim() + "</li>";
                text = text.Replace("{ChangeExtendedEN}", changelogEN).Replace("{ChangeExtendedRU}", changelogRU).Replace("{LinkExtended}", LinkExtended.Text);

                //Thread.Sleep(100);

                TextBaseEN.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden; TextBaseEN.TextWrapping = TextWrapping.Wrap;
                TextBaseRU.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden; TextBaseRU.TextWrapping = TextWrapping.Wrap;
                TextExtendedEN.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden; TextExtendedEN.TextWrapping = TextWrapping.Wrap;
                TextExtendedRU.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden; TextExtendedRU.TextWrapping = TextWrapping.Wrap;


                // Сохранение результата
                if (File.Exists("update.json")) File.Delete("update.json");
                File.WriteAllText("update.json", text);

                MessageBox.Show("Файл успешно сохранен");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка:" + Environment.NewLine + ex.Message + Environment.NewLine + "-----------------" + ex.StackTrace);
            }
        }

        private async Task Backup(int sec = 30)
        {
            while (BackupWork)
            {
                try
                {
                    SetStatus(sec);
                    await Task.Delay(sec * 1000);
                    SetStatus(sec, 1);

                    //  Visibility textblock notification
                    if (TextBaseRU.Text.Trim().Length > 0 ||
                        TextBaseEN.Text.Trim().Length > 0 ||
                        LinkBase.Text.Trim().Length > 0 ||

                        TextExtendedRU.Text.Trim().Length > 0 ||
                        TextExtendedEN.Text.Trim().Length > 0 ||
                        LinkExtended.Text.Trim().Length > 0)
                    {
                        BackupsFound.Visibility = Visibility.Hidden;
                    }

                    /*
                     *      BASE
                     */
                    //  TextBaseRU
                    if (TextBaseRU.Text.Trim().Length > 0)
                    {
                        if (File.Exists("TextBaseRU.bak")) File.Delete("TextBaseRU.bak");
                        File.WriteAllText("TextBaseRU.bak", TextBaseRU.Text.Trim());
                    }

                    //  TextBaseEN
                    if (TextBaseEN.Text.Trim().Length > 0)
                    {
                        if (File.Exists("TextBaseEN.bak")) File.Delete("TextBaseEN.bak");
                        File.WriteAllText("TextBaseEN.bak", TextBaseEN.Text.Trim());
                    }

                    //  LinkBase
                    if (LinkBase.Text.Trim().Length > 0)
                    {
                        if (File.Exists("LinkBase.bak")) File.Delete("LinkBase.bak");
                        File.WriteAllText("LinkBase.bak", LinkBase.Text.Trim());
                    }



                    /*
                     *      EXTENDED
                     */

                    //  TextExtendedRU
                    if (TextExtendedRU.Text.Trim().Length > 0)
                    {
                        if (File.Exists("TextExtendedRU.bak")) File.Delete("TextExtendedRU.bak");
                        File.WriteAllText("TextExtendedRU.bak", TextExtendedRU.Text.Trim());
                    }

                    //  TextExtendedRU
                    if (TextExtendedEN.Text.Trim().Length > 0)
                    {
                        if (File.Exists("TextExtendedEN.bak")) File.Delete("TextExtendedEN.bak");
                        File.WriteAllText("TextExtendedEN.bak", TextExtendedEN.Text.Trim());
                    }

                    //  LinkExtended
                    if (LinkExtended.Text.Trim().Length > 0)
                    {
                        if (File.Exists("LinkExtended.bak")) File.Delete("LinkExtended.bak");
                        File.WriteAllText("LinkExtended.bak", LinkExtended.Text.Trim());
                    }


                    /*
                     *      OTHER
                     */

                    // TanksVersion
                    if (TanksVersion.Text.Trim().Length > 0)
                    {
                        if (File.Exists("TanksVersion.bak")) File.Delete("TanksVersion.bak");
                        File.WriteAllText("TanksVersion.bak", TanksVersion.Text.Trim());
                    }

                    // PackVersion
                    if (PackVersion.Text.Trim().Length > 0)
                    {
                        if (File.Exists("PackVersion.bak")) File.Delete("PackVersion.bak");
                        File.WriteAllText("PackVersion.bak", PackVersion.Text.Trim());
                    }
                }
                catch (Exception) { SetStatus(sec, 2); }
            }
        }

        private async Task Recovery(bool recovery = true)
        {
            try
            {
                if (recovery)
                {
                    /*
                     *      BASE
                     */
                    if (File.Exists("TextBaseRU.bak"))
                        TextBaseRU.Text = File.ReadAllText("TextBaseRU.bak");

                    //  TextBaseEN
                    if (File.Exists("TextBaseEN.bak"))
                        TextBaseEN.Text = File.ReadAllText("TextBaseEN.bak");

                    //  LinkBase
                    if (File.Exists("LinkBase.bak"))
                        LinkBase.Text = File.ReadAllText("LinkBase.bak");


                    /*
                     *      EXTENDED
                     */
                    //  TextExtendedRU
                    if (File.Exists("TextExtendedRU.bak"))
                        TextExtendedRU.Text = File.ReadAllText("TextExtendedRU.bak");

                    //  TextExtendedRU
                    if (File.Exists("TextExtendedEN.bak"))
                        TextExtendedEN.Text = File.ReadAllText("TextExtendedEN.bak");

                    //  LinkExtended
                    if (File.Exists("LinkExtended.bak"))
                        LinkExtended.Text = File.ReadAllText("LinkExtended.bak");

                    /*
                     *      OTHER
                     */

                    //  TanksVersion
                    if (File.Exists("TanksVersion.bak"))
                        TanksVersion.Text = File.ReadAllText("TanksVersion.bak");

                    //  PackVersion
                    if (File.Exists("PackVersion.bak"))
                        PackVersion.Text = File.ReadAllText("PackVersion.bak");

                    //  Visibility textblock notification
                    BackupsFound.Visibility = Visibility.Hidden;
                }
                else
                {
                    BackupsFound.Visibility = (File.Exists("TextBaseRU.bak") ||
                        File.Exists("TextBaseEN.bak") ||
                        File.Exists("LinkBase.bak") ||

                        File.Exists("TextExtendedRU.bak") ||
                        File.Exists("TextExtendedEN.bak") ||
                        File.Exists("LinkExtended.bak")) ?
                        Visibility.Visible : Visibility.Hidden;
                }
            }
            catch (Exception) { }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (TextBaseRU.Text.Trim().Length > 0 ||
                TextBaseEN.Text.Trim().Length > 0 ||
                LinkBase.Text.Trim().Length > 0 ||

                TextExtendedRU.Text.Trim().Length > 0 ||
                TextExtendedEN.Text.Trim().Length > 0 ||
                LinkExtended.Text.Trim().Length > 0)
            {
                if (MessageBox.Show("Вы точно хотите очистить ВСЕ поля?", Properties.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    TextBaseRU.Text = String.Empty;
                    TextBaseEN.Text = String.Empty;
                    LinkBase.Text = String.Empty;

                    TextExtendedRU.Text = String.Empty;
                    TextExtendedEN.Text = String.Empty;
                    LinkExtended.Text = String.Empty;

                    PackVersion.Text = "0";
                    TanksVersion.Text = "0.9.5";
                }
            }
            else
            {
                MessageBox.Show("Поля чистые, как белый лист :)", Properties.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Recovery(false); // Восстановление данных, если бэкап найден
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Recovery(); // Восстановление данных, если бэкап найден
        }

        private async Task Timer(int sec = 30)
        {
            try
            {
                while (TimerBool && sec > 0)
                {
                    RunTimer.Text = (--sec).ToString();
                    await Task.Delay(1000);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Применение цветового стиля элементы
        /// </summary>
        /// <param name="status">Идентификатор статуса определения цвета, где:
        /// 0 - Ожидание
        /// 1 - Сохранение</param>
        /// <returns>Отсутствует</returns>
        private async Task SetStatus(int sec = 0, int status = 0)
        {
            try
            {
                switch (status)
                {
                    case 1:  //  Сохранение
                        RunStatus.Text = "Сохранение";
                        RunStatus.Foreground = new SolidColorBrush(Color.FromRgb(34, 172, 34));
                        TimerBool = false;
                        break;

                    case 2:  //  Ошибка
                        RunStatus.Text = "Ошибка";
                        RunStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 70, 0));

                        TimerBool = true; Timer(sec);
                        break;

                    case 3:  //  Отключено
                        RunStatus.Text = "Отключено";
                        RunStatus.Foreground = new SolidColorBrush(Color.FromRgb(93, 93, 93));

                        TimerBool = false;
                        break;

                    default:  //  Ожидание
                        RunStatus.Text = "Ожидание";
                        RunStatus.Foreground = new SolidColorBrush(Color.FromRgb(93, 93, 93));

                        TimerBool = true; Timer(sec);
                        break;
                }
            }
            catch (Exception) { RunStatus.Foreground = new SolidColorBrush(Color.FromRgb(93, 93, 93)); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Autosave.Text == "Включить")
            {
                BackupWork = true;
                Backup(10); // Активация функции автосохранения
                Autosave.Text = "Выключить";
            }
            else
            {
                BackupWork = false;
                Autosave.Text = "Включить";
                SetStatus(1, 3);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (TextBaseRU.Text.Trim().Length > 0 ||
                TextBaseEN.Text.Trim().Length > 0 ||
                LinkBase.Text.Trim().Length > 0 ||

                TextExtendedRU.Text.Trim().Length > 0 ||
                TextExtendedEN.Text.Trim().Length > 0 ||
                LinkExtended.Text.Trim().Length > 0)
            {
                if (MessageBox.Show("На форме обнаружены введенные данные." + Environment.NewLine +
                    "Вы действительно хотите их заменить?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    LoadJSON();
            }
            else
                LoadJSON();
        }

        private void LoadJSON()
        {
            JObject obj = JObject.Parse(File.ReadAllText("update.json"));

            /*
             * Head
             */
            PackVersion.Text = obj.SelectToken("base.version").ToString();
            try { TanksVersion.Text = obj.SelectToken("client").ToString(); }
            catch (Exception) { }


            /*
             * Base
             */
            TextBaseRU.Text = Items(obj.SelectToken("base.changelog.ru").ToString());
            TextBaseEN.Text = Items(obj.SelectToken("base.changelog.en").ToString());
            LinkBase.Text = obj.SelectToken("base.download").ToString();


            /*
             * Extended
             */
            TextExtendedRU.Text = Items(obj.SelectToken("extended.changelog.ru").ToString());
            TextExtendedEN.Text = Items(obj.SelectToken("extended.changelog.en").ToString());
            LinkExtended.Text = obj.SelectToken("extended.download").ToString();
        }

        private string Items(string text)
        {
            string list = String.Empty;

            if (text.Trim().Length > 0)
                foreach (Match m in Regex.Matches(text, "<li>(.*?)</li>"))
                    list += m.Groups[1].Value + Environment.NewLine;

            return list;
        }
    }
}
