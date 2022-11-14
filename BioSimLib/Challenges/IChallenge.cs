namespace BioSimLib.Challenges;

public interface IChallenge
{
    public Challenge Type { get; }

    // Returns true and a score 0.0..1.0 if passed, false if failed
    public (bool, float) PassedSurvivalCriterion(Player player);
}