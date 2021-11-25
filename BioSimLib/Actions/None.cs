using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions;

public class None : IAction
{
    public Action Type => Action.NONE;
    public override string ToString() => "none";
    public string ShortName => "Nop";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}