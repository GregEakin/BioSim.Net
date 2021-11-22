namespace BioSimLib.Actions;

public interface IAction
{
    public Action Type { get; }
    public string ShortName { get; }
    public bool IsEnabled();
    public float Calc(Params p, Grid grid, Indiv indiv, uint simStep);
}