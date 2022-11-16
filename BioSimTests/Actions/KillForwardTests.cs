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
using BioSimLib.Sensors;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests.Actions;

public class KillForwardTests
{
    [Fact]
    public void TypeTest()
    {
        var action = new KillForward();
        Assert.Equal(Action.KILL_FORWARD, action.Type);
    }

    [Fact]
    public void ToStringTest()
    {
        var action = new KillForward();
        Assert.Equal("kill fwd", action.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var action = new KillForward();
        Assert.Equal("KlF", action.ShortName);
    }

    [Fact]
    public void NoNeighborTest()
    {
        var config = new Config { maxNumberNeurons = 1, sizeX = 8, sizeY = 8 };
        var board = new Board(config);

        var geneBuilder = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.KILL_FORWARD,
            WeightAsFloat = 1.0f
        };

        var genome = new GenomeBuilder(config.maxNumberNeurons, new[] { geneBuilder.ToUint() }).ToGenome();
        var loc = new Coord { X = 3, Y = 4 };
        var critter = board.NewCritter(genome, loc);

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.KILL_FORWARD] = 1.0f;

        var action = new KillForward();
        action.Execute(config, board, critter, 0, actionLevels);

        Assert.Empty(board.Critters.DeathQueue);
    }

    [Fact]
    public void ExecuteNotTest()
    {
        var config = new Config { maxNumberNeurons = 1, population = 2, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var critter = board.NewCritter(genome, new Coord { X = 3, Y = 4 });
        critter.LastMoveDir = new Dir(Dir.Compass.W);
        var victim = board.NewCritter(genome, new Coord { X = 2, Y = 4 });

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.KILL_FORWARD] = 0.0f;

        var action = new KillForward();
        action.Execute(config, board, critter, 0, actionLevels);

        Assert.Empty(board.Critters.DeathQueue);
        // Assert.False(victim.Alive);
    }

    [Fact]
    public void ExecuteVictimNotAliveTest()
    {
        var config = new Config { maxNumberNeurons = 1, population = 2, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var critter = board.NewCritter(genome, new Coord { X = 3, Y = 4 });
        critter.LastMoveDir = new Dir(Dir.Compass.W);
        var victim = board.NewCritter(genome, new Coord { X = 2, Y = 4 });
        victim.Alive = false;

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.KILL_FORWARD] = 0.05f;

        var action = new KillForward();
        action.Execute(config, board, critter, 0, actionLevels);

        Assert.Empty(board.Critters.DeathQueue);
    }

    [Fact]
    public void ExecuteTest()
    {
        var config = new Config { maxNumberNeurons = 1, population = 2, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var critter = board.NewCritter(genome, new Coord { X = 3, Y = 4 });
        critter.LastMoveDir = new Dir(Dir.Compass.W);
        critter.Responsiveness = 1.0f;
        var victim = board.NewCritter(genome, new Coord { X = 2, Y = 4 });
        victim.Alive = true;

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.KILL_FORWARD] = 0.05f;

        var action = new KillForward();
        action.Execute(config, board, critter, 0, actionLevels);

        Assert.NotEmpty(board.Critters.DeathQueue);
    }

    [Fact]
    public void MovementTest()
    {
        var action = new KillForward();
        var (x, y) = action.Move(Array.Empty<float>(), new Dir(Dir.Compass.CENTER));
        Assert.Equal(0.0, x);
        Assert.Equal(0.0, y);
    }
}