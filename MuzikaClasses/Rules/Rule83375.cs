namespace MuzikaClasses.Rules
{
    public class Rule83375 : Rule
    {
        public override bool[,] Iterate()
        {
            CircularArray2D<bool> newGrid = new CircularArray2D<bool>(Grid.Width, Grid.Height);

            for (int y = 0; y < newGrid.Height; y++)
            {
                bool current;
                bool previous;
                bool preprevious;
                bool current2;
                bool previous2;
                bool preprevious2;
                //2 cells down
                int y2 = y + 2;

                for (int x = 0; x < newGrid.Width; x++)
                {
                    current = Grid[x, y];
                    current2 = Grid[x, y2];

                    previous = Grid[x - 1, y];
                    preprevious = Grid[x - 2, y];
                    previous2 = Grid[x - 1, y2];
                    preprevious2 = Grid[x - 2, y2];

                    bool active = (preprevious && previous) || (previous && preprevious2) || (current && previous);
                    int targetX = x - 1;
                    int targetY = y + 1;

                    newGrid[targetX, targetY] = active;
                }
            }

            Grid.Content = newGrid.Content;

            return Grid.Content;
        }
    }
}