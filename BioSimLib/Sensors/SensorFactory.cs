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

namespace BioSimLib.Sensors;

public class SensorFactory
{
    private readonly ISensor?[] _sensors = new ISensor?[Enum.GetNames<Sensor>().Length];

    public ISensor this[Sensor sensor] => _sensors[(int)sensor] ?? new False();

    public SensorFactory(Config p, Board board)
    {
        var sensors = new ISensor[]
        {
            new LocationX(p),
            new LocationY(p),
            new BoundaryDistX(p),
            new BoundaryDist(p),
            new BoundaryDistY(p),
            new GeneticSimilarityForward(p, board.Grid),
            new LastMoveDirX(),
            new LastMoveDirY(),
            new LongProbePopulationForward(board.Grid),
            new LongProbeBarrierForward(board.Grid),
            new Population(p, board.Grid),
            new PopulationForward(board.Grid),
            new PopulationLeftRight(board.Grid),
            new Oscillator(),
            new Age(p),
            new BarrierForward(p, board.Grid),
            new BarrierLeftRight(p, board.Grid),
            new Random(),
            new Signal(board.Signals),
            new SignalFwd(board.Signals),
            new SignalLR(board.Signals),
            new True(),
            new False(),
        };

        foreach (var sensor in sensors)
            _sensors[(int)sensor.Type] = sensor;
    }

    public SensorFactory(IEnumerable<ISensor?> sensors)
    {
        foreach (var sensor in sensors)
            if (sensor != null)
                _sensors[(int)sensor.Type] = sensor;
    }
}