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
using BioSimLib.Positions;

namespace BioSimLib.Actions;

// Responsiveness action - convert neuron action level from arbitrary float range
// to the range 0.0..1.0. If this action neuron is enabled but not driven, will
// default to mid-level 0.5.
[Action]
public class SetResponsiveness : IAction
{
    public Action Type => Action.SET_RESPONSIVENESS;
    public override string ToString() => "set inv-responsiveness";
    public string ShortName => "Res";

    public void Execute(Config p, Board board, Critter player, uint simStep, float[] actionLevels)
    {
        var level = actionLevels[(int)Action.SET_RESPONSIVENESS];
        level = (float)((Math.Tanh(level) + 1.0) / 2.0);
        player.Responsiveness = level;
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}