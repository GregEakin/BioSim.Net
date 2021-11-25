using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib;

public readonly struct Barrier
{
    public Grid.BarrierType Type { get; }
    public Coord Loc { get; }

    public Barrier(Grid.BarrierType type, Coord loc)
    {
        Loc = loc;
        Type = type;
    }
}