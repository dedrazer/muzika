namespace MuzikaClasses.Rules
{
    public class RuleM : Rule
    {
        public override bool[,] Iterate()
        {
            int width = Grid.Width;
            int height = Grid.Width;
            bool[,] newGrid = new bool[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newGrid[x, y] = ((Grid[x + 1, y] && Grid[x, y - 1]) || (Grid[x - 1, y] && Grid[x, y + 1]))
                        ^ ((Grid[x - 1, y] && Grid[x, y - 1]) || (Grid[x + 1, y] && Grid[x, y + 1]))
                        ^ !(Grid[x - 2, y - 2] || Grid[x - 1, y - 2] || Grid[x, y - 2] || Grid[x + 1, y - 2] || Grid[x + 2, y - 2]
                        || Grid[x - 2, y - 1] || Grid[x - 1, y - 1] || Grid[x, y - 1] || Grid[x + 1, y - 1] || Grid[x + 2, y - 1]
                        || Grid[x - 2, y] || Grid[x - 1, y] || Grid[x, y] || Grid[x + 1, y] || Grid[x + 2, y]
                        || Grid[x - 2, y + 1] || Grid[x - 1, y + 1] || Grid[x, y + 1] || Grid[x + 1, y + 1] || Grid[x + 2, y + 1]
                        || Grid[x - 2, y + 2] || Grid[x - 1, y + 2] || Grid[x, y + 2] || Grid[x + 1, y + 2] || Grid[x + 2, y + 2]);
                }
            }

            Grid.Content = newGrid;

            return Grid.Content;
        }
    }
}