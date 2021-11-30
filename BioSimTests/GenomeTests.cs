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

using BioSimLib;
using BioSimLib.Genes;
using Xunit;

namespace BioSimTests;

public class GenomeBuilderTests
{
    [Fact]
    public void Test1()
    {
        var p = new Config
        {
            genomeMaxLength = 24,
            maxNumberNeurons = 12,
        };
        var dna = new uint[]
        {
            0x2649045B,
            0x514CA58A,
            0x7ACBe677,
            0x6A2212E0,
            0x3399EEB5,
            0x1B453CDE,
            0x5BCFBE75,
            0x4766fAED,
            0x9018129F,
            0x8BA265EB,
            0x677380A7,
            0xBE70A342,
            0x0C745DD6,
            0x72BD0662,
            0x6AB20437,
            0x8AFD008C,
            0x72FE8B81,
            0x13EF5141,
            0x29F58628,
            0x5E23fA09,
            0x7E4495DD,
            0x77C3AfA5,
            0x4E292DDC,
            0x87F32F07,
        };

        var builder = new GenomeBuilder(p.maxNumberNeurons, dna);
        var genome = builder.ToGenome();
        // var connectionList = genome.MakeRenumberedConnectionList();
    }

    [Fact]
    public void Test2()
    {
        var p = new Config
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

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna);
        // ar connectionList = genome.MakeRenumberedConnectionList();
    }

    [Fact]
    public void Test3()
    {
        var p = new Config
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

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna);
        // var connectionList = genome.MakeRenumberedConnectionList();
    }

    [Fact]
    public void MakeNodeTest()
    {
        var p = new Config
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0x00850f86u,
            0x00833514u,
            0x00000165u,
            0x83868864u,
        };

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna);
        // var data = genome.MakeNodeList();
        // Assert.Single(data);
    }

    [Fact]
    public void GeneticColorTest()
    {
        var p = new Config
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

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna);
        var color = genome.Color;
        Assert.Equal(0xFF, color.Item1);
        Assert.Equal(0xFF, color.Item2);
        Assert.Equal(0xFF, color.Item3);
    }
}

public class GenomeTests
{
    [Fact]
    public void ToGraphInfoTest()
    {
        var p = new Config
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0x00850f86u,
            0x00833514u,
            0x00000165u,
            0x83868864u,
        };

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna).ToGenome();
        var data = genome.ToGraphInfo();
        Assert.Equal("N0 N0 357\r\n\r\nN0 MOVE_RL 13588\r\n\r\nN0 SET_OSCILLATOR_PERIOD 3974\r\n\r\nBOUNDARY_DIST SET_LONGPROBE_DIST -30620\r\n\r\n", data);
    }

    [Fact]
    public void ToDnaTest()
    {
        var p = new Config
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0x00850f86u,
            0x00833514u,
            0x00000165u,
            0x83868864u,
        };

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna).ToGenome();
        var data = genome.ToDna();
        Assert.Equal("00850F86 00833514 00000165 83868864", data);
    }

    [Fact]
    public void ToStringTest()
    {
        var p = new Config
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0x00850f86u,
            0x00833514u,
            0x00000165u,
            0x83868864u,
        };

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna);
        var data = genome.ToGenome().ToString();
        Assert.Equal("N0 * 0.0435791 => N0, N0 * 1.6586914 => MOVE_RL, N0 * 0.48510742 => SET_OSCILLATOR_PERIOD, BOUNDARY_DIST * -3.737793 => SET_LONGPROBE_DIST", data);
    }
}