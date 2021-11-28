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

namespace BioSimLib.Actions;

public enum Action : byte
{
    MOVE_X, // W +- X component of movement
    MOVE_Y, // W +- Y component of movement
    MOVE_FORWARD, // W continue last direction
    MOVE_RL, // W +- component of movement
    MOVE_RANDOM, // W
    SET_OSCILLATOR_PERIOD, // I
    SET_LONGPROBE_DIST, // I
    SET_RESPONSIVENESS, // I
    EMIT_SIGNAL0, // W
    MOVE_EAST, // W
    MOVE_WEST, // W
    MOVE_NORTH, // W
    MOVE_SOUTH, // W
    MOVE_LEFT, // W
    MOVE_RIGHT, // W
    MOVE_REVERSE, // W
    KILL_FORWARD, // W
    NONE,
    SUICIDE,
    PROCREATE,
}