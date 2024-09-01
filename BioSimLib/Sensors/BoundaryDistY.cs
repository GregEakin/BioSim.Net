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

namespace BioSimLib.Sensors;

// Measures the distance to nearest boundary in the south-north axis,
// max distance is half the grid height; scaled to sensor range 0.0..1.0.
[Sensor]
public class BoundaryDistY(Config config) : ISensor
{
    public Sensor Type => Sensor.BOUNDARY_DIST_Y;
    public override string ToString() => "boundary dist Y";
    public string ShortName => "EDy";

    public float Output(Critter critter, uint simStep)
    {
        var minDistY = Math.Min(critter.LocY, config.sizeY - critter.LocY - 1);
        var sensorVal = minDistY / (config.sizeY / 2.0f);
        return sensorVal;
    }
}