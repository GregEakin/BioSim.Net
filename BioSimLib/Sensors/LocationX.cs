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