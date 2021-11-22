// See https://aka.ms/new-console-template for more information

using BioSimLib;
using BioSimLib.Sensors;

Console.WriteLine("Hello, World!");

var p = new Params();
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
    0x01042000u,
    0x91042000u,
    0x010A2000u,
    0x01802000u,
    0x80802000u,
    0x00812000u,
    0x01812000u,
};

var genome = new Genome(p, dna);
Console.WriteLine("Genome {0}", genome);

var loc = new Coord { X = 4, Y = 4 };
var individual = new Indiv(p, grid, 0, loc, genome);
grid.Move(individual, loc);

Console.WriteLine("Individual {0}", individual);

var (sensors, actions) = individual._nnet.ActionReferenceCounts();
foreach (var sensor in sensors) Console.Write("  {0}", sensor);
Console.WriteLine();
foreach (var action in actions) Console.Write("  {0}", action);
Console.WriteLine();


Console.WriteLine();
Console.WriteLine("Step 1");
var actionLevels = individual.FeedForward2(sensorsFactory, 0);
foreach (var level in actionLevels) Console.Write("{0}, ", level);
Console.WriteLine();

individual.ExecuteActions(actionLevels);

var peeps = new Peeps(p, grid);
var newLoc = new Coord { X = 5, Y = 5 };
peeps.QueueForMove(individual, newLoc);

Console.WriteLine(grid);
peeps.DrainMoveQueue();
Console.WriteLine();
Console.WriteLine(grid);