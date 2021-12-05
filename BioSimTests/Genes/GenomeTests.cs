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

namespace BioSimTests.Genes;

public class GenomeTests
{
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

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna).ToGenome();
        var data = genome.ToGraphInfo();
        Assert.Equal("N0 N0 357\r\n\r\nN0 MOVE_RL 13588\r\n\r\nN0 SET_OSCILLATOR_PERIOD 3974\r\n\r\nBOUNDARY_DIST SET_LONGPROBE_DIST -30620\r\n\r\n", data);
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

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna).ToGenome();
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

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna);
        var data = genome.ToGenome().ToString();
        Assert.Equal("N0 * 0.0435791 => N0, N0 * 1.6586914 => MOVE_RL, N0 * 0.48510742 => SET_OSCILLATOR_PERIOD, BOUNDARY_DIST * -3.737793 => SET_LONGPROBE_DIST", data);
    }
}