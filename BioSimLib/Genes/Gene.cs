﻿//    Copyright 2022 Gregory Eakin
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
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib.Genes;

// Represents a half-precision floating point number. 
// https://gist.github.com/vermorel/1d5c0212752b3e611faf84771ad4ff0d

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

    public Action SinkAction => (Action)(_sink & 0x7F);

    public byte SinkNum => (byte)(_sink & 0x7F);

    public float WeightAsFloat => WeightAsShort / 8192.0f;
    public float WeightAsOne => WeightAsShort / 32768.0f;

    public uint ToUint => ((uint)_source << 24) | ((uint)_sink << 16) | (ushort)WeightAsShort;

    public override string ToString() => ToUint.ToString("X8");

    public override bool Equals(object? obj) => obj is Gene other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_source, _sink, WeightAsShort);

    public bool Equals(Gene g2) => _source == g2._source
                                   && _sink == g2._sink
                                   && Math.Abs(WeightAsShort - g2.WeightAsShort) < 8;

    public static bool operator ==(Gene g1, Gene g2) => g1.Equals(g2);

    public static bool operator !=(Gene g1, Gene g2) => !g1.Equals(g2);

    public string ToEdge()
    {
        var builder = new StringBuilder();

        if (SourceType == GeneType.Sensor)
            builder.Append(SourceSensor);
        else
            builder.Append($"N{SourceNum}");

        builder.Append(' ');

        if (SinkType == GeneType.Action)
            builder.Append(SinkAction);
        else
            builder.Append($"N{SinkNum}");

        builder.AppendLine($" {WeightAsShort}");

        return builder.ToString();
    }
}