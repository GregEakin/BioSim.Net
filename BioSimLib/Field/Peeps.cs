// Log File Viewer - Peeps.cs
// 
// Copyright © 2021 Greg Eakin.
// 
// Greg Eakin <greg@gdbtech.info>
// 
// All Rights Reserved.

using BioSimLib.Genes;
using BioSimLib.Positions;

namespace BioSimLib.Field;

public class Peeps
{
    private readonly Critter?[] _critters;
    private readonly List<Critter> _deathQueue = new();
    private readonly List<Tuple<Critter, Coord>> _moveQueue = new();
    private readonly Config _config;
    private ushort _count = 1;

    public Peeps(Config config)
    {
        _config = config;
        _critters = new Critter?[config.population];
    }

    public int Count => _count - 1;

    public Critter? this[int index] => index < 2 || index > _count ? null : _critters[index - 2u];

    public IEnumerable<Critter> DeathQueue => _deathQueue;

    public Critter NewCritter(Genome genome, Coord loc)
    {
        var player = new Critter(_config, genome, loc, ++_count);
        _critters[player.Index - 2u] = player;
        return player;
    }

    public void Clear()
    {
        _count = 1;
        _moveQueue.Clear();
        _deathQueue.Clear();
    }

    public void QueueForMove(Critter player, Coord newLoc)
    {
        _moveQueue.Add(new Tuple<Critter, Coord>(player, newLoc));
    }

    public void DrainMoveQueue(Grid grid)
    {
        foreach (var (player, newLoc) in _moveQueue)
        {
            if (!player.Alive) continue;
            if (!grid.Move(player, newLoc))
                continue;

            player.Loc = newLoc;
            var moveDir = (newLoc - player.Loc).AsDir();
            player.LastMoveDir = moveDir;
        }

        _moveQueue.Clear();
    }

    public void QueueForDeath(Critter player)
    {
        _deathQueue.Add(player);
    }

    public void DrainDeathQueue(Grid grid)
    {
        foreach (var player in _deathQueue)
        {
            player.Alive = false;
            grid.Remove(player);
            _critters[player.Index] = null;
        }

        _deathQueue.Clear();
    }

    public IEnumerable<Genome> Survivors()
    {
        return from player in _critters
            where player.Alive && player.LocX > _config.sizeX / 2 && player.LocX < _config.sizeX - 2
            select player.Genome;
    }

    public IEnumerable<Critter> Survivors2()
    {
        return from player in _critters
            where player.Alive && player.LocX > _config.sizeX / 2 && player.LocX < _config.sizeX - 2
            select player;
    }

    public IDictionary<int, int> Census()
    {
        var dict = new Dictionary<int, int>();
        foreach (var player in _critters)
        {
            if (!player?.Alive ?? true) continue;

            var (red, green, blue) = player.Color;
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