using System.Runtime.InteropServices;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib;

// Represents a half-precision floating point number. 
// https://gist.github.com/vermorel/1d5c0212752b3e611faf84771ad4ff0d

public class GeneBuilder
{
    public Gene.GeneType SourceType { get; set; } = Gene.GeneType.Neuron;
    public int SourceNum { get; set; }
    public Gene.GeneType SinkType { get; set; } = Gene.GeneType.Neuron;
    public int SinkNum { get; set; }
    public short Weight { get; set; }

    public float WeightAsFloat
    {
        get => Weight / 8192.0f;
        set => Weight = (short)(value * 8192.0f);
    }

    public GeneBuilder() {}

    public GeneBuilder(Gene gene)
    {
        SourceType = gene.SourceType;
        SourceNum = gene.SourceNum;
        SinkType = gene.SinkType;
        SinkNum = gene.SinkNum;
        Weight = gene.WeightAsShort;
    }
}

[StructLayout(LayoutKind.Explicit, Size = 4)]
public readonly struct Gene
{
    public enum GeneType
    {
        Sensor,
        Neuron,
        Action
    }

    [FieldOffset(0)] private readonly byte _source;
    [FieldOffset(1)] private readonly byte _sink;
    [field: FieldOffset(2)] public short WeightAsShort { get; }

    public Gene(uint value)
    {
        _source = (byte)(value >> 24);
        _sink = (byte)((value >> 16) & 0xFF);
        WeightAsShort = (short)(value & 0xFFFF);
    }

    public Gene(GeneBuilder builder)
    {
        _source = (byte)((builder.SourceType == GeneType.Sensor ? 0x80 : 0x00) | (builder.SourceNum & 0x7F));
        _sink = (byte)((builder.SinkType == GeneType.Action ? 0x80 : 0x00) | (builder.SinkNum & 0x7F));
        WeightAsShort = builder.Weight;
    }

    public GeneType SourceType => (_source & 0x80) == 0x80 ? GeneType.Sensor : GeneType.Neuron;

    public Sensor SourceSensor => (Sensor)(_source & 0x7F);

    public byte SourceNum => (byte)(_source & 0x7F);

    public GeneType SinkType => (_sink & 0x80) == 0x80 ? GeneType.Action : GeneType.Neuron;

    public Action SinkNeuron => (Action)(_sink & 0x7F);

    public byte SinkNum => (byte)(_sink & 0x7F);

    public float WeightAsFloat => WeightAsShort / 8192.0f;

    // public short RandomWeight() { return randomUint(0u, 0xefffu) - 0x8000u; }

    public uint ToUint => ((uint)_source << 24) | ((uint)_sink << 16) | (ushort)WeightAsShort;

    public override string ToString() => ToUint.ToString("X8");

    public string ToEdge()
    {
        var builder = new StringBuilder();

        if (SourceType == GeneType.Sensor)
            builder.Append(SourceSensor);
        else
            builder.Append($"N{SourceNum}");

        builder.Append(' ');

        if (SinkType == GeneType.Action)
            builder.Append(SinkNeuron);
        else
            builder.Append($"N{SinkNum}");

        builder.AppendLine($" {WeightAsShort}");

        return builder.ToString();
    }
}