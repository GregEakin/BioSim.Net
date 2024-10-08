﻿// Copyright 2022 Gregory Eakin
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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

public sealed class Cell
{
    private readonly Path _path;

    public Critter Critter { get; private set; }

    public UIElement Element => _path;

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

    private static bool IsEnabled(IAction action)
    {
        return (int)action.Type < (int)Action.KILL_FORWARD;
    }

    public void Update(Board board, SensorFactory sensorFactory, ActionFactory actionFactory, float[] actionLevels,
        float[] neuronAccumulator, uint simStep)
    {
        if (!Critter.Alive)
            return;

        Array.Clear(actionLevels);
        Array.Clear(neuronAccumulator);

        Critter.FeedForward(sensorFactory, actionLevels, neuronAccumulator, simStep);
        Critter.ExecuteActions(actionFactory, IsEnabled, actionLevels, simStep);
        var newLoc = Critter.ExecuteMoves(actionFactory, IsEnabled, actionLevels);
        if (board.Grid.IsInBounds(newLoc))
            board.Critters.QueueForMove(Critter, newLoc);
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

    public void ReplaceCritter(Critter critter)
    {
        Critter = critter;

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