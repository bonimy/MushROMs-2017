namespace MushROMs.Editors
{
    partial class ColorizeForm
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
            this.btnReset = new System.Windows.Forms.Button();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.chkColorize = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxColorize = new System.Windows.Forms.GroupBox();
            this.lblWeight = new System.Windows.Forms.Label();
            this.ltbAlpha = new MushROMs.Controls.LinkedTrackBar();
            this.ntbAlpha = new MushROMs.Controls.IntegerTextBox();
            this.lblLightness = new System.Windows.Forms.Label();
            this.ltbLightness = new MushROMs.Controls.LinkedTrackBar();
            this.ntbLightness = new MushROMs.Controls.IntegerTextBox();
            this.lblSaturation = new System.Windows.Forms.Label();
            this.ltbSaturation = new MushROMs.Controls.LinkedTrackBar();
            this.ntbSaturation = new MushROMs.Controls.IntegerTextBox();
            this.lblHue = new System.Windows.Forms.Label();
            this.ltbHue = new MushROMs.Controls.LinkedTrackBar();
            this.ntbHue = new MushROMs.Controls.IntegerTextBox();
            this.chkLuma = new System.Windows.Forms.CheckBox();
            this.gbxColorize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ltbAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbLightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbHue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(151, 293);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Location = new System.Drawing.Point(12, 320);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(64, 17);
            this.chkPreview.TabIndex = 3;
            this.chkPreview.Text = "Preview";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.Preview_CheckedChanged);
            // 
            // chkColorize
            // 
            this.chkColorize.AutoSize = true;
            this.chkColorize.Location = new System.Drawing.Point(12, 297);
            this.chkColorize.Name = "chkColorize";
            this.chkColorize.Size = new System.Drawing.Size(63, 17);
            this.chkColorize.TabIndex = 1;
            this.chkColorize.Text = "Colorize";
            this.chkColorize.UseVisualStyleBackColor = true;
            this.chkColorize.CheckedChanged += new System.EventHandler(this.Colorize_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(232, 293);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(313, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbxColorize
            // 
            this.gbxColorize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxColorize.Controls.Add(this.lblWeight);
            this.gbxColorize.Controls.Add(this.ltbAlpha);
            this.gbxColorize.Controls.Add(this.ntbAlpha);
            this.gbxColorize.Controls.Add(this.lblLightness);
            this.gbxColorize.Controls.Add(this.ltbLightness);
            this.gbxColorize.Controls.Add(this.ntbLightness);
            this.gbxColorize.Controls.Add(this.lblSaturation);
            this.gbxColorize.Controls.Add(this.ltbSaturation);
            this.gbxColorize.Controls.Add(this.ntbSaturation);
            this.gbxColorize.Controls.Add(this.lblHue);
            this.gbxColorize.Controls.Add(this.ltbHue);
            this.gbxColorize.Controls.Add(this.ntbHue);
            this.gbxColorize.Location = new System.Drawing.Point(12, 12);
            this.gbxColorize.Name = "gbxColorize";
            this.gbxColorize.Size = new System.Drawing.Size(376, 275);
            this.gbxColorize.TabIndex = 0;
            this.gbxColorize.TabStop = false;
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(6, 208);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(37, 13);
            this.lblWeight.TabIndex = 0;
            this.lblWeight.Text = "Alpha:";
            // 
            // ltnAlpha
            // 
            this.ltbAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltbAlpha.IntegerComponent = this.ntbAlpha;
            this.ltbAlpha.Location = new System.Drawing.Point(6, 224);
            this.ltbAlpha.Maximum = 100;
            this.ltbAlpha.Name = "ltnAlpha";
            this.ltbAlpha.Size = new System.Drawing.Size(318, 45);
            this.ltbAlpha.TabIndex = 6;
            this.ltbAlpha.TickFrequency = 5;
            this.ltbAlpha.Value = 100;
            this.ltbAlpha.ValueChanged += new System.EventHandler(this.HSLE_ValueChanged);
            // 
            // ntbAlpha
            // 
            this.ntbAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ntbAlpha.Location = new System.Drawing.Point(330, 228);
            this.ntbAlpha.MaxLength = 4;
            this.ntbAlpha.Name = "ntbAlpha";
            this.ntbAlpha.Size = new System.Drawing.Size(40, 20);
            this.ntbAlpha.TabIndex = 7;
            this.ntbAlpha.Text = "100";
            this.ntbAlpha.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbAlpha.Value = 100;
            // 
            // lblLightness
            // 
            this.lblLightness.AutoSize = true;
            this.lblLightness.Location = new System.Drawing.Point(6, 144);
            this.lblLightness.Name = "lblLightness";
            this.lblLightness.Size = new System.Drawing.Size(55, 13);
            this.lblLightness.TabIndex = 0;
            this.lblLightness.Text = "Lightness:";
            // 
            // ltbLightness
            // 
            this.ltbLightness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltbLightness.IntegerComponent = this.ntbLightness;
            this.ltbLightness.LargeChange = 10;
            this.ltbLightness.Location = new System.Drawing.Point(6, 160);
            this.ltbLightness.Maximum = 100;
            this.ltbLightness.Minimum = -100;
            this.ltbLightness.Name = "ltbLightness";
            this.ltbLightness.Size = new System.Drawing.Size(318, 45);
            this.ltbLightness.TabIndex = 4;
            this.ltbLightness.TickFrequency = 10;
            this.ltbLightness.ValueChanged += new System.EventHandler(this.HSLE_ValueChanged);
            // 
            // ntbLightness
            // 
            this.ntbLightness.AllowNegative = true;
            this.ntbLightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ntbLightness.Location = new System.Drawing.Point(330, 164);
            this.ntbLightness.MaxLength = 4;
            this.ntbLightness.Name = "ntbLightness";
            this.ntbLightness.Size = new System.Drawing.Size(40, 20);
            this.ntbLightness.TabIndex = 5;
            this.ntbLightness.Text = "0";
            this.ntbLightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSaturation
            // 
            this.lblSaturation.AutoSize = true;
            this.lblSaturation.Location = new System.Drawing.Point(6, 80);
            this.lblSaturation.Name = "lblSaturation";
            this.lblSaturation.Size = new System.Drawing.Size(58, 13);
            this.lblSaturation.TabIndex = 0;
            this.lblSaturation.Text = "Saturation:";
            // 
            // ltbSaturation
            // 
            this.ltbSaturation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltbSaturation.IntegerComponent = this.ntbSaturation;
            this.ltbSaturation.LargeChange = 10;
            this.ltbSaturation.Location = new System.Drawing.Point(6, 96);
            this.ltbSaturation.Maximum = 100;
            this.ltbSaturation.Minimum = -100;
            this.ltbSaturation.Name = "ltbSaturation";
            this.ltbSaturation.Size = new System.Drawing.Size(318, 45);
            this.ltbSaturation.TabIndex = 2;
            this.ltbSaturation.TickFrequency = 10;
            this.ltbSaturation.ValueChanged += new System.EventHandler(this.HSLE_ValueChanged);
            // 
            // ntbSaturation
            // 
            this.ntbSaturation.AllowNegative = true;
            this.ntbSaturation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ntbSaturation.Location = new System.Drawing.Point(330, 100);
            this.ntbSaturation.MaxLength = 4;
            this.ntbSaturation.Name = "ntbSaturation";
            this.ntbSaturation.Size = new System.Drawing.Size(40, 20);
            this.ntbSaturation.TabIndex = 3;
            this.ntbSaturation.Text = "0";
            this.ntbSaturation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblHue
            // 
            this.lblHue.AutoSize = true;
            this.lblHue.Location = new System.Drawing.Point(6, 16);
            this.lblHue.Name = "lblHue";
            this.lblHue.Size = new System.Drawing.Size(30, 13);
            this.lblHue.TabIndex = 0;
            this.lblHue.Text = "Hue:";
            // 
            // ltbHue
            // 
            this.ltbHue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltbHue.IntegerComponent = this.ntbHue;
            this.ltbHue.LargeChange = 18;
            this.ltbHue.Location = new System.Drawing.Point(6, 32);
            this.ltbHue.Maximum = 180;
            this.ltbHue.Minimum = -180;
            this.ltbHue.Name = "ltbHue";
            this.ltbHue.Size = new System.Drawing.Size(318, 45);
            this.ltbHue.TabIndex = 0;
            this.ltbHue.TickFrequency = 18;
            this.ltbHue.ValueChanged += new System.EventHandler(this.HSLE_ValueChanged);
            // 
            // ntbHue
            // 
            this.ntbHue.AllowNegative = true;
            this.ntbHue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ntbHue.Location = new System.Drawing.Point(330, 36);
            this.ntbHue.MaxLength = 4;
            this.ntbHue.Name = "ntbHue";
            this.ntbHue.Size = new System.Drawing.Size(40, 20);
            this.ntbHue.TabIndex = 1;
            this.ntbHue.Text = "0";
            this.ntbHue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkLuma
            // 
            this.chkLuma.AutoSize = true;
            this.chkLuma.Location = new System.Drawing.Point(81, 297);
            this.chkLuma.Name = "chkLuma";
            this.chkLuma.Size = new System.Drawing.Size(52, 17);
            this.chkLuma.TabIndex = 2;
            this.chkLuma.Text = "Luma";
            this.chkLuma.UseVisualStyleBackColor = true;
            this.chkLuma.CheckedChanged += new System.EventHandler(this.chkLuma_CheckedChanged);
            // 
            // ColorizeForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(400, 344);
            this.Controls.Add(this.chkLuma);
            this.Controls.Add(this.gbxColorize);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.chkColorize);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorizeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Colorize Palette";
            this.Shown += new System.EventHandler(this.ColorizeForm_Shown);
            this.gbxColorize.ResumeLayout(false);
            this.gbxColorize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ltbAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbLightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbHue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.LinkedTrackBar ltbHue;
        private Controls.IntegerTextBox ntbHue;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.CheckBox chkColorize;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbxColorize;
        private System.Windows.Forms.Label lblHue;
        private System.Windows.Forms.Label lblWeight;
        private Controls.LinkedTrackBar ltbAlpha;
        private Controls.IntegerTextBox ntbAlpha;
        private System.Windows.Forms.Label lblLightness;
        private Controls.LinkedTrackBar ltbLightness;
        private Controls.IntegerTextBox ntbLightness;
        private System.Windows.Forms.Label lblSaturation;
        private Controls.LinkedTrackBar ltbSaturation;
        private Controls.IntegerTextBox ntbSaturation;
        private System.Windows.Forms.CheckBox chkLuma;
    }
}