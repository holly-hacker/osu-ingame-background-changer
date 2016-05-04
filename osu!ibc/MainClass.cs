using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyHook;

namespace osu_ibc
{
    public class MainClass : IEntryPoint
    {
        //TODO: create hkCreateFileA, in case W doesn't work completely

        public static string NewFilePath = "";  //TODO seperate png and jpeg
        LocalHook hkCreateFileW;
        static Dictionary<string, string> fileDictionary = new Dictionary<string, string>();

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

        public MainClass(RemoteHooking.IContext InContext) {}

        public void Run(RemoteHooking.IContext InContext)  //method that gets called
        {
            //spawn FileSelectForm
            FileSelectForm fsf = new FileSelectForm();
            if (fsf.ShowDialog() != DialogResult.OK) {
                //user didn't complete file selection, show warning
                if (MessageBox.Show("You did not select a file.\nDo you want to use invalid files (yes) or exit (no)?","osu!ibc - Warning!",MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
                    return;
                }
            }
            else {
                NewFilePath = fsf.SelectedFile;
            }

            //create hooks for CreateFileA and CreateFileW
            try {
                hkCreateFileW = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"), new DCreateFileW(CreateFileWHook), this);
                hkCreateFileW.ThreadACL.SetExclusiveACL(new[] {0});
                RemoteHooking.WakeUpProcess();
                MessageBox.Show("Finished?");
            }
            catch (Exception ex) {
                MessageBox.Show("Exception occured while creating hooks: " + ex.Message);
            }

            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        static IntPtr CreateFileWHook(
                        [MarshalAs(UnmanagedType.LPWStr)] string filename,
                        [MarshalAs(UnmanagedType.U4)] FileAccess access,
                        [MarshalAs(UnmanagedType.U4)] FileShare share,
                        IntPtr securityAttributes,
                        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                        [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
                        IntPtr templateFile)
        {
            string extension = Path.GetExtension(filename);
            switch (extension) {
                case ".png":
                case ".jpg":
                case ".jpeg":

                    if (!Path.GetDirectoryName(filename).Contains("Songs")) {    //TODO get songs directory by reading cfg file, return if not in it
                        goto end;
                    }

                    //if image is read, check if folder contains a .osu file
                    if (Helper.AddBeatmapsToDictionary(Path.GetDirectoryName(filename), ref fileDictionary)) {
                        if (extension == Path.GetExtension(NewFilePath)) {
                            //TODO
                        }
                    }
                    
                    break;
            }

            end:
            return CreateFileW(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }
    }
}
