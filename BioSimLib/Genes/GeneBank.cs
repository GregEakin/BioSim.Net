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
    // private readonly Dictionary<uint[], WeakReference<Genome>> _bank = new();
    
    private int _count;

    public GeneBank(Config p)
    {
        _p = p;
    }

    public IEnumerable<Genome> Startup()
    {
        for (var i = 0u; i < _p.population; i++)
        {
            Genome genome;
            do
            {
                var builder = new GenomeBuilder(_p.genomeMaxLength, _p.maxNumberNeurons);
                genome = builder.ToGenome();
            } while (genome.Length == 0);

            // _bank.Add(i, new WeakReference<Genome>(genome));

            yield return genome;
        }
    }

    public IEnumerable<Genome> FindSurvivors()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Genome> NewGeneration(IEnumerable<Genome> survivors)
    {
        var data = survivors.ToArray();
        if (data.Length <= 0)
        {
            _count = _p.population;

            for (var i = 0u; i < _p.population; i++)
            {
                Genome genome;
                do
                {
                    var builder = new GenomeBuilder(_p.genomeMaxLength, _p.maxNumberNeurons);
                    genome = builder.ToGenome();
                } while (genome.Length == 0);

                // _bank.Add(i, new WeakReference<Genome>(genome));

                yield return genome;
            }
        }
        else
        {
            _count = data.Length;

            for (var i = 0u; i < _p.population; i++)
            {
                var index = i % data.Length;
                var source = data[index];
                var builder = new GenomeBuilder(_p.maxNumberNeurons, source);
                var mutated = builder.Mutate();
                yield return mutated 
                    ? builder.ToGenome() 
                    : source;
            }
        }
    }

    // Genome AddGene(uint[] dna)
    // {
    //     var found = _bank.TryGetValue(dna, out var genomeReference);
    //     if (!found || genomeReference == null)
    //     {
    //         var builder1 = new GenomeBuilder(_p.maxNumberNeurons, dna);
    //         // genome1.Optimize();
    //         _bank.Add(dna, new WeakReference<Genome>(builder1.ToGenome()));
    //         return builder1.ToGenome();
    //     }
    //
    //     var available = genomeReference.TryGetTarget(out var builder3);
    //     if (!available || builder3 == null)
    //     {
    //         builder3 = new GenomeBuilder(_p.maxNumberNeurons, dna).ToGenome();
    //         // genome3.Optimize();
    //         genomeReference.SetTarget(builder3);
    //         return builder3;
    //     }
    //
    //     return builder3;
    // }

    Genome Sex(string mother, string father)
    {
        // Combine the DNA from each parent
        // Perform mutations
        //    Swap genes between the two
        //    Adjust the weights by +- %
        return new GenomeBuilder(_p.maxNumberNeurons, new uint[] { }).ToGenome();
    }

    // public override string ToString()
    // {
    //     var count = 0;
    //     foreach (var pair in _bank)
    //     {
    //         if (pair.Value.TryGetTarget(out var genome))
    //             count++;
    //     }
    //
    //     return count.ToString();
    // }

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
        var aFlags = new bool[al];
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
            return 0.0f;

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
        i = i - ((i >> 1) & 0x55555555); // add pairs of bits
        i = (i & 0x33333333) + ((i >> 2) & 0x33333333); // quads
        i = (i + (i >> 4)) & 0x0F0F0F0F; // groups of 8
        return (i * 0x01010101) >> 24; // horizontal sum of bytes
    }

    public static float HammingDistanceBytes(Genome genome1, Genome genome2)
    {
        // Works only for genomes of equal length
        if (genome1.Length != genome2.Length)
            return 0.0f;

        var byteCount = genome1.Select((gene, i) => gene.Equals(genome2[i]) ? 1.0f : 0.0f).Sum();
        return byteCount / genome1.Length;
    }
}