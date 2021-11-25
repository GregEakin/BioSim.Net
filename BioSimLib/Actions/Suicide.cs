using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions;

public class Suicide : IAction
{
    public Action Type => Action.SUICIDE;
    public override string ToString() => "suicide";
    public string ShortName => "Die";

    public bool Enabled => false;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
        board.Peeps.QueueForDeath(player);
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}