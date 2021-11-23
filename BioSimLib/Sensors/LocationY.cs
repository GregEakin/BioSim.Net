namespace BioSimLib.Sensors;

public class LocationY : ISensor
{
    private readonly Config _p;

    public LocationY(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.LOC_Y;
    public override string ToString() => "location y";
    public string ShortName => "Ly";

    public float Output(Player player, uint simStep)
    {
        return (float)player._loc.Y / (_p.sizeX - 1u);
    }
}