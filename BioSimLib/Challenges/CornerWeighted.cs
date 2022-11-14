using BioSimLib.Positions;

namespace BioSimLib.Challenges;

[Challenge]
public class CornerWeighted : IChallenge
{
    public CornerWeighted(Config p)
    {
        _p = p;
    }

    private readonly Config _p;
    public Challenge Type => Challenge.CornerWeighted;

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var radius = _p.sizeX / 4.0f;

        var distance = (new Coord(0, 0) - player._loc).Length();
        if (distance <= radius)
            return (true, (radius - distance) / radius);

        distance = (new Coord(0, (short)(_p.sizeY - 1)) - player._loc).Length();
        if (distance <= radius)
            return (true, (radius - distance) / radius);

        distance = (new Coord((short)(_p.sizeX - 1), 0) - player._loc).Length();
        if (distance <= radius)
            return (true, (radius - distance) / radius);

        distance = (new Coord((short)(_p.sizeX - 1), (short)(_p.sizeY - 1)) - player._loc).Length();
        if (distance <= radius)
            return (true, (radius - distance) / radius);

        return (false, 0.0f);
    }
}