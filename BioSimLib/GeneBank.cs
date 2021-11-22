namespace BioSimLib;

public class GeneBank
{
    private readonly Params _p;
    private readonly Dictionary<uint[], WeakReference<Genome>> _bank = new();

    public GeneBank(Params p)
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
}