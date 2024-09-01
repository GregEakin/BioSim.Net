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

public class EmitSignal0Tests
{
    [Fact]
    public void TypeTest()
    {
        var config = new Config();
        var board = new Board(config);
        var action = new EmitSignal0(board);
        Assert.Equal(Action.EMIT_SIGNAL0, action.Type);
    }

    [Fact]
    public void ToStringTest()
    {
        var config = new Config();
        var board = new Board(config);
        var action = new EmitSignal0(board);
        Assert.Equal("emit signal 0", action.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var config = new Config();
        var board = new Board(config);
        var action = new EmitSignal0(board);
        Assert.Equal("SG", action.ShortName);
    }

    [Fact]
    public void ExecuteTest()
    {
        var config = new Config { maxNumberNeurons = 1, signalLayers = 1, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var loc = new Coord (3, 4);
        var critter = board.NewCritter(genome, loc);

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.EMIT_SIGNAL0] = 0.6f;

        var action = new EmitSignal0(board);
        action.Execute(critter, 0, actionLevels);
    }

    [Fact]
    public void MovementTest()
    {
        var config = new Config();
        var board = new Board(config);
        var action = new EmitSignal0(board);
        var (x, y) = action.Move([], new Dir(Dir.Compass.CENTER));
        Assert.Equal(0.0, x);
        Assert.Equal(0.0, y);
    }
}