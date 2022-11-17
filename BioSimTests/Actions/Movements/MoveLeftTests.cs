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
using BioSimLib.Actions.Movements;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests.Actions.Movements;

public class MoveLeftTests
{
    [Fact]
    public void TypeTest()
    {
        var action = new MoveLeft();
        Assert.Equal(Action.MOVE_LEFT, action.Type);
    }

    [Fact]
    public void ToStringTest()
    {
        var action = new MoveLeft();
        Assert.Equal("move left", action.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var action = new MoveLeft();
        Assert.Equal("MvL", action.ShortName);
    }

    [Fact]
    public void ExecuteTest()
    {
        var config = new Config { maxNumberNeurons = 1, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var critter = board.NewCritter(genome, new Coord (3, 4));

        var action = new MoveLeft();
        action.Execute(critter, 0, Array.Empty<float>());
    }

    [Fact]
    public void MoveDisabledTest()
    {
        var movement = new MoveLeft();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_LEFT] = 0.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(0.0f, x);
        Assert.Equal(0.0f, y);
    }

    [Fact]
    public void MoveEnabledTest()
    {
        var movement = new MoveLeft();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_LEFT] = 1.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(0.0f, x);
        Assert.Equal(-1.0f, y);
    }
}