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
using BioSimLib.BarrierFactory;
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
    private readonly GeneBank _bank;
    private readonly BarrierFactory _barrierFactory;
    private readonly SensorFactory _sensorFactory;
    private readonly ActionFactory _actionFactory;
    private readonly Rectangle _box1;
    private readonly Rectangle _box2;
    private readonly Cell[] _critters;
    private readonly float[] _actionLevels = new float[Enum.GetNames<Action>().Length];
    private readonly float[] _neuronAccumulators;

    private double _scaleFactor = 3.4;
    private uint _generation;
    private uint _simStep;
    private int _census;
    private int _skipUpdate = 10;

    public MainWindow()
    {
        InitializeComponent();

        WorldSize.Text = $"{_p.sizeX}x{_p.sizeY}";
        Population.Text = _p.population.ToString();
        StepGen.Text = _p.stepsPerGeneration.ToString();
        GenomeLen.Text = $"{_p.genomeMaxLength} genes";
        NeuronLen.Text = _p.maxNumberNeurons.ToString();

        _bank = new GeneBank(_p);
        _board = new Board(_p);
        _barrierFactory = new BarrierFactory(_board.Grid);
        _sensorFactory = new SensorFactory(_p, _board);
        _actionFactory = new ActionFactory();
        _critters = new Cell[_p.population];
        _neuronAccumulators = new float[_p.maxNumberNeurons];

        _box1 = new Rectangle
        {
            Stroke = Brushes.LavenderBlush,
            Fill = Brushes.LavenderBlush,
            Height = _p.sizeX * _scaleFactor,
            Width = (_p.sizeY / 2.0 - 2.0) * _scaleFactor
        };
        _box1.SetValue(Canvas.LeftProperty, 0.0);
        MyCanvas.Children.Add(_box1);

        _box2 = new Rectangle
        {
            Stroke = Brushes.LavenderBlush,
            Fill = Brushes.LavenderBlush,
            Height = _p.sizeX * _scaleFactor,
            Width = 2.0 * _scaleFactor
        };
        _box2.SetValue(Canvas.LeftProperty, (_p.sizeX - 2.0) * _scaleFactor);
        MyCanvas.Children.Add(_box2);

        var i = 0;
        foreach (var genome in _bank.Startup())
        {
            var player = _board.CreatePlayer(genome);
            _critters[i] = new Cell(player);
            MyCanvas.Children.Add(_critters[i].Element);
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
            var census = _board.Peeps.Census();
            _census = census.Count;
        }

        if (_simStep >= _p.stepsPerGeneration)
        {
            _generation++;
            _simStep = 0u;
            var i = 0;
            var survivors = _board.NewGeneration();
            foreach (var genome in _bank.NewGeneration(survivors))
            {
                var player = _board.CreatePlayer(genome);
                _critters[i].PlayerChanged(player);
                i++;
            }

            var sortedList = _bank.Survivors.OrderByDescending(o => o.Item2).ToArray();
            {
                var (red, green, blue) = sortedList[0].Item1;
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
                Icon0.Children.Add(path);

                Item0.Text = sortedList[0].Item2.ToString();
            }

            {
                var (red, green, blue) = sortedList[1].Item1;
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
                Icon1.Children.Add(path);

                Item1.Text = sortedList[1].Item2.ToString();
            }

            {
                var (red, green, blue) = sortedList[2].Item1;
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
                Icon2.Children.Add(path);

                Item2.Text = sortedList[2].Item2.ToString();
            }

            {
                var (red, green, blue) = sortedList[3].Item1;
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
                Icon3.Children.Add(path);

                Item3.Text = sortedList[3].Item2.ToString();
            }

            {
                var (red, green, blue) = sortedList[4].Item1;
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
                Icon4.Children.Add(path);

                Item4.Text = sortedList[4].Item2.ToString();
            }
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

        foreach (var critter in _critters)
            critter.Draw(MyCanvas, _scaleFactor);
    }

    private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var w1 = MyCanvas.ActualWidth / _p.sizeX;
        var h1 = MyCanvas.ActualHeight / _p.sizeY;
        _scaleFactor = Math.Min(w1, h1);

        _box1.Height = _p.sizeX * _scaleFactor;
        _box1.Width = (_p.sizeY / 2.0 - 2.0) * _scaleFactor;
        _box1.SetValue(Canvas.LeftProperty, 0.0);

        _box2.Height = _p.sizeX * _scaleFactor;
        _box2.Width = 2.0 * _scaleFactor;
        _box2.SetValue(Canvas.LeftProperty, (_p.sizeX - 2.0) * _scaleFactor);

        foreach (var critter in _critters)
            critter.SizeChanged(_scaleFactor);
    }

    private void OnTimerOnTick(object? s, EventArgs e)
    {
        for (var i = 0; i < _skipUpdate; i++)
            Update();

        Draw();
    }
}