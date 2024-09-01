// Copyright 2022 Gregory Eakin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//       http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using BioSimLib.Genes;
using BioSimLib.Positions;

namespace BioSimLib.Field;

public sealed class Critters(Config config)
{
    private readonly Critter?[] _critters = new Critter?[config.population];
    private readonly List<Critter> _deathQueue = [];
    private readonly List<Tuple<Critter, Coord>> _moveQueue = [];
    private ushort _count = 1;

    public int Count => _count - 1;

    public Critter? this[int index] => index < 2 || index > _count ? null : _critters[index - 2u];

    public IEnumerable<Critter> DeathQueue => _deathQueue;

    public Critter NewCritter(Genome genome, Coord loc)
    {
        var critter = new Critter(config, genome, loc, ++_count);
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
            var oldDir = critter.Loc;
            if (grid.Move(critter, newLoc))
            {
                critter.Loc = newLoc;
                critter.LastMoveDir = (newLoc - oldDir).AsDir();
            }
            else
            {
                critter.LastMoveDir = new Dir(Dir.Compass.CENTER);
            }
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
            where critter.Alive && critter.LocX > config.sizeX / 2 && critter.LocX < config.sizeX - 2
            select critter.Genome;
    }

    public IEnumerable<Critter> Survivors2()
    {
        return from critter in _critters
            where critter.Alive && critter.LocX > config.sizeX / 2 && critter.LocX < config.sizeX - 2
            select critter;
    }

    public IDictionary<int, int> Census()
    {
        var dict = new Dictionary<int, int>();
        foreach (var critter in _critters)
        {
            if (critter is not { Alive: true }) continue;

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