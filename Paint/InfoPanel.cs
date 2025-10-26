using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class ShapeInfoPanel : UserControl
    {
        private Label lblInfo;
        private Button btnRaise;
        private Button btnLower;
        private Button btnRemove;
        private Shape selectedShape;

        public event Action<bool> ShapeRemoved;
        public event Action<bool> ShapeLevelChanged;

        public ShapeInfoPanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            lblInfo = new Label();
            lblInfo.AutoSize = true;
            lblInfo.Text = "Фигура не выбрана.";
            lblInfo.Location = new Point(10, 10);
            Controls.Add(lblInfo);

            btnRaise = new Button();
            btnRaise.Text = "На передний план";
            btnRaise.Location = new Point(10, 40);
            btnRaise.Click += RaiseShape;
            Controls.Add(btnRaise);

            btnLower = new Button();
            btnLower.Text = "На задний план";
            btnLower.Location = new Point(10, 65);
            btnLower.Click += LowerShape;
            Controls.Add(btnLower);

            btnRemove = new Button();
            btnRemove.Text = "Удалить";
            btnRemove.Location = new Point(10, 90);
            btnRemove.Click += RemoveShape;
            Controls.Add(btnRemove);

            KeyDown += ShapeManagerPanel_KeyDown;

            AutoScaleDimensions = new SizeF(4F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = "ShapeInfoPanel";

            ResumeLayout(false);
        }

        private void RaiseShape(object sender, EventArgs e)
        {
            if (selectedShape != null)
            {
                ShapeLevelChanged?.Invoke(true);
            }
        }

        private void LowerShape(object sender, EventArgs e)
        {
            if (selectedShape != null)
            {
                ShapeLevelChanged?.Invoke(false);
            }
        }

        private void RemoveShape(object sender, EventArgs e)
        {
            if (selectedShape != null)
            {
                ShapeRemoved?.Invoke(true);
                SetSelectedShape(null);
            }
        }

        private void ShapeManagerPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveShape(sender, e);
            }
        }

        public void SetSelectedShape(Shape shape)
        {
            selectedShape = shape;
            if (shape != null)
            {
                lblInfo.Text = $"Выделена фигура: {shape}";
            }
            else
            {
                lblInfo.Text = "Фигура не выбрана.";
            }
        }

    }
}
