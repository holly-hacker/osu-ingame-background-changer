using System;
using System.Diagnostics;
using System.IO;
using EasyHook;

namespace osu_ibc_injector
{
    class Program
    {
        public static Process InjectProcess;
        private const string DllName = "osu!ibc.dll";
        private static readonly string DllPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + DllName;

        static void Main(string[] args)
        {
            //pretty stuff
            Console.Title = "osu!ibc";
            Console.WriteLine("osu!ibc - osu! ingame background changer");
            Console.WriteLine();
            Console.Write("Looking for osu!...");

            //get osu!
            InjectProcess = FindOsuProcess();
            Console.WriteLine(" found osu! (PID: " + InjectProcess.Id + ")");

            //inject dll
            //if not present, I could ask user to select dll
            if (!File.Exists(DllPath)) Exit(DllName + " not found");

            //inject DLL into osu!
            RemoteHooking.Inject(InjectProcess.Id, InjectionOptions.DoNotRequireStrongName, DllPath, DllPath);
        }

        public static Process FindOsuProcess()
        {
            //could handle length>1 in another way if needed
            Process[] processes = Process.GetProcessesByName("osu!");
            if (processes.Length <= 0) Exit(" could not find osu! process!", true);
            if (processes.Length == 1) return processes[0];
            if (processes.Length >= 2) Exit(" multiple osu! processes found!", true);
            return null;    //should not occur
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
    }
}
