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

public class LastMoveDirXTests
{
    [Fact]
    public void TypeTest()
    {
        var sensor = new LastMoveDirX();
        Assert.Equal(Sensor.LAST_MOVE_DIR_X, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var sensor = new LastMoveDirX();
        Assert.Equal("last move dir X", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var sensor = new LastMoveDirX();
        Assert.Equal("LMx", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var config = new Config { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var genome = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();

        var critter = board.NewCritter(genome, new Coord(2, 2));
        critter.LastMoveDir = new Dir(Dir.Compass.W);

        var sensor = new LastMoveDirX();
        Assert.Equal(0.0f, sensor.Output(critter, 25));
    }
}