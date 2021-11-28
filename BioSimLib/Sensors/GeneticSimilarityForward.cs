using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;

namespace BioSimLib.Sensors;

public class GeneticSimilarityForward : ISensor
{
    private readonly Config _p;
    private readonly Grid _grid;

    public GeneticSimilarityForward(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public Sensor Type => Sensor.GENETIC_SIM_FWD;
    public override string ToString() => "genetic similarity forward";
    public string ShortName => "Gen";

    public float Output(Player player, uint simStep)
    {
        var forward = player._loc + player.LastMoveDir;
        var partner = _grid[forward];
        if (partner is not { Alive: true }) return 0.0f;
        var method = (GeneBank.ComparisonMethods)_p.genomeComparisonMethod;
        var sensorVal = GeneBank.GenomeSimilarity(method, player._genome, partner._genome); 
        return sensorVal;
    }
}