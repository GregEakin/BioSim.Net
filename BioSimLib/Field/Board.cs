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
    public Grid Grid { get; }
    public Peeps Peeps { get; }
    public Signals Signals { get; }

    public Board(Config p)
    {
        _p = p;
        Peeps = new Peeps(p);
        Grid = new Grid(p, Peeps);
        Signals = new Signals(p);
    }

    public Player NewPlayer(Genome genome, Coord loc) => Grid.CreatePlayer(genome, loc);

    public Barrier NewBarrier(Coord loc) => Grid.CreateBarrier(loc);

    public Player CreatePlayer(Genome genome)
    {
        var loc = Grid.FindEmptyLocation();
        var player = NewPlayer(genome, loc);
        return player;
    }

    public IEnumerable<Genome> NewGeneration()
    {
        var survivors = Peeps.Survivors();
        Grid.ZeroFill();
        Signals.ZeroFill();
        Peeps.Clear();
        return survivors;
    }
}