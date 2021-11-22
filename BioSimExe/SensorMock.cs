using BioSimLib;
using BioSimLib.Sensors;

public class SensorMock : ISensor
{
    public Sensor Type => _type;
    public string ShortName => "Mock";

    public float Output(Params p, Indiv indiv, uint simStep)
    {
        return _output;
    }

    private readonly Sensor _type;
    private readonly string _name;
    private readonly float _output;

    public SensorMock(Sensor sensor, string name, float output)
    {
        _type = sensor;
        _name = name;
        _output = output;
    }
}