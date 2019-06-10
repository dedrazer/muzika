namespace MuzikaClasses.Rules
{
    public class Rule333 : Rule
    {
        public override bool[,] Iterate()
        {
            int width = Grid.Width;
            int height = Grid.Width;
            bool[,] newGrid = new bool[width, height];

            for (short y = 0; y < height; y++)
            {
                for (short x = 0; x < width; x++)
                {
                    Neighbourhood n = new Neighbourhood(Grid.Content, x, y, 2);

                    bool active = (n.Neighbours[4] && n.Neighbours[5] &&
                            !(n.Neighbours[0] || n.Neighbours[1] || n.Neighbours[2] || n.Neighbours[6] || n.Neighbours[7] || n.Neighbours[8]))
                        || (n.Neighbours[3] && n.Neighbours[4] &&
                            !(n.Neighbours[0] || n.Neighbours[1] || n.Neighbours[5] || n.Neighbours[6] || n.Neighbours[7] || n.Neighbours[8]))
                        || (n.Neighbours[2] && n.Neighbours[7] &&
                            !(n.Neighbours[0] || n.Neighbours[1] || n.Neighbours[3] || n.Neighbours[4] || n.Neighbours[5] || n.Neighbours[6] || n.Neighbours[8]))
                        || (n.Neighbours[3] && n.Neighbours[8] &&
                            !(n.Neighbours[0] || n.Neighbours[1] || n.Neighbours[2] || n.Neighbours[4] || n.Neighbours[5] || n.Neighbours[6] || n.Neighbours[7]));

                    newGrid[x, y] = active;
                }
            }

            Grid.Content = newGrid;

            return Grid.Content;
        }
    }
}