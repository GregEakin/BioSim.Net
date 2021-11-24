namespace BioSimLib.Actions;

public class ActionFactory
{
    private readonly IAction[] _actions = new IAction[Enum.GetNames<Action>().Length];

    public IAction this[Action action] => _actions[(int)action];

    public ActionFactory()
    {
        var actions = new IAction[]
        {
            new MoveRandom(),
            new MoveWest(),
            new MoveNorth(),
        };

        foreach (var action in actions)
            _actions[(int)action.Type] = action;
    }

    public ActionFactory(IEnumerable<IAction> actions)
    {
        foreach (var action in actions)
            _actions[(int)action.Type] = action;
    }
}