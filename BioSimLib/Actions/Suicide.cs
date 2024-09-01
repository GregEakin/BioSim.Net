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
using BioSimLib.Positions;

namespace BioSimLib.Actions;

[Action]
public class Suicide(Board board) : IAction
{
    public Action Type => Action.SUICIDE;
    public override string ToString() => "suicide";
    public string ShortName => "Die";

    public void Execute(Critter critter, uint simStep, float[] actionLevels)
    {
        var dieThreshold = 0.5f;
        var level = actionLevels[(int)Action.SUICIDE];
        level = (float)((Math.Tanh(level) + 1.0) / 2.0);
        level *= critter.ResponsivenessAdjusted;
        if (level <= dieThreshold || !critter.Prob2Bool(level))
            return;

        board.Critters.QueueForDeath(critter);
    }

    public (float dx, float dy) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}