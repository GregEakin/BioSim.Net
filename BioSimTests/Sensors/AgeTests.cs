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

public class AgeTests
{
    [Fact]
    public void TypeTest()
    {
        var p = new Config() { stepsPerGeneration = 100 };
        var sensor = new Age(p);
        Assert.Equal(Sensor.AGE, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var p = new Config() { stepsPerGeneration = 100 };
        var sensor = new Age(p);
        Assert.Equal("age", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var p = new Config() { stepsPerGeneration = 100 };
        var sensor = new Age(p);
        Assert.Equal("Age", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config() { population = 10, stepsPerGeneration = 100, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });

        var player = board.NewPlayer(genome, new Coord(2, 2));

        var sensor = new Age(p);
        Assert.Equal(0.25f, sensor.Output(player, 25));
    }
}