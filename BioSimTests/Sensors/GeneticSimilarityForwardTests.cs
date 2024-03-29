//    Copyright 2022 Gregory Eakin
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

using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class GeneticSimilarityForwardTests
{
    [Fact]
    public void TypeTest()
    {
        var config = new Config { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var sensor = new GeneticSimilarityForward(config, board.Grid);
        Assert.Equal(Sensor.GENETIC_SIM_FWD, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var config = new Config { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var sensor = new GeneticSimilarityForward(config, board.Grid);
        Assert.Equal("genetic similarity forward", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var config = new Config { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var sensor = new GeneticSimilarityForward(config, board.Grid);
        Assert.Equal("Gen", sensor.ShortName);
    }

    [Fact]
    public void Output_EmptyCellTest()
    {
        var config = new Config { population = 10, genomeComparisonMethod = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var genome = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();

        var critter = board.NewCritter(genome, new Coord(2, 2));
        critter.LastMoveDir = new Dir(Dir.Compass.W);

        var sensor = new GeneticSimilarityForward(config, board.Grid);
        Assert.Equal(0.00f, sensor.Output(critter, 25));
    }

    [Fact]
    public void Output_DifferentTest()
    {
        var config = new Config { population = 10, genomeComparisonMethod = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var genome1 = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        var genome2 = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x95821008u }).ToGenome();

        var critter = board.NewCritter(genome1, new Coord(2, 2));
        critter.LastMoveDir = new Dir(Dir.Compass.W);
        board.NewCritter(genome2, new Coord(1, 2));

        var sensor = new GeneticSimilarityForward(config, board.Grid);
        Assert.Equal(0.00f, sensor.Output(critter, 25));
    }

    [Fact]
    public void OutputTest()
    {
        var config = new Config { population = 10, genomeComparisonMethod = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var genome1 = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        var genome2 = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x95821007u }).ToGenome();

        var critter = board.NewCritter(genome1, new Coord(2, 2));
        critter.LastMoveDir = new Dir(Dir.Compass.W);
        board.NewCritter(genome2, new Coord(1, 2));

        var sensor = new GeneticSimilarityForward(config, board.Grid);
        Assert.Equal(1.00f, sensor.Output(critter, 25));
    }
}