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
using BioSimLib.Actions;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests.Actions;

public class SetLongProbeDistanceTests
{
    [Fact]
    public void TypeTest()
    {
        var action = new SetLongProbeDist();
        Assert.Equal(Action.SET_LONGPROBE_DIST, action.Type);
    }

    [Fact]
    public void ToStringTest()
    {
        var action = new SetLongProbeDist();
        Assert.Equal("set longprobe dist", action.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var action = new SetLongProbeDist();
        Assert.Equal("LPD", action.ShortName);
    }

    [Fact]
    public void ExecuteNotSetTest()
    {
        var config = new Config { maxNumberNeurons = 1, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var critter = board.NewCritter(genome, new Coord(3, 4));

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.SET_LONGPROBE_DIST] = 0.05f;

        var action = new SetLongProbeDist();
        action.Execute(critter, 0, actionLevels);

        Assert.Equal(17u, critter.LongProbeDist);
    }

    [Fact]
    public void ExecuteTest()
    {
        var config = new Config { maxNumberNeurons = 1, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var critter = board.NewCritter(genome, new Coord(3, 4));

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.SET_LONGPROBE_DIST] = 0.1f;

        var action = new SetLongProbeDist();
        action.Execute(critter, 0, actionLevels);

        Assert.Equal(18u, critter.LongProbeDist);
    }

    [Fact]
    public void MovementTest()
    {
        var action = new SetLongProbeDist();
        var (x, y) = action.Move([], new Dir(Dir.Compass.CENTER));
        Assert.Equal(0.0, x);
        Assert.Equal(0.0, y);
    }
}