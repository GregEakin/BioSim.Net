namespace BioSimLib.Challenges;

[Challenge]
public class AgainstAnyWall : IChallenge
{
    private readonly Config _p;
    public Challenge Type => Challenge.AgainstAnyWall;

    public AgainstAnyWall(Config p)
    {
        _p = p;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var onEdge = player._loc.X == 0
                     || player._loc.X == _p.sizeX - 1
                     || player._loc.Y == 0
                     || player._loc.Y == _p.sizeY - 1;

        return onEdge
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}