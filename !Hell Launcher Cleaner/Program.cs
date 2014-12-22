using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _Hell_Launcher_Cleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleKeyInfo cki;
                bool repeat = true;

                while (repeat)
                {
                    Console.WriteLine("");
                    Console.WriteLine("          =====================================");
                    Console.WriteLine("          =  !Hell Multipack Launcher Cleaner =");
                    Console.WriteLine("          =-----------------------------------=");
                    Console.WriteLine("          =  AI RUS - Professional IT support =");
                    Console.WriteLine("          =        https://ai-rus.com         =");
                    Console.WriteLine("          =====================================");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Select cleaning category:");
                    Console.WriteLine("  [1] All cleaning");
                    Console.WriteLine("  [2] Cleaning DLL libraries");
                    Console.WriteLine("  [3] Cleaning Launcher settings");
                    Console.WriteLine("  [enter] Cancel");
                    Console.WriteLine("");
                    cki = Console.ReadKey();

                    Console.WriteLine(" - selected");
                    Console.WriteLine("");
                    Console.WriteLine("");

                    switch (cki.Key)
                    {
                        case ConsoleKey.D1:
                            Delete("Ionic.Zip.dll");
                            Delete("Newtonsoft.Json.dll");
                            Delete("Ookii.Dialogs.Wpf.dll");
                            Delete("Processes.Library.dll");
                            Delete("restart.exe");
                            Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.json");
                            break;

                        case ConsoleKey.D2:
                            Delete("Ionic.Zip.dll");
                            Delete("Newtonsoft.Json.dll");
                            Delete("Ookii.Dialogs.Wpf.dll");
                            Delete("Processes.Library.dll");
                            Delete("restart.exe");
                            break;

                        case ConsoleKey.D3:
                            Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Wargaming.net\WorldOfTanks\settings.json");
                            break;

                        default: break;
                    }

                    Console.WriteLine("");
                    Console.WriteLine("");                    
                    Console.WriteLine("[y/n] Repeat operations?");
                    cki = Console.ReadKey();
                    if (cki.Key == ConsoleKey.N) repeat = false;
                    else Console.Clear();
                }
            }
            catch (Exception) { }

            Console.WriteLine("");
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static void Delete(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                    Console.WriteLine("  > {0} deleted", filename);
                }
                else Console.WriteLine("  x {0} not found", filename);
            }
            catch (Exception) { }
        }
    }
}
