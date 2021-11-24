namespace BioSimLib;

public class Peeps
{
    private readonly Config _p;
    private readonly Player?[] _players;
    private ushort _count = 1;

    private readonly List<Tuple<Player, Coord>> _moveQueue = new();

    public Peeps(Config p)
    {
        _p = p;
        _players = new Player?[p.population];
    }

    public Player NewPlayer(Grid grid, Genome genome, Coord loc)
    {
        var player = new Player(_p, grid, genome, loc, ++_count);
        _players[player._index - 2u] = player;
        return player;
    }

    public Player? this[int index]
    {
        get
        {
            if (index <= 0 || index > _count)
                return null;

            var player = _players[index - 2u];
            return player;
        }
    }

    public void QueueForMove(Player player, Coord newLoc)
    {
        _moveQueue.Add(new Tuple<Player, Coord>(player, newLoc));
    }

    public void DrainMoveQueue(Grid grid)
    {
        foreach (var (player, newLoc) in _moveQueue)
        {
            if (!grid.Move(player, newLoc)) continue;
            player._loc = newLoc;
            var moveDir = (newLoc - player._loc).AsDir();
            player._lastMoveDir = moveDir;
        }

        _moveQueue.Clear();
    }
}