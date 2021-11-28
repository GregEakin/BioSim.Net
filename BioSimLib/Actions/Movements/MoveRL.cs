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

namespace BioSimLib.Actions.Movements;

public class MoveRL : IMovementAction
{
    public Action Type => Action.MOVE_RL;
    public override string ToString() => "move R-L";
    public string ShortName => "MRL";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_RL];
        var offset = lastMoveDir.Rotate90DegCw().AsNormalizedCoord();
        level = 2.0f * (level - 0.5f);
        return (offset.X * level, offset.Y * level);
    }
}