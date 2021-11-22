namespace BioSimLib.Sensors;

public class BoundaryDist : ISensor
{
    public Sensor Type => Sensor.BOUNDARY_DIST;
    public override string ToString() => "boundary dist";
    public string ShortName => "ED";

    public float Calc(Params p, Indiv indiv, uint simStep)
    {
        var distX = Math.Min(indiv.loc.X, (p.sizeX - indiv.loc.X) - 1);
        var distY = Math.Min(indiv.loc.Y, (p.sizeY - indiv.loc.Y) - 1);
        var closest = Math.Min(distX, distY);
        var maxPossible = Math.Max(p.sizeX / 2 - 1, p.sizeY / 2 - 1);
        var sensorVal = (float)closest / maxPossible;
        return sensorVal;
    }
}