﻿// Copyright 2022 Gregory Eakin
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

using BioSimLib.Challenges;
using BioSimLib.Genes;
using BioSimLib.Positions;
using static BioSimLib.Genes.GeneBank;

namespace BioSimLib.Field;

public sealed class Board
{
    private readonly Config _config;
    public Grid Grid { get; }
    public Critters Critters { get; }
    public Signals Signals { get; }

    public Board(Config config)
    {
        _config = config;
        Critters = new Critters(config);
        Grid = new Grid(config, Critters);
        Signals = new Signals(config);
    }

    public Barrier NewBarrier(Coord loc)
    {
        return Grid.CreateBarrier(loc);
    }

    public Critter NewCritter(Genome genome, Coord loc)
    {
        return Grid.CreateCritter(genome, loc);
    }

    public Critter NewCritter(Genome genome)
    {
        var loc = Grid.FindEmptyLocation();
        var critter = NewCritter(genome, loc);
        return critter;
    }

    public IEnumerable<Genome> NewGeneration()
    {
        var survivors = Critters.Survivors();
        Grid.ZeroFill();
        Signals.ZeroFill();
        Critters.Clear();
        return survivors;
    }

    public int SpawnNewGeneration(uint generation, IChallenge challenge, uint deathCount)
    {
        // var sacrificedCount = 0u;
        // var parents = new List<(Critter critter, float score)>();
        // var parentGenome = new List<Genome>();
        //
        // if (challenge.Type == Challenge.Altruism)
        // {
        //     foreach (var survivor in Critters.Survivors2())
        //     {
        //         var (alive, value) = challenge.PassedSurvivalCriterion(survivor);
        //         if (alive && survivor.NeuralNet.Length > 0)
        //             parents.Add((survivor, value));
        //     }
        // }
        // else
        // {
        //     var considerKinship = true;
        //     var sacrifices = new List<Critter>();
        //     foreach (var survivor in Critters.Survivors2())
        //     {
        //         var (alive, value) = challenge.PassedSurvivalCriterion(survivor);
        //         if (alive && survivor.NeuralNet.Length > 0)
        //             parents.Add((survivor, value));
        //         else
        //         {
        //             if (considerKinship)
        //                 sacrifices.Add(survivor);
        //             else
        //                 sacrificedCount++;
        //         }
        //     }
        //
        //     var generationToApplyKinship = 10u;
        //     var altruismFactor = 10u;
        //
        //     if (considerKinship)
        //     {
        //         if (generation > generationToApplyKinship)
        //         {
        //             var threshold = 0.7f;
        //             var survivingKin = new List<(Critter, float)>();
        //
        //             for (var passes = 0u; passes < altruismFactor; ++passes)
        //             {
        //                 foreach (var sacrifice in sacrifices)
        //                 {
        //                     var startIndex = RandomUint(0, parents.Count - 1);
        //                     for (var count = 0; count < parents.Count; ++count)
        //                     {
        //                         var (possibleParent, value) = parents[(startIndex + count) % parents.Count];
        //                         var g1 = sacrifice.Genome;
        //                         var g2 = possibleParent.Genome;
        //                         var comparisonMethod = (ComparisonMethods)_config.genomeComparisonMethod;
        //                         var similarity = GeneBank.GenomeSimilarity(comparisonMethod, g1, g2);
        //                         if (similarity < threshold) continue;
        //                         survivingKin.Add((possibleParent, value));
        //                         break;
        //                     }
        //                 }
        //             }
        //
        //             parents = survivingKin;
        //         }
        //     }
        //     else
        //     {
        //         var numberSaved = (int)(sacrificedCount * altruismFactor);
        //         if (parents.Count > 0 && numberSaved < parents.Count)
        //             parents.RemoveAt(numberSaved);
        //     }
        // }
        //
        // parents.Sort((p1, p2) => p1.score.CompareTo(p2.score));
        //
        // parentGenome.Capacity = parents.Count;
        // foreach (var (critter, _) in parents)
        //     parentGenome.Add(critter.Genome);
        //
        // // appendEpochLog(generation, parentGenomes.size(), murderCount);
        //
        // if (parentGenome.Count > 0)
        //     initializeNewGeneration(parentGenomes, generation + 1);
        // else
        //     initializeGeneration0();
        //
        //return parentGenome.Count;
        return 0;
    }
}