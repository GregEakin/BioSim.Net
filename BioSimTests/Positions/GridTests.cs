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
    public void ZeroFillTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        board.NewBarrier(loc1);
        Assert.True(board.Grid.IsBarrierAt(loc1));

        var loc2 = new Coord { X = 1, Y = 2 };
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        board.NewCritter(genome, loc2);
        Assert.True(board.Grid.IsOccupiedAt(loc2));

        board.Grid.ZeroFill();
        Assert.False(board.Grid.IsBarrierAt(loc1));
        Assert.False(board.Grid.IsOccupiedAt(loc2));
    }

    [Fact]
    public void SizeXTest()
    {
        var p = new Config { sizeX = 5, sizeY = 3 };
        var board = new Board(p);

        Assert.Equal(5, board.Grid.SizeX);
    }

    [Fact]
    public void SizeYTest()
    {
        var p = new Config { sizeX = 5, sizeY = 3 };
        var board = new Board(p);

        Assert.Equal(3, board.Grid.SizeY);
    }

    [Fact]
    public void InBoundTest()
    {
        var p = new Config { sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        Assert.True(board.Grid.IsInBounds(loc1));

        var loc2 = new Coord { X = -1, Y = 1 };
        Assert.False(board.Grid.IsInBounds(loc2));

        var loc3 = new Coord { X = 1, Y = -1 };
        Assert.False(board.Grid.IsInBounds(loc3));

        var loc4 = new Coord { X = 3, Y = 1 };
        Assert.False(board.Grid.IsInBounds(loc4));

        var loc5 = new Coord { X = 1, Y = 3 };
        Assert.False(board.Grid.IsInBounds(loc5));
    }

    [Fact]
    public void EmptyAtLocTest()
    {
        var p = new Config { sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        Assert.True(board.Grid.IsEmptyAt(loc1));

        board.NewBarrier(loc1);
        Assert.False(board.Grid.IsEmptyAt(loc1));
    }

    [Fact]
    public void EmptyAtXyTest()
    {
        var p = new Config { sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        Assert.True(board.Grid.IsEmptyAt(1, 1));

        board.NewBarrier(loc1);
        Assert.False(board.Grid.IsEmptyAt(1, 1));
    }

    [Fact]
    public void BarrierAtTest()
    {
        var p = new Config { sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        Assert.False(board.Grid.IsBarrierAt(loc1));

        board.NewBarrier(loc1);
        Assert.True(board.Grid.IsBarrierAt(loc1));
    }

    [Fact]
    public void OccupiedAtLocTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        Assert.False(board.Grid.IsOccupiedAt(loc1));

        board.NewBarrier(loc1);
        Assert.False(board.Grid.IsOccupiedAt(loc1));

        var loc2 = new Coord { X = 1, Y = 2 };
        Assert.False(board.Grid.IsOccupiedAt(loc2));

        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        board.NewCritter(genome, loc2);
        Assert.True(board.Grid.IsOccupiedAt(loc2));
    }

    [Fact]
    public void OccupiedAtXyTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        Assert.False(board.Grid.IsOccupiedAt(1, 1));

        board.NewBarrier(loc1);
        Assert.False(board.Grid.IsOccupiedAt(1, 1));

        var loc2 = new Coord { X = 1, Y = 2 };
        Assert.False(board.Grid.IsOccupiedAt(1, 2));

        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        board.NewCritter(genome, loc2);
        Assert.True(board.Grid.IsOccupiedAt(1, 2));
    }

    [Fact]
    public void BorderTest()
    {
        var p = new Config { sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        Assert.False(board.Grid.IsBorder(loc1));

        var loc2 = new Coord { X = 0, Y = 1 };
        Assert.True(board.Grid.IsBorder(loc2));

        var loc3 = new Coord { X = 1, Y = 0 };
        Assert.True(board.Grid.IsBorder(loc3));

        var loc4 = new Coord { X = 2, Y = 1 };
        Assert.True(board.Grid.IsBorder(loc4));

        var loc5 = new Coord { X = 1, Y = 2 };
        Assert.True(board.Grid.IsBorder(loc5));
    }

    [Fact]
    public void AtLocTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        Assert.Equal(0, board.Grid.At(loc1));

        board.NewBarrier(loc1);
        Assert.Equal(1, board.Grid.At(loc1));

        var loc2 = new Coord { X = 1, Y = 2 };
        Assert.Equal(0, board.Grid.At(loc2));

        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        board.NewCritter(genome, loc2);
        Assert.Equal(2, board.Grid.At(loc2));
    }

    [Fact]
    public void AtXyTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        Assert.Equal(0, board.Grid.At(1, 1));

        board.NewBarrier(loc1);
        Assert.Equal(1, board.Grid.At(1, 1));

        var loc2 = new Coord { X = 1, Y = 2 };
        Assert.Equal(0, board.Grid.At(1, 2));

        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        board.NewCritter(genome, loc2);
        Assert.Equal(2, board.Grid.At(1, 2));
    }

    [Fact]
    public void RemoveTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc2 = new Coord { X = 1, Y = 2 };

        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        var player = board.NewCritter(genome, loc2);
        Assert.Equal(2, board.Grid.At(loc2));

        board.Grid.Remove(player);

        Assert.Equal(0, board.Grid.At(loc2));
    }

    [Fact]
    public void FindEmptyLocationTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        board.NewBarrier(loc1);
        Assert.True(board.Grid.IsBarrierAt(loc1));

        var loc2 = new Coord { X = 1, Y = 2 };
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        board.NewCritter(genome, loc2);
        Assert.True(board.Grid.IsOccupiedAt(loc2));

        var loc = board.Grid.FindEmptyLocation();
        Assert.NotEqual(loc1, loc);
        Assert.NotEqual(loc2, loc);
    }

    [Fact]
    public void PeepsXyTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        board.NewBarrier(loc1);
        Assert.True(board.Grid.IsBarrierAt(loc1));

        var loc2 = new Coord { X = 1, Y = 2 };
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        var player = board.NewCritter(genome, loc2);
        Assert.True(board.Grid.IsOccupiedAt(loc2));

        var peep1 = board.Grid[0, 0];
        Assert.Null(peep1);

        var peep2 = board.Grid[1, 1];
        Assert.Null(peep2);

        var peep3 = board.Grid[1, 2];
        Assert.Equal(player, peep3);
    }

    [Fact]
    public void PeepsLocTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        board.NewBarrier(loc1);
        Assert.True(board.Grid.IsBarrierAt(loc1));

        var loc2 = new Coord { X = 1, Y = 2 };
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        var player = board.NewCritter(genome, loc2);
        Assert.True(board.Grid.IsOccupiedAt(loc2));

        var peep1 = board.Grid[new Coord(0, 0)];
        Assert.Null(peep1);

        var peep2 = board.Grid[loc1];
        Assert.Null(peep2);

        var peep3 = board.Grid[loc2];
        Assert.Equal(player, peep3);
    }

    [Fact]
    public void ToStringTest()
    {
        var p = new Config { maxNumberNeurons = 1, population = 2, sizeX = 3, sizeY = 3 };
        var board = new Board(p);

        var loc1 = new Coord { X = 1, Y = 1 };
        board.NewBarrier(loc1);

        var loc2 = new Coord { X = 1, Y = 2 };
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        board.NewCritter(genome, loc2);

        Assert.Equal(" . . .\r\n . * 2\r\n . . .\r\n", board.Grid.ToString());
    }

    [Fact]
    public void LongProbeBarrierFwd_NoBarrierTest()
    {
        var p = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(p);

        var loc = new Coord { X = 2, Y = 2 };
        var dir1 = new Dir(Dir.Compass.W);
        var locations = board.Grid.LongProbeBarrierFwd(loc, dir1, 5);

        Assert.Equal(5.0f, locations);
        Assert.Equal(new Coord { X = 2, Y = 2 }, loc);
    }

    [Fact]
    public void LongProbeBarrierFwdTest()
    {
        var p = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        board.NewBarrier(new Coord(0, 2));

        var loc = new Coord { X = 2, Y = 2 };
        var dir1 = new Dir(Dir.Compass.W);
        var locations = board.Grid.LongProbeBarrierFwd(loc, dir1, 5);

        Assert.Equal(1.0f, locations);
        Assert.Equal(new Coord { X = 2, Y = 2 }, loc);
    }


    [Fact]
    public void PopulationDensityAlongAxisTest()
    {
        var p = new Config { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x95821000u }).ToGenome();
        for (var i = 0; i < 5; i++)
            board.NewCritter(genome, new Coord(1, (short)i));

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
            board.NewCritter(genome, new Coord(1, (short)i));

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
            board.NewCritter(genome, new Coord(1, (short)i));

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