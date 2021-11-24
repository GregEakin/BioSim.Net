namespace BioSimLib;

public struct Polar
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
}