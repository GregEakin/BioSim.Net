namespace BioSimLib;

public class Barriers
{
    private readonly Barrier?[] _barriers = Array.Empty<Barrier?>();
    private ushort _count = 1;

    public Barrier NewBarrier(Grid.BarrierType type, Coord loc)
    {
        var barrier = new Barrier(type, loc);
        return barrier;
    }

    public Barrier? this[int index]
    {
        get
        {
            if (index <= 1 || index > _count)
                return null;

            var barrier = _barriers[index - 2u];
            return barrier;
        }
    }
}