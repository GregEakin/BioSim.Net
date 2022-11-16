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

namespace BioSimLib.Challenges;

[Challenge]
public class Text : IChallenge
{
    private readonly Grid _grid;
    public Challenge Type => Challenge.Text;

    public Text(Grid grid)
    {
        _grid = grid;
    }

    public (bool, float) PassedSurvivalCriterion(Critter player)
    {
        var minNeighbors = 22u;
        var maxNeighbors = 2u;
        var radius = 1.5f;

        if (_grid.IsBorder(player.Loc))
            return (false, 0.0f);

        var count = 0u;
        var f = (short x, short y) => { if (_grid.IsOccupiedAt(x, y)) ++count; };

        _grid.VisitNeighborhood(player.Loc, radius, f);
        return count >= minNeighbors && count <= maxNeighbors
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}