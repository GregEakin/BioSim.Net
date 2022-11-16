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

namespace BioSimLib.Actions.Movements;

[Action]
public class MoveReverse : IMovementAction
{
    public Action Type => Action.MOVE_REVERSE;
    public override string ToString() => "move reverse";
    public string ShortName => "Mrv";

    public void Execute(Config config, Board board, Critter critter, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_REVERSE];
        var offset = lastMoveDir.AsNormalizedCoord();
        return (-offset.X * level, -offset.Y * level);
    }
}