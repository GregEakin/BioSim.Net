using BioSimLib.Field;

namespace BioSimLib.Sensors;

public class SensorFactory
{
    private readonly ISensor?[] _sensors = new ISensor?[Enum.GetNames<Sensor>().Length];

    public ISensor? this[Sensor sensor] => _sensors[(int)sensor];

    public SensorFactory(Config p, Board board)
    {
        var sensors = new ISensor[]
        {
            new LocationX(p),
            new LocationY(p),
            new BoundaryDistX(p),
            new BoundaryDist(p),
            new BoundaryDistY(p),
            new GeneticSimilarityForward(p, board.Grid),
            new LastMoveDirX(),
            new LastMoveDirY(),
            new LongProbePopulationForward(board.Grid),
            new LongProbeBarrierForward(board.Grid),
            new Population(p, board.Grid),
            new PopulationForward(board.Grid),
            new PopulationLeftRight(board.Grid),
            new Oscillator(),
            new Age(p),
            new BarrierForward(p, board.Grid),
            new BarrierLeftRight(p, board.Grid),
            new Random(),
            new Signal(board.Signals),
            new SignalFwd(board.Signals),
            new SignalLR(board.Signals),
            new True(),
            new False(),
        };

        foreach (var sensor in sensors)
            _sensors[(int)sensor.Type] = sensor;
    }

    public SensorFactory(IEnumerable<ISensor?> sensors)
    {
        foreach (var sensor in sensors)
            if (sensor != null)
                _sensors[(int)sensor.Type] = sensor;
    }
}