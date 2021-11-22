namespace BioSimLib.Challenges;

[Flags]
public enum Challenge
{
    Circle = 0x0001,
    RightHalf = 0x0002,
    RightQuarter = 0x0004,
    Text = 0x0008,
    CenterWeighted = 0x0010,
    CenterUnweighted = 0x0020,
    Corner = 0x0040,
    CornerWeighted = 0x0080,
    MigrateDistance = 0x0100,
    CenterSparse = 0x0200,
    LeftEighth = 0x0400,
    RadioactiveWalls = 0x0800,
    AgainstAnyWall = 0x1000,
    TouchAnyWall = 0x2000,
    EastWestEighths = 0x4000,
    NearBarrier = 0x8000,
    Pairs = 0x10000,
    LocationSequence = 0x20000,
    Altruism = 0x40000,
    AltruismSacrifice = 0x80000,
}