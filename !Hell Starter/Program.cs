using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace _Hell_Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args[1] != "")
                {
                    Process.Start(args[1] + args[0]);
                }
                else
                {
                    Process.Start(args[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("-------------------------------------");
                try
                {
                    Process.Start(args[0]);
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.Message);
                }
                Console.Read();
            }
        }
    }
}
