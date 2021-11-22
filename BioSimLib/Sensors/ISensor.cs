namespace BioSimLib.Sensors;

public interface ISensor
{
    public Sensor Type { get; }
    public string ShortName { get; }
    public float Calc(Params p, Indiv indiv, uint simStep);
}