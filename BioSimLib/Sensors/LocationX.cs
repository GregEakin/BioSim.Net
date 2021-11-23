namespace BioSimLib.Sensors;

public class LocationX : ISensor
{
    public Sensor Type => Sensor.LOC_X;
    public override string ToString() => "location x";
    public string ShortName => "Lx";

    public float Output(Params p, Player player, uint simStep)
    {
        return player._age / p.stepsPerGeneration;
    }
}