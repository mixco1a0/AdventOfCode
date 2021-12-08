using System.Numerics;
using System;

namespace AoC.Core
{
    public class Point<T> : Pair<T, T>
        where T : IComparable
    {
        public T X { get => First; set => First = value; }
        public T Y { get => Last; set => Last = value; }
        public bool SortByX { get { return m_sortByFirst; } set { m_sortByFirst = value; } }

        public Point() : base() { }

        public Point(T x, T y) : base(x, y) { }

        public Point(Point<T> other) : base(other) { }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }

    public class Point : Point<int>
    {
        public Point(int x, int y) : base(x, y) { }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
    }
}