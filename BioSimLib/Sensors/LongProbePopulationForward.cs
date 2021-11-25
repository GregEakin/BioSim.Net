using BioSimLib.Field;

namespace BioSimLib.Sensors;

public class LongProbePopulationForward : ISensor
{
    private readonly Grid _grid;

    public LongProbePopulationForward(Grid grid)
    {
        _grid = grid;
    }

    public Sensor Type => Sensor.LONGPROBE_POP_FWD;
    public override string ToString() => "long probe population fwd";
    public string ShortName => "LPf";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _grid.LongProbePopulationFwd(player._loc, player._lastMoveDir, player._longProbeDist) / (float)player._longProbeDist;
        return sensorVal;
    }
}