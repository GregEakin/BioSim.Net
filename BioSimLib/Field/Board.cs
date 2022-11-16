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

    public Barrier NewBarrier(Coord loc) => Grid.CreateBarrier(loc);

    public Critter NewCritter(Genome genome, Coord loc) => Grid.CreateCritter(genome, loc);

    public Critter NewCritter(Genome genome)
    {
        var loc = Grid.FindEmptyLocation();
        var player = NewCritter(genome, loc);
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

    // public int SpawnNewGeneration(uint generation, uint deathCount)
    // {
    //     var sacrificedCount = 0u;
    //     var parents = new List<(Player, float)>();
    //     var parentGenome = new List<Genome>();
    //
    //     if (_p.challenge == Challenge.Altruism)
    //     {
    //         foreach (var survivor in Peeps.Survivors2())
    //         {
    //             var (alive, value) = PassedSurvivalCriterion(survivor);
    //             if (alive && survivor._nnet.Length > 0)
    //                 parents.Add((alive, value));
    //         }
    //     }
    //     else
    //     {
    //         var considerKinship = true;
    //         var sacrifices = new List<Player>();
    //         foreach (var survivor in Peeps.Survivors2())
    //         {
    //             var (alive, value) = PassedSurvivalCriterion(survivor);
    //             if (alive && survivor._nnet.Length > 0)
    //                 parents.Add((alive, value));
    //             else
    //             {
    //                 if (considerKinship)
    //                     sacrifices.Add(survivor);
    //                 else
    //                     sacrificedCount++;
    //             }
    //         }
    //
    //         var generationToApplyKinship = 10u;
    //         var altruismFactor = 10u;
    //
    //         if (considerKinship)
    //         {
    //             if (generation > generationToApplyKinship)
    //             {
    //                 var threshold = 0.7f;
    //                 var survivingKin = new List<(Player, float)>();
    //
    //                 for (var passes = 0u; passes < altruismFactor; ++passes)
    //                 {
    //                     foreach (var sacrifice in sacrifices)
    //                     {
    //                         var startIndex = randomUint(0, parents.Count - 1);
    //                         for (var count = 0; count < parents.Count; ++count)
    //                         {
    //                             var (possibleParent, value) = parents[(startIndex + count) % parents.Count];
    //                             var g1 = sacrifice._genome;
    //                             var g2 = possibleParent._genome;
    //                             var similarity = genomeSimilarity(g1, g2);
    //                             if (similarity < threshold) continue;
    //                             survivingKin.Add((possibleParent, value));
    //                             break;
    //                         }
    //                     }
    //                 }
    //
    //                 parents = survivingKin;
    //             }
    //         }
    //         else
    //         {
    //             var numberSaved = (int)(sacrificedCount * altruismFactor);
    //             if (parents.Count > 0 && numberSaved < parents.Count)
    //                 parents.RemoveAt(numberSaved);
    //         }
    //     }
    //
    //     parents.Sort((p1, p2) => p1.Item2.CompareTo(p2.Item2));
    //
    //     parentGenome.Capacity = parents.Count;
    //     foreach (var (player, _) in parents) 
    //         parentGenome.Add(player._genome);
    //
    //     // appendEpochLog(generation, parentGenomes.size(), murderCount);
    //
    //     if (parentGenome.Count > 0)
    //         initializeNewGeneration(parentGenomes, generation + 1);
    //     else
    //         initializeGeneration0();
    //
    //     return parentGenome.Count;
    // }
}