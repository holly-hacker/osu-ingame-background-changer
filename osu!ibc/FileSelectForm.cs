using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace osu_ibc
{
    public partial class FileSelectForm : Form
    {
        public string SelectedFile;
        public bool AllowReopen;

        public FileSelectForm()
        {
            InitializeComponent();

            //set return value
            DialogResult = DialogResult.Cancel;

            //snag icon from osu!, purely for esthetic purposes
            if (!UpdateFormIcon()) Icon = SystemIcons.Shield;

            //set initial values
            AllowReopen = cbReopen.Checked;

            //bring to front
            BringToFront();
            Focus();
        }

        public bool UpdateFormIcon()
        {
            try {
                string path = Process.GetCurrentProcess().MainModule.FileName;  //get osu! location
                Icon = Icon.ExtractAssociatedIcon(path);                        //extract icon from file
                return true;
            }
            catch (Exception ex) {
                Debug.WriteLine("Could not get osu!'s icon: " + ex.Message);
                return false;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //open file dialog and select png|jpg file
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg|All Files|*";  //allowing all files if users feel like experimenting
            if (ofd.ShowDialog() == DialogResult.OK) {
                SelectedFile = ofd.FileName;                //set string value for path
                pbPreview.ImageLocation = SelectedFile;     //update preview
                btnDone.Enabled = true;                     //enable button
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK; //set return value to OK
        }

        private void cbReopen_CheckedChanged(object sender, EventArgs e)
        {
            AllowReopen = ((CheckBox)sender).Checked;
        }
    }
}
