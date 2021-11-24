﻿namespace BioSimLib.Actions;

public class MoveRandom : IAction
{
    public Action Type => Action.MOVE_RANDOM;
    public override string ToString() => "move random";
    public string ShortName => "Mrn";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }
}