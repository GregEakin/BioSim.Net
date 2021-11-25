using BioSimLib;
using BioSimLib.Actions;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests.Actions;

// public class ActionMock : IAction
// {
//     private readonly Action<Config, Grid, Signals, Player, uint, float[]> _action;
//     public Action Type { get; }
//     public string ShortName { get; }
//     public bool Enabled { get; }
//
//     public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels) =>
//         _action(p, board, player, simStep, actionLevels);
//
//  
//     public ActionMock(Action type, string shortName, bool enabled,
//         Action<Config, Grid, Signals, Player, uint, float[]> action)
//     {
//         Type = type;
//         ShortName = shortName;
//         Enabled = enabled;
//         _action = action;
//     }
// }

public class EmitSignalTests
{
    [Fact]
    public void MovementTest()
    {
        var p = new Config { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
        var board = new Board(p);

        var dna = new[]
        {
            0x95882000u,  // TRUE * 1  => EMIT_SIGNAL0
        };

        var genome = new Genome(p, dna);
        var loc = new Coord { X = 3, Y = 4 };
        var player = board.Grid.NewPlayer(genome, loc);
        player.ResponsivenessAdjusted = 1.0f;

        var actionLevels = new[]
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f,
        };

        actionLevels[(int)Action.EMIT_SIGNAL0] = 0.6f;

        var action = new EmitSignal();
        action.Execute(p, board, player, 0, actionLevels);
    }
}