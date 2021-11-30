//    Copyright 2021 Gregory Eakin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using Xunit;

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

        var genome = new GenomeBuilder(p.maxNumberNeurons, dna).ToGenome();
        var loc = new Coord { X = 3, Y = 4 };
        var player = board.NewPlayer(genome, loc);
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