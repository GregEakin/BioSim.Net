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

using BioSimLib.Genes;
using Xunit;

namespace BioSimTests.Genes;

public class GeneBuilderTests
{
    [Fact]
    public void SourceTypeSensorTest()
    {
        var builder = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
        };

        Assert.Equal(Gene.GeneType.Sensor, builder.SourceType);
        Assert.Equal(0x80000000u, builder.ToUint());
    }

    [Fact]
    public void SourceTypeNeuronTest()
    {
        var builder = new GeneBuilder
        {
            SourceType = Gene.GeneType.Neuron,
        };

        Assert.Equal(Gene.GeneType.Neuron, builder.SourceType);
        Assert.Equal(0x00000000u, builder.ToUint());
    }
}