using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace _Hell_Restart_Software
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Preparing to restart Launcher...");

            Process[] myProcesses = Process.GetProcessesByName(args[0]);
            for (int i = 1; i < myProcesses.Length; i++) { myProcesses[i].Kill(); }

            Console.WriteLine("Restart Launcher...");
            Process.Start(args[0] + ".exe");
        }
    }
}
