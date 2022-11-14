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

using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using Xunit;

namespace BioSimTests.Positions;

public class GridTests
{
    [Fact]
    public void PopulationDensityAlongAxisTest()
    {
        var p = new Config { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        for (var i = 0; i < 5; i++)
            board.NewPlayer(genome, new Coord(1, (short)i));

        var loc = new Coord { X = 2, Y = 2 };

        var dir1 = new Dir(Dir.Compass.W);
        var population1 = board.Grid.GetPopulationDensityAlongAxis(loc, dir1);
        Assert.Equal(0.5833333f, population1);

        var dir2 = new Dir(Dir.Compass.N);
        var population2 = board.Grid.GetPopulationDensityAlongAxis(loc, dir2);
        Assert.Equal(0.5f, population2);
    }

    [Fact]
    public void PopulationDensityAlongAxis_CenterTest()
    {
        var p = new Config { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        for (var i = 0; i < 5; i++)
            board.NewPlayer(genome, new Coord(1, (short)i));

        var loc = new Coord { X = 2, Y = 2 };

        var dir1 = new Dir(Dir.Compass.CENTER);
        var population1 = board.Grid.GetPopulationDensityAlongAxis(loc, dir1);
        Assert.Equal(0.0f, population1);
    }

    [Fact]
    public void VisitNeighborhoodTest()
    {
        var p = new Config { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        for (var i = 0; i < 5; i++)
            board.NewPlayer(genome, new Coord(1, (short)i));

        var loc = new Coord { X = 2, Y = 2 };

        var squares = 0;
        var critters = 0;

        void F(short x, short y)
        {
            squares++;
            if (board.Grid.IsOccupiedAt(x, y))
                critters++;
        }

        board.Grid.VisitNeighborhood(loc, 2.0f, F);
        Assert.Equal(13, squares);
        Assert.Equal(3, critters);
    }
}