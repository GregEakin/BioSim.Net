using System.Collections;
using System.Text;

namespace BioSimLib;

public class Genome : IEnumerable<Gene>
{
    private readonly Params _p;
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

            return Math.Min(neurons.Count, _p.maxNumberNeurons);
        }
    }

    public int Length => _genome.Length;

    public Genome(Params p, uint[] dna)
    {
        _p = p;
        _genome = dna.Select(code => new Gene { ToUint = code }).ToArray();
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

            builder.Append($" {gene.WeightAsShort}");
        }

        return builder.ToString();
    }

    public void Optimize()
    {
        // This prunes unused neurons
        //
    }
}