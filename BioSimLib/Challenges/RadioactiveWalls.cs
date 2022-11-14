namespace BioSimLib.Challenges;

[Challenge]
public class RadioactiveWalls : IChallenge
{
    public Challenge Type => Challenge.RadioactiveWalls;

    public RadioactiveWalls()
    {
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        return (true, 1.0f);
    }
}