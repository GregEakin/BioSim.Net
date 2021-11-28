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
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

Console.WriteLine("Hello, World!");

var p = new Config() { maxNumberNeurons = 2, sizeX = 5, sizeY = 5 };
Console.WriteLine("Config: {0}", p);

var board = new Board(p);

var sensorFactory = new SensorFactory(p, board);

var dna = new[]
{
    0x018A2000u,
    0x91842000u,
    0x01842000u,
    0x01012000u,
    0x00012000u,
    0x80002000u,
    0x01002000u,
};

var genome = new Genome(p, dna);
Console.WriteLine("Genome: {0}", genome);

var loc = new Coord { X = 1, Y = 2 };
var player = board.NewPlayer(genome, loc);
board.Grid.Move(player, loc);

Console.WriteLine("Player: {0}", player);

var (sensors, actions) = player._nnet.ActionReferenceCounts();
Console.Write("  Sensor counts: ");
foreach (var sensor in sensors) Console.Write("  {0}", sensor);
Console.WriteLine();
Console.Write("  Action counts: ");
foreach (var action in actions) Console.Write("  {0}", action);
Console.WriteLine();

Console.WriteLine();
Console.WriteLine(board.Grid);

var actionFactory = new ActionFactory();
static bool IsEnabled(IAction action) => (int)action.Type < (int)Action.KILL_FORWARD;

for (var simStep = 0u; simStep < 3u; ++simStep)
{
    Console.WriteLine("Step {0}", ++simStep);
    var actionLevels = new float[Enum.GetNames<Action>().Length];
    var neuronAccumulators = new float[p.maxNumberNeurons];
    player.FeedForward(sensorFactory, actionLevels, neuronAccumulators, simStep);
    Console.Write("  Action levels: ");
    foreach (var level in actionLevels) Console.Write("{0}, ", level);
    Console.WriteLine();

    player.ExecuteActions(actionFactory, board, IsEnabled, actionLevels, simStep);
    var newLoc = player.ExecuteMoves(actionFactory, IsEnabled, actionLevels, simStep);
    if (board.Grid.IsInBounds(newLoc) && board.Grid.IsEmptyAt(newLoc))
        board.Peeps.QueueForMove(player, newLoc);

    board.Peeps.DrainMoveQueue(board.Grid);
    Console.WriteLine();
    Console.WriteLine(board.Grid);
}