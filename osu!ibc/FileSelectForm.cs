using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osu_ibc
{
    public partial class FileSelectForm : Form
    {
        public string SelectedFile;

        public FileSelectForm()
        {
            InitializeComponent();

            //set return value
            DialogResult = DialogResult.Cancel;

            //snag icon from osu!, purely for esthetic purposes
            if (!UpdateFormIcon()) Icon = SystemIcons.Shield;
        }

        public bool UpdateFormIcon()
        {
            try {
                string path = Process.GetCurrentProcess().MainModule.FileName; //get osu! location
                Icon = Icon.ExtractAssociatedIcon(path); //extract icon from file
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
                //set string value for path
                SelectedFile = ofd.FileName;

                //update preview
                pbPreview.ImageLocation = SelectedFile;

                //enable button
                btnDone.Enabled = true;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            //cleanup?
            //set return value to OK
            DialogResult = DialogResult.OK;
        }
    }
}
