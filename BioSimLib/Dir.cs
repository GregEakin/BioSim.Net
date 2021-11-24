namespace BioSimLib;

public struct Dir
{
    public enum Compass : byte
    {
        SW, S, SE, W, CENTER, E, NW, N, NE
    }

    private static readonly Random Random = new();
    public static Dir Random8()
    {
        return new Dir(Compass.N).Rotate(Random.Next(0, 7));
    }

    public Dir(Compass dir = Compass.CENTER)
    {
        _dir9 = dir;
    }

    // public static implicit operator Dir(Compass d) => new Dir(d);

    private readonly Compass _dir9;

    public byte AsInt() => (byte)_dir9;

    public Coord AsNormalizedCoord()
    {
        var d = AsInt();
        return new Coord { X = (short)(d % 3 - 1), Y = (short)(d / 3 - 1) };
    }

    public Coord AsNormalizedPolar()
    {
        var d = AsInt();
        return new Coord { X = (short)(d % 3 - 1), Y = (short)(d / 3 - 1) };
    }

    public Dir Rotate(int n = 0)
    {
        throw new NotImplementedException();
    }

    public Dir Rotate90DegCw() => Rotate(2);
    public Dir Rotate90DegCcw() => Rotate(-2);
    public Dir Rotate180Deg() => Rotate(4);

    public static bool operator ==(Dir dir, Compass compass) => dir.AsInt() == (byte)compass;
    public static bool operator !=(Dir dir, Compass compass) => dir.AsInt() != (byte)compass;
    public static bool operator ==(Dir dir1, Dir dir2) => dir1.AsInt() == dir2.AsInt();
    public static bool operator !=(Dir dir1, Dir dir2) => dir1.AsInt() != dir2.AsInt();
}