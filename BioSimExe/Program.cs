// See https://aka.ms/new-console-template for more information

using BioSimLib;
using BioSimLib.Sensors;

Console.WriteLine("Hello, World!");

var p = new Params() { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
Console.WriteLine("Parameters {0}", p);

var grid = new Grid(p);

//var sensorFactory = new SensorFactory();
var sensorsFactory = new SensorFactory(
    new ISensor[]
    {
        new SensorMock(Sensor.LOC_X, "Lx", 0.2f),
        null, null, null, null, null, null, null, null, null,
        null, null, null, null, null, null, null,
        new SensorMock(Sensor.RANDOM, "Rnd", 0.1f),
        null, null, null,
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

var loc = new Coord { X = 4, Y = 4 };
var player = grid.NewPlayer(genome, loc);
grid.Move(player, loc);

Console.WriteLine("Individual: {0}", player);

var (sensors, actions) = player._nnet.ActionReferenceCounts();
foreach (var sensor in sensors) Console.Write("  {0}", sensor);
Console.WriteLine();
foreach (var action in actions) Console.Write("  {0}", action);
Console.WriteLine();


Console.WriteLine();
Console.WriteLine("Step 1");
var actionLevels = player.FeedForward4(sensorsFactory, 0);
foreach (var level in actionLevels) Console.Write("{0}, ", level);
Console.WriteLine();

player.ExecuteActions(actionLevels);

var peeps = new Peeps(p, grid);
var newLoc = new Coord { X = 5, Y = 5 };
peeps.QueueForMove(player, newLoc);

Console.WriteLine(grid);
peeps.DrainMoveQueue();
Console.WriteLine();
Console.WriteLine(grid);