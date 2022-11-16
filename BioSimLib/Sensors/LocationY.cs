﻿//    Copyright 2021 Gregory Eakin
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

// Maps current Y location 0..p.sizeY-1 to sensor range 0.0..1.0
[Sensor]
public class LocationY : ISensor
{
    private readonly Config _p;

    public LocationY(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.LOC_Y;
    public override string ToString() => "location y";
    public string ShortName => "Ly";

    public float Output(Critter player, uint simStep)
    {
        return (float)player.LocY / (_p.sizeX - 1u);
    }
}