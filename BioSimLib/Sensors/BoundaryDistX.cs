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

namespace BioSimLib.Sensors;

// Measures the distance to nearest boundary in the east-west axis,
// max distance is half the grid width; scaled to sensor range 0.0..1.0.
[Sensor]
public class BoundaryDistX : ISensor
{
    private readonly Config _p;

    public BoundaryDistX(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.BOUNDARY_DIST_X;
    public override string ToString() => "boundary dist X";
    public string ShortName => "EDx";

    public float Output(Player player, uint simStep)
    {
        var minDistX = Math.Min(player._loc.X, _p.sizeX - player._loc.X - 1);
        var sensorVal = minDistX / (_p.sizeX / 2.0f);
        return sensorVal;
    }
}