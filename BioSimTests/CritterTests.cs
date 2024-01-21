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

namespace BioSimTests;

public class CritterTests
{
    [Fact]
    public void FeedForwardTest()
    {
        var sensorFactory = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.LOC_X, "Lx", 0.2f),
                new SensorMock(Sensor.CHANCE, "Rnd", 0.1f),
            }
        );

        var config = new Config { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
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

        var genome = new GenomeBuilder(config.maxNumberNeurons, dna).ToGenome();
        var loc = new Coord (4, 4);
        var critter = board.NewCritter(genome, loc);
        critter.NeuralNet[0].Driven = true;
        critter.NeuralNet[0].Output = 0.6f;
        critter.NeuralNet[1].Driven = true;
        critter.NeuralNet[1].Output = 0.4f;

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        var neuronAccumulators = new float[config.maxNumberNeurons];
        critter.FeedForward(sensorFactory, actionLevels, neuronAccumulators, 0);
        Assert.Equal(0.577324271f, critter.NeuralNet[0].Output);
        Assert.Equal(0.916998565f, critter.NeuralNet[1].Output);

        var expectedLevels = new float[Enum.GetNames<Action>().Length];
        expectedLevels[(int)Action.MOVE_RANDOM] = 0.47961697f;
        expectedLevels[(int)Action.MOVE_WEST] = 0.379948974f;
        Assert.Equal(expectedLevels, actionLevels);
    }

    static bool IsEnabled(IAction action) => (int)action.Type < (int)Action.KILL_FORWARD;

    [Fact]
    public void MovementTest()
    {
        var config = new Config { maxNumberNeurons = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(config);

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

        var genome = new GenomeBuilder(config.maxNumberNeurons, dna).ToGenome();
        var loc = new Coord (1, 2);
        var critter = board.NewCritter(genome, loc);
        critter.NeuralNet[0].Driven = true;
        critter.NeuralNet[0].Output = 0.6f;
        critter.NeuralNet[1].Driven = true;
        critter.NeuralNet[1].Output = 0.4f;

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_RANDOM] = 0.63671756f;
        actionLevels[(int)Action.MOVE_WEST] = 0.5370496f;

        var factory = new ActionFactory(config, board);
        critter.ExecuteActions(factory, IsEnabled, actionLevels, 0);
        var newLoc = critter.ExecuteMoves(factory, IsEnabled, actionLevels);

        // Assert.Equal(2, newLoc.X);
        // Assert.Equal(3, newLoc.Y);
    }

    [Fact]
    public void GetSignalDensityAlongAxisTest()
    {
        // GetSignalDensityAlongAxis
    }

    [Fact]
    public void ResponseCurveTest()
    {
        // responsivenessCurveKFactor = 1, 2, 3 or 4
        var config = new Config { responsivenessCurveKFactor = 1 };
        var genome = new GenomeBuilder(1, 1).ToGenome();
        var loc = new Coord (1, 1);
        var critter = new Critter(config, genome, loc, 1);

        var factor0 = critter.ResponseCurve(0.0f);
        Assert.Equal(0.0f, factor0);

        var factor1 = critter.ResponseCurve(0.5f);
        Assert.Equal(0.319444448f, factor1);

        var factor2 = critter.ResponseCurve(1.0f);
        Assert.Equal(1.0f, factor2);
    }
}