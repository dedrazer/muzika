using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MuzikaClasses
{
    public class CellularAutomata
    {
        public static int rule { get; set; } = 30;

        public static bool[,] Iterate(bool[,] grid)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            bool[,] newGrid = new bool[width, height];

            switch (rule)
            {
                case 30:
                    for (int y = 0; y < height; y++)
                    {
                        bool current;
                        bool previous;
                        bool preprevious;
                        for (int x = 0; x < width; x++)
                        {
                            current = grid[x, y];
                            if (x > 1)
                            {
                                previous = grid[x - 1, y];
                                preprevious = grid[x - 2, y];
                            }
                            else if (x > 0)
                            {
                                previous = grid[x - 1, y];
                                preprevious = grid[width - 1, y];
                            }
                            else
                            {
                                previous = grid[width - 1, y];
                                preprevious = grid[width - 2, y];
                            }

                            //[left_cell XOR(central_cell OR right_cell)]
                            bool active = preprevious ^ (previous || current);
                            int targetX = x - 1;
                            int targetY = y + 1;

                            if (targetY >= height)
                            {
                                targetY = 0;
                            }
                            if (targetY < 0)
                            {
                                targetY = height - 1;
                            }
                            if (targetX >= width)
                            {
                                targetX = 0;
                            }
                            if (targetX < 0)
                            {
                                targetX = width - 1;
                            }

                            newGrid[targetX, targetY] = active;
                        }
                    }
                    break;
            }

            return newGrid;
        }
    }
}