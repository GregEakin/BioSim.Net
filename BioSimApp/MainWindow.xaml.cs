//    Copyright 2022 Gregory Eakin
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

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.BarrierFactory;
using BioSimLib.Challenges;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromMilliseconds(30) };

    private readonly Config _config = new()
    {
        sizeX = 128,
        sizeY = 128,
        population = 1000,
        stepsPerGeneration = 300,
        genomeMaxLength = 24,
        maxNumberNeurons = 12,
        populationSensorRadius = 10,
        signalSensorRadius = 10,
        shortProbeBarrierDistance = 4,
        longProbeDistance = 10,
        signalLayers = 1,
        challenge = Challenge.CornerWeighted,
    };

    private readonly Board _board;
    private readonly GeneBank _bank;
    private readonly BarrierFactory _barrierFactory;
    private readonly ChallengeFactory _challengeFactory;
    private readonly SensorFactory _sensorFactory;
    private readonly ActionFactory _actionFactory;
    private readonly Rectangle _box1;
    private readonly Rectangle _box2;
    private readonly Cell[] _cells;
    private readonly float[] _actionLevels = new float[Enum.GetNames<Action>().Length];
    private readonly float[] _neuronAccumulators;
    private readonly Canvas[] _icons;
    private readonly TextBlock[] _items;


    private double _scaleFactor = 3.4;
    private uint _generation;
    private uint _simStep;
    private int _census;
    private int _skipUpdate = 10;

    public MainWindow()
    {
        InitializeComponent();

        WorldSize.Text = $"{_config.sizeX}x{_config.sizeY}";
        Population.Text = _config.population.ToString();
        StepGen.Text = _config.stepsPerGeneration.ToString();
        GenomeLen.Text = $"{_config.genomeMaxLength} genes";
        NeuronLen.Text = _config.maxNumberNeurons.ToString();

        _bank = new GeneBank(_config);
        _board = new Board(_config);
        _barrierFactory = new BarrierFactory(_board.Grid);
        _challengeFactory = new ChallengeFactory(_config, _board.Grid);
        _sensorFactory = new SensorFactory(_config, _board);
        _actionFactory = new ActionFactory(_config, _board);
        _cells = new Cell[_config.population];
        _neuronAccumulators = new float[_config.maxNumberNeurons];

        _box1 = new Rectangle
        {
            Stroke = Brushes.LavenderBlush,
            Fill = Brushes.LavenderBlush,
            Height = _config.sizeX * _scaleFactor,
            Width = (_config.sizeY / 2.0 - 2.0) * _scaleFactor
        };
        _box1.SetValue(Canvas.LeftProperty, 0.0);
        MyCanvas.Children.Add(_box1);

        _box2 = new Rectangle
        {
            Stroke = Brushes.LavenderBlush,
            Fill = Brushes.LavenderBlush,
            Height = _config.sizeX * _scaleFactor,
            Width = 2.0 * _scaleFactor
        };
        _box2.SetValue(Canvas.LeftProperty, (_config.sizeX - 2.0) * _scaleFactor);
        MyCanvas.Children.Add(_box2);

        _icons = new[]{ Icon0, Icon1, Icon2, Icon3, Icon4 };
        _items = new[]{ Item0, Item1, Item2, Item3, Item4 };

        var i = 0;
        foreach (var genome in _bank.Startup())
        {
            var critter = _board.NewCritter(genome);
            _cells[i] = new Cell(critter);
            MyCanvas.Children.Add(_cells[i].Element);
            i++;
        }

        SizeChanged += MainWindow_SizeChanged;

        _timer.Tick += OnTimerOnTick;
        _timer.Start();
    }

    public void Update()
    {
        if (_simStep == 1u && _generation % 5 == 0)
        {
            var census = _board.Critters.Census();
            _census = census.Count;
        }

        if (_simStep >= _config.stepsPerGeneration)
        {
            _generation++;
            _simStep = 0u;
            var i = 0;
            var survivors = _board.NewGeneration();
            foreach (var genome in _bank.NewGeneration(survivors))
            {
                var critter = _board.NewCritter(genome);
                _cells[i].CritterChanged(critter);
                i++;
            }

            var j = 0;
            foreach (var ((red, green, blue), population) in _bank.Survivors.OrderByDescending(o => o.population))
            {
                var color = Color.FromRgb(red, green, blue);
                var brush = new SolidColorBrush(color);
                var path = new Path
                {
                    Fill = brush,
                    Stroke = brush,
                    Data = new EllipseGeometry
                    {
                        RadiusX = 7.0,
                        RadiusY = 7.0
                    },
                    StrokeThickness = 0.1,
                };

                _icons[j].Children.Add(path);
                _items[j].Text = population.ToString();
                j++;
            }
        }

        foreach (var critter in _cells)
            critter.Update(_board, _sensorFactory, _actionFactory, _actionLevels, _neuronAccumulators, _simStep);

        _board.Critters.DrainDeathQueue(_board.Grid);
        _board.Critters.DrainMoveQueue(_board.Grid);

        _simStep++;
    }

    public void Draw()
    {
        Generation.Text = _generation.ToString();
        SimStep.Text = _simStep.ToString();
        Census.Text = $"{_census} colors";

        foreach (var critter in _cells)
            critter.Draw(MyCanvas, _scaleFactor);
    }

    private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var w1 = MyCanvas.ActualWidth / _config.sizeX;
        var h1 = MyCanvas.ActualHeight / _config.sizeY;
        _scaleFactor = Math.Min(w1, h1);

        _box1.Height = _config.sizeX * _scaleFactor;
        _box1.Width = (_config.sizeY / 2.0 - 2.0) * _scaleFactor;
        _box1.SetValue(Canvas.LeftProperty, 0.0);

        _box2.Height = _config.sizeX * _scaleFactor;
        _box2.Width = 2.0 * _scaleFactor;
        _box2.SetValue(Canvas.LeftProperty, (_config.sizeX - 2.0) * _scaleFactor);

        foreach (var critter in _cells)
            critter.SizeChanged(_scaleFactor);
    }

    private void OnTimerOnTick(object? s, EventArgs e)
    {
        for (var i = 0; i < _skipUpdate; i++)
            Update();

        Draw();
    }
}