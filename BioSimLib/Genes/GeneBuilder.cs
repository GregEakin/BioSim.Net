// Copyright 2022 Gregory Eakin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//       http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib.Genes;

public class GeneBuilder
{
    public GeneBuilder()
    {
    }

    public GeneBuilder(uint dna)
    {
        SourceType = (dna & 0x80000000u) == 0x80000000u ? Gene.GeneType.Sensor : Gene.GeneType.Neuron;
        SourceNum = (byte)((dna & 0x7F000000u) >> 24);

        SinkType = (dna & 0x00800000) == 0x00800000u ? Gene.GeneType.Action : Gene.GeneType.Neuron;
        SinkNum = (byte)((dna & 0x007F0000u) >> 16);

        Weight = (short)(dna & 0x0000FFFFu);
    }

    public GeneBuilder(Gene gene)
    {
        SourceType = gene.SourceType;
        SourceNum = gene.SourceNum;
        SinkType = gene.SinkType;
        SinkNum = gene.SinkNum;
        Weight = gene.WeightAsShort;
    }

    public Gene.GeneType SourceType { get; set; } = Gene.GeneType.Neuron;
    public byte SourceNum { get; set; }
    public Gene.GeneType SinkType { get; set; } = Gene.GeneType.Neuron;
    public byte SinkNum { get; set; }
    public short Weight { get; set; }

    public Sensor SourceSensor
    {
        get => (Sensor)(SourceNum & 0x7F);
        set => SourceNum = (byte)value;
    }

    public Action SinkAction
    {
        get => (Action)(SinkNum & 0x7F);
        set => SinkNum = (byte)value;
    }

    public float WeightAsFloat
    {
        get => Weight / 8192.0f;
        set => Weight = (short)(value * 8192.0f);
    }

    public byte Color => (byte)(((SourceNum & 0x03) << 6)
                                | ((SinkNum & 0x03) << 4)
                                | ((Weight & 0x7800) >> 11));

    public Gene ToGene()
    {
        return new Gene(this);
    }

    public uint ToUint()
    {
        var dna = (SourceType == Gene.GeneType.Sensor ? 0x80000000u : 0x00000000u)
                  | ((SourceNum & 0x7Fu) << 24)
                  | (SinkType == Gene.GeneType.Action ? 0x00800000u : 0x00000000u)
                  | ((SinkNum & 0x7Fu) << 16)
                  | (ushort)Weight;
        return dna;
    }
}