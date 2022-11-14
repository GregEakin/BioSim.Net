using BioSimLib.Positions;

namespace BioSimLib.Challenges;

[Challenge]
public class Corner : IChallenge
{
    public Corner(Config p)
    {
        _p = p;
    }

    private readonly Config _p;
    public Challenge Type => Challenge.Corner;

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var radius = _p.sizeX / 8.0f;

        var distance1 = (new Coord(0, 0) - player._loc).Length();
        if (distance1 <= radius)
            return (true, 1.0f);

        var distance2 = (new Coord(0, (short)(_p.sizeY - 1)) - player._loc).Length();
        if (distance2 <= radius)
            return (true, 1.0f);

        var distance3 = (new Coord((short)(_p.sizeX - 1), 0) - player._loc).Length();
        if (distance3 <= radius)
            return (true, 1.0f);

        var distance4 = (new Coord((short)(_p.sizeX - 1), (short)(_p.sizeY - 1)) - player._loc).Length();
        if (distance4 <= radius)
            return (true, 1.0f);

        return (false, 0.0f);
    }
}