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

using System.Collections.Specialized;
using BioSimLib.Actions;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib;

public sealed class Critter
{
    private static readonly Action[] ActionEnums =
    {
        Action.SET_RESPONSIVENESS,
        Action.SET_OSCILLATOR_PERIOD,
        Action.SET_LONGPROBE_DIST,
        Action.EMIT_SIGNAL0,
        Action.KILL_FORWARD,
    };

    private static readonly Action[] MoveEnums =
    {
        Action.MOVE_X,
        Action.MOVE_Y,
        Action.MOVE_EAST,
        Action.MOVE_WEST,
        Action.MOVE_NORTH,
        Action.MOVE_SOUTH,
        Action.MOVE_FORWARD,
        Action.MOVE_REVERSE,
        Action.MOVE_LEFT,
        Action.MOVE_RIGHT,
        Action.MOVE_RL,
        Action.MOVE_RANDOM,
    };

    private readonly System.Random _random = new();
    private readonly Config _config;
    private bool _alive;
    private Coord _loc;
    private float _responsiveness;

    public Critter(Config config, Genome genome, Coord loc, ushort index)
    {
        _config = config;
        _loc = loc;
        BirthLocation = loc;
        Index = index;
        Genome = genome;
        Alive = genome.Length > 0;
        NeuralNet = new NeuralNet(Genome);

        BirthDate = 0u;
        OscPeriod = 34u; // ToDo !!! define a constant
        Responsiveness = 0.5f; // range 0.0..1.0
        LongProbeDist = config.longProbeDistance;
    }

    public bool Alive
    {
        get => _alive;
        set
        {
            _alive = value;
            if (value)
                Genome.AddCritter();
            else
                Genome.RemoveCritter();
        }
    }

    public uint BirthDate { get; }

    public Coord BirthLocation { get; }

    public BitVector32 ChallengeBits { get; } = new(0);

    public (byte, byte, byte) Color => Genome.Color;

    public Genome Genome { get; }

    public ushort Index { get; }

    public Dir LastMoveDir { get; set; } = Dir.Random8();

    public Coord Loc
    {
        get => _loc;
        set => _loc = value;
    }

    public short LocX => _loc.X;
    
    public short LocY => _loc.Y;

    public uint LongProbeDist { get; set; }

    public NeuralNet NeuralNet { get; }

    public uint OscPeriod { get; set; }

    public float Responsiveness
    {
        get => _responsiveness;
        set
        {
            _responsiveness = value;
            ResponsivenessAdjusted = ResponseCurve(value);
        }
    }

    public float ResponsivenessAdjusted { get; private set; }

    public override string ToString() => $"Index {Index}, Pos {Loc}, Neural Net {NeuralNet}";

    public void FeedForward(SensorFactory sensorFactory, float[] actionLevels, float[] neuronAccumulators, uint simStep)
    {
        foreach (var connection in Genome)
        {
            var value = connection.SourceType == Gene.GeneType.Sensor
                ? sensorFactory[connection.SourceSensor]?.Output(this, simStep) ?? 0.0f
                : NeuralNet[connection.SourceNum].Output;

            if (connection.SinkType == Gene.GeneType.Action)
                actionLevels[connection.SinkNum] += connection.WeightAsFloat * (float)Math.Tanh(value);
            else
                neuronAccumulators[connection.SinkNum] += connection.WeightAsFloat * (float)Math.Tanh(value);
        }

        for (var i = 0; i < NeuralNet.Length; i++)
            NeuralNet[i].Output = neuronAccumulators[i];
    }

    // This takes a probability from 0.0..1.0 and adjusts it according to an
    // exponential curve. The steepness of the curve is determined by the K factor
    // which is a small positive integer. This tends to reduce the activity level
    // a bit (makes the critters less reactive and jittery).
    public float ResponseCurve(float r)
    {
        var k = _config.responsivenessCurveKFactor;
        var value = Math.Pow(r - 2.0f, -2.0f * k) - Math.Pow(2.0f, -2.0f * k) * (1.0f - r);
        return (float)value;
    }

    public void ExecuteActions(ActionFactory factory, Board board, Func<IAction, bool> isEnabled, float[] actionLevels,
        uint simStep)
    {
        foreach (var actionEnum in ActionEnums)
        {
            var action = factory[actionEnum];
            if (action == null || !isEnabled(action))
                continue;

            action.Execute(_config, board, this, simStep, actionLevels);
        }
    }

    public Coord ExecuteMoves(ActionFactory factory, Func<IAction, bool> isEnabled, float[] actionLevels, uint simStep)
    {
        var moveX = 0.0f;
        var moveY = 0.0f;

        foreach (var moveEnum in MoveEnums)
        {
            var action = factory[moveEnum];
            if (action == null || !isEnabled(action))
                continue;

            var (x, y) = action.Move(actionLevels, LastMoveDir);
            moveX += x;
            moveY += y;
        }

        moveX = (float)Math.Tanh(moveX) * ResponsivenessAdjusted;
        moveY = (float)Math.Tanh(moveY) * ResponsivenessAdjusted;

        var probX = Prob2Bool(Math.Abs(moveX)) ? 1 : 0;
        var probY = Prob2Bool(Math.Abs(moveY)) ? 1 : 0;

        var sigNumX = moveX < 0.0f ? -1 : 1;
        var sigNumY = moveY < 0.0f ? -1 : 1;

        var movementOffset = new Coord { X = (short)(probX * sigNumX), Y = (short)(probY * sigNumY) };
        var newLoc = Loc + movementOffset;
        return newLoc;
    }

    public bool Prob2Bool(float factor)
    {
        return _random.NextSingle() < factor;
    }
}