using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace ChangeRevision
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * args[0] - Путь к файлу
             * args[1] - Номер ревизии
             * args[2] - release или debug ?
             */
            //string text = File.ReadAllText(args[0] + @"Properties\AssemblyInfo.cs");
            string text = File.ReadAllText(args[0]);

            /*
             *      AssemblyVersion
             */
            Match match = new Regex("AssemblyVersion\\(\"(.*?)\"\\)").Match(text);
            Version ver = new Version(match.Groups[1].Value);
            int build = args[2] == "Release" ? ver.Build + 1 : ver.Build;
            Version newVer = new Version(ver.Major, ver.Minor, build, Convert.ToInt16(args[1]));

            text = Regex.Replace(text, @"AssemblyVersion\((.*?)\)", "AssemblyVersion(\"" + newVer.ToString() + "\")");
            text = Regex.Replace(text, @"AssemblyFileVersionAttribute\((.*?)\)", "AssemblyFileVersionAttribute(\"" + newVer.ToString() + "\")");
            text = Regex.Replace(text, @"AssemblyFileVersion\((.*?)\)", "AssemblyFileVersion(\"" + newVer.ToString() + "\")");

            File.WriteAllText(args[0], text);

            Console.ReadLine();
        }
    }
}
