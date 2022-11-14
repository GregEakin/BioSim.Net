namespace BioSimLib.Challenges;

[Challenge]
public class EastWestEighths : IChallenge
{
    private readonly Config _p;
    public Challenge Type => Challenge.EastWestEighths;

    public EastWestEighths(Config p)
    {
        _p = p;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        return player._loc.X < _p.sizeX / 8 || player._loc.X >= _p.sizeX - _p.sizeX / 8
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}