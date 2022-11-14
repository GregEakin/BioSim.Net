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

namespace BioSimLib.BarrierFactory;

[Barrier]
public class VerticalBarConstantLocation : IBarrierFactory
{
    private readonly Grid _grid;

    public VerticalBarConstantLocation(Grid grid)
    {
        _grid = grid;
    }

    public int Type => 1;

    public void CreateBarrier()
    {
        var minX = (short)(_grid.SizeX / 2);
        var maxX = (short)(minX + 1);
        var minY = (short)(_grid.SizeY / 4);
        var maxY = (short)(minY + _grid.SizeY / 2);

        for (var x = minX; x <= maxX; ++x)
        for (var y = minY; y <= maxY; ++y)
            _grid.SetBarrier(x, y);
    }
}