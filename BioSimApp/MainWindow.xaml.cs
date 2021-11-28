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

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using BioSimLib;
using BioSimLib.Actions;
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
    private readonly Config _p = new()
    {
        sizeX = 128,
        sizeY = 128,
        population = 1000,
        stepsPerGeneration = 300,
        genomeMaxLength = 24,
        maxNumberNeurons = 12,
        populationSensorRadius = 10,
        shortProbeBarrierDistance = 4,
        longProbeDistance = 10,
        signalLayers = 1,
    };

    private readonly Board _board;
    private readonly SensorFactory _sensorFactory;
    private readonly ActionFactory _actionFactory;
    private readonly Cell[] _critters;
    private readonly float[] _actionLevels = new float[Enum.GetNames<Action>().Length];
    private readonly float[] _neuronAccumulators;

    private double _saleFactor = 1.0;
    private uint _generation;
    private uint _simStep;
    private int _census;
    private int _skipUpdate = 10;

    public MainWindow()
    {
        _board = new Board(_p);
        _sensorFactory = new SensorFactory(_p, _board);
        _actionFactory = new ActionFactory();
        _critters = new Cell[_p.population];
        _neuronAccumulators = new float[_p.maxNumberNeurons];

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
        if (_simStep < _skipUpdate && _generation % 5 == 0)
        {
            var census = _board.Peeps.Census();
            _census = census.Count;
        }

        if (_simStep > _p.stepsPerGeneration)
        {
            _generation++;
            _simStep = 0u;
            var players = _board.NewGeneration().ToArray();
            for (var i = 0; i < _p.population; i++)
                _critters[i] = new Cell(players[i]);
        }

        foreach (var critter in _critters)
            critter.Update(_board, _sensorFactory, _actionFactory, _actionLevels, _neuronAccumulators, _simStep);

        _board.Peeps.DrainDeathQueue(_board.Grid);
        _board.Peeps.DrainMoveQueue(_board.Grid);

        _simStep++;
    }

    public void Draw()
    {
        Generation.Text = _generation.ToString();
        SimStep.Text = _simStep.ToString();
        Census.Text = $"{_census} colors";

        MyCanvas.Children.Clear();
        DrawKillZone();

        foreach (var critter in _critters)
            critter.Draw(MyCanvas, _saleFactor);
    }

    private void DrawKillZone()
    {
        var box1 = new Rectangle
        {
            Stroke = Brushes.LightPink,
            Fill = Brushes.LightPink,
            Height = _p.sizeY * _saleFactor,
            Width = _p.sizeX * _saleFactor / 2.0
        };
        MyCanvas.Children.Add(box1);

        var box2 = new Rectangle
        {
            Stroke = Brushes.LightPink,
            Fill = Brushes.LightPink,
            Height = _p.sizeY * _saleFactor,
            Width = 2 * _saleFactor,
        };
        box2.SetValue(Canvas.LeftProperty, (_p.sizeX - 2.0) * _saleFactor);
        MyCanvas.Children.Add(box2);
    }

    private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var w1 = MyCanvas.ActualWidth / _p.sizeX;
        var h1 = MyCanvas.ActualHeight / _p.sizeY;
        _saleFactor = Math.Min(w1, h1);
    }

    private void OnTimerOnTick(object? s, EventArgs e)
    {
        for (var i = 0; i < _skipUpdate; i++)
            Update();

        Draw();
    }
}