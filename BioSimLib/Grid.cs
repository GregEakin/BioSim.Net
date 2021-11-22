using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace BioSimLib;

public class Grid
{
    public enum BarrierType
    {
        A,   // Vertical bar in constant location
        B,   // Vertical bar in random location
        C,   // five blocks staggered
        D,   // Horizontal bar in constant location
        E,   // Three floating islands -- different locations every generation
        F,   // Spots, specified number, radius, locations
    }

    public Grid(Params p)
    {

    }

    public void Set(Coord loc, short index) { }

    public void CreateBarrier(BarrierType barrierType)
    {

    }

    // private List<Coord> barrierLocations;
    // private List<Coord> barrierCenters;

    public bool IsEmptyAt(Coord newLoc)
    {
        return true;
    }

    public bool Move(Indiv indiv, Coord newLoc)
    {
        if (!IsEmptyAt(newLoc))
            return false;

        _grid[indiv.loc.X, indiv.loc.Y] = null;
        _grid[newLoc.X, newLoc.Y] = indiv;
        return true;
    }

    public Indiv? this[int x, int y] => _grid[x, y];

    private readonly Indiv?[,] _grid = new Indiv[8, 8];

    public override string ToString()
    {
        var builder = new StringBuilder();
        for (int x = 0; x < _grid.GetLength(0); x += 1)
        {
            for (int y = 0; y < _grid.GetLength(1); y += 1)
            {
                var indiv = _grid[x, y];
                Console.Write(" {0}", indiv != null ? indiv.index : ".");
            }

            Console.WriteLine();
        }

        return builder.ToString();
    }
}