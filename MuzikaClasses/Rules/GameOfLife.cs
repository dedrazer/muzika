namespace MuzikaClasses.Rules
{
    public class GameOfLife : Rule
    {
        public override bool[,] Iterate()
        {
            int width = Grid.Width;
            int height = Grid.Height;

            bool[,] result = new bool[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (Grid[x, y])
                        result[x, y] = Kill(x, y);
                    else
                        result[x, y] = Reproduce(x, y);
            
            Grid.Content = result;
            return Grid.Content;
        }

        /// <summary>
        /// count the number of active neighbours and return true if this means life
        /// </summary>
        /// <param name="x">x co-ordinate</param>
        /// <param name="y">y co-ordinate</param>
        /// <param name="grid">grid</param>
        public bool Kill(int x, int y)
        {
            int counter = 0;
            for (int xCo = x - 1; xCo <= x + 1; xCo++)
                for (int yCo = y - 1; yCo <= y + 1; yCo++)
                    if (Grid[xCo, yCo])
                        counter++;

            if (counter == 2 || counter == 3)
                return true;

            return false;
        }

        /// <summary>
        /// count the number of active neighbours and return true if this means life
        /// </summary>
        /// <param name="x">x co-ordinate</param>
        /// <param name="y">y co-ordinate</param>
        /// <param name="grid">grid</param>
        public bool Reproduce(int x, int y)
        {
            int counter = 0;
            for (int xCo = x - 1; xCo <= x + 1; xCo++)
                for (int yCo = y - 1; yCo <= y + 1; yCo++)
                    if (Grid[xCo, yCo] && !(xCo == x && yCo == y))
                        counter++;

            if (counter == 3)
                return true;

            return false;
        }
    }
}