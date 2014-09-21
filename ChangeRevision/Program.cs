using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                if (File.Exists(args[0] + @"\revision.txt")) File.Delete(args[0] + @"\revision.txt");

                
                Process process = new Process();
                process.StartInfo.FileName = "\"c:\\Program Files (x86)\\Git\\cmd\\git.exe\"";
                process.StartInfo.Arguments = @"rev-list master --count";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                StringBuilder output = new StringBuilder();
                int timeout = 5000;

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();

                    if (process.WaitForExit(timeout) && outputWaitHandle.WaitOne(timeout))
                    {
                        string text = File.ReadAllText(args[0] + @"\Properties\AssemblyInfo.cs");

                        Match match = new Regex("AssemblyVersion\\(\"(.*?)\"\\)").Match(text);
                        Version ver = new Version(match.Groups[1].Value);
                        int build = args[1] == "Release" ? ver.Build + 1 : ver.Build;
                        Version newVer = new Version(ver.Major, ver.Minor, build, Convert.ToInt16(output.ToString().Trim()));

                        text = Regex.Replace(text, @"AssemblyVersion\((.*?)\)", "AssemblyVersion(\"" + newVer.ToString() + "\")");
                        text = Regex.Replace(text, @"AssemblyFileVersionAttribute\((.*?)\)", "AssemblyFileVersionAttribute(\"" + newVer.ToString() + "\")");
                        text = Regex.Replace(text, @"AssemblyFileVersion\((.*?)\)", "AssemblyFileVersion(\"" + newVer.ToString() + "\")");

                        File.WriteAllText(args[0] + @"\Properties\AssemblyInfo.cs", text);
                    }
                }


                /*int i = 0;
                while (i < 10)
                {
                    if (!File.Exists(args[0] + @"\revision.txt"))
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("Sleepeng: " + i.ToString());
                    }
                    else
                    {
                        i = 20;
                    }
                    i++;
                }*/

                /*string rev = File.ReadAllText(args[0] + @"\revision.txt").Trim();
                rev = rev.Remove(rev.IndexOf("	"));

                /*
                 * args[0] - Project name
                 * args[1] - release или debug ?
                 *
                string text = File.ReadAllText(args[0] + @"\Properties\AssemblyInfo.cs");

                Match match = new Regex("AssemblyVersion\\(\"(.*?)\"\\)").Match(text);
                Version ver = new Version(match.Groups[1].Value);
                int build = args[1] == "Release" ? ver.Build + 1 : ver.Build;
                Version newVer = new Version(ver.Major, ver.Minor, build, Convert.ToInt16(rev));

                text = Regex.Replace(text, @"AssemblyVersion\((.*?)\)", "AssemblyVersion(\"" + newVer.ToString() + "\")");
                text = Regex.Replace(text, @"AssemblyFileVersionAttribute\((.*?)\)", "AssemblyFileVersionAttribute(\"" + newVer.ToString() + "\")");
                text = Regex.Replace(text, @"AssemblyFileVersion\((.*?)\)", "AssemblyFileVersion(\"" + newVer.ToString() + "\")");

                File.WriteAllText(args[0] + @"\Properties\AssemblyInfo.cs", text);
                Console.ReadLine();*/
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
