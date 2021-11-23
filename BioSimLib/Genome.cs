using System.Collections;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib;

public class Genome : IEnumerable<Gene>
{
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

    public Genome(Config p, uint[] dna)
    {
        _p = p;
        _genome = dna.Select(code => new Gene { ToUint = code }).ToArray();
        _connectionList = MakeRenumberedConnectionList();
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

    public Gene[] MakeRenumberedConnectionList()
    {
        var connectionList = new List<Gene>();
        foreach (var gene in _genome)
        {
            connectionList.Add(gene);

            if (gene.SourceType == Gene.GeneType.Sensor)
                gene.SourceNum %= (byte)Enum.GetNames<Sensor>().Length;
            else
                gene.SourceNum %= (byte)_p.maxNumberNeurons;

            if (gene.SinkType == Gene.GeneType.Action)
                gene.SinkNum %= (byte)Enum.GetNames<Action>().Length;
            else
                gene.SinkNum %= (byte)_p.maxNumberNeurons;
        }

        return connectionList.ToArray();
    }

    public void RemoveConnectionsToNeuron(Gene[] connections, NodeMap nodeMap, int neuronNumber)
    {
        foreach (var itConn in connections)
        {
            if (itConn.SinkType != Gene.GeneType.Neuron || itConn.SinkNum != neuronNumber) continue;
            if (itConn.SourceType == Gene.GeneType.Neuron)
                --nodeMap[itConn.SourceNum].numOutputs;
            // connections.Remove(itConn);
        }
    }

    public void CullUselessNeurons(Gene[] connections, NodeMap nodeMap)
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

    public NodeMap MakeNodeList()
    {
        var nodeMap = new NodeMap();
        foreach (var conn in _connectionList)
        {
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

            builder.Append(' ');

            if (gene.SinkType == Gene.GeneType.Action)
                builder.Append(gene.SinkNeuron);
            else
                builder.Append($"N{gene.SinkNum}");

            builder.Append($" {gene.WeightAsFloat}, ");
        }

        if (builder.Length > 2)
            builder.Remove(builder.Length - 2, 2);

        return builder.ToString();
    }

    // uint8_t makeGeneticColor(const Genome &genome)
    // {
    //     return ((genome.size() & 1u)
    //             | ((genome.front().sourceType)     << 1)
    //             | ((genome.back().sourceType)      << 2)
    //             | ((genome.front().sinkType)       << 3)
    //             | ((genome.back().sinkType)        << 4)
    //             | ((genome.front().sourceNum & 1u) << 5)
    //             | ((genome.front().sinkNum & 1u)   << 6)
    //             | ((genome.back().sourceNum & 1u)  << 7));
    // }

public void Optimize()
    {
        // This prunes unused neurons
        //
    }
}