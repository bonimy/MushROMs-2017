namespace MushROMs.Editors
{
    partial class Obj16StatusControl
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
            this.gbxZoom = new System.Windows.Forms.GroupBox();
            this.cbxZoom = new System.Windows.Forms.ComboBox();
            this.gbxROMViewing = new System.Windows.Forms.GroupBox();
            this.btnNextByte = new System.Windows.Forms.Button();
            this.btnLastByte = new System.Windows.Forms.Button();
            this.gbxZoom.SuspendLayout();
            this.gbxROMViewing.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxZoom
            // 
            this.gbxZoom.Controls.Add(this.cbxZoom);
            this.gbxZoom.Location = new System.Drawing.Point(3, 3);
            this.gbxZoom.Name = "gbxZoom";
            this.gbxZoom.Size = new System.Drawing.Size(56, 44);
            this.gbxZoom.TabIndex = 13;
            this.gbxZoom.TabStop = false;
            this.gbxZoom.Text = "Zoom";
            // 
            // cbxZoom
            // 
            this.cbxZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cbxZoom.FormattingEnabled = true;
            this.cbxZoom.ItemHeight = 13;
            this.cbxZoom.Items.AddRange(new object[] {
            "1x",
            "2x",
            "3x",
            "4x"});
            this.cbxZoom.Location = new System.Drawing.Point(6, 16);
            this.cbxZoom.Name = "cbxZoom";
            this.cbxZoom.Size = new System.Drawing.Size(44, 21);
            this.cbxZoom.TabIndex = 0;
            // 
            // gbxROMViewing
            // 
            this.gbxROMViewing.Controls.Add(this.btnNextByte);
            this.gbxROMViewing.Controls.Add(this.btnLastByte);
            this.gbxROMViewing.Location = new System.Drawing.Point(65, 3);
            this.gbxROMViewing.Name = "gbxROMViewing";
            this.gbxROMViewing.Size = new System.Drawing.Size(64, 44);
            this.gbxROMViewing.TabIndex = 14;
            this.gbxROMViewing.TabStop = false;
            this.gbxROMViewing.Text = "ROM";
            // 
            // btnNextByte
            // 
            this.btnNextByte.AutoSize = true;
            this.btnNextByte.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNextByte.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.btnNextByte.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNextByte.Location = new System.Drawing.Point(35, 15);
            this.btnNextByte.Name = "btnNextByte";
            this.btnNextByte.Size = new System.Drawing.Size(23, 23);
            this.btnNextByte.TabIndex = 1;
            this.btnNextByte.Text = "+";
            this.btnNextByte.UseVisualStyleBackColor = true;
            // 
            // btnLastByte
            // 
            this.btnLastByte.AutoSize = true;
            this.btnLastByte.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLastByte.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.btnLastByte.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLastByte.Location = new System.Drawing.Point(6, 15);
            this.btnLastByte.Name = "btnLastByte";
            this.btnLastByte.Size = new System.Drawing.Size(23, 23);
            this.btnLastByte.TabIndex = 0;
            this.btnLastByte.Text = "-";
            this.btnLastByte.UseVisualStyleBackColor = true;
            // 
            // Obj16StatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxZoom);
            this.Controls.Add(this.gbxROMViewing);
            this.Name = "Obj16StatusControl";
            this.Size = new System.Drawing.Size(132, 50);
            this.gbxZoom.ResumeLayout(false);
            this.gbxROMViewing.ResumeLayout(false);
            this.gbxROMViewing.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxZoom;
        private System.Windows.Forms.ComboBox cbxZoom;
        private System.Windows.Forms.GroupBox gbxROMViewing;
        private System.Windows.Forms.Button btnNextByte;
        private System.Windows.Forms.Button btnLastByte;
    }
}
