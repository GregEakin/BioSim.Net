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

    private readonly Config _p;
    private readonly Player?[] _players;
    private readonly ushort[,] _board;
    private ushort _count = 0;

    public Grid(Config p)
    {
        _p = p;
        _players = new Player?[p.population];
        _board = new ushort[p.sizeX, p.sizeY];
    }

    public Player NewPlayer(Genome genome, Coord loc)
    {
        var player = new Player(_p, this, genome, loc, ++_count);
        _players[player._index - 1u] = player;
        _board[loc.X, loc.Y] = player._index;
        return player;
    }

    public void Set(Coord loc, Player player)
    {
        _board[loc.X, loc.Y] = player._index;
        player._loc = loc;
    }

    public Player? this[int x, int y]
    {
        get
        {
            var index = _board[x, y];
            return index != 0u ? _players[index - 1u] : null;
        }
    }

    private void CreateBarrier(BarrierType barrierType)
    {

    }

    // private List<Coord> barrierLocations;
    // private List<Coord> barrierCenters;

    public bool IsEmptyAt(Coord newLoc)
    {
        return _board[newLoc.X, newLoc.Y] == 0;
    }

    public bool Move(Player player, Coord newLoc)
    {
        if (!IsEmptyAt(newLoc))
            return false;

        _board[player._loc.X, player._loc.Y] = 0;
        _board[newLoc.X, newLoc.Y] = player._index;
        player._loc = newLoc;
        return true;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var x = 0; x < _board.GetLength(0); x++)
        {
            for (var y = 0; y < _board.GetLength(1); y++)
            {
                var index = _board[x, y];
                if (index == 0)
                {
                    Console.Write(" .");
                    continue;
                }

                var player = _players[index - 1u];
                Console.Write(" {0}", player != null ? player._index : ".");
            }

            Console.WriteLine();
        }

        return builder.ToString();
    }
}