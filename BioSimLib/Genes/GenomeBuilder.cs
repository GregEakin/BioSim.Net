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

            var red = _genome.First().Color;
            var blue = _genome.Skip(_genome.Count / 2).First().Color;
            var green = _genome.Last().Color;
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
                ? (byte)Enum.GetNames<Sensor>().Length
                : (byte)_maxNumberNeurons;

            builder.SinkNum %= builder.SinkType == Gene.GeneType.Action
                ? (byte)Enum.GetNames<Action>().Length
                : (byte)_maxNumberNeurons;
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
            var neurons = new Dictionary<byte, byte>();
            if (builder.SourceType == Gene.GeneType.Neuron)
            {
                var num = builder.SourceNum;
                var found = neurons.TryGetValue(num, out var next);
                if (!found)
                {
                    next = (byte)neurons.Count;
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
                    next = (byte)neurons.Count;
                    neurons.Add(num, next);
                }

                builder.SinkNum = next;
            }
        }
    }
}