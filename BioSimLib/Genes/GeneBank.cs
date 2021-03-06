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

using System.Runtime.InteropServices;

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
        return new Genome(_p, new uint[] { });
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

    public enum ComparisonMethods
    {
        JARO_WINKLER_DISTANCE,
        HAMMING_DISTANCE_BITS,
        HAMMING_DISTANCE_BYTES,
    }

    public static float GenomeSimilarity(ComparisonMethods genomeComparisonMethod, Genome g1, Genome g2)
    {
        return genomeComparisonMethod switch
        {
            ComparisonMethods.JARO_WINKLER_DISTANCE => JaroWinklerDistance(g1, g2),
            ComparisonMethods.HAMMING_DISTANCE_BITS => HammingDistanceBits(g1, g2),
            ComparisonMethods.HAMMING_DISTANCE_BYTES => HammingDistanceBytes(g1, g2),
            _ => throw new NotImplementedException()
        };
    }

    // The JaroWinklerDistance() function is adapted from the C version at
    // https://github.com/miguelvps/c/blob/master/jarowinkler.c
    // under a GNU license, ver. 3. This comparison function is useful if
    // the simulator allows genomes to change length, or if genes are allowed
    // to relocate to different offsets in the genome. I.e., this function is
    // tolerant of gaps, relocations, and genomes of unequal lengths.
    //
    public static float JaroWinklerDistance(Genome genome1, Genome genome2)
    {
        var max = (int a, int b) => a > b ? a : b;
        var min = (int a, int b) => a < b ? a : b;

        int l;
        int m = 0, t = 0;

        const int maxNumGenesToCompare = 20;
        var sl = min(maxNumGenesToCompare, genome1.Length); // optimization: approximate for long genomes
        var al = min(maxNumGenesToCompare, genome2.Length);

        if (sl == 0 || al == 0)
            return 0.0f;

        var sFlags = new bool[sl];
        var aFlags = new bool[sl];
        var range = max(0, max(sl, al) / 2 - 1);

        /* calculate matching characters */
        for (var i = 0; i < al; i++)
        {
            int j;
            for (j = max(i - range, 0), l = min(i + range + 1, sl); j < l; j++)
            {
                if (!genome2[i].Equals(genome1[j]) || sFlags[j]) continue;
                sFlags[j] = true;
                aFlags[i] = true;
                m++;
                break;
            }
        }

        if (m == 0)
            return 0.0f;

        /* calculate character transpositions */
        l = 0;
        for (var i = 0; i < al; i++)
        {
            if (!aFlags[i]) continue;
            int j;
            for (j = l; j < sl; j++)
            {
                if (!sFlags[j]) continue;
                l = j + 1;
                break;
            }

            if (!genome2[i].Equals(genome1[j]))
                t++;
        }

        t /= 2;

        /* Jaro distance */
        var dw = ((float)m / sl + (float)m / al + (float)(m - t) / m) / 3.0f;
        return dw;
    }

    public static float HammingDistanceBits(Genome genome1, Genome genome2)
    {
        // Works only for genomes of equal length
        if (genome1.Length != genome2.Length)
            throw new ArgumentException();

        var bitCount = genome1.Select((gene, i) => (int)NumberOfSetBits(gene.ToUint ^ genome2[i].ToUint)).Sum();
        var lengthBits = genome1.Length * Marshal.SizeOf<Gene>() * 8;

        // For two completely random bit patterns, about half the bits will differ,
        // resulting in c. 50% match. We will scale that by 2X to make the range
        // from 0 to 1.0. We clip the value to 1.0 in case the two patterns are
        // negatively correlated for some reason.
        return 1.0f - Math.Min(1.0f, 2.0f * bitCount / lengthBits);
    }

    public static uint NumberOfSetBits(uint i)
    {
        i = i - ((i >> 1) & 0x55555555);        // add pairs of bits
        i = (i & 0x33333333) + ((i >> 2) & 0x33333333);  // quads
        i = (i + (i >> 4)) & 0x0F0F0F0F;        // groups of 8
        return (i * 0x01010101) >> 24;          // horizontal sum of bytes
    }

    public static float HammingDistanceBytes(Genome genome1, Genome genome2)
    {
        // Works only for genomes of equal length
        if (genome1.Length != genome2.Length)
            throw new ArgumentException();

        var byteCount = genome1.Select((gene, i) => gene.Equals(genome2[i]) ? 1.0f : 0.0f).Sum();
        return byteCount / genome1.Length;
    }
}