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

public readonly struct Board
{
    private readonly Config _p;
    public Barriers Barriers { get; }
    public Grid Grid { get; }
    public Peeps Peeps { get; }
    public Signals Signals { get; }

    public Board(Config p)
    {
        _p = p;
        Barriers = new Barriers();
        Peeps = new Peeps(p);
        Grid = new Grid(p, Peeps, Barriers);
        Signals = new Signals(p);
    }

    public Player NewPlayer(Genome genome, Coord loc) => Grid.CreatePlayer(genome, loc);

    public Barrier NewBarrier(Coord loc) => Grid.CreateBarrier(Grid.BarrierType.A, loc);

    public IEnumerable<Player> NewGeneration()
    {
        var players = new List<Player>();

        Grid.ZeroFill();
        Signals.ZeroFill();
        var survivors = Peeps.Survivors().ToArray();
        if (survivors.Length == 0) throw new NotImplementedException("Everybody died.");
        Peeps.Clear();
        for (var i = 0; i < _p.population; i++)
        {
            var index = i % survivors.Length;
            var survivor = survivors[index];
            var genome = survivor.Mutate();
            var loc = Grid.FindEmptyLocation();
            var player = NewPlayer(genome, loc);
            players.Add(player);
        }

        return players;
    }
}