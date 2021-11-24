﻿namespace BioSimLib.Actions;

public class Procreate : IAction
{
    public Action Type => Action.PROCREATE;
    public override string ToString() => "procreate";
    public string ShortName => "Sex";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }
}