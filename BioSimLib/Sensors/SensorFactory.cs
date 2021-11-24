namespace BioSimLib.Sensors;

public class SensorFactory
{
    private readonly ISensor[] _sensors = new ISensor[Enum.GetNames<Sensor>().Length];

    public ISensor this[Sensor sensor] => _sensors[(int)sensor];

    public SensorFactory(Config p, Grid grid, Signals signals)
    {
        var sensors = new ISensor[]
        {
            new LocationX(p),
            new LocationX(p),
            new BoundaryDistX(p),
            new BoundaryDist(p),
            new BoundaryDistY(p),
            new GeneticSimilarityForward(grid),
            new LastMoveDirX(p),
            new LastMoveDirY(p),
            new LongProbePopulationForward(grid),
            new LongProbeBarrierForward(grid),
            new Population(p, grid),
            new PopulationForward(grid),
            new PopulationForward(grid),
            new Oscillator(),
            new Age(p),
            new BarrierForward(p, grid),
            new BarrierLeftRight(p, grid),
            new Random(),
            new Signal(signals),
            new SignalFwd(signals),
            new SignalLR(signals),
            new True(),
            new False(),
        };

        foreach (var sensor in sensors)
            _sensors[(int)sensor.Type] = sensor;
    }

    public SensorFactory(IEnumerable<ISensor> sensors)
    {
        foreach (var sensor in sensors) 
            _sensors[(int)sensor.Type] = sensor;
    }
}