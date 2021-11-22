using System.Runtime.InteropServices;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public class Gene
{
    public enum GeneType
    {
        Sensor,
        Neuron,
        Action
    }

    [FieldOffset(0)] private byte _source;
    [FieldOffset(1)] private byte _sink;
    [field: FieldOffset(2)] public short WeightAsShort { get; set; }

    public GeneType SourceType
    {
        get => (_source & 0x80) == 0x80 ? GeneType.Sensor : GeneType.Neuron;
        set
        {
            switch (value)
            {
                case GeneType.Sensor:
                    _source |= 0x80;
                    break;
                case GeneType.Neuron:
                    _source &= 0x7F;
                    break;
                case GeneType.Action:
                default:
                    throw new ArgumentException("Can't have Action gene type for a source.");
            }
        }
    }

    public Sensor SourceNeuron
    {
        get => (Sensor)(_source & 0x7F);
        set => _source = (byte)((_source & 0x80) | ((byte)value & 0x7F));
    }

    public byte SourceNum
    {
        get => (byte)(_source & 0x7F);
        set => _source = (byte)((_source & 0x80) | (value & 0x7F));
    }

    public GeneType SinkType
    {
        get => (_sink & 0x80) == 0x80 ? GeneType.Neuron : GeneType.Action;
        set
        {
            switch (value)
            {
                case GeneType.Neuron:
                    _sink |= 0x80;
                    break;
                case GeneType.Action:
                    _sink &= 0x7F;
                    break;
                case GeneType.Sensor:
                default:
                    throw new ArgumentException("Can't have Sensor gene type for a sink.");
            }
        }
    }

    public Action SinkNeuron
    {
        get => (Action)(_sink & 0x7F);
        set => _sink = (byte)((_sink & 0x80) | ((byte)value & 0x7F));
    }

    public byte SinkNum
    {
        get => (byte)(_sink & 0x7F);
        set => _sink = (byte)((_sink & 0x80) | (value & 0x7F));
    }

    public float WeightAsFloat
    {
        get => WeightAsShort / 8192.0f;
        set => WeightAsShort = (short)(value * 8192.0f);
    }

    // public short RandomWeight() { return randomUint(0u, 0xefffu) - 0x8000u; }

    public uint ToUint
    {
        get => ((uint)_source << 24) | ((uint)_sink << 16) | (ushort)WeightAsShort;
        set
        {
            _source = (byte)(value >> 24);
            _sink = (byte)((value >> 16) & 0xFF);
            WeightAsShort = (short)(value & 0xFFFF);
        }
    }

    public override string ToString() => ToUint.ToString("X8");

    public string ToEdge()
    {
        var builder = new StringBuilder();

        if (SourceType == Gene.GeneType.Sensor)
            builder.Append(SourceNeuron);
        else
            builder.Append($"N{SourceNum}");

        builder.Append(' ');

        if (SinkType == Gene.GeneType.Action)
            builder.Append(SinkNeuron);
        else
            builder.Append($"N{SinkNum}");

        builder.AppendLine($" {WeightAsShort}");

        return builder.ToString();
    }
}