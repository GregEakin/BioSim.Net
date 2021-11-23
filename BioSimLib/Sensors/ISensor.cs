namespace BioSimLib.Sensors;

public interface ISensor
{
    public Sensor Type { get; }
    public string ShortName { get; }
    public float Output(Config p, Player player, uint simStep);
}