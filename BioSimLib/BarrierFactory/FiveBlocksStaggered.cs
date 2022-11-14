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
public class FiveBlocksStaggered : IBarrierFactory
{
    private readonly Grid _grid;

    public FiveBlocksStaggered(Grid grid)
    {
        _grid = grid;
    }

    public int Type => 3;

    public void CreateBarrier()
    {
        var blockSizeX = 2;
        var blockSizeY = _grid.SizeX / 3;

        var x0 = (short)(_grid.SizeX / 4 - blockSizeX / 2);
        var y0 = (short)(_grid.SizeY / 4 - blockSizeY / 2);
        var x1 = (short)(x0 + blockSizeX);
        var y1 = (short)(y0 + blockSizeY);

        DrawBox(x0, y0, x1, y1);
        x0 += (short)(_grid.SizeX / 2);
        x1 = (short)(x0 + blockSizeX);
        DrawBox(x0, y0, x1, y1);
        y0 += (short)(_grid.SizeY / 2);
        y1 = (short)(y0 + blockSizeY);
        DrawBox(x0, y0, x1, y1);
        x0 -= (short)(_grid.SizeX / 2);
        x1 = (short)(x0 + blockSizeX);
        DrawBox(x0, y0, x1, y1);
        x0 = (short)(_grid.SizeX / 2 - blockSizeX / 2);
        x1 = (short)(x0 + blockSizeX);
        y0 = (short)(_grid.SizeY / 2 - blockSizeY / 2);
        y1 = (short)(y0 + blockSizeY);
        DrawBox(x0, y0, x1, y1);
    }

    private void DrawBox(short minX, short minY, short maxX, short maxY)
    {
        for (var x = minX; x <= maxX; ++x)
        for (var y = minY; y <= maxY; ++y)
            _grid.SetBarrier(x, y);
    }
}