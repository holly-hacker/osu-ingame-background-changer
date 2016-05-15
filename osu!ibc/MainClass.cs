using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using EasyHook;

namespace osu_ibc
{
    public class MainClass : IEntryPoint
    {
        public static string NewFilePath = "";
        public static bool OpenUsingHotkey = true;
        LocalHook _hkCreateFileW;   //ascii version not needed
        static Dictionary<string, string> _fileDictionary = new Dictionary<string, string>();

        //stuff for opening fsf again
        public const short KeyMenu1 = 0x4F; //O-key
        public const short KeyMenu2 = 0x50; //P-key
        public const short MenuWaitTicks = 5;   //hold for 5 seconds
        public static int MenuWaitCounter = 0;

        //Imports
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateFileW(
             [MarshalAs(UnmanagedType.LPWStr)] string filename,
             [MarshalAs(UnmanagedType.U4)] FileAccess access,
             [MarshalAs(UnmanagedType.U4)] FileShare share,
             IntPtr securityAttributes,
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
             [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
             IntPtr templateFile);

        //Delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate IntPtr DCreateFileW(
        [MarshalAs(UnmanagedType.LPWStr)] string filename,
             [MarshalAs(UnmanagedType.U4)] FileAccess access,
             [MarshalAs(UnmanagedType.U4)] FileShare share,
             IntPtr securityAttributes,
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
             [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
             IntPtr templateFile);

        public MainClass(RemoteHooking.IContext inContext) {}

        public void Run(RemoteHooking.IContext inContext)  //method that gets called
        {
#if DEBUG
            Helper.AllocConsole();
            Console.WriteLine("Allocated Console");
            Console.Title = "osu!ibc console {000}";
#endif

            //spawn FileSelectForm
#if DEBUG
            Console.WriteLine("Spawing FileSelectForm");
#endif
            ShowFileSelect();

            //create hooks for CreateFileW
#if DEBUG
            Console.WriteLine($"Creating hooks");
#endif
            try {
                _hkCreateFileW = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"), new DCreateFileW(CreateFileWHook), this);
                _hkCreateFileW.ThreadACL.SetExclusiveACL(new[] {0});
                RemoteHooking.WakeUpProcess();
            }
            catch (Exception ex) {
                MessageBox.Show("Exception occured while creating hooks: " + ex.Message);
                return;
            }

#if DEBUG
            Console.WriteLine($"Finished creating hooks, we're finished setting up!");
#endif

            while (true) {
                Thread.Sleep(1000); //just sleeeep

#if DEBUG
                Console.Title = "osu!ibc console {" + new Random().Next(100, 999) + "}";
#endif

                //check if keys are pressed
                if (OpenUsingHotkey) {
                    if (Helper.IsKeyPressed(KeyMenu1) && Helper.IsKeyPressed(KeyMenu2)) { //if both keys are pressed
                        MenuWaitCounter++; //incement timer by one

                        if (MenuWaitCounter >= MenuWaitTicks) { //if counter is larger than value
                            ShowFileSelect(); //open dialog
                            MenuWaitCounter = 0; //reset counter
                        }
                    }
                    else MenuWaitCounter = 0; //otherwise reset timer to 0
                }
            }
        }

        public void ShowFileSelect()
        {
            FileSelectForm fsf = new FileSelectForm();
            if (fsf.ShowDialog() != DialogResult.OK) {  //user didn't complete file selection, show warning
                if (MessageBox.Show("You did not select a file.\nDo you want to remove all bg's (yes) or exit (no)?", "osu!ibc - Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
            } else NewFilePath = fsf.SelectedFile;
            OpenUsingHotkey = fsf.AllowReopen;
        }

        static IntPtr CreateFileWHook(  [MarshalAs(UnmanagedType.LPWStr)] string filename,
                                        [MarshalAs(UnmanagedType.U4)] FileAccess access,
                                        [MarshalAs(UnmanagedType.U4)] FileShare share,
                                        IntPtr securityAttributes,
                                        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                                        [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
                                        IntPtr templateFile)
        {
            string extension = Path.GetExtension(filename);
            switch (extension?.ToLower()) {
                case ".png":
                case ".jpg":
                case ".jpeg":
                    if (Path.GetDirectoryName(filename).Contains("Songs")) {
                        if (Helper.AddBeatmapsToDictionary(Path.GetDirectoryName(filename), ref _fileDictionary)) {
#if DEBUG
                            Console.WriteLine($" - Changing {filename.Remove(0, filename.IndexOf("Songs"))} into {Path.GetFileName(NewFilePath)}");
#endif
                            filename = NewFilePath; //this is a bg
                        }
                    }
#if DEBUG
                    else Console.WriteLine($" - non-bg file {Path.GetFileName(filename)}");
#endif
                    break;
            }

            return CreateFileW(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }
    }
}
