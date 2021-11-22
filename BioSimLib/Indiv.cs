using System.Collections.Specialized;
using System.Numerics;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib;

public class Indiv
{
    public readonly Params p;
    public readonly Genome genome;
    public readonly NeuralNet nnet; // derived from .genome
    public readonly short index; // index into peeps[] container
    public readonly Coord birthLoc;
    public readonly BitVector32 challengeBits; // modified when the indiv accomplishes some task

    public bool alive;
    public Coord loc; // refers to a location in grid[][]
    public uint age;
    public float responsiveness; // 0.0..1.0 (0 is like asleep)
    public uint oscPeriod; // 2..4*p.stepsPerGeneration (TBD, see executeActions())
    public uint longProbeDist; // distance for long forward probe for obstructions
    public Dir lastMoveDir; // direction of last movement

    public override string ToString()
    {
        return $"Neural Net {nnet}";
    }

    public Indiv(Params p, Grid grid, short index, Coord loc, Genome genome)
    {
        this.p = p;
        this.genome = genome;
        nnet = new NeuralNet(genome);

        this.index = index;
        this.loc = loc;
        birthLoc = loc;
        grid.Set(loc, index);
        age = 0u;
        oscPeriod = 34u; // ToDo !!! define a constant
        alive = true;
        lastMoveDir = Dir.Random8();
        responsiveness = 0.5f; // range 0.0..1.0
        longProbeDist = p.longProbeDistance;
        challengeBits = new BitVector32(0); // will be set true when some task gets accomplished

        CreateWiringFromGenome(p);
    }

    public float[] FeedForward(uint simStep)
    {
        var factory = new SensorFactory();

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        var neuronAccumulators = new float[nnet.Length];
        var neuronOutputsComputed = false;
        foreach (var conn in genome)
        {
            if (conn.SinkType == Gene.GeneType.Action && !neuronOutputsComputed)
            {
                for (var neuronIndex = 0; neuronIndex < nnet.Length; ++neuronIndex)
                {
                    if (nnet[neuronIndex].Driven)
                        nnet[neuronIndex].Output = (float)Math.Tanh(neuronAccumulators[neuronIndex]);
                }

                neuronOutputsComputed = true;
            }

            var inputVal = conn.SourceType == Gene.GeneType.Sensor
                ? factory[conn.SourceNeuron].Calc(p, this, simStep)
                : nnet[conn.SourceNum].Output;

            if (conn.SinkType == Gene.GeneType.Action)
                actionLevels[conn.SinkNum] += inputVal * conn.WeightAsFloat;
            else
                neuronAccumulators[conn.SinkNum] += inputVal * conn.WeightAsFloat;
        }

        return actionLevels;
    }

    public string PrintGraphInfo()
    {
        var builder = new StringBuilder();
        foreach (var conn in genome)
        {
            builder.AppendLine(conn.ToEdge());
        }

        return builder.ToString();
    }

    public static ConnectionList MakeRenumberedConnectionList(Params p, Genome genome)
    {
        var connectionList = new ConnectionList();
        foreach (var gene in genome)
        {
            connectionList.Add(gene);

            if (gene.SourceType == Gene.GeneType.Neuron)
                gene.SourceNum %= (byte)p.maxNumberNeurons;
            else
                gene.SourceNum %= (byte)Enum.GetNames<Sensor>().Length;

            if (gene.SinkType == Gene.GeneType.Neuron)
                gene.SinkNum %= (byte)p.maxNumberNeurons;
            else
                gene.SinkNum %= (byte)Enum.GetNames<Action>().Length;
        }

        return connectionList;
    }

    public static NodeMap MakeNodeList(ConnectionList connectionList)
    {
        var nodeMap = new NodeMap();
        foreach (var conn in connectionList)
        {
            if (conn.SinkType == Gene.GeneType.Neuron)
            {
                var found = nodeMap.TryGetValue(conn.SinkNum, out var it);
                if (!found || it == null)
                {
                    it = new Node();
                    nodeMap.Add(conn.SinkNum, it);
                }

                if (conn.SourceType == Gene.GeneType.Neuron && conn.SourceNum == conn.SinkNum)
                    ++it.numSelfInputs;
                else
                    ++it.numInputsFromSensorsOrOtherNeurons;
            }

            if (conn.SourceType == Gene.GeneType.Neuron)
            {
                var found = nodeMap.TryGetValue(conn.SourceNum, out var it);
                if (!found || it == null)
                {
                    it = new Node();
                    nodeMap.Add(conn.SourceNum, it);
                }

                ++it.numOutputs;
            }
        }

        return nodeMap;
    }

    public void CullUselessNeurons(ConnectionList connections, NodeMap nodeMap)
    {
        var allDone = false;
        while (!allDone)
        {
            allDone = true;
            foreach (var (key, value) in nodeMap)
            {
                if (value.numOutputs != value.numSelfInputs) continue;
                allDone = false;
                RemoveConnectionsToNeuron(connections, nodeMap, key);
                nodeMap.Remove(key);
            }
        }
    }

    public void RemoveConnectionsToNeuron(ConnectionList connections, NodeMap nodeMap, int neuronNumber)
    {
        foreach (var itConn in connections)
        {
            if (itConn.SinkType != Gene.GeneType.Neuron || itConn.SinkNum != neuronNumber) continue;
            if (itConn.SourceType == Gene.GeneType.Neuron)
                --nodeMap[itConn.SourceNum].numOutputs;
            connections.Remove(itConn);
        }
    }

    public void CreateWiringFromGenome(Params p)
    {
        var connectionList = MakeRenumberedConnectionList(p, genome);
        var nodeMap = MakeNodeList(connectionList);
        CullUselessNeurons(connectionList, nodeMap);

        byte newNumber = 0;
        foreach (var node in nodeMap)
            node.Value.remappedNumber = newNumber++;

        // nnet._connections.Clear();

        foreach (var conn in connectionList)
        {
            if (conn.SinkType != Gene.GeneType.Neuron) continue;
            // nnet._connections.Add(conn);
            var newConn = conn; // nnet._connections.Last();
            newConn.SinkNum = nodeMap[newConn.SinkNum].remappedNumber;
            if (newConn.SourceType == Gene.GeneType.Neuron)
                newConn.SourceNum = nodeMap[newConn.SourceNum].remappedNumber;
        }

        foreach (var conn in connectionList)
        {
            if (conn.SinkType != Gene.GeneType.Action) continue;
            // nnet._connections.Add(conn);
            var newConn = conn; // nnet._connections.Last();
            if (newConn.SourceType == Gene.GeneType.Neuron)
                newConn.SourceNum = nodeMap[newConn.SourceNum].remappedNumber;
        }

        // nnet._neurons.Clear();
        // for (var neuronNum = 0; neuronNum < nodeMap.Length; neuronNum++)
        // {
        //     var neuron = new NeuralNet.Neuron
        //     {
        //         Output = NeuralNet.Neuron.InitialNeuronOutput(),
        //         Driven = nodeMap[neuronNum].numInputsFromSensorsOrOtherNeurons != 0u
        //     };
        //
        //     nnet._neurons.Add(neuron);
        // }
    }

    public float ResponseCurve(float r)
    {
        var k = p.responsivenessCurveKFactor;
        var value = Math.Pow((r - 2.0f), -2.0f * k) - Math.Pow(2.0f, -2.0f * k) * (1.0f - r);
        return (float)value;
    }

    public void ExecuteActions(float[] actionLevels)
    {
        bool IsEnabled(Action action) => (int)action < (int)Action.KILL_FORWARD;

        if (IsEnabled(Action.SET_RESPONSIVENESS))
        {
            var value = actionLevels[(int)Action.SET_RESPONSIVENESS];
            responsiveness = (float)(Math.Tanh(value) + 1.0 / 2.0);
        }

        var responsivenessAdjusted = ResponseCurve(responsiveness);

        // var level = 0.0f;
        var offset = new Coord();
        var lastMoveOffset = lastMoveDir.AsNormalizedCoord();

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
        var newLoc = new Coord { X = (short)(loc.X + movementOffset.X), Y = (short)(loc.Y + movementOffset.Y) };
        // if (_grid.IsInBounds(newLoc)) 
        //      peeps.QueueForMove(this, newLoc);

        Console.WriteLine("X {0}, Y {0}", newLoc.X, newLoc.Y);
    }

    public static short prob2bool(float factor)
    {
        return 1;
    }
}