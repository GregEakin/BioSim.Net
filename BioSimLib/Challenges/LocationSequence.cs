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

namespace BioSimLib.Challenges;

[Challenge]
public class LocationSequence : IChallenge
{
    public Challenge Type => Challenge.LocationSequence;

    public LocationSequence()
    {
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var count = 0u;
        var maxNumberOfBits = 32;

        for (var n = 0; n < maxNumberOfBits; ++n)
        {
            var i = 1 << n;
            if (player._challengeBits[i]) ++count;
        }

        return count > 0
            ? (true, count / (float)maxNumberOfBits)
            : (false, 0.0f);
    }
}