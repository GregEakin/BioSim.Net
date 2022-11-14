using BioSimLib.Field;

namespace BioSimLib.Challenges;

[Challenge]
public class NearBarrier : IChallenge
{
    private readonly Config _p;
    private readonly Grid _grid;

    public Challenge Type => Challenge.NearBarrier;

    public NearBarrier(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var radius = _p.sizeX / 2.0f;

        var barrierCenters = _grid.GetBarrierCenters();
        var minDistance = 1e8f;
        foreach (var center in barrierCenters)
        {
            float distance = (player._loc - center).Length();
            if (distance < minDistance)
                minDistance = distance;
        }

        return minDistance <= radius
            ? (true, 1.0f - minDistance / radius)
            : (false, 0.0f);
    }
}