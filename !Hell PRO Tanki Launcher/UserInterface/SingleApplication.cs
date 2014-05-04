using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace _Hell_PRO_Tanki_Launcher.UserInterface
{
    public sealed class SingleApplication
    {
        private enum ShowWindowCommand
        {
            Restore = 9
        }

        private static Mutex mutex;

        public static bool Run(Form mainForm)
        {
            if (IsAlreadyRunning())
            {
                SwitchToCurrentInstance();
                return false;
            }

            Application.Run(mainForm);
            return true;
        }

        private static IntPtr GetCurrentInstanceWindowHandle()
        {
            var currentProcess = Process.GetCurrentProcess();

            foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (process.Id != currentProcess.Id && process.MainWindowHandle != IntPtr.Zero)
                    return process.MainWindowHandle;
            }

            return IntPtr.Zero;
        }

        private static void SwitchToCurrentInstance()
        {
            var windowHandle = GetCurrentInstanceWindowHandle();

            if (windowHandle != IntPtr.Zero)
            {
                if (IsIconicWindow(windowHandle))
                    ShowWindow(windowHandle, ShowWindowCommand.Restore);

                SetForegroundWindow(windowHandle);
            }
        }

        private static bool IsAlreadyRunning()
        {
            string exeName = new FileInfo(Application.ExecutablePath).Name.ToUpper();
            bool createdNew;

            mutex = new Mutex(true, "Global\\" + exeName, out createdNew);

            if (createdNew)
                mutex.ReleaseMutex();

            return !createdNew;
        }

        private static bool IsIconicWindow(IntPtr windowHandle)
        {
            return IsIconic(windowHandle) != 0;
        }

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr windowHandle, ShowWindowCommand showCommand);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr windowHandle);

        [DllImport("user32.dll")]
        private static extern int IsIconic(IntPtr windowHandle);
    }
}
