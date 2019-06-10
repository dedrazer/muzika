using System;

namespace MuzikaClasses
{
    public class CircularArray<T> where T : new()
    {
        private int Length;
        private T[] Content;

        public CircularArray(int size)
        {
            Content = new T[size];
            Length = size;
        }

        public T this[int i]{
            get {
                if (i < 0)
                    return Content[Math.Abs((Length + i) % Length)];
                else if (i >= Length)
                    return Content[i % Length];
                return Content[i];
            }
        }
    }
}