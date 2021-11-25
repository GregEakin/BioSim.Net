namespace BioSimLib;

public class Board
{
    public Grid Grid { get; }
    public Peeps Peeps { get; }
    public Signals Signals { get; }
    public Barriers Barriers { get; }

    public Board(Config p)
    {
        Peeps = new Peeps(p);
        Grid = new Grid(p, Peeps);
        Signals = new Signals(p);
        Barriers = new Barriers();
    }
}