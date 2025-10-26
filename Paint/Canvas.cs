using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace Paint
{
    public class CustomCanvas : Panel
    {
        private List<Shape> shapes = new List<Shape>();
        private Shape curShape;
        private int selectedShapeInd = -1;
        private Point start;
        private Point end;
        private bool isDrawing = false;
        private bool isMoving = false;
        private bool isDraging = false;
        private Form1 form1;

        public enum ActiveShapeType
        {
            None,
            Circle,
            Rectangle,
            Line
        };

        private ActiveShapeType activeShape = ActiveShapeType.None;
        private Brush curBrush = Brushes.Black;
        private Pen curPen = Pens.Black;
        public void ChooseShape(ActiveShapeType type)
        {
            selectedShapeInd = -1;
            activeShape = type;
        }
        public void ChooseInColor(Color c)
        {
            curBrush = new SolidBrush(c);
        }
        public void ChooseOutColor(Color c)
        {
            curPen = new Pen(c);
        }
        public void SetMoving(bool b)
        {
            isMoving = b;
        }

        public CustomCanvas(Form1 form)
        {
            form1 = form;
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            BackColor = Color.White;
        }
        public void LoadShapes(List<Shape> s)
        {
            if (s != null)
            {
                shapes = s;
            }
        }
        public List<Shape> SaveShapes()
        {
            return shapes;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            foreach (var shape in shapes)
            {
                shape.Draw(g);
            }
            if (isDrawing || (isMoving && selectedShapeInd > -1))
            {
                curShape.Draw(g);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                start = e.Location;
                if (isMoving)
                {
                    SelectShape(start);
                    isDraging = true;
                    Invalidate();
                }
                else isDrawing = activeShape != ActiveShapeType.None;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            end = e.Location;
            if (isDrawing)
            {
                CreateShape(start, end, activeShape);
                AddNewShape();
            }
            else if (isDraging && selectedShapeInd > -1)
            {
                Point tlp = new Point(Math.Min(shapes[selectedShapeInd].TopLeftPoint.X, shapes[selectedShapeInd].BottomRightPoint.X), Math.Min(shapes[selectedShapeInd].TopLeftPoint.Y, shapes[selectedShapeInd].BottomRightPoint.Y));
                Point brp = new Point(Math.Max(shapes[selectedShapeInd].TopLeftPoint.X, shapes[selectedShapeInd].BottomRightPoint.X), Math.Max(shapes[selectedShapeInd].TopLeftPoint.Y, shapes[selectedShapeInd].BottomRightPoint.Y));
                tlp.Offset(end.X - start.X, end.Y - start.Y); brp.Offset(end.X - start.X, end.Y - start.Y);
                curShape.TopLeftPoint = tlp;
                curShape.BottomRightPoint = brp;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (isDrawing) 
            { 
                end = e.Location;
                CreateShape(start, end, activeShape);
                isDrawing = false;
                if ((end.X - start.X) * (end.X - start.X) + (end.Y - start.Y) * (end.Y - start.Y) >= 5)
                {
                    AddNewShape();
                }
            }
            else if (isMoving && isDraging && selectedShapeInd > -1)
            {
                isDraging = false;
                Point tlp = shapes[selectedShapeInd].TopLeftPoint, brp = shapes[selectedShapeInd].BottomRightPoint;
                tlp.Offset(end.X - start.X, end.Y - start.Y); brp.Offset(end.X - start.X, end.Y - start.Y);
                shapes[selectedShapeInd].TopLeftPoint = tlp;
                shapes[selectedShapeInd].BottomRightPoint = brp;
                form1.SetSelectedShape(shapes[selectedShapeInd]);
                Invalidate();
            }
        }

        private void AddNewShape()
        {
            if (!isDrawing)
            {
                shapes.Add(curShape);
            }
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
            curShape.FillBrush = curBrush;
            curShape.OutlinePen = curPen;
            form1.SetSelectedShape(curShape);
        }


        private void SelectShape(Point point)
        {
            for (int i = shapes.Count-1; i > -1; i--)
            {
                if (shapes[i].ContainsPoint(point))
                {
                    selectedShapeInd = i;
                    form1.SetSelectedShape(shapes[i]);
                    curShape = new Rectangle(shapes[i].TopLeftPoint, shapes[i].BottomRightPoint);
                    curBrush = new SolidBrush(Color.Transparent);
                    curPen = new Pen(Color.DarkGray, 5);
                    curPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    curShape.FillBrush = curBrush;
                    curShape.OutlinePen = curPen;
                    return;
                }
            }
            selectedShapeInd = -1;
        }


        public void RemoveShape()
        {
            if (selectedShapeInd > -1)
            {
                shapes.RemoveAt(selectedShapeInd);
                selectedShapeInd = -1;
                Invalidate();
            }
        }

        public void ChangeShapeLayer(bool up)
        {
            if (selectedShapeInd > -1){
            int index2 = selectedShapeInd + (up ? 1 : -1);
                if (index2 > -1 && index2 < shapes.Count)
                {
                    Shape temp = shapes[selectedShapeInd];
                    shapes[selectedShapeInd] = shapes[index2];
                    shapes[index2] = temp;
                    selectedShapeInd = index2;
                    Invalidate();
                }
            }
        }
    }
}
