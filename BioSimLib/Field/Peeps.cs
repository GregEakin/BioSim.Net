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
    private readonly Player?[] _players;
    private ushort _count = 1;

    private readonly List<Tuple<Player, Coord>> _moveQueue = new();
    private readonly List<Player> _deathQueue = new();

    public Peeps(Config p)
    {
        _p = p;
        _players = new Player?[p.population];
    }

    public Player NewPlayer(Genome genome, Coord loc)
    {
        var player = new Player(_p, genome, loc, ++_count);
        _players[player._index - 2u] = player;
        return player;
    }

    public void Clear()
    {
        _count = 1;
        _moveQueue.Clear();
        _deathQueue.Clear();
    }

    public int Count => _count - 1;

    public Player? this[int index]
    {
        get
        {
            if (index < 2 || index > _count)
                return null;

            return _players[index - 2u];
        }
    }

    public void QueueForMove(Player player, Coord newLoc)
    {
        _moveQueue.Add(new Tuple<Player, Coord>(player, newLoc));
    }

    public void DrainMoveQueue(Grid grid)
    {
        foreach (var (player, newLoc) in _moveQueue)
        {
            if (!grid.Move(player, newLoc))
                continue;

            player._loc = newLoc;
            var moveDir = (newLoc - player._loc).AsDir();
            player.LastMoveDir = moveDir;
        }

        _moveQueue.Clear();
    }

    public IEnumerable<Player> DeathQueue => _deathQueue;

    public void QueueForDeath(Player player)
    {
        _deathQueue.Add(player);
    }

    public void DrainDeathQueue(Grid grid)
    {
        foreach (var player in _deathQueue)
        {
            player._alive = false;
            grid.Remove(player);
            _players[player._index] = null;
        }

        _deathQueue.Clear();
    }

    public IEnumerable<Genome> Survivors() => from player in _players where player.Alive && player._loc.X > _p.sizeX / 2 && player._loc.X < _p.sizeX - 2 select player._genome;

    public IDictionary<int, int> Census()
    {
        var dict = new Dictionary<int, int>();
        foreach (var player in _players)
        {
            if (player is not { Alive: true }) continue;

            var (red, green, blue) = player.Color;
            var key = (red << 16) | (green << 8) | blue;
            var found = dict.TryGetValue(key, out var count);
            if (!found)
            {
                dict.Add(key, 1);
                continue;
            }
            dict[key]++;
        }

        return dict;
    }
}