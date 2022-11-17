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

namespace BioSimLib.Actions.Movements;

[Action]
public class MoveX : IMovementAction
{
    public Action Type => Action.MOVE_X;
    public override string ToString() => "move X";
    public string ShortName => "MvX";

    public void Execute(Critter critter, uint simStep, float[] actionLevels)
    {
    }

    public (float dx, float dy) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (actionLevels[(int)Action.MOVE_X], 0.0f);
    }
}