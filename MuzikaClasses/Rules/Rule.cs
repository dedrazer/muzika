using System;

namespace MuzikaClasses.Rules
{
    public abstract class Rule : ICellularAutomata
    {
        public CircularArray2D<bool> Grid { get; internal set; }
        internal CircularArray2D<bool> StartingGrid;

        public virtual bool[,] Iterate()
        {
            throw new InvalidOperationException();
        }
        
        public bool this[int x, int y]
        {
            get
            {
                return Grid[x, y];
            }
            set
            {
                Grid[x, y] = value;
            }
        }
    }
}