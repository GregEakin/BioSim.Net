using System.Collections;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;
using Random = System.Random;

namespace BioSimLib.Genes;

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
    private readonly Random _random = new();

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

    public Genome(Config p)
    {
        _p = p;

        var list = new List<Gene>();
        for (var i = 0; i < _p.genomeMaxLength; i++)
        {
            var dna = (uint)_random.Next();
            list.Add(new Gene(dna));
        }

        _genome = list.ToArray();
        _connectionList = MakeRenumberedConnectionList().ToArray();
    }

    public Genome(Config p, Genome genome)
    {
        _p = p;
        _genome = genome._genome;
        _connectionList = genome._connectionList;
    }

    public Genome(Config p, IEnumerable<uint> dna)
    {
        _p = p;
        _genome = dna.Select(code => new Gene(code)).ToArray();
        _connectionList = MakeRenumberedConnectionList().ToArray();
    }

    public int Length => _genome.Length;

    public Gene this[int index] => _genome[index];

    public IEnumerator<Gene> GetEnumerator() => _connectionList.Cast<Gene>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _connectionList.GetEnumerator();

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

    public void RemoveConnectionsToNeuron(IEnumerable<Gene> connections, Dictionary<int, Node> nodeMap,
        int neuronNumber)
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
                if (!found || it == null)
                {
                    it = new Node();
                    nodeMap.Add(conn.SourceNum, it);
                }

                ++it.NumOutputs;
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
                    ++it.NumSelfInputs;
                else
                    ++it.NumInputsFromSensorsOrOtherNeurons;
            }
        }

        return nodeMap;
    }

    public string ToGraphInfo()
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

    public (byte, byte, byte) Color
    {
        get
        {
            // var front = _genome.First();
            // var red = (byte)(((front.SourceType == Gene.GeneType.Neuron ? 1 : 0) << 7) 
            //                  | ((front.SinkType == Gene.GeneType.Neuron ? 1 : 0) << 6)
            //                  | ((front.WeightAsShort & 0x7E00) >> 9));
            //
            // var mid = _genome.Skip(_genome.Length / 2).First();
            // var green = (byte)(((mid.SourceType == Gene.GeneType.Neuron ? 1 : 0) << 7)
            //                    | ((mid.SinkType == Gene.GeneType.Neuron ? 1 : 0) << 6)
            //                    | ((mid.WeightAsShort & 0x7E00) >> 9));
            //
            // var back = _genome.Last();
            // var blue = (byte)(((back.SourceType == Gene.GeneType.Neuron ? 1 : 0) << 7)
            //                   | ((back.SinkType == Gene.GeneType.Neuron ? 1 : 0) << 6)
            //                   | ((back.WeightAsShort & 0x7E00) >> 9));

            var front = _genome.First();
            var red = (byte)(((front.SourceNum & 0x03) << 6)
                             | (front.SinkNum & 0x03 << 4)
                             | ((front.WeightAsShort & 0x7800) >> 11));

            var mid = _genome.Skip(_genome.Length / 2).First();
            var blue = (byte)(((mid.SourceNum & 0x03) << 6)
                              | (mid.SinkNum & 0x03 << 4)
                              | ((mid.WeightAsShort & 0x7800) >> 11));

            var back = _genome.Last();
            var green = (byte)(((back.SourceNum & 0x03) << 6)
                               | (back.SinkNum & 0x03 << 4)
                               | ((back.WeightAsShort & 0x7800) >> 11));

            return (red, green, blue);
        }
    }

    public void Optimize()
    {
        // This prunes unused neurons
        //
    }

    public Genome Mutate()
    {
        var genes = new uint[_genome.Length];
        for (var i = 0; i < genes.Length; i++)
        {
            var gene = _genome[i];
            var chance = _random.Next(1000);
            switch (chance)
            {
                case < 3:
                {
                    var builder = new GeneBuilder(gene);
                    builder.WeightAsFloat = (float)(builder.WeightAsFloat * (0.1 * _random.NextDouble() + 0.95));
                    genes[i] = new Gene(builder).ToUint;
                    break;
                }
                case < 5:
                {
                    var builder = new GeneBuilder(gene);
                    var bit = _random.Next(16);
                    var value = new Gene(builder).ToUint ^ (0x00010000 << bit);
                    genes[i] = (uint)value;
                    break;
                }
                default:
                    genes[i] = gene.ToUint;
                    break;
            }
        }

        return new Genome(_p, genes);
    }
}