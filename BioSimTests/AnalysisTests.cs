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

using BioSimLib;
using BioSimLib.Genes;
using Xunit;

namespace BioSimTests;

public class AnalysisTests
{
    [Fact]
    public void Test1()
    {
        var config = new Config
        {
            maxNumberNeurons = 5
        };
        var genome = new GenomeBuilder(config.maxNumberNeurons, new []{ 0x840B7FFFu }).ToGenome();
        var net = new NeuralNet(genome);
        Assert.Equal("840B7FFF", net.ToString());
    }
}