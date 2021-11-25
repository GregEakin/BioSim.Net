namespace BioSimLib;

public readonly struct Board
{
    public Grid Grid { get; }
    public Peeps Peeps { get; }
    public Signals Signals { get; }
    public Barriers Barriers { get; }

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