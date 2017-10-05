namespace MushROMs.Editors
{
    partial class GFXForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GFXForm));
            this.vsbMain = new System.Windows.Forms.VScrollBar();
            this.hsbMain = new System.Windows.Forms.HScrollBar();
            this.gfcMain = new MushROMs.Editors.GFXControl();
            this.estMain = new MushROMs.Editors.GFXStatusControl();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.tssMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.plcColors = new MushROMs.Editors.PaletteControl();
            this.stsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // vsbMain
            // 
            this.vsbMain.Location = new System.Drawing.Point(258, 0);
            this.vsbMain.Name = "vsbMain";
            this.vsbMain.Size = new System.Drawing.Size(17, 258);
            this.vsbMain.TabIndex = 5;
            // 
            // hsbMain
            // 
            this.hsbMain.Location = new System.Drawing.Point(0, 258);
            this.hsbMain.Name = "hsbMain";
            this.hsbMain.Size = new System.Drawing.Size(258, 17);
            this.hsbMain.TabIndex = 4;
            // 
            // gfcMain
            // 
            this.gfcMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gfcMain.BackgroundImage")));
            this.gfcMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gfcMain.HorizontalScrollBar = this.hsbMain;
            this.gfcMain.Location = new System.Drawing.Point(0, 0);
            this.gfcMain.Name = "gfcMain";
            this.gfcMain.Size = new System.Drawing.Size(258, 258);
            this.gfcMain.TabIndex = 0;
            this.gfcMain.VerticalScrollBar = this.vsbMain;
            // 
            // estMain
            // 
            this.estMain.Location = new System.Drawing.Point(278, 0);
            this.estMain.Name = "estMain";
            this.estMain.Size = new System.Drawing.Size(126, 96);
            this.estMain.TabIndex = 6;
            this.estMain.GraphicsFormatChanged += new System.EventHandler(this.GFXStatus_GraphicsFormatChanged);
            this.estMain.ZoomScaleChanged += new System.EventHandler(this.GFXStatus_ZoomScaleChanged);
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssMain});
            this.stsMain.Location = new System.Drawing.Point(0, 292);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(406, 22);
            this.stsMain.SizingGrip = false;
            this.stsMain.TabIndex = 0;
            this.stsMain.Text = "...";
            // 
            // tssMain
            // 
            this.tssMain.Name = "tssMain";
            this.tssMain.Size = new System.Drawing.Size(73, 17);
            this.tssMain.Text = "Editor Ready";
            // 
            // plcColors
            // 
            this.plcColors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("plcColors.BackgroundImage")));
            this.plcColors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plcColors.HorizontalScrollBar = null;
            this.plcColors.Location = new System.Drawing.Point(0, 276);
            this.plcColors.Name = "plcColors";
            this.plcColors.Size = new System.Drawing.Size(256, 16);
            this.plcColors.TabIndex = 7;
            this.plcColors.VerticalScrollBar = null;
            // 
            // GFXForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 314);
            this.Controls.Add(this.plcColors);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.estMain);
            this.Controls.Add(this.gfcMain);
            this.Controls.Add(this.vsbMain);
            this.Controls.Add(this.hsbMain);
            this.Name = "GFXForm";
            this.Text = "GFXForm";
            this.TileMapControl = this.gfcMain;
            this.Load += new System.EventHandler(this.GFXForm_Load);
            this.ResizeEnd += new System.EventHandler(this.GFXForm_ResizeEnd);
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.VScrollBar vsbMain;
        private System.Windows.Forms.HScrollBar hsbMain;
        private GFXControl gfcMain;
        private GFXStatusControl estMain;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStripStatusLabel tssMain;
        private PaletteControl plcColors;
    }
}