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

// Returns a random sensor value in the range 0.0..1.0.
[Sensor]
public class Random : ISensor
{
    private readonly System.Random _random = new();

    public Sensor Type => Sensor.RANDOM;
    public override string ToString() => "random";
    public string ShortName => "Rnd";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _random.NextSingle();
        return sensorVal;
    }
}