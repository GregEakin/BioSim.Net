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

public class GenomeTests
{
    [Fact]
    public void Test1()
    {
        var p = new Config
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

        var genome = new Genome(p, dna);
        var connectionList = genome.MakeRenumberedConnectionList();
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

        var genome = new Genome(p, dna);
        var connectionList = genome.MakeRenumberedConnectionList();
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

        var genome = new Genome(p, dna);
        var data = genome.MakeNodeList();
        Assert.Single(data);
    }

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

        var genome = new Genome(p, dna);
        var data = genome.ToGraphInfo();
        Assert.Equal("N0 SET_OSCILLATOR_PERIOD 3974\r\n\r\nN0 MOVE_RL 13588\r\n\r\nN0 N0 357\r\n\r\nBOUNDARY_DIST SET_LONGPROBE_DIST -30620\r\n\r\n", data);
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

        var genome = new Genome(p, dna);
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

        var genome = new Genome(p, dna);
        var data = genome.ToString();
        Assert.Equal("N0 * 0.48510742 => SET_OSCILLATOR_PERIOD, N0 * 1.6586914 => MOVE_RL, N0 * 0.0435791 => N0, BOUNDARY_DIST * -3.737793 => SET_LONGPROBE_DIST", data);
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

        var genome = new Genome(p, dna);
        var color = genome.Color;
        Assert.Equal(0x21, color.Item1);
        Assert.Equal(0x51, color.Item2);
        Assert.Equal(0x80, color.Item3);
    }
}