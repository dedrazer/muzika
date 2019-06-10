namespace MuzikaClasses.Rules
{
    public class Rule5 : Rule
    {
        public override bool[,] Iterate()
        {
            CircularArray2D<bool> newGrid = new CircularArray2D<bool>(Grid.Width, Grid.Height);

            for (int y = 0; y < newGrid.Height; y++)
            {
                int Y5 = y - 5;

                if (Y5 < 0)
                {
                    Y5 += newGrid.Height;
                }
                for (int x = 0; x < newGrid.Width; x++)
                {
                    newGrid[x, y] = Grid[x, Y5];
                }
            }

            Grid.Content = newGrid.Content;

            return Grid.Content;
        }
    }
}