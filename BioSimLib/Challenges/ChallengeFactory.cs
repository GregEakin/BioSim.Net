using System.Reflection;
using BioSimLib.Field;

namespace BioSimLib.Challenges;

public class ChallengeFactory
{
    private readonly IChallenge?[] _challenges = new IChallenge?[41];
    public IChallenge? this[int index] => _challenges[index];

    public ChallengeFactory(Config config, Board board) : this(config, board.Grid) {}

    public ChallengeFactory(Config config, Grid grid)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            if (!type.GetCustomAttributes(false).OfType<ChallengeAttribute>().Any()) continue;

            var i1 = type.GetConstructor([]);
            if (i1 != null)
            {
                var factory = (IChallenge)i1.Invoke([]);
                _challenges[(int)factory.Type] = factory;
                continue;
            }

            var i2 = type.GetConstructor([typeof(Config)]);
            if (i2 != null)
            {
                var factory = (IChallenge)i2.Invoke([config]);
                _challenges[(int)factory.Type] = factory;
                continue;
            }

            var i3 = type.GetConstructor([typeof(Grid)]);
            if (i3 != null)
            {
                var factory = (IChallenge)i3.Invoke([grid]);
                _challenges[(int)factory.Type] = factory;
                continue;
            }

            var i4 = type.GetConstructor([typeof(Config), typeof(Grid)]);
            if (i4 != null)
            {
                var factory = (IChallenge)i4.Invoke([config, grid]);
                _challenges[(int)factory.Type] = factory;
                continue;
            }

            throw new Exception("Ctor for Challenge not found.");
        }
    }

    public ChallengeFactory(IEnumerable<IChallenge?> challenges)
    {
        foreach (var challenge in challenges)
            if (challenge != null)
                _challenges[(int)challenge.Type] = challenge;
    }
}