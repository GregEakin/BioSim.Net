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

public class EmitSignal : IAction
{
    public Action Type => Action.EMIT_SIGNAL0;
    public override string ToString() => "emit signal 0";
    public string ShortName => "SG";
    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
        var emitThreshold = 0.5f;
        var level = actionLevels[(int)Action.EMIT_SIGNAL0];
        level = (float)((Math.Tanh(level) + 1.0) / 2.0);
        level *= player.ResponsivenessAdjusted;
        if (level > emitThreshold && Player.Prob2Bool(level)) 
            board.Signals.Increment(0, player._loc);
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}