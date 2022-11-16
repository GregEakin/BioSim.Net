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

public class SetResponsivenessTests
{
    [Fact]
    public void TypeTest()
    {
        var action = new SetResponsiveness();
        Assert.Equal(Action.SET_RESPONSIVENESS, action.Type);
    }

    [Fact]
    public void ToStringTest()
    {
        var action = new SetResponsiveness();
        Assert.Equal("set inv-responsiveness", action.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var action = new SetResponsiveness();
        Assert.Equal("Res", action.ShortName);
    }

    [Fact]
    public void ExecuteNotSetTest()
    {
        var config = new Config { maxNumberNeurons = 1, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var critter = board.NewCritter(genome, new Coord { X = 3, Y = 4 });

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.SET_RESPONSIVENESS] = 0.0f;

        var action = new SetResponsiveness();
        action.Execute(config, board, critter, 0, actionLevels);

        Assert.Equal(0.5f, critter.Responsiveness);
    }

    [Fact]
    public void ExecuteTest()
    {
        var config = new Config { maxNumberNeurons = 1, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var critter = board.NewCritter(genome, new Coord { X = 3, Y = 4 });

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.SET_RESPONSIVENESS] = 0.05f;

        var action = new SetResponsiveness();
        action.Execute(config, board, critter, 0, actionLevels);

        Assert.Equal(0.5249792f, critter.Responsiveness);
    }

    [Fact]
    public void MovementTest()
    {
        var action = new SetResponsiveness();
        var (x, y) = action.Move(Array.Empty<float>(), new Dir(Dir.Compass.CENTER));
        Assert.Equal(0.0, x);
        Assert.Equal(0.0, y);
    }
}