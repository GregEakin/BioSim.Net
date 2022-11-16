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

namespace BioSimLib.Sensors;

public class SensorFactory
{
    private readonly ISensor?[] _sensors = new ISensor?[Enum.GetNames<Sensor>().Length];

    public ISensor? this[Sensor sensor] => _sensors[(int)sensor];

    public SensorFactory(Config config, Board board)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            if (!type.GetCustomAttributes(false).OfType<SensorAttribute>().Any()) continue;

            var i1 = type.GetConstructor(Array.Empty<Type>());
            if (i1 != null)
            {
                var sensor = (ISensor)i1.Invoke(Array.Empty<object>());
                _sensors[(int)sensor.Type] = sensor;
                continue;
            }

            var i2 = type.GetConstructor(new[] { typeof(Config) });
            if (i2 != null)
            {
                var sensor = (ISensor)i2.Invoke(new object[] { config });
                _sensors[(int)sensor.Type] = sensor;
                continue;
            }

            var i3 = type.GetConstructor(new[] { typeof(Config), typeof(Grid) });
            if (i3 != null)
            {
                var sensor = (ISensor)i3.Invoke(new object[] { config, board.Grid });
                _sensors[(int)sensor.Type] = sensor;
                continue;
            }

            var i4 = type.GetConstructor(new[] { typeof(Grid) });
            if (i4 != null)
            {
                var sensor = (ISensor)i4.Invoke(new object[] { board.Grid });
                _sensors[(int)sensor.Type] = sensor;
                continue;
            }

            var i5 = type.GetConstructor(new[] { typeof(Signals) });
            if (i5 != null)
            {
                var sensor = (ISensor)i5.Invoke(new object[] { board.Signals });
                _sensors[(int)sensor.Type] = sensor;
                continue;
            }

            throw new Exception("Ctor for Sensor not found.");
        }
    }

    public SensorFactory(IEnumerable<ISensor?> sensors)
    {
        foreach (var sensor in sensors)
            if (sensor != null)
                _sensors[(int)sensor.Type] = sensor;
    }
}