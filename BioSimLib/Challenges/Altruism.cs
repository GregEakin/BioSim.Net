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
public class Altruism : IChallenge
{
    private readonly Config _config;
    public Challenge Type => Challenge.Altruism;

    public Altruism(Config config)
    {
        _config = config;
    }

    public (bool, float) PassedSurvivalCriterion(Critter player)
    {
        var safeCenter = new Coord((short)(_config.sizeX / 4.0), (short)(_config.sizeY / 4.0));
        var radius = _config.sizeX / 4.0f;

        var offset = safeCenter - player.Loc;
        var distance = offset.Length();
        return distance <= radius
            ? (true, (radius - distance) / radius)
            : (false, 0.0f);
    }
}