namespace BioSimLib.Actions;

public class ActionFactory
{
    private readonly IAction[] _actions = // new ISensor[Enum.GetNames<Sensor>().Length];
    {
        null,               // MOVE_X, // W +- X component of movement
        null,               // MOVE_Y, // W +- Y component of movement
        null,               // MOVE_FORWARD, // W continue last direction
        null,               // MOVE_RL, // W +- component of movement
        null,               // MOVE_RANDOM, // W
        null,               // SET_OSCILLATOR_PERIOD, // I
        null,               // SET_LONGPROBE_DIST, // I
        null,               // SET_RESPONSIVENESS, // I
        null,               // EMIT_SIGNAL0, // W
        null,               // MOVE_EAST, // W
        null,               // MOVE_WEST, // W
        new MoveNorth(),    // MOVE_NORTH, // W
        null,               // MOVE_SOUTH, // W
        null,               // MOVE_LEFT, // W
        null,               // MOVE_RIGHT, // W
        null,               // MOVE_REVERSE, // W
        null,               // KILL_FORWARD, // W
    };

    public IAction this[Action action] => _actions[(int)action];

}