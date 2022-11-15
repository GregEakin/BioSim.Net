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

// Measures the distance to the nearest barrier in the forward
// direction. If non found, returns the maximum sensor value.
// Maps the result to the sensor range 0.0..1.0.
[Sensor]
public class LongProbeBarrierForward : ISensor
{
    private readonly Grid _grid;

    public LongProbeBarrierForward(Grid grid)
    {
        _grid = grid;
    }

    public Sensor Type => Sensor.LONGPROBE_BAR_FWD;
    public override string ToString() => "long probe barrier fwd";
    public string ShortName => "LPb";

    public float Output(Player player, uint simStep)
    {
        var longProbeBarrierFwd = _grid.LongProbeBarrierFwd(player._loc, player.LastMoveDir, player._longProbeDist);
        var sensorVal = longProbeBarrierFwd / player._longProbeDist;
        return sensorVal;
    }
}