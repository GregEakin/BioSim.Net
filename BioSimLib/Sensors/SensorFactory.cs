namespace BioSimLib.Sensors;

public class SensorFactory
{
    private readonly ISensor[] _sensors;

    public ISensor this[Sensor sensor] => _sensors[(int)sensor];

    public SensorFactory(Config p, Grid grid)
    {
    _sensors = new ISensor[]
        {
            new LocationX(p),       
            new LocationX(p),       
            new BoundaryDistY(p),   
            new BoundaryDist(p),    
            new BoundaryDistY(p),   
            new GeneticSimilarityForward(grid),
            null,                   // LAST_MOVE_DIR_X, // I +- amount of X movement in last movement
            null,                   // LAST_MOVE_DIR_Y, // I +- amount of Y movement in last movement
            null,                   // LONGPROBE_POP_FWD, // W long look for population forward
            null,                   // LONGPROBE_BAR_FWD, // W long look for barriers forward
            null,                   // POPULATION, // W population density in neighborhood
            null,                   // POPULATION_FWD, // W population density in the forward-reverse axis
            null,                   // POPULATION_LR, // W population density in the left-right axis
            null,                   // OSC1, // I oscillator +-value
            new Age(p),             // AGE, // I
            null,                   // BARRIER_FWD, // W neighborhood barrier distance forward-reverse axis
            null,                   // BARRIER_LR, // W neighborhood barrier distance left-right axis
            new Random(),           // RANDOM, //   random sensor value, uniform distribution
            null,                   // SIGNAL0, // W strength of signal0 in neighborhood
            null,                   // SIGNAL0_FWD, // W strength of signal0 in the forward-reverse axis
            null,                   // SIGNAL0_LR, // W strength of signal0 in the left-right axis
        };
    }

    public SensorFactory(ISensor[] sensors)
    {
        _sensors = sensors;
    }
}