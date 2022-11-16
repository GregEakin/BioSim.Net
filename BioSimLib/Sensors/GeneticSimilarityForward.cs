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

using BioSimLib.Field;
using BioSimLib.Genes;

namespace BioSimLib.Sensors;

// Return minimum sensor value if nobody is alive in the forward adjacent location,
// else returns a similarity match in the sensor range 0.0..1.0
[Sensor]
public class GeneticSimilarityForward : ISensor
{
    private readonly Config _p;
    private readonly Grid _grid;

    public GeneticSimilarityForward(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public Sensor Type => Sensor.GENETIC_SIM_FWD;
    public override string ToString() => "genetic similarity forward";
    public string ShortName => "Gen";

    public float Output(Critter player, uint simStep)
    {
        var forward = player.Loc + player.LastMoveDir;
        if (!_grid.IsInBounds(forward)) return 0.0f;
        var partner = _grid[forward];
        if (partner is not { Alive: true }) return 0.0f;
        var method = (GeneBank.ComparisonMethods)_p.genomeComparisonMethod;
        var sensorVal = GeneBank.GenomeSimilarity(method, player.Genome, partner.Genome);
        return sensorVal;
    }
}