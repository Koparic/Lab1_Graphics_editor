using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public abstract class Shape
    {
        public Point TopLeftPoint { get; set; }
        public Point BottomRightPoint { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public Brush FillBrush { get; set; }
        public Pen OutlinePen { get; set; }
        public abstract void Draw(Graphics graphics);

        public abstract bool ContainsPoint(Point point);
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

        public override bool ContainsPoint(Point point)
        {
            double cx = TopLeftPoint.X + Width / 2f, cy = TopLeftPoint.Y + Height / 2f;

            double dx = (point.X - cx) / (Width / 2f);
            double dy = (point.Y - cy) / (Height / 2f);

            return (dx * dx + dy * dy <= 1);
        }
    }

    class Rectangle : Shape
    {
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
        public override bool ContainsPoint(Point point)
        {
            return (TopLeftPoint.X <= point.X && point.X <= BottomRightPoint.X) && (TopLeftPoint.Y <= point.Y && point.Y <= BottomRightPoint.Y);
        }
    }
    
    class Line : Shape
    {
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

        public override bool ContainsPoint(Point point)
        {
            double segmentLengthSq = DistanceSquared(TopLeftPoint, BottomRightPoint);
            double epsilon = 8;
            if (segmentLengthSq == 0)
                return Distance(point, TopLeftPoint) <= epsilon;

            double t = DotProduct(
                VectorSubtract(point, TopLeftPoint),
                VectorSubtract(BottomRightPoint, TopLeftPoint)
            ) / segmentLengthSq;

            if (t < 0) t = 0;
            if (t > 1) t = 1;

            Point nearestPoint = new Point((int)(TopLeftPoint.X + t * (BottomRightPoint.X - TopLeftPoint.X)),
                                          (int)(TopLeftPoint.Y + t * (BottomRightPoint.Y - TopLeftPoint.Y)));

            double distance = Distance(point, nearestPoint);

            return distance <= epsilon;
        }

        private static double Distance(Point p1, Point p2) =>
            Math.Sqrt(DistanceSquared(p1, p2));

        private static double DistanceSquared(Point p1, Point p2) =>
            (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

        private static double DotProduct(Point v1, Point v2) =>
            v1.X * v2.X + v1.Y * v2.Y;

        private static Point VectorSubtract(Point p1, Point p2) =>
            new Point(p1.X - p2.X, p1.Y - p2.Y);
    }

}
