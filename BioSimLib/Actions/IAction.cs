namespace BioSimLib.Actions;

public interface IAction
{
    public Action Type { get; }
    public string ShortName { get; }
    public bool Enabled { get; }
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels);
    public (float, float) Move(float[] actionLevels, Dir lastMoveDir);
}