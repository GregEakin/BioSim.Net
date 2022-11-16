// Log File Viewer - Critters.cs
// 
// Copyright © 2021 Greg Eakin.
// 
// Greg Eakin <greg@gdbtech.info>
// 
// All Rights Reserved.

using BioSimLib.Genes;
using BioSimLib.Positions;

namespace BioSimLib.Field;

public class Critters
{
    private readonly Critter?[] _critters;
    private readonly List<Critter> _deathQueue = new();
    private readonly List<Tuple<Critter, Coord>> _moveQueue = new();
    private readonly Config _config;
    private ushort _count = 1;

    public Critters(Config config)
    {
        _config = config;
        _critters = new Critter?[config.population];
    }

    public int Count => _count - 1;

    public Critter? this[int index] => index < 2 || index > _count ? null : _critters[index - 2u];

    public IEnumerable<Critter> DeathQueue => _deathQueue;

    public Critter NewCritter(Genome genome, Coord loc)
    {
        var critter = new Critter(_config, genome, loc, ++_count);
        _critters[critter.Index - 2u] = critter;
        return critter;
    }

    public void Clear()
    {
        _count = 1;
        _moveQueue.Clear();
        _deathQueue.Clear();
    }

    public void QueueForMove(Critter critter, Coord newLoc)
    {
        _moveQueue.Add(new Tuple<Critter, Coord>(critter, newLoc));
    }

    public void DrainMoveQueue(Grid grid)
    {
        foreach (var (critter, newLoc) in _moveQueue)
        {
            if (!critter.Alive) continue;
            if (!grid.Move(critter, newLoc))
                continue;

            critter.Loc = newLoc;
            var moveDir = (newLoc - critter.Loc).AsDir();
            critter.LastMoveDir = moveDir;
        }

        _moveQueue.Clear();
    }

    public void QueueForDeath(Critter critter)
    {
        _deathQueue.Add(critter);
    }

    public void DrainDeathQueue(Grid grid)
    {
        foreach (var critter in _deathQueue)
        {
            critter.Alive = false;
            grid.Remove(critter);
            _critters[critter.Index] = null;
        }

        _deathQueue.Clear();
    }

    public IEnumerable<Genome> Survivors()
    {
        return from critter in _critters
            where critter.Alive && critter.LocX > _config.sizeX / 2 && critter.LocX < _config.sizeX - 2
            select critter.Genome;
    }

    public IEnumerable<Critter> Survivors2()
    {
        return from critter in _critters
            where critter.Alive && critter.LocX > _config.sizeX / 2 && critter.LocX < _config.sizeX - 2
            select critter;
    }

    public IDictionary<int, int> Census()
    {
        var dict = new Dictionary<int, int>();
        foreach (var critter in _critters)
        {
            if (!critter?.Alive ?? true) continue;

            var (red, green, blue) = critter.Color;
            var key = (red << 16) | (green << 8) | blue;
            var found = dict.ContainsKey(key);
            if (!found)
                dict.Add(key, 1);
            else
                dict[key]++;
        }

        return dict;
    }
}