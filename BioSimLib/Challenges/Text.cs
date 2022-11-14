using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Challenges;

[Challenge]
public class Text : IChallenge
{
    private readonly Grid _grid;
    public Challenge Type => Challenge.Text;

    public Text(Grid grid)
    {
        _grid = grid;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var minNeighbors = 22u;
        var maxNeighbors = 2u;
        var radius = 1.5f;

        if (_grid.IsBorder(player._loc))
            return (false, 0.0f);

        var count = 0u;
        var f = (Coord loc2) => { if (_grid.IsOccupiedAt(loc2)) ++count; };

        _grid.VisitNeighborhood(player._loc, radius, f);
        return count >= minNeighbors && count <= maxNeighbors
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}