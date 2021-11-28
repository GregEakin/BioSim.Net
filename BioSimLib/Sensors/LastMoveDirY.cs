namespace BioSimLib.Sensors;

public class LastMoveDirY : ISensor
{
    public Sensor Type => Sensor.LAST_MOVE_DIR_Y;
    public override string ToString() => "last move dir Y";
    public string ShortName => "LMy";

    public float Output(Player player, uint simStep)
    {
        var lastY = player.LastMoveDir.AsNormalizedCoord().Y;
        var sensorVal = lastY == 0
            ? 0.5f
            : lastY == -1 ? 0.0f : 1.0f;
        return sensorVal;
    }
}