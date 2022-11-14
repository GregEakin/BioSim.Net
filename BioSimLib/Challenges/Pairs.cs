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

namespace BioSimLib.Challenges;

[Challenge]
public class Pairs : IChallenge
{
    private readonly Config _p;
    private readonly Grid _grid;

    public Challenge Type => Challenge.Pairs;

    public Pairs(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var onEdge = player._loc.X == 0
                     || player._loc.X == _p.sizeX - 1
                     || player._loc.Y == 0
                     || player._loc.Y == _p.sizeY - 1;

        if (onEdge)
            return (false, 0.0f);

        var count = 0u;
        for (var x = (short)(player._loc.X - 1); x <= player._loc.X + 1; ++x)
        for (var y = (short)(player._loc.Y - 1); y <= player._loc.Y + 1; ++y)
        {
            var tloc = new Coord(x, y);
            if (tloc == player._loc || !_grid.IsInBounds(tloc) || !_grid.IsOccupiedAt(tloc)) continue;

            ++count;
            if (count != 1)
                return (false, 0.0f);

            for (var x1 = (short)(tloc.X - 1); x1 <= tloc.X + 1; ++x1)
            for (var y1 = (short)(tloc.Y - 1); y1 <= tloc.Y + 1; ++y1)
            {
                var tloc1 = new Coord(x1, y1);
                if (tloc1 != tloc
                    && tloc1 != player._loc
                    && _grid.IsInBounds(tloc1)
                    && _grid.IsOccupiedAt(tloc1))
                    return (false, 0.0f);
            }
        }

        return count == 1
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}