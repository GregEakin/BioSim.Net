//    Copyright 2021 Gregory Eakin
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

using System.Collections;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;
using Random = System.Random;

namespace BioSimLib.Genes;

public class GenomeBuilder : IEnumerable<GeneBuilder>
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
    private readonly List<GeneBuilder> _genes = new();

    public int Neurons
    {
        get
        {
            var neurons = new HashSet<int>();
            foreach (var builder in _genes)
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
            if (_genes.Count == 0)
                return (0xFF, 0xFF, 0xFF);

            var red = _genes.First().Color;
            var blue = _genes.Skip(_genes.Count / 2).First().Color;
            var green = _genes.Last().Color;
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
        _dna = dna.ToArray();
    }

    public GenomeBuilder(int maxNeurons, Genome genome)
    {
        _maxNumberNeurons = maxNeurons;
        _dna = genome.Dna.ToArray();
    }

    public int Length => _dna.Length;

    public Gene this[int index] => _genes[index].ToGene();

    public IEnumerator<GeneBuilder> GetEnumerator() => _genes.Cast<GeneBuilder>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _genes.GetEnumerator();

    public bool Mutate()
    {
        var mutated = false;
        for (var i = 0; i < _dna.Length; i++)
        {
            var dna = _dna[i];
            var chance = Rng.Next(1000);
            switch (chance)
            {
                case < 3:
                {
                    mutated = true;

                    var builder = new GeneBuilder(dna);
                    builder.WeightAsFloat += builder.WeightAsFloat * (0.1f * Rng.NextSingle() - 0.05f);
                    _dna[i] = new Gene(builder).ToUint;
                    break;
                }
                case < 5:
                {
                    mutated = true;

                    var bit = Rng.Next(16);
                    var value = dna ^ (0x00010000 << bit);
                    _dna[i] = (uint)value;
                    break;
                }
            }
        }

        return mutated;
    }

    public Genome ToGenome()
    {
        SetupGenome();
        MakeRenumberedConnectionList();
        _genes.RemoveAll(UnusedSensorsAndActions);
        RemoveUnusedNeurons();
        CombineDuplicateNeurons();
        CompressNeurons();

        var sortedList = _genes.OrderBy(o => o.ToUint());
        var genes = sortedList.Select(builder => new Gene(builder));

        return new Genome(_dna, genes.ToArray(), Neurons, Color);
    }

    public void SetupGenome()
    {
        _genes.Clear();
        foreach (var dna in _dna)
            _genes.Add(new GeneBuilder(dna));
    }

    public void MakeRenumberedConnectionList()
    {
        foreach (var builder in _genes)
        {
            builder.SourceNum %= builder.SourceType == Gene.GeneType.Sensor
                ? (byte)Enum.GetNames<Sensor>().Length
                : (byte)_maxNumberNeurons;

            builder.SinkNum %= builder.SinkType == Gene.GeneType.Action
                ? (byte)Enum.GetNames<Action>().Length
                : (byte)_maxNumberNeurons;
        }
    }

    private bool UnusedSensorsAndActions(GeneBuilder gene) =>
        gene.SourceType == Gene.GeneType.Sensor && gene.SourceSensor is Sensor.FALSE
        || gene.SinkType == Gene.GeneType.Action && gene.SinkAction is Action.NONE or Action.KILL_FORWARD or Action.PROCREATE or Action.SUICIDE
        || gene.Weight == 0;

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
                _genes.RemoveAll(gene => gene.SourceType == Gene.GeneType.Neuron && gene.SourceNum == num
                                          || gene.SinkType == Gene.GeneType.Neuron && gene.SinkNum == num);
            }
        }
    }

    public void CombineDuplicateNeurons()
    {

    }

    public Dictionary<int, Node> MakeNodeList()
    {
        var nodeMap = new Dictionary<int, Node>();
        foreach (var conn in _genes)
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
        foreach (var builder in _genes)
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