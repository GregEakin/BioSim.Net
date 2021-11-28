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

using BioSimLib.Positions;

namespace BioSimLib.Field;

public class Barriers
{
    private readonly Barrier?[] _barriers = Array.Empty<Barrier?>();
    private ushort _count = 1;

    public Barrier NewBarrier(Grid.BarrierType type, Coord loc)
    {
        var barrier = new Barrier(type, loc);
        return barrier;
    }

    public Barrier? this[int index]
    {
        get
        {
            if (index <= 1 || index > _count)
                return null;

            var barrier = _barriers[index - 2u];
            return barrier;
        }
    }
}