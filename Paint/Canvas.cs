using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Paint
{
    using System.Drawing;
    using System.Windows.Forms;
  

    public class CustomCanvas : Panel
    {
        private List<Shape> shapes = new List<Shape>();
        private Shape curShape;
        private Point start;
        private Point end;
        private bool isDrawing = false;

        public enum ActiveShapeType
        {
            None,
            Circle,
            Rectangle,
            Line
        };

        private ActiveShapeType activeShape = ActiveShapeType.None;
        public void ChooseShape(ActiveShapeType type)
        {
            activeShape = type;
        }

        public CustomCanvas()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            BackColor = Color.White;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            foreach (var shape in shapes)
            {
                shape.Draw(g);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                start = e.Location;
                isDrawing = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDrawing)
            {
                end = e.Location;
                CreateShape(start, end, activeShape);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (isDrawing) 
            { 
                end = e.Location;
                CreateShape(start, end, activeShape);
                AddNewShape();
            }
        }

        private void AddNewShape()
        {
            shapes.Add(curShape);
            Invalidate();
        }

        private void CreateShape(Point startPoint, Point endPoint, ActiveShapeType shapeType)
        {
            switch (shapeType)
            {
                case ActiveShapeType.Circle:
                    curShape = new Circle(startPoint, endPoint);
                    break;
                case ActiveShapeType.Rectangle:
                    curShape = new Rectangle(startPoint, endPoint);
                    break;
                case ActiveShapeType.Line:
                    curShape = new Line(startPoint, endPoint);
                    break;
                default:
                    break;
            }
        }

        private void MoveSelectedShape(Point currentLocation)
        {
        }

        private void StopDragging()
        {
        }
    }
}
