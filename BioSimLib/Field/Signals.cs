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

public class Signals
{
    private const byte SIGNAL_MAX = byte.MaxValue;

    private readonly Config _p;
    private readonly byte[,,] _data;

    public Signals(Config p)
    {
        _p = p;
        _data = new byte[p.signalLayers, p.sizeX, p.sizeY];
    }

    public byte GetMagnitude(uint layerNum, Coord loc) => _data[layerNum, loc.X, loc.Y];
    public byte GetMagnitude(uint layerNum, short x, short y) => _data[layerNum, x, y];

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

        VisitNeighborhood(center, _p.signalSensorRadius, F);
        var maxSum = (float)countLocs * byte.MaxValue;
        var sensorVal = sum / maxSum;

        return sensorVal;
    }

    public void VisitNeighborhood(Coord loc, float radius, Action<short, short> f)
    {
        for (var dx = -Math.Min((int)radius, loc.X); dx <= Math.Min((int)radius, _p.sizeX - loc.X - 1); ++dx)
        {
            var x = loc.X + dx;
            var extentY = (int)Math.Sqrt(radius * radius - dx * dx);
            for (var dy = -Math.Min(extentY, loc.Y); dy <= Math.Min(extentY, _p.sizeY - loc.Y - 1); ++dy)
            {
                var y = loc.Y + dy;
                f((short)x, (short)y);
            }
        }
    }

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

        VisitNeighborhood(loc, _p.signalSensorRadius, F);
        var maxSumMag = 6.0 * _p.signalSensorRadius * byte.MaxValue;
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
        for (var x = 0; x < _p.sizeX; ++x)
        for (var y = 0; y < _p.sizeY; ++y)
            if (_data[layerNum, x, y] >= fadeAmount)
                _data[layerNum, x, y] -= fadeAmount; // fade center cell
            else
                _data[layerNum, x, y] = 0;
    }

    public void ZeroFill()
    {
    }
}