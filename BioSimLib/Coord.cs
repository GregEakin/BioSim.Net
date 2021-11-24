namespace BioSimLib;

public struct Coord
{
    public Dir Heading()
    {
        return new Dir(Dir.Compass.CENTER);
    }

    public short X { get; set; }
    public short Y { get; set; }

    public static bool operator ==(Coord coord1, Coord coord2) => coord1.X == coord2.X && coord1.Y == coord2.Y;
    public static bool operator !=(Coord coord1, Coord coord2) => coord1.X != coord2.X && coord1.Y != coord2.Y;

    public static Coord Add(Coord coord1, Coord coord2) => new() { X = (short)(coord1.X + coord2.X), Y = (short)(coord1.Y + coord2.Y)};
    public static Coord operator -(Coord coord1, Coord coord2) => new() { X = (short)(coord1.X - coord2.X), Y = (short)(coord1.Y - coord2.Y) };

    public Coord AsNormalizedCoord()
    {
        throw new NotImplementedException();
    }

    public double RaySameness(Dir dir)
    {
        throw new NotImplementedException();
    }
}