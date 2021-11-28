namespace BioSimLib.Sensors;

public class LastMoveDirX : ISensor
{
    public Sensor Type => Sensor.LAST_MOVE_DIR_X;
    public override string ToString() => "last move dir X";
    public string ShortName => "LMx";

    public float Output(Player player, uint simStep)
    {
        var lastX = player.LastMoveDir.AsNormalizedCoord().X;
        var sensorVal = lastX == 0 
            ? 0.5f 
            : lastX == -1 ? 0.0f : 1.0f;
        return sensorVal;
    }
}