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

                Console.WriteLine("Wait to closing " + args[0]);
                Process[] myProcesses = Process.GetProcessesByName(args[0]);
                for (int i = 1; i < myProcesses.Length; i++) { myProcesses[i].Kill(); }

                Thread.Sleep(500);

                Console.WriteLine("Terminate process!");
                while (Process.GetProcessesByName(args[0]).Length > 0)
                {
                    Process[] myProcesses2 = Process.GetProcessesByName(args[0]);
                    for (int i = 1; i < myProcesses2.Length; i++) { myProcesses2[i].Kill(); }
                }

                Console.WriteLine("Restart "+args[0]);
                Console.WriteLine("----------------------------------");
                Process.Start(args[0]);

                //Console.Read();
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
