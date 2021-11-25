using BioSimLib.Field;

namespace BioSimLib.Sensors;

public class LongProbeBarrierForward : ISensor
{
    private readonly Grid _grid;

    public LongProbeBarrierForward(Grid grid)
    {
        _grid = grid;
    }

    public Sensor Type => Sensor.LONGPROBE_BAR_FWD;
    public override string ToString() => "long probe barrier fwd";
    public string ShortName => "LPb";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _grid.LongProbeBarrierFwd(player._loc, player._lastMoveDir, player._longProbeDist) / (float)player._longProbeDist;
        return sensorVal;
    }
}