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

namespace BioSimLib.BarrierFactory;

public class BarrierFactory
{
    private readonly IBarrierFactory?[] _factories = new IBarrierFactory?[7];
    public IBarrierFactory? this[int index] => _factories[index];

    public BarrierFactory(Board board) : this(board.Grid)
    {
    }

    public BarrierFactory(Grid grid)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            if (!type.GetCustomAttributes(false).OfType<BarrierAttribute>().Any()) continue;

            var i1 = type.GetConstructor([]);
            if (i1 != null)
            {
                var factory = (IBarrierFactory)i1.Invoke([]);
                _factories[factory.Type] = factory;
                continue;
            }

            var i2 = type.GetConstructor([typeof(Grid)]);
            if (i2 != null)
            {
                var factory = (IBarrierFactory)i2.Invoke([grid]);
                _factories[factory.Type] = factory;
                continue;
            }

            throw new Exception("Ctor for Barrier Factory not found.");
        }
    }

    public BarrierFactory(IEnumerable<IBarrierFactory?> factories)
    {
        foreach (var factory in factories)
            if (factory != null)
                _factories[factory.Type] = factory;
    }
}