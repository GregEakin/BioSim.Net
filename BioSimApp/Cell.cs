// Log File Viewer - Cell.cs
// 
// Copyright © 2021 Greg Eakin.
// 
// Greg Eakin <greg@gdbtech.info>
// 
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.Field;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimApp;

public class Cell
{
    private readonly Path _path;

    public Cell(Critter critter)
    {
        Critter = critter;

        var (red, green, blue) = Critter.Color;
        var color = Color.FromRgb(red, green, blue);
        var brush = new SolidColorBrush(color);
        _path = new Path
        {
            Fill = brush,
            Stroke = brush,
            Data = new EllipseGeometry
            {
                RadiusX = 0.5,
                RadiusY = 0.5
            },
            StrokeThickness = 0.1,
        };

        _path.SetValue(Canvas.LeftProperty, 0.5 + Critter.LocX);
        _path.SetValue(Canvas.TopProperty, 0.5 + Critter.LocY);
    }

    public Critter Critter { get; private set; }

    public UIElement Element => _path;

    private static bool IsEnabled(IAction action) => (int)action.Type < (int)Action.KILL_FORWARD;

    public void Update(Board board, SensorFactory sensorFactory, ActionFactory actionFactory, float[] actionLevels,
        float[] neuronAccumulator, uint simStep)
    {
        if (!Critter.Alive)
            return;

        Array.Clear(actionLevels);
        Array.Clear(neuronAccumulator);

        Critter.FeedForward(sensorFactory, actionLevels, neuronAccumulator, simStep);
        Critter.ExecuteActions(actionFactory, board, IsEnabled, actionLevels, simStep);
        var newLoc = Critter.ExecuteMoves(actionFactory, IsEnabled, actionLevels, simStep);
        if (board.Grid.IsInBounds(newLoc))
            board.Peeps.QueueForMove(Critter, newLoc);
    }

    public void Draw(Canvas canvas, double scaleFactor)
    {
        if (!Critter.Alive)
        {
            _path.SetValue(Canvas.LeftProperty, -100.0);
            _path.SetValue(Canvas.TopProperty, -100.0);
            return;
        }

        _path.SetValue(Canvas.LeftProperty, (0.5 + Critter.LocX) * scaleFactor);
        _path.SetValue(Canvas.TopProperty, (0.5 + Critter.LocY) * scaleFactor);
    }

    public void CritterChanged(Critter player)
    {
        Critter = player;

        var (red, green, blue) = Critter.Color;
        var color = Color.FromRgb(red, green, blue);
        if (_path.Fill is SolidColorBrush b1)
            b1.Color = color;
        if (_path.Stroke is SolidColorBrush b2)
            b2.Color = color;
    }

    public void SizeChanged(double scaleFactor)
    {
        if (_path.Data is not EllipseGeometry ellipseGeometry) return;
        ellipseGeometry.RadiusX = 0.5 * scaleFactor;
        ellipseGeometry.RadiusY = 0.5 * scaleFactor;
    }
}