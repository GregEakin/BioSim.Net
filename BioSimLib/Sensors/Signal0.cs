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

// Returns magnitude of signal0 in the local neighborhood, with
// 0.0..maxSignalSum converted to sensorRange 0.0..1.0
[Sensor]
public class Signal0(Signals signals) : ISensor
{
    public Sensor Type => Sensor.SIGNAL0;
    public override string ToString() => "signal 0";
    public string ShortName => "Sg";

    public float Output(Critter critter, uint simStep)
    {
        var sensorVal = signals.GetSignalDensity(0u, critter.Loc);
        return sensorVal;
    }
}