using System.Collections;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib;

public class Genome : IEnumerable<Gene>
{
    public class Node
    {
        public byte RemappedNumber { get; set; }
        public int NumOutputs { get; set; }
        public int NumSelfInputs { get; set; }
        public int NumInputsFromSensorsOrOtherNeurons { get; set; }
    }

    private readonly Config _p;
    private readonly Gene[] _genome;
    private readonly Gene[] _connectionList;

    public int NeuronsNeeded
    {
        get
        {
            var neurons = new HashSet<byte>();
            foreach (var gene in _genome)
            {
                if (gene.SourceType == Gene.GeneType.Neuron)
                    neurons.Add(gene.SourceNum);

                if (gene.SinkType == Gene.GeneType.Neuron)
                    neurons.Add(gene.SinkNum);
            }

            return Math.Min(neurons.Count, _p.maxNumberNeurons);
        }
    }

    public int Length => _genome.Length;

    public Genome(Config p, IEnumerable<uint> dna)
    {
        _p = p;
        _genome = dna.Select(code => new Gene(code)).ToArray();
        _connectionList = MakeRenumberedConnectionList().ToArray();
    }

    public Gene this[int index] => _genome[index];

    public IEnumerator<Gene> GetEnumerator()
    {
        return _connectionList.Cast<Gene>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _connectionList.GetEnumerator();
    }

    public IEnumerable<Gene> MakeRenumberedConnectionList()
    {
        var connectionList = new List<Gene>();
        foreach (var gene in _genome)
        {
            var builder = new GeneBuilder();
            builder.SourceType = gene.SourceType;
            builder.SourceNum = gene.SourceType == Gene.GeneType.Sensor
                ? gene.SourceNum % Enum.GetNames<Sensor>().Length
                : gene.SourceNum % _p.maxNumberNeurons;

            builder.SinkType = gene.SinkType;
            builder.SinkNum = gene.SinkType == Gene.GeneType.Action
                ? gene.SinkNum % Enum.GetNames<Action>().Length
                : gene.SinkNum % _p.maxNumberNeurons;

            builder.Weight = gene.WeightAsShort;
            var newGene = new Gene(builder);
            connectionList.Add(newGene);
        }

        return connectionList;
    }

    public void RemoveConnectionsToNeuron(IEnumerable<Gene> connections, Dictionary<int, Node> nodeMap, int neuronNumber)
    {
        foreach (var itConn in connections)
        {
            if (itConn.SinkType != Gene.GeneType.Neuron || itConn.SinkNum != neuronNumber) continue;
            if (itConn.SourceType == Gene.GeneType.Neuron)
                --nodeMap[itConn.SourceNum].NumOutputs;

            // connections.Remove(itConn);
        }
    }

    public void CullUselessNeurons(IEnumerable<Gene> connections, Dictionary<int, Node> nodeMap)
    {
        var allDone = false;
        while (!allDone)
        {
            allDone = true;
            foreach (var (key, value) in nodeMap)
            {
                if (value.NumOutputs != value.NumSelfInputs) continue;
                allDone = false;
                RemoveConnectionsToNeuron(connections, nodeMap, key);
                nodeMap.Remove(key);
            }
        }
    }

    public Dictionary<int, Node> MakeNodeList()
    {
        var nodeMap = new Dictionary<int, Node>();
        foreach (var conn in _connectionList)
        {
            if (conn.SourceType == Gene.GeneType.Neuron)
            {
                var found = nodeMap.TryGetValue(conn.SourceNum, out var it);
                if (!found)
                {
                    it = new Node();
                    nodeMap.Add(conn.SourceNum, it);
                }

                ++it.NumOutputs;
            }

            if (conn.SinkType == Gene.GeneType.Neuron)
            {
                var found = nodeMap.TryGetValue(conn.SinkNum, out var it);
                if (!found)
                {
                    it = new Node();
                    nodeMap.Add(conn.SinkNum, it);
                }

                if (conn.SourceType == Gene.GeneType.Neuron && conn.SourceNum == conn.SinkNum)
                    ++it.NumSelfInputs;
                else
                    ++it.NumInputsFromSensorsOrOtherNeurons;
            }
        }

        return nodeMap;
    }

    public string PrintGraphInfo()
    {
        var builder = new StringBuilder();
        foreach (var conn in _genome)
            builder.AppendLine(conn.ToEdge());

        return builder.ToString();
    }

    public string ToDna()
    {
        var builder = new StringBuilder();
        const int genesPerLine = 8;
        var count = 0;
        foreach (var gene in _genome)
        {
            if (count >= genesPerLine)
            {
                builder.AppendLine();
                count = 0;
            }
            else if (count != 0)
                builder.Append(' ');

            builder.Append(gene);
            ++count;
        }

        return builder.ToString();
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var gene in _genome)
        {
            if (gene.SourceType == Gene.GeneType.Sensor)
                builder.Append(gene.SourceSensor);
            else
                builder.Append($"N{gene.SourceNum}");

            builder.Append($" * {gene.WeightAsFloat} => ");

            if (gene.SinkType == Gene.GeneType.Action)
                builder.Append(gene.SinkNeuron);
            else
                builder.Append($"N{gene.SinkNum}");

            builder.Append(", ");
        }

        if (builder.Length > 2)
            builder.Remove(builder.Length - 2, 2);

        return builder.ToString();
    }

    public byte MakeGeneticColor()
    {
        var front = _genome.First();
        var back = _genome.Last();
        var color = (_genome.Length & 1)
                    | ((front.SourceType == Gene.GeneType.Sensor ? 1 : 0) << 1)
                    | ((back.SourceType == Gene.GeneType.Sensor ? 1 : 0) << 2)
                    | ((front.SinkType == Gene.GeneType.Action ? 1 : 0) << 3)
                    | ((back.SinkType == Gene.GeneType.Action ? 1 : 0) << 4)
                    | ((front.SourceNum & 1) << 5)
                    | ((front.SinkNum & 1) << 6)
                    | ((back.SourceNum & 1) << 7);
        return (byte)color;
    }

    public void Optimize()
    {
        // This prunes unused neurons
        //
    }
}