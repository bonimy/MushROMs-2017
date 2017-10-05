using System;
using System.IO;
using System.Windows.Forms;
using MushROMs.Editors;
using MushROMs.SNES;

namespace MushROMs.SMAS.Controls
{
    public partial class Form1 : Form
    {
        public ROM SMAS
        {
            get;
            private set;
        }

        public PaletteEditor Palette
        {
            get;
            private set;
        }

        public GFXEditor GFX
        {
            get;
            private set;
        }

        public Obj16Editor Map16Editor
        {
            get;
            private set;
        }

        public LevelData LevelData
        {
            get;
            private set;
        }

        private int _currentMap
        {
            get;
            set;
        }

        public int CurrentMap
        {
            get => _currentMap;
            set
            {
                _currentMap = value;
                LevelData = new LevelData(SMAS, CurrentMap);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        public void OpenFile()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.DefaultExt = ".sfc";
                dlg.Filter = "Super Mario All-Stars ROMs (*.smc;*.sfc)|*.smc;*.sfc|All files (*.*)|*.*";
                dlg.FilterIndex = 0;
                dlg.Multiselect = false;
                dlg.Title = "Select ROM";
                if (dlg.ShowDialog() == DialogResult.OK)
                    OpenFile(dlg.FileName);
            }
        }

        public void OpenFile(string path)
        {
            var data = File.ReadAllBytes(path);
            SMAS = ROM.CreateFromData(data);

            CurrentMap = 0;
            CreatePaletteForm();
            CreateGFXForm();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void CreatePaletteForm()
        {
            Palette = LevelData.LoadPalette();
            var form = new PaletteForm(Palette)
            {
                MdiParent = this
            };
            form.Show();
        }

        private void CreateGFXForm()
        {
            GFX = LevelData.GetBG1BG2GFX();
            GFX.GraphicsFormat = GraphicsFormat.Format4BppSnes;
            var form = new GFXForm(GFX);
            form.TileMapControl.Palette = Palette;
            form.MdiParent = this;
            form.Show();

            Map16Editor = LevelData.GetMap16Tiles();
            var f2 = new Obj16Form(Map16Editor);
            f2.TileMapControl.Palette = Palette;
            Palette.SelectAll();
            f2.TileMapControl.PaletteData = Palette.GetSelectionData();
            GFX.SelectAll();
            f2.TileMapControl.GFXData = GFX.GetSelectionData();
            f2.MdiParent = this;
            f2.Show();
        }
    }
}
