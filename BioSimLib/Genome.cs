using System.Collections;
using System.Text;

namespace BioSimLib;

public class Genome : IEnumerable<Gene>
{
    private readonly Gene[] _genome;

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

            return neurons.Count;
        }
    }

    public int Length => _genome.Length;

    public Genome(Params p)
    {
        _genome = new Gene[] { new() { ToUint = 0x840B7FFFu } };
    }

    public Gene this[int index] => _genome[index];

    public IEnumerator<Gene> GetEnumerator()
    {
        return _genome.Cast<Gene>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _genome.GetEnumerator();
    }

    public override string ToString()
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

    public string ToText()
    {
        var builder = new StringBuilder();
        foreach (var gene in _genome)
        {
            if (gene.SourceType == Gene.GeneType.Sensor)
                builder.Append(gene.SourceNeuron);
            else
                builder.Append($"N{gene.SourceNum}");

            builder.Append(' ');

            if (gene.SinkType == Gene.GeneType.Action)
                builder.Append(gene.SinkNeuron);
            else
                builder.Append($"N{gene.SinkNum}");

            builder.Append($" {gene.WeightAsShort}");
        }

        return builder.ToString();
    }
}