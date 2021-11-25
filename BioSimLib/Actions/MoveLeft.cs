namespace BioSimLib.Actions;

public class MoveLeft : IAction
{
    public Action Type => Action.MOVE_LEFT;
    public override string ToString() => "move left";
    public string ShortName => "MvL";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_LEFT];
        var offset = lastMoveDir.Rotate90DegCcw().AsNormalizedCoord();
        return (offset.X * level, offset.Y * level);
    }
}