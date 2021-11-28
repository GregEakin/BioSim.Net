using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Sensors;

namespace BioSimApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private double ScaleFactor { get; set; } = 1.0;
    private uint _generation;
    private uint _simStep;
    
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromMilliseconds(30) };
    private readonly Config _p = new()
    {
        sizeX = 128,
        sizeY = 128,
        population = 1000,
        stepsPerGeneration = 300,
        genomeMaxLength = 4,
        maxNumberNeurons = 3,
        populationSensorRadius = 10, 
        shortProbeBarrierDistance = 4, 
        signalLayers = 1,
    };

    private readonly Board _board;
    private readonly SensorFactory _sensorFactory;
    private readonly ActionFactory _actionFactory;
    private readonly Cell[] _critters;

    public MainWindow()
    {
        _board = new Board(_p);
        _sensorFactory = new SensorFactory(_p, _board);
        _actionFactory = new ActionFactory();
        _critters = new Cell[_p.population];

        for (var i = 0; i < _p.population; i++)
        {
            var genome = new Genome(_p);
            var loc = _board.Grid.FindEmptyLocation();
            var player = _board.NewPlayer(genome, loc);
            _critters[i] = new Cell(player);
        }

        InitializeComponent();

        _timer.Tick += OnTimerOnTick;
        _timer.Start();

        SizeChanged += MainWindow_SizeChanged;

        WorldSize.Text = $"{_p.sizeX}x{_p.sizeY}";
        Population.Text = _p.population.ToString();
        StepGen.Text = _p.stepsPerGeneration.ToString();
        GenomeLen.Text = $"{_p.genomeMaxLength} genes";
        NeuronLen.Text = _p.maxNumberNeurons.ToString();
    }

    public void Update()
    {
        _simStep++;
        if (_simStep > _p.stepsPerGeneration)
        {
            _generation++;
            _simStep = 0u;
            var players = _board.NewGeneration().ToArray();
            for (var i = 0; i < _p.population; i++)
                _critters[i] = new Cell(players[i]);
        }

        foreach (var critter in _critters)
            critter.Update(_board, _sensorFactory, _actionFactory, 0u);

        _board.Peeps.DrainMoveQueue(_board.Grid);
    }

    public void Draw()
    {
        MyCanvas.Children.Clear();
        Generation.Text = _generation.ToString();
        SimStep.Text = _simStep.ToString();

        foreach (var critter in _critters) 
            critter.Draw(MyCanvas, ScaleFactor);
    }

    private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var w1 = MyCanvas.ActualWidth / _p.sizeX;
        var h1 = MyCanvas.ActualHeight / _p.sizeY;
        ScaleFactor = Math.Min(w1, h1);
    }

    private void OnTimerOnTick(object? s, EventArgs e)
    {
        Update();
        Draw();
    }
}