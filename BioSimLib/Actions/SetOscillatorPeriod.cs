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

public class SetOscillatorPeriod : IAction
{
    public Action Type => Action.SET_OSCILLATOR_PERIOD;
    public override string ToString() => "set osc1";
    public string ShortName => "Osc";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
        var period = actionLevels[(int)Action.SET_OSCILLATOR_PERIOD];
        var newPeriodF01 = (float)((Math.Tanh(period) + 1.0f) / 2.0f); 
        var newPeriod = 1u + (uint)(1.5f + Math.Exp(7.0f * newPeriodF01));
        player._oscPeriod = newPeriod;
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}