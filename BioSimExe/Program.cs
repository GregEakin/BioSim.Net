﻿// See https://aka.ms/new-console-template for more information

using BioSimExe;
using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

Console.WriteLine("Hello, World!");

var p = new Config() { maxNumberNeurons = 2, sizeX = 5, sizeY = 5 };
Console.WriteLine("Config: {0}", p);

var board = new Board(p);

//var sensorFactory = new SensorFactory();
var sensorsFactory = new SensorFactory(
    new ISensor[]
    {
        new SensorMock(Sensor.LOC_X, "Lx", 0.2f),
        new SensorMock(Sensor.RANDOM, "Rnd", 0.1f),
    }
);

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
var player = board.Grid.NewPlayer(genome, loc);
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

var factory = new ActionFactory();
bool IsEnabled(IAction action) => (int)action.Type < (int)Action.KILL_FORWARD;

var simStep = 0u;
Console.WriteLine("Step {0}", ++simStep);
var actionLevels1 = player.FeedForward(sensorsFactory, simStep);
Console.Write("  Action levels: ");
foreach (var level in actionLevels1) Console.Write("{0}, ", level);
Console.WriteLine();

player.ExecuteActions(factory, board, IsEnabled, actionLevels1, simStep);
var newLoc1 = player.ExecuteMoves(factory, IsEnabled, actionLevels1, simStep);
if (board.Grid.IsInBounds(newLoc1) && board.Grid.IsEmptyAt(newLoc1))
    board.Peeps.QueueForMove(player, newLoc1);

board.Peeps.DrainMoveQueue(board.Grid);
Console.WriteLine();
Console.WriteLine(board.Grid);

Console.WriteLine("Step {0}", ++simStep);
var actionLevels2 = player.FeedForward(sensorsFactory, simStep);
Console.Write("  Action levels: ");
foreach (var level in actionLevels2) Console.Write("{0}, ", level);
Console.WriteLine();

player.ExecuteActions(factory, board, IsEnabled, actionLevels2, simStep);
var newLoc2 = player.ExecuteMoves(factory, IsEnabled, actionLevels2, simStep);
if (board.Grid.IsInBounds(newLoc2) && board.Grid.IsEmptyAt(newLoc2))
    board.Peeps.QueueForMove(player, newLoc2);

board.Peeps.DrainMoveQueue(board.Grid);
Console.WriteLine();
Console.WriteLine(board.Grid);