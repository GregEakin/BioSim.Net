//    Copyright 2021 Gregory Eakin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using BioSimLib.Genes;
using BioSimLib.Positions;

namespace BioSimLib.Field;

public class Peeps
{
    private readonly Config _p;
    private readonly Critter?[] _critters;
    private ushort _count = 1;

    private readonly List<Tuple<Critter, Coord>> _moveQueue = new();
    private readonly List<Critter> _deathQueue = new();

    public Peeps(Config p)
    {
        _p = p;
        _critters = new Critter?[p.population];
    }

    public Critter NewCritter(Genome genome, Coord loc)
    {
        var player = new Critter(_p, genome, loc, ++_count);
        _critters[player.Index - 2u] = player;
        return player;
    }

    public void Clear()
    {
        _count = 1;
        _moveQueue.Clear();
        _deathQueue.Clear();
    }

    public int Count => _count - 1;

    public Critter? this[int index] => index < 2 || index > _count ? null : _critters[index - 2u];

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

    public IEnumerable<Critter> DeathQueue => _deathQueue;

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

    public IEnumerable<Genome> Survivors() => from player in _critters
        where player.Alive && player.LocX > _p.sizeX / 2 && player.LocX < _p.sizeX - 2
        select player.Genome;

    public IEnumerable<Critter> Survivors2() => from player in _critters
        where player.Alive && player.LocX > _p.sizeX / 2 && player.LocX < _p.sizeX - 2
        select player;

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