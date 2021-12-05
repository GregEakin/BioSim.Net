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

    public IEnumerable<Player> Startup()
    {
        for (var i = 0; i < _p.population; i++)
        {
            var genome = RandomGenome();
            var loc = Grid.FindEmptyLocation();
            var player = NewPlayer(genome, loc);
            yield return player;
        }
    }

    public IEnumerable<Player> NewGeneration()
    {
        var survivors = Peeps.Survivors().ToArray();
        Grid.ZeroFill();
        Signals.ZeroFill();
        Peeps.Clear();

        for (var i = 0; i < _p.population; i++)
        {
            var genome = survivors.Length <= 0
                ? RandomGenome()
                : ChildGenome(survivors[i % survivors.Length]);
            var loc = Grid.FindEmptyLocation();
            var player = NewPlayer(genome, loc);
            yield return player;
        }
    }

    public Genome RandomGenome()
    {
        Genome genome;
        do
        {
            var builder = new GenomeBuilder(_p.genomeMaxLength, _p.maxNumberNeurons);
            genome = builder.ToGenome();
        } while (genome.Length == 0);

        return genome;
    }

    public Genome ChildGenome(Genome survivor)
    {
        var builder = new GenomeBuilder(_p.maxNumberNeurons, survivor);
        builder.Mutate();
        var genome = builder.ToGenome();
        return genome;
    }
}