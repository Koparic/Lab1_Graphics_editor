namespace Paint
{
    public partial class Form1 : Form
    {
        ToolStrip ts = new ToolStrip();
        private ToolStripButton btnCircle;
        private ToolStripButton btnRectangle;
        private ToolStripButton btnLine;
        CustomCanvas customCanvas = new CustomCanvas();

        public Form1()
        {
            InitializeComponent();

            customCanvas.Dock = DockStyle.Fill; 
            customCanvas.BackColor = Color.White;
            Controls.Add(customCanvas);

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

            Controls.Add(ts);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void BtnCircle_Click(object sender, EventArgs e)
        {
            customCanvas.ChooseShape(CustomCanvas.ActiveShapeType.Circle);
            BtnUpdate();
            btnCircle.Checked = true;
        }

        void BtnRectangle_Click(object sender, EventArgs e)
        {
            customCanvas.ChooseShape(CustomCanvas.ActiveShapeType.Rectangle);
            BtnUpdate();
            btnRectangle.Checked = true;
        }

        void BtnLine_Click(object sender, EventArgs e)
        {
            customCanvas.ChooseShape(CustomCanvas.ActiveShapeType.Line);
            BtnUpdate();
            btnLine.Checked = true;
        }

        void BtnUpdate()
        {
            foreach (ToolStripButton bttn in ts.Items)
            {
                bttn.Checked = false;
            }
        }

    }
}
