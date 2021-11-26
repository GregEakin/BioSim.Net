using BioSimLib.Positions;
using Xunit;

namespace BioSimTests.Positions;

public class CoordTests
{
    [Fact]
    public void CenterTest1()
    {
        var coord = new Coord(0, 0);
        Assert.Equal(new Dir(Dir.Compass.CENTER), coord.AsDir());
    }

    [Fact]
    public void DirTest1()
    {
        var coord = new Coord(0, -1);
        Assert.Equal(new Dir(Dir.Compass.W), coord.AsDir());
    }
}