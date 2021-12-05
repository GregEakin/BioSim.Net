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

using System;
using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests;

public class PlayerTests
{
    [Fact]
    public void FeedForwardTest()
    {
        var sensorFactory = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.LOC_X, "Lx", 0.2f),
                new SensorMock(Sensor.RANDOM, "Rnd", 0.1f),
            }
        );

        var p = new Config { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
        var board = new Board(p);
        var dna = new[]
        {
            0x00012000u,
            0x01002000u,
            0x018A2000u,
            0x01842000u,
            0x01012000u,
            0x80002000u,
            0x91842000u,
        };

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna).ToGenome();
        var loc = new Coord { X = 4, Y = 4 };
        var player = board.NewPlayer(genome, loc);
        player._nnet[0].Driven = true;
        player._nnet[0].Output = 0.6f;
        player._nnet[1].Driven = true;
        player._nnet[1].Output = 0.4f;

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        var neuronAccumulators = new float[p.maxNumberNeurons];
        player.FeedForward(sensorFactory, actionLevels, neuronAccumulators, 0);
        Assert.Equal(0.73442495f, player._nnet[0].Output);
        Assert.Equal(1.07409918f, player._nnet[1].Output);

        var expectedLevels = new float[Enum.GetNames<Action>().Length];
        expectedLevels[(int)Action.MOVE_RANDOM] = 0.63671756f;
        expectedLevels[(int)Action.MOVE_WEST] = 0.5370496f;
        Assert.Equal(expectedLevels, actionLevels);
    }

    static bool IsEnabled(IAction action) => (int)action.Type < (int)Action.KILL_FORWARD;

    [Fact]
    public void MovementTest()
    {
        var p = new Config { maxNumberNeurons = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(p);

        var dna = new[]
        {
            0x00012000u,
            0x01002000u,
            0x018A2000u,
            0x01822000u,
            0x01012000u,
            0x80002000u,
            0x81822000u,
        };

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna).ToGenome();
        var loc = new Coord { X = 1, Y = 2 };
        var player = board.NewPlayer(genome, loc);
        player._nnet[0].Driven = true;
        player._nnet[0].Output = 0.6f;
        player._nnet[1].Driven = true;
        player._nnet[1].Output = 0.4f;

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_RANDOM] = 0.63671756f;
        actionLevels[(int)Action.MOVE_WEST] = 0.5370496f;

        var factory = new ActionFactory();
        player.ExecuteActions(factory, board, IsEnabled, actionLevels, 0);
        var newLoc = player.ExecuteMoves(factory, IsEnabled, actionLevels, 0);

        // Assert.Equal(2, newLoc.X);
        // Assert.Equal(3, newLoc.Y);
    }

    [Fact]
    public void GetSignalDensityAlongAxisTest()
    {
        // GetSignalDensityAlongAxis
    }
}