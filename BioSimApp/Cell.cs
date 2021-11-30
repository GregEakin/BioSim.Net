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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.Field;
using BioSimLib.Sensors;

namespace BioSimApp;

public class Cell
{
    public Player Player { get; private set; }
    private readonly Path _path;

    public UIElement Element => _path;

    public Cell(Player player)
    {
        Player = player;

        var (red, green, blue) = Player.Color;
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

        _path.SetValue(Canvas.LeftProperty, 0.5 + Player._loc.X);
        _path.SetValue(Canvas.TopProperty, 0.5 + Player._loc.Y);
    }

    public void Update(Board board, SensorFactory sensorsFactory, ActionFactory actionFactory, float[] actionLevels, float[] neuronAccumlator, uint simStep)
    {
        if (!Player.Alive)
            return;

        for (var i = 0; i < actionLevels.Length; i++) actionLevels[i] = 0.0f;
        for (var i = 0; i < neuronAccumlator.Length; i++) neuronAccumlator[i] = 0.0f;

        Player.FeedForward(sensorsFactory, actionLevels, neuronAccumlator, simStep);
        Player.ExecuteActions(actionFactory, board, actionLevels, simStep);
        var newLoc = Player.ExecuteMoves(actionFactory, actionLevels, simStep);
        if (board.Grid.IsInBounds(newLoc))
            board.Peeps.QueueForMove(Player, newLoc);
    }

    public void Draw(Canvas myCanvas, double scaleFactor)
    {
        if (!Player.Alive)
        {
            _path.SetValue(Canvas.LeftProperty, -100.0);
            _path.SetValue(Canvas.TopProperty, -100.0);
            return;
        }

        _path.SetValue(Canvas.LeftProperty, (0.5 + Player._loc.X) * scaleFactor);
        _path.SetValue(Canvas.TopProperty, (0.5 + Player._loc.Y) * scaleFactor);
    }

    public void PlayerChanged(Player player)
    {
        Player = player;

        var (red, green, blue) = Player.Color;
        var color = Color.FromRgb(red, green, blue);
        var brush = new SolidColorBrush(color);
        _path.Fill = brush;
        _path.Stroke = brush;
    }

    public void SizeChanged(double scaleFactor)
    {
        _path.Data = new EllipseGeometry
        {
            RadiusX = 0.5 * scaleFactor,
            RadiusY = 0.5 * scaleFactor
        };
    }
}