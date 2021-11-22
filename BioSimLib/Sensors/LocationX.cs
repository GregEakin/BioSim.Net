namespace BioSimLib.Sensors;

public class LocationX : ISensor
{
    public Sensor Type => Sensor.LOC_X;
    public override string ToString() => "location x";
    public string ShortName => "Lx";

    public float Output(Params p, Indiv indiv, uint simStep)
    {
        return indiv._age / p.stepsPerGeneration;
    }
}