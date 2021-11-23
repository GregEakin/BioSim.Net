using BioSimLib;
using BioSimLib.Sensors;

namespace BioSimTests;

public class SensorMock : ISensor
{
    public Sensor Type { get; }

    public string ShortName { get; }

    private readonly float _output;

    public SensorMock(Sensor sensor, string name, float output)
    {
        Type = sensor;
        ShortName = name;
        _output = output;
    }

    public float Output(Config p, Player player, uint simStep)
    {
        return _output;
    }
}