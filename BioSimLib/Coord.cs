namespace BioSimLib;

public struct Coord
{
    public Dir Heading()
    {
        return new Dir(Dir.Compass.CENTER);
    }

    public short X { get; set; }
    public short Y { get; set; }

    public Coord(short x0 = 0, short y0 = 0)
    {
        X = x0;
        Y = y0;
    }

    public bool IsNormalized() => X >= -1 && X <= 1 && Y >= -1 && Y <= 1;

    public Coord Normalize()
    {
        throw new NotImplementedException();
    }

    public uint Length() => (uint)(Math.Sqrt(X * X + Y * Y));

    public Dir AsDir() => throw new NotImplementedException();

    public Polar AsPolar() => throw new NotImplementedException();

    public static bool operator ==(Coord coord1, Coord coord2) => coord1.X == coord2.X && coord1.Y == coord2.Y;
    public static bool operator !=(Coord coord1, Coord coord2) => coord1.X != coord2.X && coord1.Y != coord2.Y;
    public static Coord operator +(Coord coord1, Coord coord2) => new() { X = (short)(coord1.X + coord2.X), Y = (short)(coord1.Y + coord2.Y) };
    public static Coord operator -(Coord coord1, Coord coord2) => new() { X = (short)(coord1.X - coord2.X), Y = (short)(coord1.Y - coord2.Y) };
    public static Coord operator *(Coord coord, int a) => new() { X = (short)(coord.X * a), Y = (short)(coord.Y * a) };
    public static Coord operator +(Coord coord, Dir dir) => coord + dir.AsNormalizedCoord();
    public static Coord operator -(Coord coord, Dir dir) => coord - dir.AsNormalizedCoord();

    public double RaySameness(Coord other)
    {
        throw new NotImplementedException();
    }

    public double RaySameness(Dir dir)
    {
        throw new NotImplementedException();
    }
}