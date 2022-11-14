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
public class SignalLR : ISensor
{
    private readonly Signals _signals;

    public SignalLR(Signals signals)
    {
        _signals = signals;
    }

    public Sensor Type => Sensor.SIGNAL0_LR;
    public override string ToString() => "signal 0 LR";
    public string ShortName => "Slr";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _signals.GetSignalDensityAlongAxis(0u, player._loc, player.LastMoveDir.Rotate90DegCw());
        return sensorVal;
    }
}