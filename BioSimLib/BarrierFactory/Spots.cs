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

namespace BioSimLib.BarrierFactory;

[Barrier]
public class Spots(Grid grid) : IBarrierFactory
{
    public int Type => 6;

    public void CreateBarrier()
    {
        var numberOfLocations = 5u;
        var radius = 5.0f;

        var f = (short x, short y) => { grid.SetBarrier(new Coord(x,y)); };

        var verticalSliceSize = grid.SizeY / (numberOfLocations + 1);
        for (var n = 1u; n <= numberOfLocations; ++n)
            grid.VisitNeighborhood((short)(grid.SizeX / 2), (short)(n * verticalSliceSize), radius, f);
    }
}