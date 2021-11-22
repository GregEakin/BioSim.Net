// See https://aka.ms/new-console-template for more information

using System.Numerics;
using BioSimLib;

Console.WriteLine("Hello, World!");

var p = new Params();
Console.WriteLine("Parameters {0}", p);

var grid = new Grid(p);

var genome = new Genome(p);
Console.WriteLine("Genome {0}", genome);

var loc = new Coord { X = 4, Y = 4 };
var individual = new Indiv(p, grid, 0, loc, genome);
grid.Move(individual, loc);

Console.WriteLine("Individual {0}", individual);

var (sensors, actions) = individual.nnet.ActionReferenceCounts();
foreach (var sensor in sensors) Console.Write("  {0}", sensor);
Console.WriteLine();
foreach (var action in actions) Console.Write("  {0}", action);
Console.WriteLine();


Console.WriteLine();
Console.WriteLine("Step 1");
var actionLevels = individual.FeedForward(0);
foreach (var level in actionLevels) Console.Write("  {0}", level);
Console.WriteLine();

individual.ExecuteActions(actionLevels);

var peeps = new Peeps(p, grid);
var newLoc = new Coord { X = 5, Y = 5 };
peeps.QueueForMove(individual, newLoc);

Console.WriteLine(grid);
peeps.DrainMoveQueue();
Console.WriteLine();
Console.WriteLine(grid);
