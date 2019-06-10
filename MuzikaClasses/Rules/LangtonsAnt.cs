namespace MuzikaClasses.Rules
{
    public class LangtonsAnt : Rule
    {
        public int Step { get; set; } = 2;
        public bool VaryRule = true;
        public bool Filtered { get; set; } = false;
        
        public override bool[,] Iterate()
        {
            int width = Grid.Width;
            int height = Grid.Height;

            //initialize ant index
            int i = 0;

            for (int y = 0; y < height; y++)
            {
                if (Grid[0, y])
                {
                    i = y;
                    break;
                }
            }

            for (int x = 0; x < width; x++)
            {
                Grid[x, i] = !Grid[x, i];
                if (Grid[x, i])
                    i -= Step;
                else
                    i += Step;
                
                if (Filtered)
                //filter vertically adjacent notes
                    for (int y = 1; y < height; y++)
                        if (Grid[x, y - 1] && Grid[x, y])
                            Grid[x, y] = false;
            }

            //optionally vary rule
            if (VaryRule && Grid.Equals(StartingGrid))
                Step = (Step+1)%height;

            return Grid.Content;
        }
    }
}