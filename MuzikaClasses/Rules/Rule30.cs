﻿namespace MuzikaClasses.Rules
{
    public class Rule30 : Rule
    {
        public override bool[,] Iterate()
        {
            CircularArray2D<bool> newGrid = new CircularArray2D<bool>(Grid.Width, Grid.Height);

            for (int y = 0; y < newGrid.Height; y++)
            {
                bool current;
                bool previous;
                bool preprevious;
                for (int x = 0; x < newGrid.Width; x++)
                {
                    current = Grid[x, y];

                    previous = Grid[x - 1, y];
                    preprevious = Grid[x - 2, y];

                    //[left_cell XOR(central_cell OR right_cell)]
                    bool active = preprevious ^ (previous || current);
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