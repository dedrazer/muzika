using System;

namespace MuzikaClasses
{
    public class CircularArray2D<T>
    {
        public int Width, Height;
        public T[,] Content;

        public CircularArray2D(T[,] content)
        {
            Content = content;
            Width = content.GetLength(0);
            Height = content.GetLength(1);
        }

        public CircularArray2D(int width, int height)
        {
            Content = new T[width, height];
            Width = width;
            Height = height;
        }

        public T this[int x, int y]{
            get {
                if (x < 0)
                    x = Math.Abs((Width + x) % Width);
                else if (x >= Width)
                    x = x % Width;

                if (y < 0)
                    y = Math.Abs((Height + y) % Height);
                else if (y >= Height)
                    y = y % Height;

                return Content[x, y];
            }
            set
            {
                if (x < 0)
                    x = Math.Abs((Width + x) % Width);
                else if (x >= Width)
                    x = x % Width;

                if (y < 0)
                    y = Math.Abs((Height + y) % Height);
                else if (y >= Height)
                    y = y % Height;

                Content[x, y] = value;
            }
        }

        public override bool Equals(object obj)
        {
            bool equal = true;
            CircularArray2D<T> comparison = (CircularArray2D<T>)obj;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (!Content[x, y].Equals(comparison.Content[x, y]))
                        equal = false;

            return equal;
        }

        public override int GetHashCode()
        {
            int total = 0;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    total += Content[x, y].GetHashCode();

            return total;
        }
    }
}