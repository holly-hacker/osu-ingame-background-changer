using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHook;

namespace osu_ibc_injector
{
    class Program
    {
        public static Process osuProcess;
        private const string DllName = "osu!ibc.dll";
        private static string DllPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + DllName;

        static void Main(string[] args)
        {
            Console.Title = "osu!ibc";
            Console.WriteLine("osu!ibc - osu! ingame background changer");
            Console.WriteLine();
            Console.Write("Waiting for osu!...");

            //get osu!
            osuProcess = FindOsuProcess();
            if (osuProcess == null) Exit(" did not find a single osu! process!", true);
            Console.WriteLine(" found osu! (PID: " + osuProcess.Id + ")");

            //inject dll
            //if not present, ask user to select dll
            if (!File.Exists(DllPath)) {
                //TODO: ask for file
                Exit("DLL file not found");
            }
            RemoteHooking.Inject(osuProcess.Id, InjectionOptions.DoNotRequireStrongName, DllPath, DllPath);
        }

        public static void Exit(string message = "Press any key to exit...", bool bad = false)
        {
            ConsoleColor oldColor = Console.BackgroundColor;

            if (bad) Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            if (bad) Console.BackgroundColor = oldColor;
            Console.ReadKey();
            Environment.Exit(bad ? -1 : 0);
        }

        public static Process FindOsuProcess()
        {
            Process[] processes = Process.GetProcessesByName("osu!");
            if (processes.Length == 0) return null;
            if (processes.Length == 1) return processes[0];

            //add handler for length >1?
            return null;
        }
    }
}
