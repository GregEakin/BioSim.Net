﻿namespace BioSimLib;

public struct Coord
{
    public short X { get; set; }
    public short Y { get; set; }

    public Coord(short x0 = 0, short y0 = 0)
    {
        X = x0;
        Y = y0;
    }

    public bool IsNormalized() => X >= -1 && X <= 1 && Y >= -1 && Y <= 1;

    public Coord Normalize() => AsDir().AsNormalizedCoord();

    public uint Length() => (uint)(Math.Sqrt(X * X + Y * Y));

    public Dir AsDir()
    {
        if (X == 0 && Y == 0)
            return new Dir(Dir.Compass.CENTER);

        var TWO_PI = (float)(2.0 * Math.PI);
        var angle = Math.Atan2(Y, X);

        if (angle < 0.0f)
            angle += TWO_PI;

        angle += TWO_PI / 16.0f;
        if (angle > TWO_PI)
            angle -= TWO_PI;

        var slice = (uint)(angle / (TWO_PI / 8.0f));
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
        var dirConversion = new[]
        {
            Dir.Compass.E, Dir.Compass.NE, Dir.Compass.N, Dir.Compass.NW, Dir.Compass.W, Dir.Compass.SW, Dir.Compass.S,
            Dir.Compass.SE
        };
        return new Dir(dirConversion[slice]);
    }

    public Polar AsPolar() => new Polar((int)Length(), AsDir());

    public static bool operator ==(Coord coord1, Coord coord2) => coord1.X == coord2.X && coord1.Y == coord2.Y;
    public static bool operator !=(Coord coord1, Coord coord2) => coord1.X != coord2.X && coord1.Y != coord2.Y;

    public static Coord operator +(Coord coord1, Coord coord2) => new()
        { X = (short)(coord1.X + coord2.X), Y = (short)(coord1.Y + coord2.Y) };

    public static Coord operator -(Coord coord1, Coord coord2) => new()
        { X = (short)(coord1.X - coord2.X), Y = (short)(coord1.Y - coord2.Y) };

    public static Coord operator *(Coord coord, int a) => new() { X = (short)(coord.X * a), Y = (short)(coord.Y * a) };
    public static Coord operator +(Coord coord, Dir dir) => coord + dir.AsNormalizedCoord();
    public static Coord operator -(Coord coord, Dir dir) => coord - dir.AsNormalizedCoord();

    public double RaySameness(Coord other)
    {
        var mag1 = Math.Sqrt(X * X + Y * Y);
        var mag2 = Math.Sqrt(other.X * other.X + other.Y * other.Y);
        if (mag1 == 0.0f || mag2 == 0.0f)
            return 1.0f;

        var otherX = X * other.X + Y * other.Y;
        var dot = otherX;
        var cos = dot / (mag1 * mag2);
        cos = Math.Min(Math.Max(cos, -1.0), 1.0);
        return cos;
    }

    public double RaySameness(Dir dir) => RaySameness(dir.AsNormalizedCoord());
}