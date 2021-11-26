using System.ComponentModel.Design;
using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions.Movements;

public class MoveRL : IMovementAction
{
    public Action Type => Action.MOVE_RL;
    public override string ToString() => "move R-L";
    public string ShortName => "MRL";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_RL];
        var offset = level switch
        {
            < 0.0f => lastMoveDir.Rotate90DegCcw().AsNormalizedCoord(),
            > 0.0f => lastMoveDir.Rotate90DegCw().AsNormalizedCoord(),
            _ => new Coord(0, 0)
        };

        var absLevel = Math.Abs(level);
        return (offset.X * absLevel, offset.Y * absLevel);
    }
}