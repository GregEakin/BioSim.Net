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
public class AgainstAnyWall : IChallenge
{
    private readonly Config _config;
    public Challenge Type => Challenge.AgainstAnyWall;

    public AgainstAnyWall(Config config)
    {
        _config = config;
    }

    public (bool, float) PassedSurvivalCriterion(Critter critter)
    {
        var onEdge = critter.LocX == 0
                     || critter.LocX == _config.sizeX - 1
                     || critter.LocY == 0
                     || critter.LocY == _config.sizeY - 1;

        return onEdge
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}