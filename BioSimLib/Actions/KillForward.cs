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

// Kill forward -- if this action value is > threshold, value is converted to probability
// of an attempted murder. Probabilities under the threshold are considered 0.0.
// If this action neuron is enabled but not driven, the neighbors are safe.
[Action]
public class KillForward : IAction
{
    private readonly Board _board;

    public KillForward(Board board)
    {
        _board = board;
    }

    public Action Type => Action.KILL_FORWARD;
    public override string ToString() => "kill fwd";
    public string ShortName => "KlF";

    public void Execute(Critter critter, uint simStep, float[] actionLevels)
    {
        var killThreshold = 0.5f;
        var actionLevel = actionLevels[(int)Action.KILL_FORWARD];
        var level = (float)(((Math.Tanh(actionLevel) + 1.0) / 2.0) * critter.ResponsivenessAdjusted);
        if (level <= killThreshold || !critter.Prob2Bool(level))
            return;

        var otherLoc = critter.Loc + critter.LastMoveDir;
        if (!_board.Grid.IsInBounds(otherLoc) || !_board.Grid.IsOccupiedAt(otherLoc))
            return;

        var critter2 = _board.Grid[otherLoc];
        if (critter2 == null)
            return;

        if (critter2.Alive)
            _board.Critters.QueueForDeath(critter2);
    }

    public (float dx, float dy) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}