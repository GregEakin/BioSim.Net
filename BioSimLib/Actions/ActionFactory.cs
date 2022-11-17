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

using System.Reflection;
using BioSimLib.Field;

namespace BioSimLib.Actions;

public class ActionFactory
{
    private readonly IAction?[] _actions = new IAction?[Enum.GetNames<Action>().Length];

    public IAction? this[Action action] => _actions[(int)action];

    public ActionFactory(Config config, Board board)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            if (!type.GetCustomAttributes(false).OfType<ActionAttribute>().Any()) continue;

            var i1 = type.GetConstructor(Array.Empty<Type>());
            if (i1 != null)
            {
                var action = (IAction)i1.Invoke(Array.Empty<object>());
                _actions[(int)action.Type] = action;
                continue;
            }

            var i2 = type.GetConstructor(new[] { typeof(Config) });
            if (i2 != null)
            {
                var action = (IAction)i2.Invoke(new object[] { config });
                _actions[(int)action.Type] = action;
                continue;
            }

            var i3 = type.GetConstructor(new[] { typeof(Board) });
            if (i3 != null)
            {
                var action = (IAction)i3.Invoke(new object[] { board });
                _actions[(int)action.Type] = action;
                continue;
            }

            var i4 = type.GetConstructor(new[] { typeof(Config), typeof(Board) });
            if (i4 != null)
            {
                var action = (IAction)i4.Invoke(new object[] { config, board });
                _actions[(int)action.Type] = action;
                continue;
            }

            throw new Exception("Ctor for Action not found.");
        }
    }

    public ActionFactory(IEnumerable<IAction?> actions)
    {
        foreach (var action in actions)
            if (action != null)
                _actions[(int)action.Type] = action;
    }
}
