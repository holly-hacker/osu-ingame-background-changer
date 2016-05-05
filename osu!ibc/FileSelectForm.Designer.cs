namespace osu_ibc
{
    partial class FileSelectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.cbReopen = new System.Windows.Forms.CheckBox();
            this.lblReopen = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPreview
            // 
            this.pbPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbPreview.Location = new System.Drawing.Point(94, 12);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(190, 122);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPreview.TabIndex = 0;
            this.pbPreview.TabStop = false;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(13, 12);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "Select BG";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDone.Enabled = false;
            this.btnDone.Location = new System.Drawing.Point(13, 112);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 2;
            this.btnDone.Text = "Done!";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // cbReopen
            // 
            this.cbReopen.AutoSize = true;
            this.cbReopen.BackColor = System.Drawing.Color.Transparent;
            this.cbReopen.Checked = true;
            this.cbReopen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReopen.Location = new System.Drawing.Point(13, 41);
            this.cbReopen.Name = "cbReopen";
            this.cbReopen.Size = new System.Drawing.Size(67, 17);
            this.cbReopen.TabIndex = 3;
            this.cbReopen.Text = "Re-open";
            this.cbReopen.UseVisualStyleBackColor = false;
            this.cbReopen.CheckedChanged += new System.EventHandler(this.cbReopen_CheckedChanged);
            // 
            // lblReopen
            // 
            this.lblReopen.Location = new System.Drawing.Point(13, 61);
            this.lblReopen.Name = "lblReopen";
            this.lblReopen.Size = new System.Drawing.Size(75, 44);
            this.lblReopen.TabIndex = 4;
            this.lblReopen.Text = "To reopen, hold P and O for 5 seconds.";
            // 
            // FileSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 146);
            this.Controls.Add(this.lblReopen);
            this.Controls.Add(this.cbReopen);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.pbPreview);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileSelectForm";
            this.Text = "osu!ibc - Image File Selector";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.CheckBox cbReopen;
        private System.Windows.Forms.Label lblReopen;
    }
}