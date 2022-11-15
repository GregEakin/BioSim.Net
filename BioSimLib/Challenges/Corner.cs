﻿//    Copyright 2022 Gregory Eakin
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

using BioSimLib.Positions;

namespace BioSimLib.Challenges;

[Challenge]
public class Corner : IChallenge
{
    public Corner(Config p)
    {
        _p = p;
    }

    private readonly Config _p;
    public Challenge Type => Challenge.Corner;

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var radius = _p.sizeX / 8.0f;

        var distance1 = (new Coord(0, 0) - player._loc).Length();
        if (distance1 <= radius)
            return (true, 1.0f);

        var distance2 = (new Coord(0, (short)(_p.sizeY - 1)) - player._loc).Length();
        if (distance2 <= radius)
            return (true, 1.0f);

        var distance3 = (new Coord((short)(_p.sizeX - 1), 0) - player._loc).Length();
        if (distance3 <= radius)
            return (true, 1.0f);

        var distance4 = (new Coord((short)(_p.sizeX - 1), (short)(_p.sizeY - 1)) - player._loc).Length();
        if (distance4 <= radius)
            return (true, 1.0f);

        return (false, 0.0f);
    }
}