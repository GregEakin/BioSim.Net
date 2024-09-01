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

// Maps current X location 0..config.sizeX-1 to sensor range 0.0..1.0
[Sensor]
public class LocationX(Config config) : ISensor
{
    public Sensor Type => Sensor.LOC_X;
    public override string ToString() => "location x";
    public string ShortName => "Lx";

    public float Output(Critter critter, uint simStep)
    {
        return (float)critter.LocX / (config.sizeX - 1u);
    }
}