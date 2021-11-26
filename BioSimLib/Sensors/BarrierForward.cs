using BioSimLib.Field;

namespace BioSimLib.Sensors;

public class BarrierForward : ISensor
{
    private readonly Config _p;
    private readonly Grid _grid;

    public BarrierForward(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public Sensor Type => Sensor.BARRIER_FWD;
    public override string ToString() => "short probe barrier fwd-rev";
    public string ShortName => "Bfd";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _grid.GetShortProbeBarrierDistance(player._loc, player.LastMoveDir, _p.shortProbeBarrierDistance);
        return sensorVal;
    }
}