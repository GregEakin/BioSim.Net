namespace BioSimLib.Sensors;

public class LocationX : ISensor
{
    public Sensor Type => Sensor.LOC_X;
    public override string ToString() => "location x";
    public string ShortName => "Lx";

    public float Calc(Params p, Indiv indiv, uint simStep)
    {
        return indiv.age / p.stepsPerGeneration;
    }
}