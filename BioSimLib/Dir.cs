using System.Drawing;
using System.Numerics;

namespace BioSimLib;

public struct Dir
{
    public enum Compass : byte
    {
        SW, S, SE, W, CENTER, E, NW, N, NE
    }

    public static Dir Random8()
    {
        var dir = new Dir(Compass.SW);
        return dir;
    }

    public Dir(Compass dir)
    {
        dir9 = dir;
    }

    private readonly Compass dir9;

    public byte AsInt() => (byte) dir9;

    public Point AsNormalizedCoord()
    {
        var d = AsInt();
        return new Point(){ X = (short)(d % 3 - 1), Y = (short)(d / 3 - 1)};
    }

    public Dir Rotate90DegCw()
    {
        throw new NotImplementedException();
    }
}