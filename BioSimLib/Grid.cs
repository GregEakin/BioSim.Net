using System.Text;

namespace BioSimLib;

public class Grid
{
    public enum BarrierType
    {
        A, // Vertical bar in constant location
        B, // Vertical bar in random location
        C, // five blocks staggered
        D, // Horizontal bar in constant location
        E, // Three floating islands -- different locations every generation
        F, // Spots, specified number, radius, locations
    }

    private readonly Config _p;
    private readonly Peeps _peeps;
    private readonly ushort[,] _board;
    private readonly Random _random = new();

    public Grid(Config p, Peeps peeps)
    {
        _p = p;
        _peeps = peeps;
        _board = new ushort[p.sizeX, p.sizeY];
    }

    public Player NewPlayer(Genome genome, Coord loc)
    {
        var player = _peeps.NewPlayer(this, genome, loc);
        _board[loc.X, loc.Y] = player._index;
        return player;
    }

    public void Set(Coord loc, Player player)
    {
        _board[loc.X, loc.Y] = player._index;
        player._loc = loc;
    }

    public Player? this[int x, int y] => _peeps[_board[x, y]];

    public Player? this[Coord loc] => _peeps[_board[loc.X, loc.Y]];

    private void CreateBarrier(BarrierType barrierType)
    {
    }

    // private List<Coord> barrierLocations;
    // private List<Coord> barrierCenters;

    public bool IsEmptyAt(Coord loc)
    {
        return _board[loc.X, loc.Y] == 0;
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

                var player = _peeps[index];
                Console.Write(" {0}", player != null ? player._index : ".");
            }

            Console.WriteLine();
        }

        return builder.ToString();
    }

    Coord FindEmptyLocation()
    {
        while (true)
        {
            var x = _random.Next(0, _p.sizeX - 1);
            var y = _random.Next(0, _p.sizeY - 1);
            if (_board[x, y] == 0)
                return new Coord { X = (short)x, Y = (short)y };
        }
    }

    public void VisitNeighborhood(Coord loc, float radius, Action<Coord> f)
    {
        for (var dx = -Math.Min((int)radius, loc.X); dx <= Math.Min((int)radius, _p.sizeX - loc.X - 1); ++dx)
        {
            var x = loc.X + dx;
            var extentY = (int)Math.Sqrt(radius * radius - dx * dx);
            for (int dy = -Math.Min(extentY, loc.Y); dy <= Math.Min(extentY, _p.sizeY - loc.Y - 1); ++dy)
            {
                var y = loc.Y + dy;
                f(new Coord { X = (short)x, Y = (short)y });
            }
        }
    }

    public float LongProbePopulationFwd(Coord loc, Dir dir, uint longProbeDist)
    {
        var count = 0u;
        loc = loc + dir;
        var numLocsToTest = longProbeDist;
        while (numLocsToTest > 0u && IsInBounds(loc) && IsEmptyAt(loc))
        {
            ++count;
            loc = loc + dir;
            --numLocsToTest;
        }

        if (numLocsToTest > 0u && (!IsInBounds(loc) || IsBarrierAt(loc)))
        {
            return longProbeDist;
        }
        else
        {
            return count;
        }
    }

    private bool IsInBounds(Coord loc)
    {
        throw new NotImplementedException();
    }

    private bool IsBarrierAt(Coord loc)
    {
        throw new NotImplementedException();
    }

    public float LongProbeBarrierFwd(Coord playerLoc, Dir playerLastMoveDir, uint playerLongProbeDist)
    {
        throw new NotImplementedException();
    }

    public float GetPopulationDensityAlongAxis(Coord playerLoc, Dir playerLastMoveDir)
    {
        throw new NotImplementedException();
    }

    public float GetShortProbeBarrierDistance(Coord playerLoc, Dir playerLastMoveDir, uint pShortProbeBarrierDistance)
    {
        throw new NotImplementedException();
    }
}