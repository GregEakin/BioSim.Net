namespace BioSimLib.Sensors;

public class PopulationForward : ISensor
{
    private readonly Grid _grid;

    public PopulationForward(Grid grid)
    {
        _grid = grid;
    }

    public Sensor Type => Sensor.POPULATION_FWD;
    public override string ToString() => "population forward";
    public string ShortName => "Pfd";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _grid.GetPopulationDensityAlongAxis(player._loc, player._lastMoveDir); 
        return sensorVal;
    }
}