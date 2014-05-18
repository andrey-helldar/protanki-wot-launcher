using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_Multipack_Launcher.LocalInterface
{
    class LocInterface
    {
        public bool loop = true;

        public async Task<ImageBrush> Background()
        {
            string uri = @"pack://application:,,,/Multipack Launcher;component/Resources/back_{0}.jpg";

            try
            {
                //while (this.loop)
                //{
                    try
                    {
                        //this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Multipack Launcher;component/Resources/back_2.jpg")));
                        return new ImageBrush(new BitmapImage(new Uri(String.Format(uri, (new Random().Next(1, 7)).ToString()))));
                    }
                    catch (Exception) { return new ImageBrush(new BitmapImage(new Uri(String.Format(uri, "1")))); }

                //    await Task.Delay(10000);
                //}
            }
            catch (Exception) { return new ImageBrush(new BitmapImage(new Uri(String.Format(uri, "1")))); }
        }
    }
}
