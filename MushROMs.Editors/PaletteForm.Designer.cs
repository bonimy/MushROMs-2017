namespace MushROMs.Editors
{
    partial class PaletteForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteForm));
            this.hsbMain = new System.Windows.Forms.HScrollBar();
            this.vsbMain = new System.Windows.Forms.VScrollBar();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.tssMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.estMain = new MushROMs.Editors.PaletteStatus();
            this.plcMain = new MushROMs.Editors.PaletteControl();
            this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.invertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stsMain.SuspendLayout();
            this.cmsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // hsbMain
            // 
            this.hsbMain.LargeChange = 16;
            this.hsbMain.Location = new System.Drawing.Point(0, 258);
            this.hsbMain.Maximum = 31;
            this.hsbMain.Name = "hsbMain";
            this.hsbMain.Size = new System.Drawing.Size(258, 17);
            this.hsbMain.TabIndex = 1;
            // 
            // vsbMain
            // 
            this.vsbMain.LargeChange = 16;
            this.vsbMain.Location = new System.Drawing.Point(258, 0);
            this.vsbMain.Maximum = 16;
            this.vsbMain.Name = "vsbMain";
            this.vsbMain.Size = new System.Drawing.Size(17, 258);
            this.vsbMain.TabIndex = 2;
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssMain});
            this.stsMain.Location = new System.Drawing.Point(0, 275);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(408, 22);
            this.stsMain.SizingGrip = false;
            this.stsMain.TabIndex = 4;
            // 
            // tssMain
            // 
            this.tssMain.Name = "tssMain";
            this.tssMain.Size = new System.Drawing.Size(76, 17);
            this.tssMain.Text = "Editor Ready!";
            // 
            // estMain
            // 
            this.estMain.Location = new System.Drawing.Point(276, 0);
            this.estMain.Margin = new System.Windows.Forms.Padding(0);
            this.estMain.Name = "estMain";
            this.estMain.Size = new System.Drawing.Size(132, 132);
            this.estMain.TabIndex = 3;
            this.estMain.ZoomScaleChanged += new System.EventHandler(this.PaletteStatus_ZoomScaleChanged);
            // 
            // plcMain
            // 
            this.plcMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("plcMain.BackgroundImage")));
            this.plcMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plcMain.HorizontalScrollBar = this.hsbMain;
            this.plcMain.Location = new System.Drawing.Point(0, 0);
            this.plcMain.Name = "plcMain";
            this.plcMain.Size = new System.Drawing.Size(258, 258);
            this.plcMain.TabIndex = 0;
            this.plcMain.VerticalScrollBar = this.vsbMain;
            this.plcMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.plcMain_KeyUp);
            this.plcMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.plcMain_MouseClick);
            this.plcMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.plcMain_MouseDoubleClick);
            // 
            // cmsMain
            // 
            this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.invertToolStripMenuItem});
            this.cmsMain.Name = "cmsMain";
            this.cmsMain.Size = new System.Drawing.Size(153, 48);
            // 
            // invertToolStripMenuItem
            // 
            this.invertToolStripMenuItem.Name = "invertToolStripMenuItem";
            this.invertToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.invertToolStripMenuItem.Text = "&Invert";
            // 
            // PaletteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 297);
            this.Controls.Add(this.plcMain);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.estMain);
            this.Controls.Add(this.vsbMain);
            this.Controls.Add(this.hsbMain);
            this.MaximizeBox = false;
            this.Name = "PaletteForm";
            this.Text = "Palette Editor";
            this.TileMapControl = this.plcMain;
            this.Load += new System.EventHandler(this.PaletteForm_Load);
            this.ResizeEnd += new System.EventHandler(this.PaletteForm_ResizeEnd);
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.cmsMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.HScrollBar hsbMain;
        private System.Windows.Forms.VScrollBar vsbMain;
        private PaletteStatus estMain;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStripStatusLabel tssMain;
        private PaletteControl plcMain;
        private System.Windows.Forms.ContextMenuStrip cmsMain;
        private System.Windows.Forms.ToolStripMenuItem invertToolStripMenuItem;
    }
}