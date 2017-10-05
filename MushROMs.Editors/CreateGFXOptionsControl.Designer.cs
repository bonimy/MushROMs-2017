namespace MushROMs.Editors
{
    partial class CreateGFXOptionsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbxNumTiles = new System.Windows.Forms.GroupBox();
            this.nudNumTiles = new System.Windows.Forms.NumericUpDown();
            this.chkFromCopy = new System.Windows.Forms.CheckBox();
            this.gpxGraphicsFormat = new System.Windows.Forms.GroupBox();
            this.cbxGraphicsFormat = new System.Windows.Forms.ComboBox();
            this.gbxNumTiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTiles)).BeginInit();
            this.gpxGraphicsFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxNumTiles
            // 
            this.gbxNumTiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxNumTiles.Controls.Add(this.nudNumTiles);
            this.gbxNumTiles.Location = new System.Drawing.Point(0, 0);
            this.gbxNumTiles.Name = "gbxNumTiles";
            this.gbxNumTiles.Size = new System.Drawing.Size(162, 45);
            this.gbxNumTiles.TabIndex = 0;
            this.gbxNumTiles.TabStop = false;
            this.gbxNumTiles.Text = "Number of tiles (hexadecimal)";
            // 
            // nudNumTiles
            // 
            this.nudNumTiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudNumTiles.Hexadecimal = true;
            this.nudNumTiles.Location = new System.Drawing.Point(3, 19);
            this.nudNumTiles.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.nudNumTiles.Maximum = new decimal(new int[] {
            8388608,
            0,
            0,
            0});
            this.nudNumTiles.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumTiles.Name = "nudNumTiles";
            this.nudNumTiles.Size = new System.Drawing.Size(153, 20);
            this.nudNumTiles.TabIndex = 0;
            this.nudNumTiles.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudNumTiles.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // chkFromCopy
            // 
            this.chkFromCopy.AutoSize = true;
            this.chkFromCopy.Location = new System.Drawing.Point(3, 103);
            this.chkFromCopy.Name = "chkFromCopy";
            this.chkFromCopy.Size = new System.Drawing.Size(130, 17);
            this.chkFromCopy.TabIndex = 2;
            this.chkFromCopy.Text = "Create from copy data";
            this.chkFromCopy.UseVisualStyleBackColor = true;
            // 
            // gpxGraphicsFormat
            // 
            this.gpxGraphicsFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpxGraphicsFormat.Controls.Add(this.cbxGraphicsFormat);
            this.gpxGraphicsFormat.Location = new System.Drawing.Point(0, 51);
            this.gpxGraphicsFormat.Name = "gpxGraphicsFormat";
            this.gpxGraphicsFormat.Size = new System.Drawing.Size(162, 46);
            this.gpxGraphicsFormat.TabIndex = 1;
            this.gpxGraphicsFormat.TabStop = false;
            this.gpxGraphicsFormat.Text = "Graphics Format";
            // 
            // cbxGraphicsFormat
            // 
            this.cbxGraphicsFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxGraphicsFormat.FormattingEnabled = true;
            this.cbxGraphicsFormat.Items.AddRange(new object[] {
            "1BPP 8x8",
            "2BPP NES",
            "2BPP GB",
            "2BPP NGP",
            "2BPP VB",
            "2BPP MSX",
            "3BPP SNES",
            "3BPP 8x8",
            "4BPP SNES",
            "4BPP GBA",
            "4BPP SMS",
            "4BPP MSX",
            "4BPP 8x8",
            "4BPP PCE",
            "1BPP 16x16",
            "2BPP 16x16 PCE",
            "8BPP SNES",
            "8BB Mode7"});
            this.cbxGraphicsFormat.Location = new System.Drawing.Point(3, 19);
            this.cbxGraphicsFormat.Name = "cbxGraphicsFormat";
            this.cbxGraphicsFormat.Size = new System.Drawing.Size(153, 21);
            this.cbxGraphicsFormat.TabIndex = 0;
            // 
            // CreateGFXOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpxGraphicsFormat);
            this.Controls.Add(this.gbxNumTiles);
            this.Controls.Add(this.chkFromCopy);
            this.Name = "CreateGFXOptions";
            this.Size = new System.Drawing.Size(165, 122);
            this.gbxNumTiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTiles)).EndInit();
            this.gpxGraphicsFormat.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxNumTiles;
        private System.Windows.Forms.NumericUpDown nudNumTiles;
        private System.Windows.Forms.CheckBox chkFromCopy;
        private System.Windows.Forms.GroupBox gpxGraphicsFormat;
        private System.Windows.Forms.ComboBox cbxGraphicsFormat;
    }
}
