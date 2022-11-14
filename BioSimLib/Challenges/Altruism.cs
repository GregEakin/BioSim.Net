using BioSimLib.Positions;

namespace BioSimLib.Challenges;

[Challenge]
public class Altruism : IChallenge
{
    private readonly Config _p;
    public Challenge Type => Challenge.Altruism;

    public Altruism(Config p)
    {
        _p = p;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var safeCenter = new Coord((short)(_p.sizeX / 4.0), (short)(_p.sizeY / 4.0));
        var radius = _p.sizeX / 4.0f;

        var offset = safeCenter - player._loc;
        var distance = offset.Length();
        return distance <= radius
            ? (true, (radius - distance) / radius)
            : (false, 0.0f);
    }
}