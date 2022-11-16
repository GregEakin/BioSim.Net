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

public class LongProbeBarrierForwardTests
{
    [Fact]
    public void TypeTest()
    {
        var config = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var sensor = new LongProbeBarrierForward(board.Grid);
        Assert.Equal(Sensor.LONGPROBE_BAR_FWD, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var config = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var sensor = new LongProbeBarrierForward(board.Grid);
        Assert.Equal("long probe barrier fwd", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var config = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var sensor = new LongProbeBarrierForward(board.Grid);
        Assert.Equal("LPb", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var config = new Config { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        board.NewBarrier(new Coord(0, 2));

        var genome = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();
        var critter = board.NewCritter(genome, new Coord(2, 2));
        critter.LastMoveDir = new Dir(Dir.Compass.W);
        critter.LongProbeDist = 2;

        var sensor = new LongProbeBarrierForward(board.Grid);
        Assert.Equal(0.5f, sensor.Output(critter, 25));
    }
}