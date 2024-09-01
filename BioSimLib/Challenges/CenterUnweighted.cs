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

using BioSimLib.Positions;

namespace BioSimLib.Challenges;

[Challenge]
public class CenterUnweighted(Config config) : IChallenge
{
    public Challenge Type => Challenge.CenterUnweighted;

    public (bool passed, float score) PassedSurvivalCriterion(Critter critter)
    {
        var safeCenter = new Coord((short)(config.sizeX / 2.0), (short)(config.sizeY / 2.0));
        var radius = config.sizeX / 3.0f;

        var offset = safeCenter - critter.Loc;
        var distance = offset.Length();
        return distance <= radius
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}