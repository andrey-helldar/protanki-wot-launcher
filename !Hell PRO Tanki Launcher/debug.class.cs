using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;


namespace _Hell_PRO_Tanki_Launcher
{
    class debug
    {
        public void Save(string func, string place, string mess)
        {
            try
            {
                if (!Directory.Exists("temp")) { Directory.CreateDirectory("temp"); }
                File.WriteAllText(@"temp\" + DateTime.Now.ToString("yyyy-MM-dd h-m-s") + ".debug",
                    func + Environment.NewLine + "-------------------------------" + Environment.NewLine +
                    place + Environment.NewLine + "-------------------------------" + Environment.NewLine +
                    mess, Encoding.UTF8);

                //MessageBox.Show(fIndex.ActiveForm, mess, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception /*ex*/)
            {
                //debug(ex.Message);
            }
        }

        public bool Archive(string path)
        {
            bool exp = false;

            try
            {
                this.Delete(path);

                if (!Directory.Exists(path + @"\temp")) { return false; }
                if (!Directory.Exists(path + @"\debug")) { Directory.CreateDirectory(path + @"\debug"); }

                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.AddDirectory(path + @"\temp");
                    zip.Save(path + @"\debug\debug-" + DateTime.Now.ToString("yyyy-MM-dd h-m-s") + ".zip");
                }

                Directory.Delete(path + @"\temp", true);
            }
            catch (Exception ex)
            {
                this.Save("public bool Archive()", "Debug mode", ex.Message);
            }

            return exp;
        }

        private void Delete(string path)
        {
            try
            {
                if (Directory.Exists(path + @"\temp"))
                {
                    var info = new DirectoryInfo(path + @"\temp");

                    foreach (FileInfo file in info.GetFiles())
                    {
                        DateTime now = DateTime.Now;
                        DateTime cf = File.GetCreationTime(file.FullName);

                        TimeSpan ts = now - cf;

                        /* Удаляем все файлы, старше 3-х суток
                         * 1 минута = 60 сек
                         * 1 час = 60 минут = 3600 сек
                         * 1 сутки = 24 часа = 86400 сек
                         * 2 суток = 172800 сек
                         * 3 суток = 259200 сек
                         */

                        if (ts.TotalSeconds > 259200) { File.Delete(file.FullName); }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Save("private void Delete(string path)", "Debug mode", ex.Message);
            }
        }
    }
}
