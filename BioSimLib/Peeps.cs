namespace BioSimLib;

public class Peeps
{
    private readonly Config _p;
    private readonly Grid _grid;
    private readonly List<Tuple<Player, Coord>> _moveQueue = new();

    public Peeps(Config p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public void QueueForMove(Player player, Coord newLoc)
    {
        _moveQueue.Add(new Tuple<Player, Coord>(player, newLoc));
    }

    public void DrainMoveQueue()
    {
        foreach (var (player, newLoc) in _moveQueue)
        {
            if (!_grid.Move(player, newLoc)) continue;
            player._loc = newLoc;
            var moveDir = new Coord { X = (short)(newLoc.X - player._loc.X), Y = (short)(newLoc.Y - player._loc.Y) }.Heading();
            player._lastMoveDir = moveDir;
        }

        _moveQueue.Clear();
    }
}