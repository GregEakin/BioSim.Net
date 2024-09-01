// Copyright 2022 Gregory Eakin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//       http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace BioSimLib.Positions;

public readonly struct Coord(short x0, short y0)
{
    private static readonly Dir.Compass[] DirConversion =
    [
        Dir.Compass.E, Dir.Compass.NE, Dir.Compass.N, Dir.Compass.NW,
        Dir.Compass.W, Dir.Compass.SW, Dir.Compass.S, Dir.Compass.SE
    ];

    public short X { get; } = x0;
    public short Y { get; } = y0;

    public bool IsNormalized()
    {
        return X is >= -1 and <= 1 && Y is >= -1 and <= 1;
    }

    public Coord Normalize()
    {
        return AsDir().AsNormalizedCoord();
    }

    public uint Length()
    {
        return (uint)Math.Sqrt(X * X + Y * Y);
    }

    public Dir AsDir()
    {
        if (X == 0 && Y == 0)
            return new Dir(Dir.Compass.CENTER);

        var TWO_PI = 2.0 * Math.PI;
        var angle = Math.Atan2(Y, X);

        if (angle < 0.0)
            angle += TWO_PI;

        angle += TWO_PI / 16.0;
        if (angle > TWO_PI)
            angle -= TWO_PI;

        var slice = (uint)(angle / (TWO_PI / 8.0));
        /*
            We have to convert slice values:

                3  2  1
                4     0
                5  6  7

            into Dir8Compass value:

                6  7  8
                3  4  5
                0  1  2
        */
        return new Dir(DirConversion[slice]);
    }

    public Polar AsPolar()
    {
        return new Polar((int)Length(), AsDir());
    }

    public bool Equals(Coord coord)
    {
        return X == coord.X && Y == coord.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Coord other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(Coord coord1, Coord coord2)
    {
        return coord1.X == coord2.X && coord1.Y == coord2.Y;
    }

    public static bool operator !=(Coord coord1, Coord coord2)
    {
        return coord1.X != coord2.X && coord1.Y != coord2.Y;
    }

    public static Coord operator +(Coord coord1, Coord coord2)
    {
        return new Coord((short)(coord1.X + coord2.X), (short)(coord1.Y + coord2.Y));
    }

    public static Coord operator -(Coord coord1, Coord coord2)
    {
        return new Coord((short)(coord1.X - coord2.X), (short)(coord1.Y - coord2.Y));
    }

    public static Coord operator *(Coord coord, int a)
    {
        return new Coord((short)(coord.X * a), (short)(coord.Y * a));
    }

    public static Coord operator +(Coord coord, Dir dir)
    {
        return coord + dir.AsNormalizedCoord();
    }

    public static Coord operator -(Coord coord, Dir dir)
    {
        return coord - dir.AsNormalizedCoord();
    }

    public float RaySameness(Coord other)
    {
        var mag1 = Math.Sqrt(X * X + Y * Y);
        var mag2 = Math.Sqrt(other.X * other.X + other.Y * other.Y);
        if (mag1 == 0.0f || mag2 == 0.0f)
            return 1.0f;

        var dot = X * other.X + Y * other.Y;
        var cos = (float)(dot / (mag1 * mag2));
        var normalized = Math.Min(Math.Max(cos, -1.0f), 1.0f);
        return normalized;
    }

    public float RaySameness(Dir dir)
    {
        return RaySameness(dir.AsNormalizedCoord());
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}