namespace BioSimLib.Sensors;

public class Random : ISensor
{
    public Sensor Type => Sensor.RANDOM;
    public override string ToString() => "random";
    public string ShortName => "Rnd";

    public float Calc(Params p, Indiv indiv, uint simStep)
    {
        return indiv._age / p.stepsPerGeneration;
    }
}