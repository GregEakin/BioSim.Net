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

[Flags]
public enum Challenge
{
    Circle = 0x0001,
    RightHalf = 0x0002,
    RightQuarter = 0x0004,
    Text = 0x0008,
    CenterWeighted = 0x0010,
    CenterUnweighted = 0x0020,
    Corner = 0x0040,
    CornerWeighted = 0x0080,
    MigrateDistance = 0x0100,
    CenterSparse = 0x0200,
    LeftEighth = 0x0400,
    RadioactiveWalls = 0x0800,
    AgainstAnyWall = 0x1000,
    TouchAnyWall = 0x2000,
    EastWestEighths = 0x4000,
    NearBarrier = 0x8000,
    Pairs = 0x10000,
    LocationSequence = 0x20000,
    Altruism = 0x40000,
    AltruismSacrifice = 0x80000,
}