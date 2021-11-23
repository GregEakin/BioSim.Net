namespace BioSimLib.Sensors;

public class BoundaryDistX : ISensor
{
    private readonly Config _p;

    public BoundaryDistX(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.BOUNDARY_DIST_Y;
    public override string ToString() => "boundary dist X";
    public string ShortName => "EDx";

    public float Output(Player player, uint simStep)
    {
        var minDistX = Math.Min(player._loc.X, _p.sizeX - player._loc.X - 1);
        var sensorVal = minDistX / (_p.sizeX / 2.0f);
        return sensorVal;
    }
}