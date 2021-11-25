namespace BioSimLib.Actions;

public class KillForward : IAction
{
    public Action Type => Action.KILL_FORWARD;
    public override string ToString() => "kill fwd";
    public string ShortName => "KlF";

    public bool Enabled => false;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
        var killThreshold = 0.5f;
        var level = actionLevels[(int)Action.KILL_FORWARD];
        level = (float)((Math.Tanh(level) + 1.0) / 2.0);
        level *= player.ResponsivenessAdjusted;
        if (level <= killThreshold || !Player.Prob2Bool(level))
            return;

        var otherLoc = player._loc + player.LastMoveDir;
        if (!board.Grid.IsInBounds(otherLoc) || !board.Grid.IsOccupiedAt(otherLoc))
            return;

        var player2 = board.Grid[otherLoc];

        if (!(player2?.Alive ?? false))
            return;

        // peeps.queueForDeath(player2);
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}