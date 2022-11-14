namespace BioSimLib.Challenges;

[Challenge]
public class RightQuarter : IChallenge
{
    private readonly Config _p;
    public Challenge Type => Challenge.RightQuarter;

    public RightQuarter(Config p)
    {
        _p = p;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        return player._loc.X > _p.sizeX / 2 + _p.sizeX / 4
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}