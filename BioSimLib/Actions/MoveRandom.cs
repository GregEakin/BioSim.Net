namespace BioSimLib.Actions;

public class MoveRandom : IAction
{
    public Action Type => Action.MOVE_RANDOM;
    public override string ToString() => "move random";
    public string ShortName => "Mrn";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_RANDOM];
        var offset = Dir.Random8().AsNormalizedCoord();
        return (offset.X * level, offset.Y * level);
    }
}