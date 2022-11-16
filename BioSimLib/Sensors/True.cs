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

// Always 1.0
[Sensor]
public class True : ISensor
{
    public Sensor Type => Sensor.TRUE;
    public override string ToString() => "true";
    public string ShortName => "T";

    public float Output(Critter player, uint simStep)
    {
        return 1.0f;
    }
}