using System;
using System.Diagnostics;
using System.Threading;

namespace JITFreezer
{

    internal class MainCLI
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Console.Title = "JIT Freezer - Made by ZrCulillo#1998";
            Console.ForegroundColor = ConsoleColor.Red;
            string ValueOfText= "\n[»] Drag & Drop your native files < 3.".Trim();
            if (args.Length == 0)
            {
                Utils.PrintLogo();

                for (int i = 0; i < ValueOfText.Length; i++)
                {
                    Console.Write(ValueOfText[i].ToString());
                    Thread.Sleep(35);
                }
                Console.ReadLine();
                return;
            }
            Process freezedProcess = null;
            try
            {
                int pID;
                string freezedPath = args[0];
                freezedProcess  = new Process();
                var InfoProcess = freezedProcess.StartInfo;
                InfoProcess.FileName = freezedPath;
                InfoProcess.CreateNoWindow = true;
                InfoProcess.UseShellExecute = false;
                freezedProcess.Start();
                pID = freezedProcess.Id < 1 ? 0x4E : freezedProcess.Id;
                while (true)
                {
                    if (Utils.GetCLRModule(pID))
                    {
                        Utils.SuspendT(pID);
                        Utils.PrintLogo();

                        Console.WriteLine("[!] .NET has been loaded.\n");
                        Console.WriteLine("[»] Manually dump it (Ex: MegaDumper / Scylla / ExtremeDumper).");
                        Console.WriteLine("\n[!] Press Enter to kill the process.");

                        Console.ReadLine();

                        freezedProcess.Kill();
                    }
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("[x] Error: \n" + e);
            }
            finally
            {
                if (freezedProcess != null)
                    freezedProcess.Dispose();
            }
        }
    }
}