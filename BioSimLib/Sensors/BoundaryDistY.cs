namespace BioSimLib.Sensors;

public class BoundaryDistY : ISensor
{
    private readonly Config _p;

    public BoundaryDistY(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.BOUNDARY_DIST_Y;
    public override string ToString() => "boundary dist Y";
    public string ShortName => "EDy";

    public float Output(Player player, uint simStep)
    {
        var minDistY = Math.Min(player._loc.Y, _p.sizeY - player._loc.Y - 1);
        var sensorVal = minDistY / (_p.sizeY / 2.0f);
        return sensorVal;
    }
}