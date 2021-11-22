using System.Collections.Specialized;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib;

public class Indiv
{
    public readonly Params _p;
    public readonly Genome _genome;
    public readonly NeuralNet _nnet; 
    public readonly short _index; 
    public readonly Coord _birthLoc;
    public readonly BitVector32 _challengeBits; 

    public bool _alive;
    public Coord _loc; 
    public uint _age;
    public float _responsiveness; 
    public uint _oscPeriod; 
    public uint _longProbeDist; 
    public Dir _lastMoveDir; 

    public override string ToString()
    {
        return $"Neural Net {_nnet}";
    }

    public Indiv(Params p, Grid grid, short index, Coord loc, Genome genome)
    {
        _p = p;
        _genome = genome;
        _nnet = new NeuralNet(genome);

        _index = index;
        _loc = loc;
        _birthLoc = loc;
        grid.Set(loc, index);
        _age = 0u;
        _oscPeriod = 34u; // ToDo !!! define a constant
        _alive = true;
        _lastMoveDir = Dir.Random8();
        _responsiveness = 0.5f; // range 0.0..1.0
        _longProbeDist = p.longProbeDistance;
        _challengeBits = new BitVector32(0); // will be set true when some task gets accomplished

        CreateWiringFromGenome();
    }

    public float[] FeedForward(SensorFactory sensors, uint simStep)
    {
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        var neuronAccumulators = new float[_nnet.Length];
        var neuronOutputsComputed = false;
        foreach (var conn in _genome)
        {
            if (conn.SinkType == Gene.GeneType.Action && !neuronOutputsComputed)
            {
                for (var neuronIndex = 0; neuronIndex < _nnet.Length; ++neuronIndex)
                {
                    if (_nnet[neuronIndex].Driven)
                        _nnet[neuronIndex].Output = (float)Math.Tanh(neuronAccumulators[neuronIndex]);
                }

                neuronOutputsComputed = true;
            }

            var inputVal = conn.SourceType == Gene.GeneType.Sensor
                ? sensors[conn.SourceSensor].Output(_p, this, simStep)
                : _nnet[conn.SourceNum].Output;

            if (conn.SinkType == Gene.GeneType.Action)
                actionLevels[conn.SinkNum] += inputVal * conn.WeightAsFloat;
            else
                neuronAccumulators[conn.SinkNum] += inputVal * conn.WeightAsFloat;
        }

        return actionLevels;
    }

    public float[] FeedForward2(SensorFactory sensors, uint simStep)
    {
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        var neuronCollectors = new float[_genome.Length];
        for (var i = 0; i < _genome.Length; i++)
        {
            var conn = _genome[i];
            neuronCollectors[i] = conn.SourceType == Gene.GeneType.Sensor
                ? sensors[conn.SourceSensor].Output(_p, this, simStep)
                : _nnet[conn.SourceNum].Output;
        }

        var sum = 0.0f;
        var last = 0x100;
        for (var i = 0; i < _genome.Length; i++)
        {
            var conn = _genome[i];
            if (last != conn.SinkNum)
                sum = neuronCollectors[i];
            else
                sum += neuronCollectors[i];

            last = conn.SinkNum;
            if (conn.SinkType == Gene.GeneType.Action)
                actionLevels[conn.SinkNum] = conn.WeightAsFloat * (float)Math.Tanh(sum);
            else
                _nnet[conn.SinkNum].Output = conn.WeightAsFloat * (float)Math.Tanh(sum);
        }

        return actionLevels;
    }

    public void CreateWiringFromGenome()
    {
        var connectionList = _genome.MakeRenumberedConnectionList();
        var nodeMap = _genome.MakeNodeList();
        //@@ CullUselessNeurons(connectionList, nodeMap);

        byte newNumber = 0;
        foreach (var node in nodeMap)
            node.Value.remappedNumber = newNumber++;

        // _nnet._connections.Clear();

        foreach (var conn in connectionList)
        {
            if (conn.SinkType != Gene.GeneType.Neuron) continue;
            // _nnet._connections.Add(conn);
            var newConn = conn; // _nnet._connections.Last();
            newConn.SinkNum = nodeMap[newConn.SinkNum].remappedNumber;
            if (newConn.SourceType == Gene.GeneType.Neuron)
                newConn.SourceNum = nodeMap[newConn.SourceNum].remappedNumber;
        }

        foreach (var conn in connectionList)
        {
            if (conn.SinkType != Gene.GeneType.Action) continue;
            // _nnet._connections.Add(conn);
            var newConn = conn; // _nnet._connections.Last();
            if (newConn.SourceType == Gene.GeneType.Neuron)
                newConn.SourceNum = nodeMap[newConn.SourceNum].remappedNumber;
        }

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
        var value = Math.Pow((r - 2.0f), -2.0f * k) - Math.Pow(2.0f, -2.0f * k) * (1.0f - r);
        return (float)value;
    }

    public void ExecuteActions(float[] actionLevels)
    {
        bool IsEnabled(Action action) => (int)action < (int)Action.KILL_FORWARD;

        if (IsEnabled(Action.SET_RESPONSIVENESS))
        {
            var value = actionLevels[(int)Action.SET_RESPONSIVENESS];
            _responsiveness = (float)(Math.Tanh(value) + 1.0 / 2.0);
        }

        var responsivenessAdjusted = ResponseCurve(_responsiveness);

        // var level = 0.0f;
        var offset = new Coord();
        var lastMoveOffset = _lastMoveDir.AsNormalizedCoord();

        var moveX = IsEnabled(Action.MOVE_X) ? actionLevels[(int)Action.MOVE_X] : 0.0f;
        var moveY = IsEnabled(Action.MOVE_Y) ? actionLevels[(int)Action.MOVE_Y] : 0.0f;

        if (IsEnabled(Action.MOVE_EAST)) moveX += actionLevels[(int)Action.MOVE_EAST];
        if (IsEnabled(Action.MOVE_WEST)) moveX -= actionLevels[(int)Action.MOVE_WEST];
        if (IsEnabled(Action.MOVE_NORTH)) moveY += actionLevels[(int)Action.MOVE_NORTH];
        if (IsEnabled(Action.MOVE_SOUTH)) moveY -= actionLevels[(int)Action.MOVE_SOUTH];

        moveX = (float)Math.Tanh(moveX);
        moveY = (float)Math.Tanh(moveY);
        moveX *= responsivenessAdjusted;
        moveY *= responsivenessAdjusted;

        var probX = prob2bool(Math.Abs(moveX)); // convert abs(level) to 0 or 1
        var probY = prob2bool(Math.Abs(moveY)); // convert abs(level) to 0 or 1

        // The direction of movement (if any) along each axis is the sign
        var signumX = moveX < 0.0f ? -1 : 1;
        var signumY = moveY < 0.0f ? -1 : 1;

        // Generate a normalized movement offsetx = , shorteach component is -1, 0, or 1
        var movementOffset = new Coord { X = (short)(probX * signumX), Y = (short)(probY * signumY) };

        // Move there if it's a valid location
        var newLoc = new Coord { X = (short)(_loc.X + movementOffset.X), Y = (short)(_loc.Y + movementOffset.Y) };
        // if (_grid.IsInBounds(newLoc)) 
        //      peeps.QueueForMove(this, newLoc);

        Console.WriteLine("X {0}, Y {0}", newLoc.X, newLoc.Y);
    }

    public static short prob2bool(float factor)
    {
        return 1;
    }
}