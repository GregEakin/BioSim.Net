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
using BioSimLib.Actions.Movements;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests.Actions.Movements;

public class MoveRandomTests
{
    [Fact]
    public void TypeTest()
    {
        var action = new MoveRandom();
        Assert.Equal(Action.MOVE_RANDOM, action.Type);
    }

    [Fact]
    public void ToStringTest()
    {
        var action = new MoveRandom();
        Assert.Equal("move random", action.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var action = new MoveRandom();
        Assert.Equal("Mrn", action.ShortName);
    }

    [Fact]
    public void ExecuteTest()
    {
        var p = new Config { maxNumberNeurons = 1, sizeX = 8, sizeY = 8 };
        var board = new Board(p);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var player = board.NewPlayer(genome, new Coord { X = 3, Y = 4 });

        var action = new MoveRandom();
        action.Execute(p, board, player, 0, Array.Empty<float>());
    }

    [Fact]
    public void MoveDisabledTest()
    {
        var movement = new MoveRandom();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_RANDOM] = 0.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(0.0f, x);
        Assert.Equal(0.0f, y);
    }

    [Fact]
    public void MoveEnabledTest()
    {
        var movement = new MoveRandom();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_RANDOM] = 1.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.True(x != 0.0f || y == 0.0f);
    }
}