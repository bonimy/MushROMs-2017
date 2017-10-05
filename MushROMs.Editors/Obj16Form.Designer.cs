namespace MushROMs.Editors
{
    partial class Obj16Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Obj16Form));
            this.vsbMain = new System.Windows.Forms.VScrollBar();
            this.hsbMain = new System.Windows.Forms.HScrollBar();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.tssMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.obj16Main = new MushROMs.Editors.Obj16Control();
            this.grayscaleDialog1 = new MushROMs.Editors.GrayscaleDialog();
            this.estMain = new MushROMs.Editors.Obj16StatusControl();
            this.stsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // vsbMain
            // 
            this.vsbMain.Location = new System.Drawing.Point(258, 0);
            this.vsbMain.Name = "vsbMain";
            this.vsbMain.Size = new System.Drawing.Size(17, 258);
            this.vsbMain.TabIndex = 7;
            // 
            // hsbMain
            // 
            this.hsbMain.Location = new System.Drawing.Point(0, 258);
            this.hsbMain.Name = "hsbMain";
            this.hsbMain.Size = new System.Drawing.Size(258, 17);
            this.hsbMain.TabIndex = 6;
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssMain});
            this.stsMain.Location = new System.Drawing.Point(0, 278);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(412, 22);
            this.stsMain.SizingGrip = false;
            this.stsMain.TabIndex = 8;
            this.stsMain.Text = "...";
            // 
            // tssMain
            // 
            this.tssMain.Name = "tssMain";
            this.tssMain.Size = new System.Drawing.Size(73, 17);
            this.tssMain.Text = "Editor Ready";
            // 
            // obj16Main
            // 
            this.obj16Main.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("obj16Main.BackgroundImage")));
            this.obj16Main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.obj16Main.HorizontalScrollBar = this.hsbMain;
            this.obj16Main.Location = new System.Drawing.Point(0, 0);
            this.obj16Main.Name = "obj16Main";
            this.obj16Main.Size = new System.Drawing.Size(258, 258);
            this.obj16Main.TabIndex = 0;
            this.obj16Main.VerticalScrollBar = this.vsbMain;
            this.obj16Main.ClientSizeChanged += new System.EventHandler(this.TileMapControl_ClientSizeChanged);
            // 
            // grayscaleDialog1
            // 
            this.grayscaleDialog1.Blue = 1F;
            this.grayscaleDialog1.Green = 1F;
            this.grayscaleDialog1.Preview = true;
            this.grayscaleDialog1.Red = 1F;
            this.grayscaleDialog1.ShowHelp = false;
            this.grayscaleDialog1.Tag = null;
            this.grayscaleDialog1.Title = "Custom Grayscale";
            // 
            // estMain
            // 
            this.estMain.Location = new System.Drawing.Point(278, 1);
            this.estMain.Name = "estMain";
            this.estMain.Size = new System.Drawing.Size(132, 50);
            this.estMain.TabIndex = 9;
            this.estMain.ZoomScaleChanged += new System.EventHandler(this.Obj16Status_ZoomScaleChanged);
            // 
            // Obj16Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 300);
            this.Controls.Add(this.estMain);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.vsbMain);
            this.Controls.Add(this.hsbMain);
            this.Controls.Add(this.obj16Main);
            this.Name = "Obj16Form";
            this.Text = "Obj16Form";
            this.TileMapControl = this.obj16Main;
            this.Load += new System.EventHandler(this.Obj16Form_Load);
            this.ResizeEnd += new System.EventHandler(this.Obj16Form_ResizeEnd);
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Obj16Control obj16Main;
        private System.Windows.Forms.VScrollBar vsbMain;
        private System.Windows.Forms.HScrollBar hsbMain;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStripStatusLabel tssMain;
        private GrayscaleDialog grayscaleDialog1;
        private Obj16StatusControl estMain;
    }
}