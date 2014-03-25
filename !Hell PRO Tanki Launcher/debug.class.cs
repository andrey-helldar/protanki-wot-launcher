using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

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
            catch (Exception ex)
            {
                //debug(ex.Message);
            }
        }

        public bool Archive()
        {
            bool exp = false;

            if (!Directory.Exists("debug")) { return false; }

            using (FileStream sourceFile = File.OpenRead(@"debug"))
            using (FileStream targetFile = File.Create(@"debug-" + DateTime.Now.ToString("yyyy-MM-dd h-m-s") + ".zip"))
            using (GZipStream gzipStream = new GZipStream(targetFile, CompressionMode.Compress, false))
            {
                try
                {
                    int posByte = sourceFile.ReadByte();

                    while (posByte != -1)
                    {
                        gzipStream.WriteByte((byte)posByte);
                        posByte = sourceFile.ReadByte();
                    }

                    exp = true;
                }
                catch (Exception) { exp = false; }
            }

            return exp;
        }
    }
}
