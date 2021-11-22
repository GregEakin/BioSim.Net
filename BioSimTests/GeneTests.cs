using System.Runtime.InteropServices;
using BioSimLib;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests;

public class GeneTests
{
    [Fact]
    public void SizeTest()
    {
        var size = Marshal.SizeOf<Gene>();
        Assert.Equal(4, size);
    }

    [Fact]
    public void Test1()
    {
        var gene = new Gene
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = (Sensor)127,
            SinkType = Gene.GeneType.Neuron,
            SinkNeuron = 0,
            WeightAsShort = 0x7FFF
        };

        Assert.Equal(0xFF007FFFu, gene.ToUint);
    }

    [Fact]
    public void Test2()
    {
        var gene = new Gene
        {
            ToUint = 0xFFFFFFFF
        };

        Assert.Equal(Gene.GeneType.Sensor, gene.SourceType);
        Assert.Equal(127, (byte)gene.SourceSensor);
        Assert.Equal(Gene.GeneType.Action, gene.SinkType);
        Assert.Equal(127, gene.SinkNum);
        Assert.Equal(-1, gene.WeightAsShort);
    }

    [Fact]
    public void Test3()
    {
        var gene = new Gene
        {
            ToUint = 0xFF00FFFFu  // 267419647
        };

        Assert.Equal(Gene.GeneType.Sensor, gene.SourceType);
        Assert.Equal(127, (byte)gene.SourceSensor);
        Assert.Equal(Gene.GeneType.Neuron, gene.SinkType);
        Assert.Equal(0, gene.SinkNum);
        Assert.Equal(-1, gene.WeightAsShort);
    }

    [Fact]
    public void FloatZeroTest()
    {
        var gene = new Gene
        {
            ToUint = 0x00000000u
        };

        Assert.Equal(0x00000000u, gene.ToUint);
        Assert.Equal(0.0f, gene.WeightAsFloat);
    }

    [Fact]
    public void FloatDeltaTest()
    {
        var gene = new Gene
        {
            ToUint = 0x00000001u
        };

        Assert.Equal(0x00000001u, gene.ToUint);
        Assert.Equal(0.00012207031f, gene.WeightAsFloat);
    }

    [Fact]
    public void FloatMaxTest()
    {
        var gene = new Gene
        {
            ToUint = 0x00007FFFu
        };

        Assert.Equal(0x00007FFFu, gene.ToUint);
        Assert.Equal(3.999878f, gene.WeightAsFloat);
    }

    [Fact]
    public void FloatNegDeltaTest()
    {
        var gene = new Gene
        {
            ToUint = 0x00008000u
        };

        Assert.Equal(0x00008000u, gene.ToUint);
        Assert.Equal(-4.0f, gene.WeightAsFloat);
    }

    [Fact]
    public void FloatMinTest()
    {
        var gene = new Gene
        {
            ToUint = 0x0000FFFFu
        };

        Assert.Equal(0x0000FFFFu, gene.ToUint);
        Assert.Equal(-0.00012207031f, gene.WeightAsFloat);
    }

    [Fact]
    public void FloatTest()
    {
        var gene = new Gene
        {
            WeightAsFloat = 1.0f
        };

        Assert.Equal(0x00002000u, gene.ToUint);
        Assert.Equal(1.0f, gene.WeightAsFloat);
    }

}