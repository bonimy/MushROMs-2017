namespace MushROMs.Editors
{
    partial class CreatePaletteOptionsControl
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
            this.gbxNumColors = new System.Windows.Forms.GroupBox();
            this.nudNumColors = new System.Windows.Forms.NumericUpDown();
            this.chkFromCopy = new System.Windows.Forms.CheckBox();
            this.gbxNumColors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumColors)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxNumColors
            // 
            this.gbxNumColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxNumColors.Controls.Add(this.nudNumColors);
            this.gbxNumColors.Location = new System.Drawing.Point(0, 0);
            this.gbxNumColors.Name = "gbxNumColors";
            this.gbxNumColors.Size = new System.Drawing.Size(169, 45);
            this.gbxNumColors.TabIndex = 2;
            this.gbxNumColors.TabStop = false;
            this.gbxNumColors.Text = "Number of colors (hexadecimal)";
            // 
            // nudNumColors
            // 
            this.nudNumColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudNumColors.Hexadecimal = true;
            this.nudNumColors.Location = new System.Drawing.Point(3, 19);
            this.nudNumColors.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.nudNumColors.Maximum = new decimal(new int[] {
            8388608,
            0,
            0,
            0});
            this.nudNumColors.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumColors.Name = "nudNumColors";
            this.nudNumColors.Size = new System.Drawing.Size(160, 20);
            this.nudNumColors.TabIndex = 0;
            this.nudNumColors.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudNumColors.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // chkFromCopy
            // 
            this.chkFromCopy.AutoSize = true;
            this.chkFromCopy.Location = new System.Drawing.Point(0, 51);
            this.chkFromCopy.Name = "chkFromCopy";
            this.chkFromCopy.Size = new System.Drawing.Size(130, 17);
            this.chkFromCopy.TabIndex = 3;
            this.chkFromCopy.Text = "Create from copy data";
            this.chkFromCopy.UseVisualStyleBackColor = true;
            // 
            // CreatePaletteOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxNumColors);
            this.Controls.Add(this.chkFromCopy);
            this.Name = "CreatePaletteOptions";
            this.Size = new System.Drawing.Size(169, 69);
            this.gbxNumColors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumColors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxNumColors;
        private System.Windows.Forms.NumericUpDown nudNumColors;
        private System.Windows.Forms.CheckBox chkFromCopy;
    }
}
