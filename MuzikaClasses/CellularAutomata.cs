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
        private static int iterations = 0;

        public static int rule { get; set; } = 5;

        public static bool[,] Iterate(bool[,] grid)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            bool[,] newGrid = new bool[width, height];

            //if (iterations == 1)
            //{
            //    rule = 833752;
            //}
            //else if (iterations == 3)
            //{
            //    rule = 833753;
            //}

            switch (rule)
            {
                case 5:
                    for (int y = 0; y < height; y++)
                    {
                        int Y5 = y - 5;

                        if (Y5 < 0)
                        {
                            Y5 += height;
                        }
                        for (int x = 0; x < width; x++)
                        {
                            newGrid[x, y] = grid[x, Y5];
                        }
                    }
                    break;
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
                case 333:
                    bool[] nine = new bool[9];
                    for (short y = 0; y < height; y++)
                    {
                        for (short x = 0; x < width; x++)
                        {
                            Neighbourhood n = new Neighbourhood(grid, x, y);

                            bool active = (n.Neighbours[4] && n.Neighbours[5] &&
                                    ! (n.Neighbours[0] || n.Neighbours[1] || n.Neighbours[2] || n.Neighbours[6] || n.Neighbours[7] || n.Neighbours[8]))
                                || (n.Neighbours[3] && n.Neighbours[4] && 
                                    ! (n.Neighbours[0] || n.Neighbours[1] || n.Neighbours[5] || n.Neighbours[6] || n.Neighbours[7] || n.Neighbours[8]))
                                || (n.Neighbours[2] && n.Neighbours[7] &&
                                    ! (n.Neighbours[0] || n.Neighbours[1] || n.Neighbours[3] || n.Neighbours[4] || n.Neighbours[5] || n.Neighbours[6] || n.Neighbours[8]))
                                || (n.Neighbours[3] && n.Neighbours[8] &&
                                    ! (n.Neighbours[0] || n.Neighbours[1] || n.Neighbours[2] || n.Neighbours[4] || n.Neighbours[5] || n.Neighbours[6] || n.Neighbours[7]));
                            
                            newGrid[x, y] = active;
                        }
                    }
                    break;
                case 83375:
                    for (int y = 0; y < height; y++)
                    {
                        bool current;
                        bool previous;
                        bool preprevious;
                        bool current2;
                        bool previous2;
                        bool preprevious2;
                        //2 cells down
                        int y2 = y + 2;
                        if (y2 > height)
                        {
                            y2 = 1;
                        }
                        else if (y2 == height)
                        {
                            y2 = 0;
                        }
                        for (int x = 0; x < width; x++)
                        {
                            current = grid[x, y];
                            current2 = grid[x, y2];
                            if (x > 1)
                            {
                                previous = grid[x - 1, y];
                                preprevious = grid[x - 2, y];
                                previous2 = grid[x - 1, y2];
                                preprevious2 = grid[x - 2, y2];
                            }
                            else if (x > 0)
                            {
                                previous = grid[x - 1, y];
                                preprevious = grid[width - 1, y];
                                previous2 = grid[x - 1, y2];
                                preprevious2 = grid[width - 1, y2];
                            }
                            else
                            {
                                previous = grid[width - 1, y];
                                preprevious = grid[width - 2, y];
                                previous2 = grid[width - 1, y2];
                                preprevious2 = grid[width - 2, y2];
                            }

                            bool active = (preprevious && previous) || (previous && preprevious2) || (current && previous);
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
                case 833752:
                    for (int y = 0; y < height; y++)
                    {
                        bool current;
                        bool previous;
                        bool preprevious;
                        bool current2;
                        bool previous2;
                        bool preprevious2;
                        //2 cells down
                        int y2 = y + 2;
                        if (y2 > height)
                        {
                            y2 = 1;
                        }
                        else if (y2 == height)
                        {
                            y2 = 0;
                        }
                        for (int x = 0; x < width; x++)
                        {
                            current = grid[x, y];
                            current2 = grid[x, y2];
                            if (x > 1)
                            {
                                previous = grid[x - 1, y];
                                preprevious = grid[x - 2, y];
                                previous2 = grid[x - 1, y2];
                                preprevious2 = grid[x - 2, y2];
                            }
                            else if (x > 0)
                            {
                                previous = grid[x - 1, y];
                                preprevious = grid[width - 1, y];
                                previous2 = grid[x - 1, y2];
                                preprevious2 = grid[width - 1, y2];
                            }
                            else
                            {
                                previous = grid[width - 1, y];
                                preprevious = grid[width - 2, y];
                                previous2 = grid[width - 1, y2];
                                preprevious2 = grid[width - 2, y2];
                            }

                            bool active = (preprevious && previous) || (previous && preprevious2) || (current && previous);
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

            iterations++;

            return newGrid;
        }
    }
}