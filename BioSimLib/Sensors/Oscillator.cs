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

[Sensor]
public class Oscillator : ISensor
{
    public Sensor Type => Sensor.OSC1;
    public override string ToString() => "oscillator";
    public string ShortName => "Osc";

    public float Output(Player player, uint simStep)
    {
        var phase = simStep % player._oscPeriod / (double)player._oscPeriod;
        var factor = (float)((-Math.Cos(phase * 2.0 * Math.PI) + 1.0) / 2.0);
        var sensorVal = Math.Min(1.0f, Math.Max(0.0f, factor));
        return sensorVal;
    }
}