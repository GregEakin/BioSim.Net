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

namespace BioSimLib.Genes;

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

    public uint AsUint()
    {
        throw new NotImplementedException();
    }
}