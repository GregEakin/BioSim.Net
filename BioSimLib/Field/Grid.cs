﻿// Log File Viewer - Grid.cs
// 
// Copyright © 2021 Greg Eakin.
// 
// Greg Eakin <greg@gdbtech.info>
// 
// All Rights Reserved.

using System.Text;
using BioSimLib.Genes;
using BioSimLib.Positions;

namespace BioSimLib.Field;

public class Grid
{
    private readonly ushort[,] _board;
    private readonly Config _config;
    private readonly Critters _critters;
    private readonly Random _random = new();

    public Grid(Config config, Critters critters)
    {
        _config = config;
        _critters = critters;
        _board = new ushort[config.sizeX, config.sizeY];
    }

    public short SizeX => (short)_board.GetLength(0);
    public short SizeY => (short)_board.GetLength(1);

    public Critter? this[int x, int y] => _critters[_board[x, y]];
    public Critter? this[Coord loc] => _critters[_board[loc.X, loc.Y]];

    public void ZeroFill()
    {
        Array.Clear(_board);
    }

    public bool IsInBounds(Coord loc) => loc.X >= 0 && loc.X < SizeX && loc.Y >= 0 && loc.Y < SizeY;

    public bool IsEmptyAt(Coord loc) => _board[loc.X, loc.Y] == 0;

    public bool IsEmptyAt(short x, short y) => _board[x, y] == 0;

    public bool IsBarrierAt(Coord loc) => _board[loc.X, loc.Y] == 1;

    public bool IsOccupiedAt(Coord loc) => _board[loc.X, loc.Y] > 1;

    public bool IsOccupiedAt(short x, short y) => _board[x, y] > 1;

    public bool IsBorder(Coord loc) => loc.X == 0 || loc.X == SizeX - 1 || loc.Y == 0 || loc.Y == SizeY - 1;

    public ushort At(Coord loc) => _board[loc.X, loc.Y];

    public ushort At(int x, int y) => _board[x, y];

    public void Set(Coord loc, Critter critter)
    {
        _board[loc.X, loc.Y] = critter.Index;
        critter.Loc = loc;
    }

    public void Set(short x, short y, Critter critter)
    {
        _board[x, y] = critter.Index;
        critter.Loc = new Coord(x, y);
    }

    public void Remove(Critter critter)
    {
        var loc = critter.Loc;
        if (_board[loc.X, loc.Y] < 2)
            return;

        _board[loc.X, loc.Y] = 0;
    }

    public Coord FindEmptyLocation()
    {
        var count = 2 * _config.sizeX * _config.sizeY;
        for (var i = 0; i < count; i++)
        {
            var x = (short)_random.Next(0, _config.sizeX);
            var y = (short)_random.Next(0, _config.sizeY);
            if (_board[x, y] == 0)
                return new Coord { X = x, Y = y };
        }

        throw new Exception("Can't find an empty square.");
    }

    public Critter CreateCritter(Genome genome, Coord loc)
    {
        var critter = _critters.NewCritter(genome, loc);
        _board[loc.X, loc.Y] = critter.Index;
        return critter;
    }

    public void SetBarrier(Coord loc)
    {
        // barrierCenters.push_back(loc);
    }

    public void SetBarrier(short x, short y)
    {
        // barrierLocations.push_back( { x, y} );
    }

    public Barrier CreateBarrier(Coord loc)
    {
        var barrier = new Barrier(loc);
        _board[loc.X, loc.Y] = 1;
        return barrier;
    }

    public IEnumerable<Coord> GetBarrierLocations()
    {
        return Array.Empty<Coord>();
    }

    public IEnumerable<Coord> GetBarrierCenters()
    {
        return Array.Empty<Coord>();
    }

    public bool Move(Critter critter, Coord newLoc)
    {
        if (!IsEmptyAt(newLoc))
            return false;

        _board[critter.LocX, critter.LocY] = 0;
        _board[newLoc.X, newLoc.Y] = critter.Index;
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
                    builder.Append(" .");
                    continue;
                }

                var critter = _critters[index];
                builder.Append($" {(critter != null ? critter.Index : "*")}");
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }

    public void VisitNeighborhood(short locX, short locY, float radius, Action<short, short> f)
    {
        for (var dx = -Math.Min((int)radius, locX); dx <= Math.Min((int)radius, SizeX - locX - 1); ++dx)
        {
            var x = locX + dx;
            var extentY = (int)Math.Sqrt(radius * radius - dx * dx);
            for (var dy = -Math.Min(extentY, locY); dy <= Math.Min(extentY, SizeY - locY - 1); ++dy)
            {
                var y = locY + dy;
                f((short)x, (short)y);
            }
        }
    }

    public void VisitNeighborhood(Coord loc, float radius, Action<short, short> f)
    {
        for (var dx = -Math.Min((int)radius, loc.X); dx <= Math.Min((int)radius, SizeX - loc.X - 1); ++dx)
        {
            var x = loc.X + dx;
            var extentY = (int)Math.Sqrt(radius * radius - dx * dx);
            for (var dy = -Math.Min(extentY, loc.Y); dy <= Math.Min(extentY, SizeY - loc.Y - 1); ++dy)
            {
                var y = loc.Y + dy;
                f((short)x, (short)y);
            }
        }
    }

    // Returns the number of locations to the next agent in the specified
    // direction, not including loc. If the probe encounters a boundary or a
    // barrier before reaching the longProbeDist distance, returns longProbeDist.
    // Returns 0..longProbeDist.
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

    // Returns the number of locations to the next barrier in the
    // specified direction, not including loc. Ignores agents in the way.
    // If the distance to the border is less than the longProbeDist distance
    // and no barriers are found, returns longProbeDist.
    // Returns 0..longProbeDist.
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

    // Converts the population along the specified axis to the sensor range. The
    // locations of neighbors are scaled by the inverse of their distance times
    // the positive absolute cosine of the difference of their angle and the
    // specified axis. The maximum positive or negative magnitude of the sum is
    // about 2*radius. We don't adjust for being close to a border, so populations
    // along borders and in corners are commonly sparser than away from borders.
    // An empty neighborhood results in a sensor value exactly midrange; below
    // midrange if the population density is greatest in the reverse direction,
    // above midrange if density is greatest in forward direction.
    public float GetPopulationDensityAlongAxis(Coord loc, Dir dir)
    {
        if (dir == Dir.Compass.CENTER || _config.populationSensorRadius <= 0.0f)
            return 0.0f;

        var sum = 0.0;
        var dirVec = dir.AsNormalizedCoord();
        var len = Math.Sqrt((double)dirVec.X * dirVec.X + (double)dirVec.Y * dirVec.Y);
        var dirVecX = dirVec.X / len;
        var dirVecY = dirVec.Y / len;

        void F(short x, short y)
        {
            var tloc = new Coord(x, y);
            if (tloc == loc || !IsOccupiedAt(tloc)) return;
            var offset = tloc - loc;
            var proj = dirVecX * offset.X + dirVecY * offset.Y;
            var contrib = proj / (offset.X * offset.X + offset.Y * offset.Y);
            sum += contrib;
        }

        VisitNeighborhood(loc, _config.populationSensorRadius, F);
        var maxSumMag = 6.0 * _config.populationSensorRadius;
        var sensorVal = (sum / maxSumMag + 1.0) / 2.0;
        return (float)sensorVal;
    }

    // Converts the number of locations (not including loc) to the next barrier location
    // along opposite directions of the specified axis to the sensor range. If no barriers
    // are found, the result is sensor mid-range. Ignores agents in the path.
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
            countFwd = probeDistance;

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
            countRev = probeDistance;

        var sensorVal = (countFwd - countRev + probeDistance) / 2.0 / probeDistance;
        return (float)sensorVal;
    }
}