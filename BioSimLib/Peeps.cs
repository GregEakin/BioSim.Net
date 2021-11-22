namespace BioSimLib;

public class Peeps
{
    private readonly Params _p;
    private readonly Grid _grid;
    private readonly List<Tuple<Indiv, Coord>> _moveQueue = new();

    public Peeps(Params p, Grid grid)
    {
        _p = p;
        _grid = grid;
    }

    public void QueueForMove(Indiv indiv, Coord newLoc)
    {
        _moveQueue.Add(new Tuple<Indiv, Coord>(indiv, newLoc));
    }

    public void DrainMoveQueue()
    {
        foreach (var (indiv, newLoc) in _moveQueue)
        {
            if (!_grid.Move(indiv, newLoc)) continue;
            indiv._loc = newLoc;
            var moveDir = new Coord { X = (short)(newLoc.X - indiv._loc.X), Y = (short)(newLoc.Y - indiv._loc.Y) }.AsDir();
            indiv._lastMoveDir = moveDir;
        }

        _moveQueue.Clear();
    }
}