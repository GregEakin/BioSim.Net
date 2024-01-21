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

using System.Runtime.InteropServices;
using BioSimLib.Genes;
using Xunit;

namespace BioSimTests.Genes;

public class GeneTests
{
    [Fact]
    public void SizeTest()
    {
        var size = Marshal.SizeOf<Gene>();
        Assert.Equal(4, size);
    }

    [Fact]
    public void SourceTypeSensorTest()
    {
        var builder = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
        };

        var gene = new Gene(builder);
        Assert.Equal(Gene.GeneType.Sensor, gene.SourceType);
        Assert.Equal(0x80000000u, gene.ToUint);
        Assert.Equal("80000000", gene.ToString());
        Assert.Equal("LOC_X N0 0\r\n", gene.ToEdge());
    }

    [Fact]
    public void SourceTypeNeuronTest()
    {
        var builder = new GeneBuilder
        {
            SourceType = Gene.GeneType.Neuron,
        };

        var gene = new Gene(builder);
        Assert.Equal(Gene.GeneType.Neuron, gene.SourceType);
        Assert.Equal(0x00000000u, gene.ToUint);
        Assert.Equal("00000000", gene.ToString());
        Assert.Equal("N0 N0 0\r\n", gene.ToEdge());
    }

    [Fact]
    public void SinkTypeSensorTest()
    {
        var builder = new GeneBuilder
        {
            SinkType = Gene.GeneType.Action,
        };

        var gene = new Gene(builder);
        Assert.Equal(Gene.GeneType.Action, gene.SinkType);
        Assert.Equal(0x00800000u, gene.ToUint);
        Assert.Equal("00800000", gene.ToString());
        Assert.Equal("N0 MOVE_X 0\r\n", gene.ToEdge());
    }

    [Fact]
    public void SinkTypeNeuronTest()
    {
        var builder = new GeneBuilder
        {
            SinkType = Gene.GeneType.Neuron,
        };

        var gene = new Gene(builder);
        Assert.Equal(Gene.GeneType.Neuron, gene.SinkType);
        Assert.Equal(0x00000000u, gene.ToUint);
        Assert.Equal("00000000", gene.ToString());
        Assert.Equal("N0 N0 0\r\n", gene.ToEdge());
    }

    [Fact]
    public void WeightAsFloatTest()
    {
        Assert.Equal(-4.0f, new Gene(0x00008000u).WeightAsFloat);
        Assert.Equal(-3.99987793f, new Gene(0x00008001u).WeightAsFloat);
        Assert.Equal(-1.0f, new Gene(0x0000E000u).WeightAsFloat);
        Assert.Equal(-0.00012207031f, new Gene(0x0000FFFFu).WeightAsFloat);
        Assert.Equal(0.0f, new Gene(0x00000000).WeightAsFloat);
        Assert.Equal(0.00012207031f, new Gene(0x00000001u).WeightAsFloat);
        Assert.Equal(1.0f, new Gene(0x00002000u).WeightAsFloat);
        Assert.Equal(3.999878f, new Gene(0x00007FFFu).WeightAsFloat);
    }

    [Fact]
    public void WeightAsOneTest()
    {
        Assert.Equal(-1.0f, new Gene(0x00008000u).WeightAsOne);
        Assert.Equal(-0.999969482f, new Gene(0x00008001u).WeightAsOne);
        Assert.Equal(-0.25f, new Gene(0x0000E000u).WeightAsOne);
        Assert.Equal(-3.05175781E-05f, new Gene(0x0000FFFFu).WeightAsOne);
        Assert.Equal(0.0f, new Gene(0x00000000).WeightAsOne);
        Assert.Equal(3.05175781E-05f, new Gene(0x00000001u).WeightAsOne);
        Assert.Equal(0.25f, new Gene(0x00002000u).WeightAsOne);
        Assert.Equal(0.999969482f, new Gene(0x00007FFFu).WeightAsOne);
    }

    [Fact]
    public void Test1()
    {
        var builder = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceNum = 127,
            SinkType = Gene.GeneType.Neuron,
            SinkNum = 0,
            Weight = 0x7FFF,
        };

        var gene = new Gene(builder);
        Assert.Equal(0xFF007FFFu, gene.ToUint);
        Assert.Equal("FF007FFF", gene.ToString());
        Assert.Equal("127 N0 32767\r\n", gene.ToEdge());
    }

    [Fact]
    public void Test2()
    {
        var gene = new Gene(0xFFFFFFFF);

        Assert.Equal(Gene.GeneType.Sensor, gene.SourceType);
        Assert.Equal(127, (byte)gene.SourceSensor);
        Assert.Equal(Gene.GeneType.Action, gene.SinkType);
        Assert.Equal(127, gene.SinkNum);
        Assert.Equal(-1, gene.WeightAsShort);

        Assert.Equal("FFFFFFFF", gene.ToString());
        Assert.Equal("127 127 -1\r\n", gene.ToEdge());
    }

    [Fact]
    public void Test3()
    {
        var gene = new Gene(0xFF00FFFFu); // 267419647

        Assert.Equal(Gene.GeneType.Sensor, gene.SourceType);
        Assert.Equal(127, (byte)gene.SourceSensor);
        Assert.Equal(Gene.GeneType.Neuron, gene.SinkType);
        Assert.Equal(0, gene.SinkNum);
        Assert.Equal(-1, gene.WeightAsShort);

        Assert.Equal("FF00FFFF", gene.ToString());
        Assert.Equal("127 N0 -1\r\n", gene.ToEdge());
    }

    [Fact]
    public void FloatZeroTest()
    {
        var gene = new Gene(0x00000000u);

        Assert.Equal(0x00000000u, gene.ToUint);
        Assert.Equal(0.0f, gene.WeightAsFloat);

        Assert.Equal("00000000", gene.ToString());
        Assert.Equal("N0 N0 0\r\n", gene.ToEdge());
    }

    [Fact]
    public void FloatDeltaTest()
    {
        var gene = new Gene(0x00000001u);

        Assert.Equal(0x00000001u, gene.ToUint);
        Assert.Equal(0.00012207031f, gene.WeightAsFloat);

        Assert.Equal("00000001", gene.ToString());
        Assert.Equal("N0 N0 1\r\n", gene.ToEdge());
    }

    [Fact]
    public void FloatMaxTest()
    {
        var gene = new Gene(0x00007FFFu);

        Assert.Equal(0x00007FFFu, gene.ToUint);
        Assert.Equal(3.999878f, gene.WeightAsFloat);

        Assert.Equal("00007FFF", gene.ToString());
        Assert.Equal("N0 N0 32767\r\n", gene.ToEdge());
    }

    [Fact]
    public void FloatLargestNegTest()
    {
        var gene = new Gene(0x00008000u);

        Assert.Equal(0x00008000u, gene.ToUint);
        Assert.Equal(-4.0f, gene.WeightAsFloat);

        Assert.Equal("00008000", gene.ToString());
        Assert.Equal("N0 N0 -32768\r\n", gene.ToEdge());
    }

    [Fact]
    public void FloatNegDeltaTest()
    {
        var gene = new Gene(0x0000FFFFu);

        Assert.Equal(0x0000FFFFu, gene.ToUint);
        Assert.Equal(-0.00012207031f, gene.WeightAsFloat);

        Assert.Equal("0000FFFF", gene.ToString());
        Assert.Equal("N0 N0 -1\r\n", gene.ToEdge());
    }

    [Fact]
    public void FloatTest()
    {
        var builder = new GeneBuilder
        {
            WeightAsFloat = 1.0f
        };

        var gene = new Gene(builder);
        Assert.Equal(0x00002000u, gene.ToUint);
        Assert.Equal(1.0f, gene.WeightAsFloat);
    }

    [Fact]
    public void EqualsTest()
    {
        var gene1 = new Gene(new GeneBuilder { Weight = 0x0000 });
        var gene2 = new Gene(new GeneBuilder { Weight = 0x0007 });

        Assert.True(gene1.Equals(gene2));
    }
}