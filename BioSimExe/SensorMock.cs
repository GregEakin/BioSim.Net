using BioSimLib;
using BioSimLib.Sensors;

namespace BioSimExe;

public class SensorMock : ISensor
{
    private readonly float _output;

    public Sensor Type { get; }

    public string ShortName { get; }

    public SensorMock(Sensor sensor, string name, float output)
    {
        Type = sensor;
        ShortName = name;
        _output = output;
    }

    public float Output(Player player, uint simStep)
    {
        return _output;
    }
}