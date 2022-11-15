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

// Finds closest boundary, compares that to the max possible dist
// to a boundary from the center, and converts that linearly to the
// sensor range 0.0..1.0
[Sensor]
public class BoundaryDist : ISensor
{
    private readonly Config _p;

    public BoundaryDist(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.BOUNDARY_DIST;
    public override string ToString() => "boundary dist";
    public string ShortName => "ED";

    public float Output(Player player, uint simStep)
    {
        var distX = Math.Min(player._loc.X, (_p.sizeX - player._loc.X) - 1);
        var distY = Math.Min(player._loc.Y, (_p.sizeY - player._loc.Y) - 1);
        var closest = Math.Min(distX, distY);
        var maxPossible = Math.Max(_p.sizeX / 2 - 1, _p.sizeY / 2 - 1);
        var sensorVal = (float)closest / maxPossible;
        return sensorVal;
    }
}