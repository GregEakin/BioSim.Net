﻿//    Copyright 2021 Gregory Eakin
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
public class LongProbePopulationForward : ISensor
{
    private readonly Grid _grid;

    public LongProbePopulationForward(Grid grid)
    {
        _grid = grid;
    }

    public Sensor Type => Sensor.LONGPROBE_POP_FWD;
    public override string ToString() => "long probe population fwd";
    public string ShortName => "LPf";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _grid.LongProbePopulationFwd(player._loc, player.LastMoveDir, player._longProbeDist) / (float)player._longProbeDist;
        return sensorVal;
    }
}