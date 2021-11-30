using System.Collections;
using System.Net;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;
using Random = System.Random;

namespace BioSimLib.Genes;

public class GenomeBuilder : IEnumerable<Gene>
{
    public class Node
    {
        public int RemappedNumber { get; set; }
        public int NumOutputs { get; set; }
        public int NumSelfInputs { get; set; }
        public int NumInputsFromSensorsOrOtherNeurons { get; set; }
    }

    private static readonly Random Rng = new();
    private readonly int _maxNumberNeurons;
    private readonly uint[] _dna;
    private readonly List<GeneBuilder> _genome = new();

    public int Neurons
    {
        get
        {
            var neurons = new HashSet<int>();
            foreach (var builder in _genome)
            {
                if (builder.SourceType == Gene.GeneType.Neuron)
                    neurons.Add(builder.SourceNum);
                if (builder.SinkType == Gene.GeneType.Neuron)
                    neurons.Add(builder.SinkNum);
            }

            return neurons.Count;
        }
    }

    public (byte, byte, byte) Color
    {
        get
        {
            if (_genome.Count == 0)
                return (0xFF, 0xFF, 0xFF);

            var front = _genome.First();
            var red = (byte)(((front.SourceNum & 0x03) << 6)
                             | (front.SinkNum & 0x03 << 4)
                             | ((front.Weight & 0x7800) >> 11));

            var mid = _genome.Skip(_genome.Count / 2).First();
            var blue = (byte)(((mid.SourceNum & 0x03) << 6)
                              | (mid.SinkNum & 0x03 << 4)
                              | ((mid.Weight & 0x7800) >> 11));

            var back = _genome.Last();
            var green = (byte)(((back.SourceNum & 0x03) << 6)
                               | (back.SinkNum & 0x03 << 4)
                               | ((back.Weight & 0x7800) >> 11));

            return (red, green, blue);
        }
    }

    public GenomeBuilder(int maxGenome, int maxNeurons)
    {
        _maxNumberNeurons = maxNeurons;
        _dna = new uint[maxGenome];
        for (var i = 0; i < _dna.Length; i++)
        {
            var lower = (uint)Rng.Next(0x10000);
            var upper = (uint)Rng.Next(0x10000);
            var dna = upper << 16 | lower;
            _dna[i] = dna;
        }
    }

    public GenomeBuilder(int maxNeurons, IEnumerable<uint> dna)
    {
        _maxNumberNeurons = maxNeurons;
        _dna = dna.Select(c => c).ToArray();
    }

    public GenomeBuilder(int maxNeurons, Genome genome)
    {
        _maxNumberNeurons = maxNeurons;
        _dna = genome.Dna;
    }

    public int Length => _dna.Length;

    public Gene this[int index] => _genome[index].ToGene();

    public IEnumerator<Gene> GetEnumerator() => _genome.Cast<Gene>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _genome.GetEnumerator();

    public void Mutate()
    {
        for (var i = 0; i < _dna.Length; i++)
        {
            var dna = _dna[i];
            var chance = Rng.Next(1000);
            switch (chance)
            {
                case < 3:
                {
                    var builder = new GeneBuilder(dna);
                    builder.WeightAsFloat += builder.WeightAsFloat * (0.1f * (float)Rng.NextDouble() - 0.05f);
                    _dna[i] = new Gene(builder).ToUint;
                    break;
                }
                case < 5:
                {
                    var bit = Rng.Next(16);
                    var value = dna ^ (0x00010000 << bit);
                    _dna[i] = (uint)value;
                    break;
                }
            }
        }
    }

    public Genome ToGenome()
    {
        SetupGenome();
        MakeRenumberedConnectionList();
        _genome.RemoveAll(gene => gene.SourceType == Gene.GeneType.Sensor && gene.SourceSensor == Sensor.FALSE);
        _genome.RemoveAll(gene => gene.SinkType == Gene.GeneType.Action && gene.SinkAction == Action.NONE);
        _genome.RemoveAll(gene => gene.SinkType == Gene.GeneType.Action && gene.SinkAction == Action.KILL_FORWARD);
        _genome.RemoveAll(gene => gene.SinkType == Gene.GeneType.Action && gene.SinkAction == Action.PROCREATE);
        _genome.RemoveAll(gene => gene.SinkType == Gene.GeneType.Action && gene.SinkAction == Action.SUICIDE);
        _genome.RemoveAll(gene => gene.Weight == 0);
        // CombineDuplicateNeurons()
        RemoveUnusedNeurons();
        CompressNeurons();

        var sortedList = _genome.OrderBy(o => o.ToUint());
        var genome = sortedList.Select(builder => new Gene(builder)).ToArray();

        return new Genome(_dna, genome, Neurons, Color);
    }

    public void SetupGenome()
    {
        _genome.Clear();
        foreach (var dna in _dna)
            _genome.Add(new GeneBuilder(dna));
    }

    public void MakeRenumberedConnectionList()
    {
        foreach (var builder in _genome)
        {
            builder.SourceNum %= builder.SourceType == Gene.GeneType.Sensor
                ? Enum.GetNames<Sensor>().Length
                : _maxNumberNeurons;

            builder.SinkNum %= builder.SinkType == Gene.GeneType.Action
                ? Enum.GetNames<Action>().Length
                : _maxNumberNeurons;
        }
    }

    public void RemoveUnusedNeurons()
    {
        var allDone = false;
        while (!allDone)
        {
            allDone = true;
            var nodeList = MakeNodeList();
            foreach (var (num, node) in nodeList)
            {
                if (node.NumOutputs > 0
                    && node.NumOutputs != node.NumSelfInputs)
                    continue;

                allDone = false;
                _genome.RemoveAll(gene => gene.SourceType == Gene.GeneType.Neuron && gene.SourceNum == num
                                          || gene.SinkType == Gene.GeneType.Neuron && gene.SinkNum == num);
            }
        }
    }

    public Dictionary<int, Node> MakeNodeList()
    {
        var nodeMap = new Dictionary<int, Node>();
        foreach (var conn in _genome)
        {
            if (conn.SourceType == Gene.GeneType.Neuron)
            {
                var found = nodeMap.TryGetValue(conn.SourceNum, out var node);
                if (!found || node == null)
                {
                    node = new Node();
                    nodeMap.Add(conn.SourceNum, node);
                }

                ++node.NumOutputs;
            }

            if (conn.SinkType == Gene.GeneType.Neuron)
            {
                var found = nodeMap.TryGetValue(conn.SinkNum, out var node);
                if (!found || node == null)
                {
                    node = new Node();
                    nodeMap.Add(conn.SinkNum, node);
                }

                if (conn.SourceType == Gene.GeneType.Neuron && conn.SourceNum == conn.SinkNum)
                    ++node.NumSelfInputs;
                else
                    ++node.NumInputsFromSensorsOrOtherNeurons;
            }
        }

        return nodeMap;
    }

    public void CompressNeurons()
    {
        foreach (var builder in _genome)
        {
            var neurons = new Dictionary<int, int>();
            if (builder.SourceType == Gene.GeneType.Neuron)
            {
                var num = builder.SourceNum;
                var found = neurons.TryGetValue(num, out var next);
                if (!found)
                {
                    next = neurons.Count;
                    neurons.Add(num, next);
                }

                builder.SourceNum = next;
            }

            if (builder.SinkType == Gene.GeneType.Neuron)
            {
                var num = builder.SinkNum;
                var found = neurons.TryGetValue(num, out var next);
                if (!found)
                {
                    next = neurons.Count;
                    neurons.Add(num, next);
                }

                builder.SinkNum = next;
            }
        }
    }
}