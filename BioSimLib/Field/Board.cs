using BioSimLib.Genes;
using BioSimLib.Positions;

namespace BioSimLib.Field;

public readonly struct Board
{
    public Barriers Barriers { get; }
    public Grid Grid { get; }
    public Peeps Peeps { get; }
    public Signals Signals { get; }

    public Board(Config p)
    {
        Barriers = new Barriers();
        Peeps = new Peeps(p);
        Grid = new Grid(p, Peeps, Barriers);
        Signals = new Signals(p);
    }

    public Player NewPlayer(Genome genome, Coord loc)
    {
        return Grid.CreatePlayer(genome, loc);
    }
}