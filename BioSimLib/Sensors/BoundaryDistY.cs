namespace BioSimLib.Sensors;

public class BoundaryDistY : ISensor
{
    public Sensor Type => Sensor.BOUNDARY_DIST_Y;
    public override string ToString() => "boundary dist Y";
    public string ShortName => "EDy";

    public float Calc(Params p, Indiv indiv, uint simStep)
    {
        var minDistY = Math.Min(indiv._loc.Y, (p.sizeY - indiv._loc.Y) - 1);
        var sensorVal = minDistY / (p.sizeY / 2.0f);
        return sensorVal;
    }
}