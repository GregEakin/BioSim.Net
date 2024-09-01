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

using BioSimLib.Field;

namespace BioSimLib.Sensors;

// Sense the nearest barrier along axis perpendicular to last movement direction, mapped
// to sensor range 0.0..1.0
[Sensor]
public class BarrierLeftRight(Config config, Grid grid) : ISensor
{
    public Sensor Type => Sensor.BARRIER_LR;
    public override string ToString() => "short probe barrier left-right";
    public string ShortName => "Blr";

    public float Output(Critter critter, uint simStep)
    {
        var sensorVal = grid.GetShortProbeBarrierDistance(critter.Loc, critter.LastMoveDir.Rotate90DegCw(), config.shortProbeBarrierDistance);
        return sensorVal;
    }
}