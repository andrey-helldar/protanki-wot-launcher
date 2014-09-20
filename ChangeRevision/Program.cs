using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ChangeRevision
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                /*
                 *      Изменяем версию в файле
                 *      Чтение из GIT
                 */
                if (File.Exists(args[0] + @"\revision.txt")) File.Delete(args[0] + @"\revision.txt");
                Process.Start(@"c:\Program Files (x86)\Git\cmd\git.exe shoerlog -s >> " + "\"" + args[0] + "\\revision.txt\"");

                string rev = File.ReadAllText(@"revision.txt").Trim();
                rev = rev.Remove(rev.IndexOf("	"));

                /*
                 * args[0] - Project name
                 * args[1] - release или debug ?
                 */
                string text = File.ReadAllText(args[0] + @"\Properties\AssemblyInfo.cs");

                Match match = new Regex("AssemblyVersion\\(\"(.*?)\"\\)").Match(text);
                Version ver = new Version(match.Groups[1].Value);
                int build = args[1] == "Release" ? ver.Build + 1 : ver.Build;
                Version newVer = new Version(ver.Major, ver.Minor, build, Convert.ToInt16(rev));

                text = Regex.Replace(text, @"AssemblyVersion\((.*?)\)", "AssemblyVersion(\"" + newVer.ToString() + "\")");
                text = Regex.Replace(text, @"AssemblyFileVersionAttribute\((.*?)\)", "AssemblyFileVersionAttribute(\"" + newVer.ToString() + "\")");
                text = Regex.Replace(text, @"AssemblyFileVersion\((.*?)\)", "AssemblyFileVersion(\"" + newVer.ToString() + "\")");

                File.WriteAllText(args[0] + @"\Properties\AssemblyInfo.cs", text);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("");
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }

        }
    }
}
