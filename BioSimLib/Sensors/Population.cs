using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Sensors;

public class Population : ISensor
{
    private readonly Config _p;
    private readonly Grid _grid;

    public Population(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public Sensor Type => Sensor.POPULATION;
    public override string ToString() => "population";
    public string ShortName => "Pop";

    public float Output(Player player, uint simStep)
    {
        var count = 0u;
        var occupied = 0u;
        void F(Coord loc)
        {
            ++count;
            if (!_grid.IsEmptyAt(loc))
                ++occupied;
        }

        Grid.VisitNeighborhood(_p, player._loc, _p.populationSensorRadius, F);
        var sensorVal = (float)occupied / count;
        return sensorVal;
    }
}