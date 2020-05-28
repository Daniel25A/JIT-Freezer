using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace JITFreezer
{
    internal class Utils
    {
        public static void PrintLogo()
        {
            Console.WriteLine(string.Empty);
            string[] JITFreezerArr = new string[]
           {
                "\t\t\t   __   _____  _____     ___             ",
                "\t\t\t   \\ \\  \\_   \\/__   \\   / __\\ __ ___  ___ _______ _ __ ",
                "\t\t\t    \\ \\  / /\\/  / /\\/  / _\\| '__/ _ \\/ _ \\_  / _ \\ '__|",
                "\t\t\t /\\_/ /\\/ /_   / /    / /  | | |  __/  __// /  __/ |   ",
                "\t\t\t \\___/\\____/   \\/     \\/   |_|  \\___|\\___/___\\___|_|   "
           };
            int r = 220;
            int g = 20;
            int b = 60;
            foreach (string Text in JITFreezerArr) {
                Console.WriteLine(Text, Color.FromArgb(r, g, b));
                g += 8;
                b += 8;
            }
            Console.WriteLine(string.Empty);
        }
        /* Thanks to: 
         * https://stackoverflow.com/questions/71257/suspend-process-in-c-sharp 
         * https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processmodulecollection?view=netframework-4.8
         */

        public static bool GetCLRModule(Int32 pID=0x2D)
        {
            ProcessModuleCollection pModuleCollection = Process.GetProcessById(pID).Modules;
            if (pModuleCollection.Count == 0) return false;
            foreach(ProcessModule Module in pModuleCollection)
            {
                if (Module.ModuleName.ToLower() == "clr.dll") return true;
            }
            return false;
        }

        [Flags]
        public enum ThreadAccess : int
        {
            SUSPEND = (0x0002),
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        public static void SuspendT(Int32 pid= 0x16D)
        {
            var pID = Process.GetProcessById(pid);

            if (string.IsNullOrEmpty(pID.ProcessName))
                return;

            foreach (ProcessThread pThread in pID.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND, false, (uint)pThread.Id);

                if (pOpenThread == IntPtr.Zero) continue;

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }
    }
}