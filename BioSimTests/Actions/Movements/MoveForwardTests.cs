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

using System;
using BioSimLib.Actions.Movements;
using BioSimLib.Positions;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests.Actions.Movements;

public class MoveForwardTests
{
    [Fact]
    public void MoveDisabledTest()
    {
        var movement = new MoveForward();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_FORWARD] = 0.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(0.0f, x);
        Assert.Equal(0.0f, y);
    }

    [Fact]
    public void MoveEnabledTest()
    {
        var movement = new MoveForward();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_FORWARD] = 1.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(-1.0f, x);
        Assert.Equal(0.0f, y);
    }
}