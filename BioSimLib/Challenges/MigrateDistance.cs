namespace BioSimLib.Challenges;

[Challenge]
public class MigrateDistance : IChallenge
{
    private readonly Config _p;
    public Challenge Type => Challenge.MigrateDistance;

    public MigrateDistance(Config p)
    {
        _p = p;
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var distance = (player._loc - player._birthLoc).Length()
                       / (float)Math.Max(_p.sizeX, _p.sizeY);
        return (true, distance);
    }
}