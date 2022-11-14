using BioSimLib.Positions;

namespace BioSimLib.Challenges;

[Challenge]
public class CenterUnweighted : IChallenge
{
    private readonly Config _p;
    public Challenge Type => Challenge.CenterUnweighted;

    public CenterUnweighted(Config p)
    {
        _p = p;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var safeCenter = new Coord((short)(_p.sizeX / 2.0), (short)(_p.sizeY / 2.0));
        var radius = _p.sizeX / 3.0f;

        var offset = safeCenter - player._loc;
        var distance = offset.Length();
        return distance <= radius
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}