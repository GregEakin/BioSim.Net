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

using BioSimLib.Actions.Movements;

namespace BioSimLib.Actions;

public class ActionFactory
{
    private readonly IAction?[] _actions = new IAction?[Enum.GetNames<Action>().Length];

    public IAction? this[Action action] => _actions[(int)action];

    private static readonly IAction[] Actions = new IAction[]
    {
        new EmitSignal(),
        new KillForward(),
        new MoveEast(),
        new MoveForward(),
        new MoveLeft(),
        new MoveNorth(),
        new MoveRandom(),
        new MoveReverse(),
        new MoveRight(),
        new MoveRL(),
        new MoveSouth(),
        new MoveWest(),
        new MoveX(),
        new MoveY(),
        new None(),
        new Procreate(),
        new SetLongProbeDist(),
        new SetOscillatorPeriod(),
        new SetResponsiveness(),
        new Suicide(),
    };

    public ActionFactory()
    {
        foreach (var action in Actions)
            _actions[(int)action.Type] = action;
    }

    public ActionFactory(IEnumerable<IAction?> actions)
    {
        foreach (var action in actions)
            if (action != null)
                _actions[(int)action.Type] = action;
    }
}