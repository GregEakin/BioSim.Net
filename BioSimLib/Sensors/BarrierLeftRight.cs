using BioSimLib.Field;

namespace BioSimLib.Sensors;

public class BarrierLeftRight : ISensor
{
    private readonly Config _p;
    private readonly Grid _grid;

    public BarrierLeftRight(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public Sensor Type => Sensor.BARRIER_LR;
    public override string ToString() => "short probe barrier left-right";
    public string ShortName => "Blr";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _grid.GetShortProbeBarrierDistance(player._loc, player._lastMoveDir.Rotate90DegCw(), _p.shortProbeBarrierDistance);
        return sensorVal;
    }
}