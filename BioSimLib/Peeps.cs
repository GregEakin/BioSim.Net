namespace BioSimLib;

public class Peeps
{
    private readonly Config _p;
    private readonly Player?[] _players;
    private ushort _count = 1;

    private readonly List<Tuple<Player, Coord>> _moveQueue = new();
    private readonly List<Player> _deathQueue = new();

    public Peeps(Config p)
    {
        _p = p;
        _players = new Player?[p.population];
    }

    public Player NewPlayer(Genome genome, Coord loc)
    {
        var player = new Player(_p, genome, loc, ++_count);
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
            if (!grid.Move(player, newLoc)) 
                continue;

            player._loc = newLoc;
            var moveDir = (newLoc - player._loc).AsDir();
            player._lastMoveDir = moveDir;
        }

        _moveQueue.Clear();
    }

    public void QueueForDeath(Player player)
    {
        _deathQueue.Add(player);
    }

    public void DrainDeathQueue(Grid grid)
    {
        foreach (var player in _deathQueue)
        {
            // grid[player._loc] = null;
            // player.Alive = false;
            _players[player._index] = null;
        }

        _moveQueue.Clear();
    }
}