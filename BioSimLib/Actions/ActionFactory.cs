namespace BioSimLib.Actions;

public class ActionFactory
{
    private readonly IAction[] _actions = new IAction[Enum.GetNames<Action>().Length];

    public IAction this[Action action] => _actions[(int)action];

    public ActionFactory()
    {
        var actions = new IAction[]
        {
            new EmitSignal(),
            new KillForward(),
            new MoveEast(),
            new MoveForward(),
            new MoveLeft(),
            new MoveNorth(),
            new MoveRandom(),
            new MoveReverse(),
            new MoveRight(),
            new MoveRL(),
            new MoveSouth(),
            new MoveWest(),
            new MoveX(),
            new MoveY(),
            new None(),
            new Procreate(),
            new SetLongProbeDist(),
            new SetOscillatorPeriod(),
            new SetResponsiveness(),
            new Suicide(),
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