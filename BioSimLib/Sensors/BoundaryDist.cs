namespace BioSimLib.Sensors;

public class BoundaryDist : ISensor
{
    public Sensor Type => Sensor.BOUNDARY_DIST;
    public override string ToString() => "boundary dist";
    public string ShortName => "ED";

    public float Output(Params p, Player player, uint simStep)
    {
        var distX = Math.Min(player._loc.X, (p.sizeX - player._loc.X) - 1);
        var distY = Math.Min(player._loc.Y, (p.sizeY - player._loc.Y) - 1);
        var closest = Math.Min(distX, distY);
        var maxPossible = Math.Max(p.sizeX / 2 - 1, p.sizeY / 2 - 1);
        var sensorVal = (float)closest / maxPossible;
        return sensorVal;
    }
}