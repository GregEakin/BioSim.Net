using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;

namespace BioSimLib.Sensors;

public class GeneticSimilarityForward : ISensor
{
    private readonly Grid _grid;

    public GeneticSimilarityForward(Grid grid)
    {
        _grid = grid;
    }

    public Sensor Type => Sensor.GENETIC_SIM_FWD;
    public override string ToString() => "genetic similarity forward";
    public string ShortName => "Gen";

    public float Output(Player player, uint simStep)
    {
        var forward = new Coord(); // player._loc + player._lastMoveDir;
        var partner = _grid[forward];
        if (partner is not { Alive: true }) return 0.0f;
        var sensorVal = GeneBank.GenomeSimilarity(player._genome, partner._genome); 
        return sensorVal;
    }
}