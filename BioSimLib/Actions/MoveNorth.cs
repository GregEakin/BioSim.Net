﻿namespace BioSimLib.Actions;

public class MoveNorth : IAction
{
    public Action Type => Action.MOVE_NORTH;
    public override string ToString() => "move north";
    public string ShortName => "MvN";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels)
    {
        return (0.0f, 0.0f);
    }
}