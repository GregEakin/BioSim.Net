﻿//    Copyright 2022 Gregory Eakin
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

using BioSimLib.Positions;

namespace BioSimLib.Actions;

// Set longProbeDistance - convert action level to 1..maxLongProbeDistance.
// If this action neuron is enabled but not driven, will default to
// mid-level period of 17 simSteps.
[Action]
public class SetLongProbeDist : IAction
{
    public Action Type => Action.SET_LONGPROBE_DIST;
    public override string ToString() => "set longprobe dist";
    public string ShortName => "LPD";

    public void Execute(Critter critter, uint simStep, float[] actionLevels)
    {
        var maxLongProbeDistance = 32u;
        var level = actionLevels[(int)Action.SET_LONGPROBE_DIST];
        level = (float)((Math.Tanh(level) + 1.0) / 2.0);
        level = 1.0f + level * maxLongProbeDistance;
        critter.LongProbeDist = (uint)level;
    }

    public (float dx, float dy) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}