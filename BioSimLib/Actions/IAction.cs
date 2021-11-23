namespace BioSimLib.Actions;

public interface IAction
{
    public Action Type { get; }
    public string ShortName { get; }
    public bool IsEnabled();
    public float Calc(Config p, Grid grid, Player player, uint simStep);
}