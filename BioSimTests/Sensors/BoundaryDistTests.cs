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
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class BoundaryDistTests
{
    [Fact]
    public void TypeTest()
    {
        var p = new Config { sizeX = 5, sizeY = 5 };
        var sensor = new BoundaryDist(p);
        Assert.Equal(Sensor.BOUNDARY_DIST, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var p = new Config { sizeX = 5, sizeY = 5 };
        var sensor = new BoundaryDist(p);
        Assert.Equal("boundary dist", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var p = new Config { sizeX = 5, sizeY = 5 };
        var sensor = new BoundaryDist(p);
        Assert.Equal("ED", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config { population = 10, shortProbeBarrierDistance = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();
        board.NewBarrier(new Coord(2, 0));

        var player = board.NewCritter(genome, new Coord(1, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);

        var sensor = new BoundaryDist(p);
        Assert.Equal(1.0f, sensor.Output(player, 0));
    }
}