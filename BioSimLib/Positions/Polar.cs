namespace BioSimLib.Positions;

public readonly struct Polar
{
    public int Mag { get; }
    public Dir Dir { get; }

    public Polar(int mag0 = 0, Dir.Compass dir0 = Dir.Compass.CENTER)
    {
        Mag = mag0;
        Dir = new Dir(dir0);
    }

    public Polar(int mag0, Dir dir0)
    {
        Mag = mag0;
        Dir = dir0;
    }

    Coord AsCoord()
    {
        if (Dir == Dir.Compass.CENTER)
            return new Coord(0, 0);

        var S = 2.0 * Math.PI / 8.0;
        var compassToRadians = new[] { 5 * S, 6 * S, 7 * S, 4 * S, 0, 0 * S, 3 * S, 2 * S, 1 * S };
        var x = (short)Math.Round(Mag * Math.Cos(compassToRadians[Dir.AsInt()]));
        var y = (short)Math.Round(Mag * Math.Sin(compassToRadians[Dir.AsInt()]));
        return new Coord(x, y);
    }
}