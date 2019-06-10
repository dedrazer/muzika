using MuzikaClasses.Rules;

namespace MuzikaClasses
{
    public interface ICellularAutomata
    {
        bool[,] Iterate();
    }

    public class CellularAutomata<T> : ICellularAutomata where T : Rule, new()
    {
        public T CA;
        public int Iterations { get; set; } = 0;
        public short Repeat = 1;

        public CellularAutomata(CircularArray2D<bool> startingGrid)
        {
            CircularArray2D<bool> grid = new CircularArray2D<bool>((bool[,])startingGrid.Content.Clone());

            CA = new T
            {
                Grid = grid,
                StartingGrid = startingGrid
            };
        }

        public bool this[int x, int y]
        {
            get
            {
                return CA[x, y];
            }
            set
            {
                CA[x, y] = value;
            }
        }

        public bool[,] Iterate()
        {
            Iterations++;
            if(Iterations%Repeat == 0)
                return CA.Iterate();
            return CA.Grid.Content;
        }

        public void SetCell(int x, int y, bool value)
        {
            CA.Grid[x, y] = value;
        }
    }
} 