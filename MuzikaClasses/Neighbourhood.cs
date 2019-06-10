namespace MuzikaClasses
{
    class Neighbourhood
	//TODO, accept any odd number and make that a neighbourhood. OR 1 = 3, 2 = 5, 3 = 7 etc.
    {
        private int Length;
        public bool[] Neighbours { get; private set; }

        /// <summary>
        /// create a Neighbourhood given the source array and the centre co-ordinates
        /// </summary>
        /// <param name="source">source grid</param>
        /// <param name="x">centre x</param>
        /// <param name="y">centre y</param>
        public Neighbourhood(bool[,] source, short x, short y, int size)
		//TODO: use an effecient/tive circular array
        {
            //create cell array of given radius
            Length = (Numbers.Two * size) + Numbers.One;
            Neighbours = new bool[Length];
            
            int i = 0, sourceWidth = source.GetLength(0), sourceHeight = source.GetLength(1);
            for (int _y = 0-size; _y <= size; _y++)
            {
                for (int _x = 0 - size; _x <= size; _x++)
                {
                    _x = x + _x;
                    _y = y + _y;

                    if (_x < 0)
                    {
                        _x = sourceWidth + _x;
                    }
                    if (_y < 0)
                    {
                        _y = sourceHeight + _y;
                    }

                    Neighbours[i] = source[_x % sourceWidth, _y % sourceHeight];
                    i++;
                }
            }
        }
    }
}