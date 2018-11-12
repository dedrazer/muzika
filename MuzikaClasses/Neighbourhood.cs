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
    class Neighbourhood
    {
        public bool[] Neighbours { get; private set; } = new bool[9];

        /// <summary>
        /// create a Neighbourhood given the source array and the centre co-ordinates
        /// </summary>
        /// <param name="source">source grid</param>
        /// <param name="x">centre x</param>
        /// <param name="y">centre y</param>
        public Neighbourhood(bool[,] source, short x, short y)
        {
            //minimum size 3x3
            if (source.GetLength(0) > 2 && source.GetLength(1) > 2)
            {
                short[] xCoOrdinates = new short[3];
                short[] yCoOrdinates = new short[3];
                //overflow
                if (x == 0)
                {
                    xCoOrdinates = new short[] {
                        (short)(source.GetLength(0) - 1),
                        0,
                        1
                    };
                }
                else if (x == source.GetLength(0) - 1)
                {
                    xCoOrdinates = new short[] {
                        (short)(x-1),
                        x,
                        0,
                    };
                }
                else
                {
                    xCoOrdinates = new short[] {
                        (short)(x-1),
                        x,
                        (short)(x+1)
                    };
                }
                if (y == 0)
                {
                    yCoOrdinates = new short[] {
                        (short)(source.GetLength(1) - 1),
                        0,
                        1
                    };
                }
                else if (y == source.GetLength(1) - 1)
                {
                    yCoOrdinates = new short[] {
                        (short)(y-1),
                        y,
                        0,
                    };
                }
                else
                {
                    yCoOrdinates = new short[] {
                        (short)(y-1),
                        y,
                        (short)(y+1)
                    };
                }

                int i = 0;
                foreach (short yCO in yCoOrdinates)
                {
                    foreach (short xCO in xCoOrdinates)
                    {
                        Neighbours[i] = source[xCO, yCO];
                        i++;
                    }
                }
            }
        }
    }
}