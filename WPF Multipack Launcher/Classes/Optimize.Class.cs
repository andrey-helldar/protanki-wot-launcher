using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WPF_Multipack_Launcher.Classes
{
    class Optimize
    {
        public async Task Start(
            bool Kill = false,
            bool ForceKill = false,
            bool Video = false,
            bool Weak = false,
            bool Aero = false,
            bool Manual = false)
        {
            int progress = 0,
                maxProgress = 0;

            try
            {
                /***************************
                 * Disable Windows Aero
                 * *************************/
                if (Manual || Aero)
                {
                    ++maxProgress;
                    Process.Start(new ProcessStartInfo("cmd", @"/c net stop uxsms"));
                    ++progress;
                }
            }
            finally { }

            /***************************
             * Kill & Force Kill
             * *************************/
            try
            {
                if (Manual || Kill)
                {
                    int session = Process.GetCurrentProcess().SessionId;
                    bool kill = false;

                    for (int i = 0; i < 2; i++)
                    {
                        int processCount = Process.GetProcesses().Length;
                        maxProgress += processCount;

                        foreach (var process in Process.GetProcesses())
                        {
                            try
                            {
                                if (process.SessionId == session &&
                                    Array.IndexOf(ProcessLibrary, process.ProcessName) == -1 && // Global processes list
                                    !ProcessList.IndexOf(process.ProcessName)) // User processes list
                                    if (!kill) process.CloseMainWindow(); else process.Kill();
                            }
                            finally { ++progress; }
                        }

                        // If ForceKill is True, then...
                        if (Manual || ForceKill)
                        {
                            kill = true;
                            await Sleep(5);
                        }
                        else
                            break;
                    }
                }
            }
            finally { }
        }

        private async Task Sleep(int sec = 5)
        {
            for (int i = 0; i < sec; i++)
                await Task.Delay(5 * 1000);
        }
    }
}
