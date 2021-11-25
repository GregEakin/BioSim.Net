namespace BioSimLib.Genes;

public class GeneBank
{
    private readonly Config _p;
    private readonly Dictionary<uint[], WeakReference<Genome>> _bank = new();

    public GeneBank(Config p)
    {
        _p = p;
    }

    Genome AddGene(uint[] dna)
    {
        var found = _bank.TryGetValue(dna, out var genomeReference);
        if (!found || genomeReference == null)
        {
            var genome1 = new Genome(_p, dna);
            // genome1.Optimize();
            _bank.Add(dna, new WeakReference<Genome>(genome1));
            return genome1;
        }

        var available = genomeReference.TryGetTarget(out var genome3);
        if (!available || genome3 == null)
        {
            genome3 = new Genome(_p, dna);
            // genome3.Optimize();
            genomeReference.SetTarget(genome3);
            return genome3;
        }

        return genome3;
    }

    Genome Sex(string mother, string father)
    {
        // Combine the DNA from each parent
        // Perform mutations
        //    Swap genes between the two
        //    Adjust the weights by +- %
        return new Genome(_p, new uint[]{});
    }

    public override string ToString()
    {
        var count = 0;
        foreach (var pair in _bank)
        {
            if (pair.Value.TryGetTarget(out var genome))
                count++;
        }

        return count.ToString();
    }

    public static float GenomeSimilarity(Genome g1, Genome g2)
    {
        // switch (p.genomeComparisonMethod) {
        //     case 0:
        //         return jaro_winkler_distance(g1, g2);
        //     case 1:
        //         return hammingDistanceBits(g1, g2);
        //     case 2:
        //         return hammingDistanceBytes(g1, g2);
        //     default:
        //         assert(false);
        // }
        return 0.0f;
    }
}