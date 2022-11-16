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

// Oscillator period action - convert action level nonlinearly to
// 2..4*config.stepsPerGeneration. If this action neuron is enabled but not driven,
// will default to 1.5 + e^(3.5) = a period of 34 simSteps.
[Action]
public class SetOscillatorPeriod : IAction
{
    public Action Type => Action.SET_OSCILLATOR_PERIOD;
    public override string ToString() => "set osc1";
    public string ShortName => "Osc";

    public void Execute(Config config, Board board, Critter critter, uint simStep, float[] actionLevels)
    {
        foreach (var level1 in actionLevels)
            if (float.IsNaN(level1))
                break;

        var level = actionLevels[(int)Action.SET_OSCILLATOR_PERIOD];
        var newPeriodF01 = (float)((Math.Tanh(level) + 1.0f) / 2.0f);
        var newPeriod = 1u + (uint)(1.5f + Math.Exp(7.0f * newPeriodF01));
        critter.OscPeriod = newPeriod;

        foreach (var level1 in actionLevels)
            if (float.IsNaN(level1))
                break;
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}