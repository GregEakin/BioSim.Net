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

using BioSimLib.Positions;

namespace BioSimLib.Actions;

// Do nothing
[Action]
public class None : IAction
{
    public Action Type => Action.NONE;
    public override string ToString() => "none";
    public string ShortName => "Nop";

    public void Execute(Critter critter, uint simStep, float[] actionLevels)
    {
    }

    public (float dx, float dy) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}