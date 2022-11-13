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

namespace BioSimLib.Sensors;

[Sensor]
public class PopulationLeftRight : ISensor
{
    private readonly Grid _grid;

    public PopulationLeftRight(Grid grid)
    {
        _grid = grid;
    }

    public Sensor Type => Sensor.POPULATION_LR;
    public override string ToString() => "population LR";
    public string ShortName => "Plr";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _grid.GetPopulationDensityAlongAxis(player._loc, player.LastMoveDir.Rotate90DegCw());
        return sensorVal;
    }
}