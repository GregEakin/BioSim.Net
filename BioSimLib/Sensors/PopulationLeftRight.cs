using BioSimLib.Field;

namespace BioSimLib.Sensors;

public class PopulationLeftRight : ISensor
{
    private readonly Grid _grid;

    public PopulationLeftRight(Grid grid)
    {
        _grid = grid;
    }

    public Sensor Type => Sensor.POPULATION_LR;
    public override string ToString() => "population LR";
    public string ShortName => "Plr";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _grid.GetPopulationDensityAlongAxis(player._loc, player._lastMoveDir.Rotate90DegCw());
        return sensorVal;
    }
}