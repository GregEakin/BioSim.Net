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
    private uint SimStep { get; set; } = 0u;
    
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
    }

    public void Update()
    {
        if (SimStep > _p.stepsPerGeneration)
        {
            SimStep = 0u;
            var players = _board.NewGeneration().ToArray();
            for (var i = 0; i < _p.population; i++)
                _critters[i] = new Cell(players[i]);
        }

        SimStep++;
        foreach (var critter in _critters)
            critter.Update(_board, _sensorFactory, _actionFactory, 0u);

        _board.Peeps.DrainMoveQueue(_board.Grid);
    }

    public void Draw()
    {
        MyCanvas.Children.Clear();
        foreach (var critter in _critters) 
            critter.Draw(MyCanvas, ScaleFactor);
    }

    private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var w1 = 0.90 * e.NewSize.Width / _p.sizeX;
        var h1 = 0.90 * e.NewSize.Height / _p.sizeY;
        ScaleFactor = Math.Min(w1, h1);
    }

    private void OnTimerOnTick(object? s, EventArgs e)
    {
        Update();
        Draw();
    }
}