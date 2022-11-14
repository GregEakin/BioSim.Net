namespace BioSimLib.Challenges;

[Challenge]
public class RightHalf : IChallenge
{
    private readonly Config _p;
    public Challenge Type => Challenge.RightHalf;

    public RightHalf(Config p)
    {
        _p = p;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        return player._loc.X > _p.sizeX / 2
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}