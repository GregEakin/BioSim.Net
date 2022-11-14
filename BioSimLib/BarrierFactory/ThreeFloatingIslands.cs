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
public class ThreeFloatingIslands : IBarrierFactory
{
    private readonly Random _random = new();
    private readonly Grid _grid;

    public ThreeFloatingIslands(Grid grid)
    {
        _grid = grid;
    }

    public int Type => 5;

    public void CreateBarrier()
    {
        var radius = 3.0f;
        var margin = 2 * (int)radius;

        Coord RandomLoc()
        {
            return new Coord((short)_random.Next(margin, _grid.SizeX - margin),
                (short)_random.Next(margin, _grid.SizeY - margin));
        }

        Coord center0 = RandomLoc();
        // Coord center1;
        // Coord center2;

        // do
        // {
        //     center1 = RandomLoc();
        // } while ((center0 - center1).Length() < margin);

        // do
        // {
        //     center2 = RandomLoc();
        // } while ((center0 - center2).Length() < margin || (center1 - center2).Length() < margin);

        var f = (short x, short y) => { _grid.SetBarrier(new Coord(x, y)); };

        _grid.VisitNeighborhood(center0, radius, f);
        //visitNeighborhood(center1, radius, f);
        //visitNeighborhood(center2, radius, f);
    }
}