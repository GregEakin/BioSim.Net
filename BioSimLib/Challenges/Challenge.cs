//    Copyright 2021 Gregory Eakin
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

public enum Challenge
{
    Circle = 0,
    RightHalf = 1,
    RightQuarter = 2,
    Text = 3,
    CenterWeighted = 4,
    CenterUnweighted = 40,
    Corner = 5,
    CornerWeighted = 6,
    MigrateDistance = 7,
    CenterSparse = 8,
    LeftEighth = 9,
    RadioactiveWalls = 10,
    AgainstAnyWall = 11,
    TouchAnyWall = 12,
    EastWestEighths = 14,
    NearBarrier = 14,
    Pairs = 15,
    LocationSequence = 16,
    Altruism = 17,
    AltruismSacrifice = 18,
}