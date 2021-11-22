﻿namespace BioSimLib.Actions;

public class MoveNorth : IAction
{
    public Action Type => Action.MOVE_NORTH;
    public override string ToString() => "move north";
    public string ShortName => "MvN";

    public bool IsEnabled() => true;
    public float Calc(Params p, Grid grid, Indiv indiv, uint simStep)
    {
        return indiv._age / p.stepsPerGeneration;
    }
}