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

// Emit signal0 - if this action value is below a threshold, nothing emitted.
// Otherwise convert the action value to a probability of emitting one unit of
// signal (pheromone).
// Pheromones may be emitted immediately (see signals.cpp). If this action neuron
// is enabled but not driven, nothing will be emitted.
[Action]
public class EmitSignal0 : IAction
{
    private readonly Board _board;

    public EmitSignal0(Board board)
    {
        _board = board;
    }

    public Action Type => Action.EMIT_SIGNAL0;
    public override string ToString() => "emit signal 0";
    public string ShortName => "SG";

    public void Execute(Critter critter, uint simStep, float[] actionLevels)
    {
        var emitThreshold = 0.5f;
        var actionLevel = actionLevels[(int)Action.EMIT_SIGNAL0];
        var level = (float)((Math.Tanh(actionLevel) + 1.0) / 2.0 * critter.ResponsivenessAdjusted);
        if (level > emitThreshold && critter.Prob2Bool(level))
            _board.Signals.Increment(0, critter.Loc);
    }

    public (float dx, float dy) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}