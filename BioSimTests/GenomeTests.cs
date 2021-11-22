using BioSimLib;
using Xunit;

namespace BioSimTests;

public class GenomeTests
{
    [Fact]
    public void Test1()
    {
        var p = new Params
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0x52562f78u,
            0x3c396612u,
            0x4989b501u,
            0x039c5fbdu,
        };

        var genome = new Genome(p, dna);
        var connectionList = genome.MakeRenumberedConnectionList();
    }

    [Fact]
    public void Test2()
    {
        var p = new Params
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0xc8257db1u,
            0x267b4645u,
            0x32470165u,
            0x61596307u,
        };

        var genome = new Genome(p, dna);
        var connectionList = genome.MakeRenumberedConnectionList();
    }

    [Fact]
    public void Test3()
    {
        var p = new Params
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0x28e90f86u,
            0x44bf3514u,
            0x32430165u,
            0xb19a8864u,
        };

        var genome = new Genome(p, dna);
        var connectionList = genome.MakeRenumberedConnectionList();
    }
}