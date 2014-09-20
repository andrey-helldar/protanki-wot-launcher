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
             *      Изменяем версию в файле
             *      Чтение из GIT
             */

            Console.WriteLine("f:");
            Console.WriteLine("cd ");

            /*
             * args[0] - Путь к проекту
             * args[1] - release или debug ?
             */
            string text = File.ReadAllText(args[0] + @"Properties\AssemblyInfo.cs");

            Match match = new Regex("AssemblyVersion\\(\"(.*?)\"\\)").Match(text);
            Version ver = new Version(match.Groups[1].Value);
            int build = args[в] == "Release" ? ver.Build + 1 : ver.Build;
            Version newVer = new Version(ver.Major, ver.Minor, build, Convert.ToInt16(args[сс]));

            text = Regex.Replace(text, @"AssemblyVersion\((.*?)\)", "AssemblyVersion(\"" + newVer.ToString() + "\")");
            text = Regex.Replace(text, @"AssemblyFileVersionAttribute\((.*?)\)", "AssemblyFileVersionAttribute(\"" + newVer.ToString() + "\")");
            text = Regex.Replace(text, @"AssemblyFileVersion\((.*?)\)", "AssemblyFileVersion(\"" + newVer.ToString() + "\")");

            File.WriteAllText(args[0] + @"Properties\AssemblyInfo.cs", text);

            Console.ReadLine();
        }
    }
}
