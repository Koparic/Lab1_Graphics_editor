namespace Paint
{
    public partial class Form1 : Form
    {
        private ToolStrip ts = new ToolStrip();
        private CustomCanvas customCanvas;
        private ShapeInfoPanel infoPanel = new ShapeInfoPanel();

        private ToolStripButton inColorPickerBtn;
        private ToolStripButton outColorPickerBtn;
        private ToolStripButton btnCircle;
        private ToolStripButton btnRectangle;
        private ToolStripButton btnLine;
        private ToolStripButton btnMove;

        private Color currentInColor = Color.Black;
        private Color currentOutColor = Color.Black;
        private Color[] colors = {Color.Red, Color.Blue, Color.Green, Color.Black, Color.White,
                Color.Yellow, Color.Purple, Color.Gray, Color.Silver, Color.Brown};

        public Form1()
        {
            InitializeComponent();

            customCanvas = new CustomCanvas(this);
            customCanvas.Dock = DockStyle.Fill; 
            customCanvas.BackColor = Color.White;
            Controls.Add(customCanvas);

            infoPanel.Dock = DockStyle.Right;
            infoPanel.ShapeRemoved += InfoPanel_ShapeRemoved;
            infoPanel.ShapeLevelChanged += InfoPanel_ShapeLevelChanged;
            Controls.Add(infoPanel);

            ts.Dock = DockStyle.Top;

            btnCircle = new ToolStripButton("Круг");
            btnCircle.Click += BtnCircle_Click;
            ts.Items.Add(btnCircle);

            btnRectangle = new ToolStripButton("Прямоугольник");
            btnRectangle.Click += BtnRectangle_Click;
            ts.Items.Add(btnRectangle);

            btnLine = new ToolStripButton("Линия");
            btnLine.Click += BtnLine_Click;
            ts.Items.Add(btnLine);

            btnMove = new ToolStripButton("Передвижение");
            btnMove.Click += BtnMove_Click;
            ts.Items.Add(btnMove);

            inColorPickerBtn = new ToolStripButton(GetColorSample(currentInColor));
            inColorPickerBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            inColorPickerBtn.Click += (s, e) => ShowPalette(inColorPickerBtn, ref currentInColor);
            ts.Items.Add(inColorPickerBtn);
            outColorPickerBtn = new ToolStripButton(GetColorSample(currentOutColor));
            outColorPickerBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            outColorPickerBtn.Click += (s, e) => ShowPalette(outColorPickerBtn, ref currentOutColor);
            ts.Items.Add(outColorPickerBtn);

            Controls.Add(ts);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void BtnCircle_Click(object sender, EventArgs e)
        {
            ColorUpldate();
            customCanvas.SetMoving(false);
            customCanvas.ChooseShape(CustomCanvas.ActiveShapeType.Circle);
            BtnUpdate();
            btnCircle.Checked = true;
        }
        void BtnRectangle_Click(object sender, EventArgs e)
        {
            ColorUpldate();
            customCanvas.SetMoving(false);
            customCanvas.ChooseShape(CustomCanvas.ActiveShapeType.Rectangle);
            BtnUpdate();
            btnRectangle.Checked = true;
        }
        void BtnLine_Click(object sender, EventArgs e)
        {
            ColorUpldate();
            customCanvas.SetMoving(false);
            customCanvas.ChooseShape(CustomCanvas.ActiveShapeType.Line);
            BtnUpdate();
            btnLine.Checked = true;
        }
        void BtnMove_Click(object sender, EventArgs e)
        {
            customCanvas.SetMoving(true);
            BtnUpdate();
            btnMove.Checked = true;
        }
        private void ShowPalette(ToolStripButton button, ref Color targetColor)
        {
            ContextMenuStrip palette = new ContextMenuStrip();

            foreach (var color in colors)
            {
                ToolStripMenuItem item = new ToolStripMenuItem("", null, Item_Click);
                item.ImageScaling = ToolStripItemImageScaling.None;
                item.Image = GetColorSample(color);
                item.Tag = Tuple.Create(button, color);
                palette.Items.Add(item);
            }
            palette.Show(button.Bounds.Left, button.Bounds.Bottom);
        }
        private void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            var tuple = (Tuple<ToolStripButton, Color>)clickedItem.Tag;
            ToolStripButton button = tuple.Item1;
            Color selectedColor = tuple.Item2;

            if (button == inColorPickerBtn)
            {
                currentInColor = selectedColor;
            }
            else if (button == outColorPickerBtn)
            {
                currentOutColor = selectedColor;
            }
            ColorUpldate();
            button.Image = GetColorSample(selectedColor);
        }
        private Image GetColorSample(Color color)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(color);
            }
            return bmp;
        }
        private void ColorUpldate()
        {
            customCanvas.ChooseInColor(currentInColor);
            customCanvas.ChooseOutColor(currentOutColor);
        }
        void BtnUpdate()
        {
            foreach (ToolStripButton bttn in ts.Items)
            {
                bttn.Checked = false;
            }
        }
        private void InfoPanel_ShapeRemoved(bool a)
        {
            customCanvas.RemoveShape();
        }
        private void InfoPanel_ShapeLevelChanged(bool up)
        {
            customCanvas.ChangeShapeLayer(up);
        }
        public void SetSelectedShape(Shape shape)
        {
            infoPanel.SetSelectedShape(shape);
        }
    }
}
