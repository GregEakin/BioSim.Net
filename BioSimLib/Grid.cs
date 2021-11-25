﻿using System.Text;

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
    private readonly Barriers _barriers;
    private readonly ushort[,] _board;
    private readonly Random _random = new();

    public Grid(Config p, Peeps peeps)
    {
        _p = p;
        _peeps = peeps;
        _barriers = new Barriers();
        _board = new ushort[p.sizeX, p.sizeY];
    }

    public void ZeroFill()
    {
        for (var x = 0; x < _board.GetLength(0); x++)
            for (var y = 0; y < _board.GetLength(1); y++)
                _board[x, y] = 0;
    }

    public short SizeX() => (short)_board.GetLength(0);
    public short SizeY() => (short)_board.GetLength(1);

    public bool IsInBounds(Coord loc) => loc.X >= 0 && loc.X < SizeX() && loc.Y >= 0 && loc.Y < SizeY();
    public bool IsEmptyAt(Coord loc) => _board[loc.X, loc.Y] == 0;
    private bool IsBarrierAt(Coord loc) => _board[loc.X, loc.Y] == 1;
    public bool IsOccupiedAt(Coord loc) => _board[loc.X, loc.Y] > 1;
    public bool IsBorder(Coord loc) => loc.X == 0 || loc.X == SizeX() - 1 && loc.Y == 0 && loc.Y == SizeY() - 1;
    public ushort At(Coord loc) => _board[loc.X, loc.Y];
    public ushort At(int x, int y) => _board[x, y];

    public void Set(Coord loc, Player player)
    {
        _board[loc.X, loc.Y] = player._index;
        player._loc = loc;
    }

    public void Set(short x, short y, Player player)
    {
        _board[x, y] = player._index;
        player._loc = new Coord(x, y);
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

    public Player? this[int x, int y] => _peeps[_board[x, y]];
    public Player? this[Coord loc] => _peeps[_board[loc.X, loc.Y]];
    
    public Player NewPlayer(Genome genome, Coord loc)
    {
        var player = _peeps.NewPlayer(genome, loc);
        _board[loc.X, loc.Y] = player._index;
        return player;
    }

    public Barrier NewBarrier(BarrierType type, Coord loc)
    {
        var barrier = _barriers.NewBarrier(type, loc);
        _board[loc.X, loc.Y] = 1;
        return barrier;
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

    public static void VisitNeighborhood(Config p, Coord loc, float radius, Action<Coord> f)
    {
        for (var dx = -Math.Min((int)radius, loc.X); dx <= Math.Min((int)radius, p.sizeX - loc.X - 1); ++dx)
        {
            var x = loc.X + dx;
            var extentY = (int)Math.Sqrt(radius * radius - dx * dx);
            for (int dy = -Math.Min(extentY, loc.Y); dy <= Math.Min(extentY, p.sizeY - loc.Y - 1); ++dy)
            {
                var y = loc.Y + dy;
                f(new Coord { X = (short)x, Y = (short)y });
            }
        }
    }

    public float LongProbePopulationFwd(Coord loc, Dir dir, uint longProbeDist)
    {
        var count = 0u;
        loc += dir;
        var numLocsToTest = longProbeDist;
        while (numLocsToTest > 0u && IsInBounds(loc) && IsEmptyAt(loc))
        {
            ++count;
            loc += dir;
            --numLocsToTest;
        }

        return numLocsToTest > 0u && (!IsInBounds(loc) || IsBarrierAt(loc)) ? longProbeDist : count;
    }

    public float LongProbeBarrierFwd(Coord loc, Dir dir, uint longProbeDist)
    {
        var count = 0u;
        loc += dir;
        var numLocsToTest = longProbeDist;
        while (numLocsToTest > 0u && IsInBounds(loc) && !IsBarrierAt(loc))
        {
            ++count;
            loc += dir;
            --numLocsToTest;
        }

        return numLocsToTest > 0u && !IsInBounds(loc) ? longProbeDist : count;
    }

    public float GetPopulationDensityAlongAxis(Coord loc, Dir dir)
    {
        var sum = 0.0;
        var f = (Coord tloc) =>
        {
            if (tloc == loc || !IsOccupiedAt(tloc)) return;
            var offset = tloc - loc;
            var anglePosCos = offset.RaySameness(dir);
            var dist = Math.Sqrt((double)offset.X * offset.X + (double)offset.Y * offset.Y);
            var contrib = 1.0 / dist * anglePosCos;
            sum += contrib;
        };

        VisitNeighborhood(_p, loc, _p.populationSensorRadius, f);
        var maxSumMag = 6.0 * _p.populationSensorRadius;

        var sensorVal = (sum / maxSumMag + 1.0) / 2.0; 
        return (float)sensorVal;
    }

    public float GetShortProbeBarrierDistance(Coord loc0, Dir dir, uint probeDistance)
    {
        var countFwd = 0u;
        var countRev = 0u;
        var loc = loc0 + dir;
        var numLocsToTest = probeDistance;

        // Scan positive direction
        while (numLocsToTest > 0u && IsInBounds(loc) && !IsBarrierAt(loc))
        {
            ++countFwd;
            loc += dir;
            --numLocsToTest;
        }
        if (numLocsToTest > 0u && !IsInBounds(loc))
        {
            countFwd = probeDistance;
        }
        
        // Scan negative direction
        numLocsToTest = probeDistance;
        loc = loc0 - dir;
        while (numLocsToTest > 0u && IsInBounds(loc) && !IsBarrierAt(loc))
        {
            ++countRev;
            loc -= dir;
            --numLocsToTest;
        }
        if (numLocsToTest > 0u && !IsInBounds(loc))
        {
            countRev = probeDistance;
        }

        float sensorVal = countFwd - countRev + probeDistance; 
        sensorVal = sensorVal / 2.0f / probeDistance; 
        return sensorVal;
    }
}