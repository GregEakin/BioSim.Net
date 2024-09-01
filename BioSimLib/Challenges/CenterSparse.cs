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
public class CenterSparse(Config config, Grid grid) : IChallenge
{
    public Challenge Type => Challenge.CenterSparse;

    public (bool passed, float score) PassedSurvivalCriterion(Critter critter)
    {
        var safeCenter = new Coord((short)(config.sizeX / 2.0), (short)(config.sizeY / 2.0));
        var outerRadius = config.sizeX / 4.0f;
        var innerRadius = 1.5f;
        var minNeighbors = 5u; // includes self
        var maxNeighbors = 8u;

        var offset = safeCenter - critter.Loc;
        var distance = offset.Length();
        if (!(distance <= outerRadius)) return (false, 0.0f);
        var count = 0f;
        var f = (short x, short y) =>
        {
            if (grid.IsOccupiedAt(x, y)) ++count;
        };

        grid.VisitNeighborhood(critter.Loc, innerRadius, f);
        if (count >= minNeighbors && count <= maxNeighbors)
            return (true, 1.0f);
        return (false, 0.0f);
    }
}