//    Copyright 2022 Gregory Eakin
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

namespace BioSimLib.Genes;

public sealed class Genome(uint[] dna, Gene[] genes, int neurons, (byte red, byte green, byte blue) color)
    : IEnumerable<Gene>
{
    public int LiveCount { get; set; }

    public int Neurons { get; } = neurons;

    public (byte red, byte green, byte blue) Color { get; } = color;

    public int Length => Genes.Length;

    public Gene this[int index] => Genes[index];

    public uint[] Dna { get; } = dna;

    public Gene[] Genes { get; } = genes;

    public IEnumerator<Gene> GetEnumerator() => Genes.Cast<Gene>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Genes.GetEnumerator();

    public string ToGraphInfo()
    {
        var builder = new StringBuilder();
        foreach (var conn in Genes)
            builder.AppendLine(conn.ToEdge());

        return builder.ToString();
    }

    public string ToDna()
    {
        var builder = new StringBuilder();
        const int genesPerLine = 8;
        var count = 0;
        foreach (var gene in Dna)
        {
            if (count >= genesPerLine)
            {
                builder.AppendLine();
                count = 0;
            }
            else if (count != 0)
            {
                builder.Append(' ');
            }

            builder.Append(gene.ToString("X8"));
            ++count;
        }

        return builder.ToString();
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var gene in Genes)
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

    public void AddCritter()
    {
        LiveCount++;
    }

    public void RemoveCritter()
    {
        LiveCount--;
    }
}