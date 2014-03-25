using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;


namespace _Hell_PRO_Tanki_Launcher
{
    class debug
    {
        public void Save(string place, string mess)
        {
            try
            {
                if (!Directory.Exists("debug")) { Directory.CreateDirectory("debug"); }
                File.WriteAllText(@"debug\" + DateTime.Now.ToString("yyyy-MM-dd h-m-s") + ".debug", place + Environment.NewLine + "-------------------------------" + Environment.NewLine + mess, Encoding.UTF8);
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
                if (!Directory.Exists(path + @"\debug")) { return false; }

                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.AddDirectory(path + @"\debug");
                    zip.Save(path + @"\debug-" + DateTime.Now.ToString("yyyy-MM-dd h-m-s") + ".zip");
                }

                Directory.Delete(path + @"\debug", true);
            }
            catch (Exception ex)
            {
                this.Save("public bool Archive()", ex.Message);
            }

            return exp;
        }
    }
}
