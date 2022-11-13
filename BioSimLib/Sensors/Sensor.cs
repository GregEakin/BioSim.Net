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

namespace BioSimLib.Sensors;

public enum Sensor : byte
{
    LOC_X, // I distance from left edge
    LOC_Y, // I distance from bottom
    BOUNDARY_DIST_X, // I X distance to nearest edge of world
    BOUNDARY_DIST, // I distance to nearest edge of world
    BOUNDARY_DIST_Y, // I Y distance to nearest edge of world
    GENETIC_SIM_FWD, // I genetic similarity forward
    LAST_MOVE_DIR_X, // I +- amount of X movement in last movement
    LAST_MOVE_DIR_Y, // I +- amount of Y movement in last movement
    LONGPROBE_POP_FWD, // W long look for population forward
    LONGPROBE_BAR_FWD, // W long look for barriers forward
    POPULATION, // W population density in neighborhood
    POPULATION_FWD, // W population density in the forward-reverse axis
    POPULATION_LR, // W population density in the left-right axis
    OSC1, // I oscillator +-value
    AGE, // I
    BARRIER_FWD, // W neighborhood barrier distance forward-reverse axis
    BARRIER_LR, // W neighborhood barrier distance left-right axis
    RANDOM, //   random sensor value, uniform distribution
    SIGNAL0, // W strength of signal0 in neighborhood
    SIGNAL0_FWD, // W strength of signal0 in the forward-reverse axis
    SIGNAL0_LR, // W strength of signal0 in the left-right axis
    TRUE,
    FALSE,
}

[AttributeUsage(AttributeTargets.All)]
internal sealed class SensorAttribute : Attribute
{
}

