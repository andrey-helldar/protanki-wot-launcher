using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace _Hell_Restarter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string process = args[0].Replace(".exe", "");

                Console.WriteLine("Wait to closing " + process + ".exe");
                Process[] myProcesses = Process.GetProcessesByName(process);
                for (int i = 1; i < myProcesses.Length; i++) { myProcesses[i].Kill(); }

                Thread.Sleep(500);

                Console.WriteLine("Terminate process!");
                while (Process.GetProcessesByName(process).Length > 0)
                {
                    Process[] myProcesses2 = Process.GetProcessesByName(process);
                    for (int i = 1; i < myProcesses2.Length; i++) { myProcesses2[i].Kill(); }
                }

                try
                {
                    if (args[1] != null)
                    {
                        File.Move(args[0], args[1]);
                        process = args[1].Replace(".exe", "");
                    }
                }
                catch (Exception) { process = args[0].Replace(".exe", ""); }


                Console.WriteLine("Restart " + process + ".exe");
                Process.Start(Directory.GetCurrentDirectory() + @"\" + process + ".exe");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("----------------------------------");
                Console.WriteLine(args[0]);
                Console.Read();
            }
        }
    }
}
