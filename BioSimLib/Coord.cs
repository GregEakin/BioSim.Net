namespace BioSimLib;

public class Coord
{
    public Dir AsDir()
    {
        return new Dir(Dir.Compass.CENTER);
    }

    public short X { get; set; }
    public short Y { get; set; }
}