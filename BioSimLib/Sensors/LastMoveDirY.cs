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

namespace BioSimLib.Sensors;

// Y component -1,0,1 maps to sensor values 0.0, 0.5, 1.0
[Sensor]
public class LastMoveDirY : ISensor
{
    public Sensor Type => Sensor.LAST_MOVE_DIR_Y;
    public override string ToString() => "last move dir Y";
    public string ShortName => "LMy";

    public float Output(Critter critter, uint simStep)
    {
        var lastY = critter.LastMoveDir.AsNormalizedCoord().Y;
        var sensorVal = lastY == 0
            ? 0.5f
            : lastY == -1
                ? 0.0f
                : 1.0f;
        return sensorVal;
    }
}