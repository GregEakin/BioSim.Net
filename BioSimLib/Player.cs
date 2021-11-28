﻿//    Copyright 2021 Gregory Eakin
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

public class Player
{
    public readonly Config _p;
    public readonly Genome _genome;
    public readonly NeuralNet _nnet;
    public readonly ushort _index;
    public readonly Coord _birthLoc;
    public readonly BitVector32 _challengeBits;
    public readonly uint _birth;

    public Coord _loc;
    public bool _alive;
    public float _responsiveness;
    public uint _oscPeriod;
    public uint _longProbeDist;

    public bool Alive { get; set; } = true;
    public Dir LastMoveDir { get; set; } = Dir.Random8();
    public float ResponsivenessAdjusted { get; set; }

    public override string ToString()
    {
        return $"Neural Net {_nnet}";
    }

    public (byte, byte, byte) Color => _genome.Color;

    public Player(Config p, Genome genome, Coord loc, ushort index)
    {
        _p = p;
        _loc = loc;
        _birthLoc = loc;
        _index = index;
        _genome = genome;
        _nnet = new NeuralNet(genome);

        _birth = 0u;
        _oscPeriod = 34u; // ToDo !!! define a constant
        _alive = true;
        _responsiveness = 0.5f; // range 0.0..1.0
        ResponsivenessAdjusted = 1.0f;
        _longProbeDist = p.longProbeDistance;
        _challengeBits = new BitVector32(0); // will be set true when some task gets accomplished

        CreateWiringFromGenome();
    }

    public void FeedForward(SensorFactory sensors, float[] actionLevels, float[] neuronAccumulators, uint simStep)
    {
        foreach (var conn in _genome)
        {
            var value = conn.SourceType == Gene.GeneType.Sensor
                ? sensors[conn.SourceSensor].Output(this, simStep)
                : _nnet[conn.SourceNum].Output;

            if (conn.SinkType == Gene.GeneType.Action)
                actionLevels[conn.SinkNum] += conn.WeightAsFloat * (float)Math.Tanh(value);
            else
                neuronAccumulators[conn.SinkNum] += conn.WeightAsFloat * (float)Math.Tanh(value);
        }

        for (var i = 0; i < _nnet.Length; i++)
            _nnet[i].Output = neuronAccumulators[i];
    }

    public void CreateWiringFromGenome()
    {
        var connectionList = _genome.MakeRenumberedConnectionList();
        var nodeMap = _genome.MakeNodeList();
        //@@ CullUselessNeurons(connectionList, nodeMap);

        byte newNumber = 0;
        foreach (var node in nodeMap)
            node.Value.RemappedNumber = newNumber++;

        // _nnet._connections.Clear();

        // foreach (var conn in connectionList)
        // {
        //     if (conn.SinkType != Gene.GeneType.Neuron) continue;
        //     // _nnet._connections.Add(conn);
        //     var newConn = conn; // _nnet._connections.Last();
        //     newConn.SinkNum = nodeMap[newConn.SinkNum].remappedNumber;
        //     if (newConn.SourceType == Gene.GeneType.Neuron)
        //         newConn.SourceNum = nodeMap[newConn.SourceNum].remappedNumber;
        // }

        // foreach (var conn in connectionList)
        // {
        //     if (conn.SinkType != Gene.GeneType.Action) continue;
        //     // _nnet._connections.Add(conn);
        //     var newConn = conn; // _nnet._connections.Last();
        //     if (newConn.SourceType == Gene.GeneType.Neuron)
        //         newConn.SourceNum = nodeMap[newConn.SourceNum].remappedNumber;
        // }

        // _nnet._neurons.Clear();
        // for (var neuronNum = 0; neuronNum < nodeMap.Length; neuronNum++)
        // {
        //     var neuron = new NeuralNet.Neuron
        //     {
        //         Output = NeuralNet.Neuron.InitialNeuronOutput(),
        //         Driven = nodeMap[neuronNum].numInputsFromSensorsOrOtherNeurons != 0u
        //     };
        //
        //     _nnet._neurons.Add(neuron);
        // }
    }

    public float ResponseCurve(float r)
    {
        var k = _p.responsivenessCurveKFactor;
        var value = Math.Pow(r - 2.0f, -2.0f * k) - Math.Pow(2.0f, -2.0f * k) * (1.0f - r);
        return (float)value;
    }

    public void ExecuteActions(ActionFactory factory, Board board, Func<IAction,bool> isEnabled, float[] actionLevels, uint simStep)
    {
        var actionEnums = new[]
        {
            Action.SET_RESPONSIVENESS,
            Action.SET_OSCILLATOR_PERIOD,
            Action.SET_LONGPROBE_DIST,
            Action.EMIT_SIGNAL0,
            Action.KILL_FORWARD,
        };
        foreach (var actionEnum in actionEnums)
        {
            var action = factory[actionEnum];
            if (action == null || !isEnabled(action) || !action.Enabled)
                continue;

            action.Execute(_p, board, this, simStep, actionLevels);
        }
    }

    public Coord ExecuteMoves(ActionFactory factory, Func<IAction, bool> isEnabled, float[] actionLevels, uint simStep)
    {
        var moveX = 0.0f;
        var moveY = 0.0f;

        var moveEnums = new[]
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

        foreach (var moveEnum in moveEnums)
        {
            var action = factory[moveEnum];
            if (action == null || !isEnabled(action) || !action.Enabled)
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
        var newLoc = _loc + movementOffset;
        return newLoc;
    }

    public static bool Prob2Bool(float factor)
    {
        return true;
    }
}