using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Challenges;

[Challenge]
public class CenterSparse : IChallenge
{
    private readonly Config _p;
    private readonly Grid _grid;
    public Challenge Type => Challenge.CenterSparse;

    public CenterSparse(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var safeCenter = new Coord((short)(_p.sizeX / 2.0), (short)(_p.sizeY / 2.0));
        var outerRadius = _p.sizeX / 4.0f;
        var innerRadius = 1.5f;
        var minNeighbors = 5u; // includes self
        var maxNeighbors = 8u;

        var offset = safeCenter - player._loc;
        var distance = offset.Length();
        if (!(distance <= outerRadius)) return (false, 0.0f);
        var count = 0f;
        var f = (Coord loc2) =>
        {
            if (_grid.IsOccupiedAt(loc2)) ++count;
        };

        _grid.VisitNeighborhood(player._loc, innerRadius, f);
        if (count >= minNeighbors && count <= maxNeighbors)
            return (true, 1.0f);
        return (false, 0.0f);
    }
}