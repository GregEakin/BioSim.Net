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

using BioSimLib.Field;

namespace BioSimLib.Challenges;

[Challenge]
public class NearBarrier(Config config, Grid grid) : IChallenge
{
    public Challenge Type => Challenge.NearBarrier;

    public (bool passed, float score) PassedSurvivalCriterion(Critter critter)
    {
        var radius = config.sizeX / 2.0f;

        var barrierCenters = grid.GetBarrierCenters();
        var minDistance = 1e8f;
        foreach (var center in barrierCenters)
        {
            float distance = (critter.Loc - center).Length();
            if (distance < minDistance)
                minDistance = distance;
        }

        return minDistance <= radius
            ? (true, 1.0f - minDistance / radius)
            : (false, 0.0f);
    }
}