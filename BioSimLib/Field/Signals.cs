﻿//    Copyright 2022 Gregory Eakin
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

public class Signals(Config config)
{
    private const byte SIGNAL_MAX = byte.MaxValue;

    private readonly byte[,,] _data = new byte[config.signalLayers, config.sizeX, config.sizeY];

    public byte GetMagnitude(uint layerNum, Coord loc) => _data[layerNum, loc.X, loc.Y];
    public byte GetMagnitude(uint layerNum, short x, short y) => _data[layerNum, x, y];

    // returns magnitude of the specified signal layer in a neighborhood, with
    // 0.0..maxSignalSum converted to the sensor range.
    public float GetSignalDensity(uint layerNum, Coord loc)
    {
        var countLocs = 0u;
        var sum = 0uL;
        var center = loc;

        void F(short x, short y)
        {
            ++countLocs;
            sum += GetMagnitude(layerNum, x, y);
        }

        VisitNeighborhood(center, config.signalSensorRadius, F);
        var maxSum = (double)countLocs * byte.MaxValue;
        var sensorVal = sum / maxSum;
        return (float)sensorVal;
    }

    public void VisitNeighborhood(Coord loc, float radius, Action<short, short> f)
    {
        for (var dx = -Math.Min((int)radius, loc.X); dx <= Math.Min((int)radius, config.sizeX - loc.X - 1); ++dx)
        {
            var x = loc.X + dx;
            var extentY = (int)Math.Sqrt(radius * radius - dx * dx);
            for (var dy = -Math.Min(extentY, loc.Y); dy <= Math.Min(extentY, config.sizeY - loc.Y - 1); ++dy)
            {
                var y = loc.Y + dy;
                f((short)x, (short)y);
            }
        }
    }

    // Converts the signal density along the specified axis to sensor range. The
    // values of cell signal levels are scaled by the inverse of their distance times
    // the positive absolute cosine of the difference of their angle and the
    // specified axis. The maximum positive or negative magnitude of the sum is
    // about 2*radius*SIGNAL_MAX (?). We don't adjust for being close to a border,
    // so signal densities along borders and in corners are commonly sparser than
    // away from borders.
    public float GetSignalDensityAlongAxis(uint layerNum, Coord loc, Dir dir)
    {
        var sum = 0.0;
        var dirVec = dir.AsNormalizedCoord();
        var len = Math.Sqrt(dirVec.X * dirVec.X + dirVec.Y * dirVec.Y);
        var dirVecX = dirVec.X / len;
        var dirVecY = dirVec.Y / len;

        void F(short x, short y)
        {
            var tloc = new Coord(x, y);
            if (tloc == loc) return;
            var offset = tloc - loc;
            var proj = dirVecX * offset.X + dirVecY * offset.Y;
            var contrib = proj * GetMagnitude(layerNum, tloc) / (offset.X * offset.X + offset.Y * offset.Y);
            sum += contrib;
        }

        VisitNeighborhood(loc, config.signalSensorRadius, F);
        var maxSumMag = 6.0 * config.signalSensorRadius * byte.MaxValue;
        var sensorVal = sum / maxSumMag;
        sensorVal = (sensorVal + 1.0) / 2.0;

        return (float)sensorVal;
    }

    public void Increment(int layerNum, Coord loc)
    {
        var radius = 1.5f;
        var centerIncreaseAmount = 2;
        var neighborIncreaseAmount = 1;

        VisitNeighborhood(loc, radius, (loc2X, loc2Y) =>
        {
            if (_data[layerNum, loc2X, loc2Y] < SIGNAL_MAX)
            {
                _data[layerNum, loc2X, loc2Y] = (byte)
                    Math.Min(SIGNAL_MAX, _data[layerNum, loc2X, loc2Y] + neighborIncreaseAmount);
            }
        });

        if (_data[layerNum, loc.X, loc.Y] < SIGNAL_MAX)
        {
            _data[layerNum, loc.X, loc.Y] = (byte)
                Math.Min(SIGNAL_MAX, _data[layerNum, loc.X, loc.Y] + centerIncreaseAmount);
        }
    }

    public void Fade(int layerNum)
    {
        var fadeAmount = (byte)1u;
        for (var x = 0; x < config.sizeX; ++x)
        for (var y = 0; y < config.sizeY; ++y)
            if (_data[layerNum, x, y] >= fadeAmount)
                _data[layerNum, x, y] -= fadeAmount; // fade center cell
            else
                _data[layerNum, x, y] = 0;
    }

    public void ZeroFill()
    {
        Array.Clear(_data);
    }
}