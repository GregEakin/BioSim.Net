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

public class PeepsTests
{
    [Fact]
    public void IndexTest()
    {
        var p = new Config { population = 5, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var players = new Critter[2];
        for (var i = 0; i < 2; i++)
            players[i] = board.NewCritter(genome, new Coord(1, (short)i));

        Assert.Null(board.Peeps[0]);
        Assert.Null(board.Peeps[1]);
        Assert.Equal(players[0], board.Peeps[2]);
        Assert.Equal(players[1], board.Peeps[3]);
        Assert.Null(board.Peeps[4]);
    }

    [Fact]
    public void CountTest()
    {
        var p = new Config { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();

        Assert.Equal(0, board.Peeps.Count);
        board.NewCritter(genome, new Coord(1, 0));
        Assert.Equal(1, board.Peeps.Count);
        board.NewCritter(genome, new Coord(1, 0));
        Assert.Equal(2, board.Peeps.Count);
    }
}