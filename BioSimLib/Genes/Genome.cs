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
using System.Text;
using Random = System.Random;

namespace BioSimLib.Genes;

public class Genome : IEnumerable<Gene>
{
    private static readonly Random Rng = new();
    private readonly uint[] _dna;
    private readonly Gene[] _genes;

    public int Neurons { get; }
    public (byte, byte, byte) Color { get; }

    public Genome(uint[] dna, Gene[] genes, int neurons, (byte, byte, byte) color)
    {
        _dna = dna;
        _genes = genes;
        Neurons = neurons;
        Color = color;
    }

    public int Length => _genes.Length;

    public Gene this[int index] => _genes[index];

    public IEnumerator<Gene> GetEnumerator() => _genes.Cast<Gene>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _genes.GetEnumerator();

    public string ToGraphInfo()
    {
        var builder = new StringBuilder();
        foreach (var conn in _genes)
            builder.AppendLine(conn.ToEdge());

        return builder.ToString();
    }

    public uint[] Dna => _dna;
    public Gene[] Genes => _genes;

    public string ToDna()
    {
        var builder = new StringBuilder();
        const int genesPerLine = 8;
        var count = 0;
        foreach (var gene in _dna)
        {
            if (count >= genesPerLine)
            {
                builder.AppendLine();
                count = 0;
            }
            else if (count != 0)
                builder.Append(' ');

            builder.Append(gene.ToString("X8"));
            ++count;
        }

        return builder.ToString();
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var gene in _genes)
        {
            if (gene.SourceType == Gene.GeneType.Sensor)
                builder.Append(gene.SourceSensor);
            else
                builder.Append($"N{gene.SourceNum}");

            builder.Append($" * {gene.WeightAsFloat} => ");

            if (gene.SinkType == Gene.GeneType.Action)
                builder.Append(gene.SinkAction);
            else
                builder.Append($"N{gene.SinkNum}");

            builder.Append(", ");
        }

        if (builder.Length > 2)
            builder.Remove(builder.Length - 2, 2);

        return builder.ToString();
    }

    public void AddPlayer()
    {
    }

    public void RemovePlayer()
    {
    }
}