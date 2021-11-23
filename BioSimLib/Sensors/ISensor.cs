namespace BioSimLib.Sensors;

public interface ISensor
{
    public Sensor Type { get; }
    public string ShortName { get; }
    public float Output(Params p, Player player, uint simStep);
}