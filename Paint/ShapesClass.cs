using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    abstract class Shape
    {
        public Point TopLeftPoint { get; set; }
        public Point BottomRightPoint { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public Brush FillBrush { get; set; }
        public Pen OutlinePen { get; set; }

        public abstract void Draw(Graphics graphics);
    }

    class Circle : Shape
    {
        public Circle(Point start, Point end)
        {
            TopLeftPoint = new Point(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            BottomRightPoint = new Point(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            Height = BottomRightPoint.Y - TopLeftPoint.Y;
            Width = BottomRightPoint.X - TopLeftPoint.X;
            FillBrush = Brushes.LightBlue;
            OutlinePen = Pens.Black;
        }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(FillBrush, TopLeftPoint.X, TopLeftPoint.Y, Width, Height);
            g.DrawEllipse(OutlinePen, TopLeftPoint.X, TopLeftPoint.Y, Width, Height);
        }
    }

    class Rectangle : Shape
    {
        public Size Size { get; set; }

        public Rectangle(Point start, Point end)
        {
            TopLeftPoint = new Point(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            BottomRightPoint = new Point(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            Height = BottomRightPoint.Y - TopLeftPoint.Y;
            Width = BottomRightPoint.X - TopLeftPoint.X;
            FillBrush = Brushes.LightGreen;
            OutlinePen = Pens.DarkGreen;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(FillBrush, TopLeftPoint.X, TopLeftPoint.Y, Width, Height);
            g.DrawRectangle(OutlinePen, TopLeftPoint.X, TopLeftPoint.Y, Width, Height);
        }
    }
    
    class Line : Shape
    {
        public Size Size { get; set; }

        public Line(Point start, Point end)
        {
            TopLeftPoint = start;
            BottomRightPoint = end;
            FillBrush = Brushes.LightGreen;
            OutlinePen = Pens.DarkGreen;
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(OutlinePen, TopLeftPoint, BottomRightPoint);
        }
    }

}
