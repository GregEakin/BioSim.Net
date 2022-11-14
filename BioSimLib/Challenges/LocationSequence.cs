namespace BioSimLib.Challenges;

[Challenge]
public class LocationSequence : IChallenge
{
    public Challenge Type => Challenge.LocationSequence;

    public LocationSequence()
    {
    }

    public (bool, float) PassedSurvivalCriterion(Player player)
    {
        var count = 0u;
        var maxNumberOfBits = 32;

        for (var n = 0; n < maxNumberOfBits; ++n)
        {
            var i = 1 << n;
            if (player._challengeBits[i]) ++count;
        }

        return count > 0
            ? (true, count / (float)maxNumberOfBits)
            : (false, 0.0f);
    }
}