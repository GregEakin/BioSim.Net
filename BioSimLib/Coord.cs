namespace BioSimLib;

public struct Coord
{
    public Dir Heading()
    {
        return new Dir(Dir.Compass.CENTER);
    }

    public short X { get; set; }
    public short Y { get; set; }
}