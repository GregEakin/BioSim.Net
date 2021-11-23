namespace BioSimLib.Sensors;

public class BoundaryDist : ISensor
{
    private readonly Config _p;

    public BoundaryDist(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.BOUNDARY_DIST;
    public override string ToString() => "boundary dist";
    public string ShortName => "ED";

    public float Output(Player player, uint simStep)
    {
        var distX = Math.Min(player._loc.X, (_p.sizeX - player._loc.X) - 1);
        var distY = Math.Min(player._loc.Y, (_p.sizeY - player._loc.Y) - 1);
        var closest = Math.Min(distX, distY);
        var maxPossible = Math.Max(_p.sizeX / 2 - 1, _p.sizeY / 2 - 1);
        var sensorVal = (float)closest / maxPossible;
        return sensorVal;
    }
}