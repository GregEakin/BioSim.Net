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

// Returns population density in neighborhood converted linearly from
// 0..100% to sensor range
[Sensor]
public class Population : ISensor
{
    private readonly Config _p;
    private readonly Grid _grid;

    public Population(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public Sensor Type => Sensor.POPULATION;
    public override string ToString() => "population";
    public string ShortName => "Pop";

    public float Output(Critter player, uint simStep)
    {
        var count = 0u;
        var occupied = 0u;

        void F(short x, short y)
        {
            ++count;
            if (!_grid.IsEmptyAt(x, y))
                ++occupied;
        }

        _grid.VisitNeighborhood(player.Loc, _p.populationSensorRadius, F);
        var sensorVal = (float)occupied / count;
        return sensorVal;
    }
}