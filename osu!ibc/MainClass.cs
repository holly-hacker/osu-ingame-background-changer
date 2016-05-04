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
        LocalHook _hkCreateFileW;   //ascii version not needed
        static Dictionary<string, string> _fileDictionary = new Dictionary<string, string>();

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
            //spawn FileSelectForm
            FileSelectForm fsf = new FileSelectForm();
            if (fsf.ShowDialog() != DialogResult.OK) {
                //user didn't complete file selection, show warning
                if (MessageBox.Show("You did not select a file.\nDo you want to remove all bg's (yes) or exit (no)?","osu!ibc - Warning!",MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
                    return;
                }
            }
            else {
                NewFilePath = fsf.SelectedFile;
            }

            //create hooks for CreateFileA and CreateFileW
            try {
                _hkCreateFileW = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"), new DCreateFileW(CreateFileWHook), this);
                _hkCreateFileW.ThreadACL.SetExclusiveACL(new[] {0});
                RemoteHooking.WakeUpProcess();
            }
            catch (Exception ex) {
                MessageBox.Show("Exception occured while creating hooks: " + ex.Message);
            }

            while (true) Thread.Sleep(1000);    //just sleeeep
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
                    if (!Path.GetDirectoryName(filename).Contains("Songs")) break;
                    if (Helper.AddBeatmapsToDictionary(Path.GetDirectoryName(filename), ref _fileDictionary)) filename = NewFilePath; //this is a bg
                    break;
            }

            return CreateFileW(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }
    }
}
