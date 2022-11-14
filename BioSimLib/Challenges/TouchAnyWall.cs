namespace BioSimLib.Challenges;

[Challenge]
public class TouchAnyWall : IChallenge
{
    public Challenge Type => Challenge.TouchAnyWall;

    public TouchAnyWall()
    {
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        return player._challengeBits.Data != 0
            ? (true, 1.0f)
            : (false, 0.0f);
    }
}