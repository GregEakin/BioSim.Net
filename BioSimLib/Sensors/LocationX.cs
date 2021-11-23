namespace BioSimLib.Sensors;

public class LocationX : ISensor
{
    private readonly Config _p;

    public LocationX(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.LOC_X;
    public override string ToString() => "location x";
    public string ShortName => "Lx";

    public float Output(Player player, uint simStep)
    {
        return (float)player._loc.X / (_p.sizeX - 1u);
    }
}

public class LastMoveDirX : ISensor
{
    private readonly Config _p;

    public LastMoveDirX(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.LAST_MOVE_DIR_X;
    public override string ToString() => "last move dir X";
    public string ShortName => "LMx";

    public float Output(Player player, uint simStep)
    {
        return (float)player._loc.X / (_p.sizeX - 1u);
    }
}